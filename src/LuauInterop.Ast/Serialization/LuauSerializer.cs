using System.Text.Json;

using LuauInterop.Ast.Nodes;
using LuauInterop.Ast.Nodes.Expressions;
using LuauInterop.Ast.Nodes.Statements;
using LuauInterop.Ast.Nodes.Types;

namespace LuauInterop.Ast.Serialization;

public static class LuauSerializer
{
    public static SyntaxTree Deserialize(string json)
    {
        var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        var errors = DeserializeErrors(root.GetProperty("errors"));
        var comments = DeserializeComments(root.GetProperty("commentLocations"));
        var rootBlock = (AstStatBlock)DeserializeNode(root.GetProperty("root"));

        return new SyntaxTree(rootBlock, comments, errors);
    }

    private static AstAttr DeserializeAttr(JsonElement element)
    {
        var location = DeserializeLocation(element.GetProperty("location").GetString()!);
        var name = element.GetProperty("name").GetString()!;
        return new AstAttr(location, name);
    }

    private static List<LuauComment> DeserializeComments(JsonElement elements)
    {
        var comments = new List<LuauComment>();
        foreach (var element in elements.EnumerateArray())
        {
            comments.Add(DeserializeComment(element));
        }
        return comments;
    }

    private static LuauComment DeserializeComment(JsonElement element) =>
        new(DeserializeLocation(element.GetProperty("location").GetString()!), element.GetProperty("type").GetString()!);

    private static List<LuauParseError> DeserializeErrors(JsonElement elements)
    {
        var errors = new List<LuauParseError>();
        foreach (var element in elements.EnumerateArray())
        {
            errors.Add(DeserializeError(element));
        }
        return errors;
    }

    private static LuauParseError DeserializeError(JsonElement element) =>
        new(DeserializeLocation(element.GetProperty("location").GetString()!), element.GetProperty("message").GetString()!);

    private static AstLocal DeserializeLocal(JsonElement element)
    {
        var name = element.GetProperty("name").GetString()!;
        var isConst = element.TryGetProperty("isConst", out var isConstProperty) && isConstProperty.GetBoolean();
        var location = DeserializeLocation(element.GetProperty("location").GetString()!);

        var local = new AstLocal(location, name, isConst);

        if (element.TryGetProperty("luauType", out var luauTypeProp) && luauTypeProp.ValueKind == JsonValueKind.Object)
        {
            local.LuauType = DeserializeType(luauTypeProp);
        }

        return local;
    }

    private static AstExpr DeserializeExpr(JsonElement element)
    {
        var type = element.GetProperty("type").GetString()!;
        var location = DeserializeLocation(element.GetProperty("location").GetString()!);

        return type switch
        {
            "AstExprGroup" => DeserializeExprGroup(element, location),
            "AstExprConstantNil" => DeserializeExprConstantNil(location),
            "AstExprConstantBool" => DeserializeExprConstantBool(element, location),
            "AstExprConstantNumber" => DeserializeExprConstantNumber(element, location),
            "AstExprConstantInteger" => DeserializeExprConstantInteger(element, location),
            "AstExprConstantString" => DeserializeExprConstantString(element, location),
            "AstExprLocal" => DeserializeExprLocal(element, location),
            "AstExprGlobal" => DeserializeExprGlobal(element, location),
            "AstExprVarargs" => DeserializeExprVarargs(location),
            "AstExprCall" => DeserializeExprCall(element, location),
            "AstExprIndexName" => DeserializeExprIndexName(element, location),
            "AstExprIndexExpr" => DeserializeExprIndexExpr(element, location),
            "AstExprFunction" => DeserializeExprFunction(element, location),
            "AstExprIfElse" => DeserializeExprIfElse(element, location),
            "AstExprInterpString" => DeserializeExprInterpString(element, location),
            "AstExprTable" => DeserializeExprTable(element, location),
            "AstExprUnary" => DeserializeExprUnary(element, location),
            "AstExprBinary" => DeserializeExprBinary(element, location),
            "AstExprTypeAssertion" => DeserializeExprTypeAssertion(element, location),
            "AstExprError" => DeserializeExprError(element, location),
            _ => throw new NotSupportedException($"Unknown expression type: {type}")
        };
    }

    private static AstExprGroup DeserializeExprGroup(JsonElement element, Location location)
    {
        var expression = DeserializeExpr(element.GetProperty("expr"));
        return new AstExprGroup(location, expression);
    }

    private static AstExprConstantNil DeserializeExprConstantNil(Location location) => new(location);

    private static AstExprConstantBool DeserializeExprConstantBool(JsonElement element, Location location) =>
        new(location, element.GetProperty("value").GetBoolean());

    private static AstExprConstantNumber DeserializeExprConstantNumber(JsonElement element, Location location)
    {
        // value may have been replaced with null if it was NaN/Infinity
        var valueProp = element.GetProperty("value");
        var value = valueProp.ValueKind == JsonValueKind.Null ? double.NaN : valueProp.GetDouble();
        return new AstExprConstantNumber(location, value);
    }

    private static AstExprConstantInteger DeserializeExprConstantInteger(JsonElement element, Location location) =>
        new(location, element.GetProperty("value").GetInt64());

    private static AstExprConstantString DeserializeExprConstantString(JsonElement element, Location location) =>
        new(location, element.GetProperty("value").GetString()!);

    private static AstExprLocal DeserializeExprLocal(JsonElement element, Location location) =>
        new(location, DeserializeLocal(element.GetProperty("local")));

    private static AstExprGlobal DeserializeExprGlobal(JsonElement element, Location location) =>
        new(location, element.GetProperty("global").GetString()!);

    private static AstExprVarargs DeserializeExprVarargs(Location location) => new(location);

    private static AstExprCall DeserializeExprCall(JsonElement element, Location location)
    {
        var func = DeserializeExpr(element.GetProperty("func"));
        var args = DeserializeExprList(element.GetProperty("args"));
        var self = element.GetProperty("self").GetBoolean();
        var argLocation = DeserializeLocation(element.GetProperty("argLocation").GetString()!);
        return new AstExprCall(location, func, args, self, argLocation);
    }

    private static AstExprIndexName DeserializeExprIndexName(JsonElement element, Location location)
    {
        var expression = DeserializeExpr(element.GetProperty("expr"));
        var index = element.GetProperty("index").GetString()!;
        var indexLocation = DeserializeLocation(element.GetProperty("indexLocation").GetString()!);
        var op = element.TryGetProperty("op", out var opProp) && opProp.ValueKind == JsonValueKind.String ? opProp.GetString() : null;
        return new AstExprIndexName(location, expression, index, indexLocation, op);
    }

    private static AstExprIndexExpr DeserializeExprIndexExpr(JsonElement element, Location location)
    {
        var expression = DeserializeExpr(element.GetProperty("expr"));
        var index = DeserializeExpr(element.GetProperty("index"));
        return new AstExprIndexExpr(location, expression, index);
    }

    private static AstExprFunction DeserializeExprFunction(JsonElement element, Location location)
    {
        var attributes = DeserializeAttrList(element.GetProperty("attributes"));
        var generics = DeserializeGenericTypeList(element.GetProperty("generics"));
        var genericPacks = DeserializeGenericTypePackList(element.GetProperty("genericPacks"));

        AstLocal? self = null;
        if (element.TryGetProperty("self", out var selfProp) && selfProp.ValueKind == JsonValueKind.Object)
        {
            self = DeserializeLocal(selfProp);
        }

        var args = DeserializeLocalList(element.GetProperty("args"));

        AstTypeList? returnAnnotation = null;
        if (element.TryGetProperty("returnAnnotation", out var retProp) && retProp.ValueKind == JsonValueKind.Object)
        {
            returnAnnotation = DeserializeTypeList(retProp);
        }

        // FIX: vararg on AstExprFunction is a bool, not an AstArgumentName object.
        // AstArgumentName vararg only appears on AstStatDeclareFunction.
        var vararg = element.TryGetProperty("vararg", out var varargProp)
            && varargProp.ValueKind == JsonValueKind.True;

        Location? varargLocation = null;
        if (element.TryGetProperty("varargLocation", out var varLocProp) && varLocProp.ValueKind == JsonValueKind.String)
        {
            varargLocation = DeserializeLocation(varLocProp.GetString()!);
        }

        AstTypePack? varargAnnotation = null;
        if (element.TryGetProperty("varargAnnotation", out var varAnnotProp) && varAnnotProp.ValueKind == JsonValueKind.Object)
        {
            varargAnnotation = DeserializeTypePack(varAnnotProp);
        }

        var body = (AstStatBlock)DeserializeNode(element.GetProperty("body"));
        var functionDepth = element.GetProperty("functionDepth").GetInt32();

        string? debugname = null;
        if (element.TryGetProperty("debugname", out var debugProp) && debugProp.ValueKind == JsonValueKind.String)
        {
            debugname = debugProp.GetString();
        }

        return new AstExprFunction(location, attributes, generics, genericPacks, self, args, returnAnnotation, vararg, varargLocation, varargAnnotation, body, functionDepth, debugname);
    }

    private static AstExprIfElse DeserializeExprIfElse(JsonElement element, Location location)
    {
        var condition = DeserializeExpr(element.GetProperty("condition"));
        var hasThen = element.GetProperty("hasThen").GetBoolean();
        var trueExpr = DeserializeExpr(element.GetProperty("trueExpr"));
        var hasElse = element.GetProperty("hasElse").GetBoolean();
        var falseExpr = DeserializeExpr(element.GetProperty("falseExpr"));
        return new AstExprIfElse(location, condition, hasThen, trueExpr, hasElse, falseExpr);
    }

    private static AstExprInterpString DeserializeExprInterpString(JsonElement element, Location location)
    {
        var strings = element.GetProperty("strings").EnumerateArray()
            .Select(s => s.GetString()!)
            .ToList();
        var expressions = DeserializeExprList(element.GetProperty("expressions"));
        return new AstExprInterpString(location, strings, expressions);
    }

    private static AstExprTable DeserializeExprTable(JsonElement element, Location location)
    {
        var items = new List<AstExprTableItem>();
        foreach (var item in element.GetProperty("items").EnumerateArray())
        {
            var kind = item.GetProperty("kind").GetString()!;
            AstExpr? key = null;
            if (kind != "item")
            {
                key = DeserializeExpr(item.GetProperty("key"));
            }
            var value = DeserializeExpr(item.GetProperty("value"));
            items.Add(new AstExprTableItem(kind, key, value));
        }
        return new AstExprTable(location, items);
    }

    private static AstExprUnary DeserializeExprUnary(JsonElement element, Location location)
    {
        var op = element.GetProperty("op").GetString()!;
        var expr = DeserializeExpr(element.GetProperty("expr"));
        return new AstExprUnary(location, op, expr);
    }

    private static AstExprBinary DeserializeExprBinary(JsonElement element, Location location)
    {
        var op = element.GetProperty("op").GetString()!;
        var left = DeserializeExpr(element.GetProperty("left"));
        var right = DeserializeExpr(element.GetProperty("right"));
        return new AstExprBinary(location, op, left, right);
    }

    private static AstExprTypeAssertion DeserializeExprTypeAssertion(JsonElement element, Location location)
    {
        var expr = DeserializeExpr(element.GetProperty("expr"));
        var annotation = DeserializeType(element.GetProperty("annotation"));
        return new AstExprTypeAssertion(location, expr, annotation);
    }

    private static AstExprError DeserializeExprError(JsonElement element, Location location)
    {
        var expressions = DeserializeExprList(element.GetProperty("expressions"));
        var messageIndex = element.GetProperty("messageIndex").GetUInt32();
        return new AstExprError(location, expressions, messageIndex);
    }

    private static AstNode DeserializeNode(JsonElement element)
    {
        var type = element.GetProperty("type").GetString()!;
        var location = DeserializeLocation(element.GetProperty("location").GetString()!);

        return type switch
        {
            "AstStatError" => DeserializeStatError(element, location),
            "AstStatBlock" => DeserializeStatBlock(element, location),
            "AstStatExpr" => DeserializeStatExpr(element, location),
            "AstStatIf" => DeserializeStatIf(element, location),
            "AstStatWhile" => DeserializeStatWhile(element, location),
            "AstStatRepeat" => DeserializeStatRepeat(element, location),
            "AstStatBreak" => DeserializeStatBreak(location),
            "AstStatContinue" => DeserializeStatContinue(location),
            "AstStatReturn" => DeserializeStatReturn(element, location),
            "AstStatLocal" => DeserializeStatLocal(element, location),
            "AstStatFor" => DeserializeStatFor(element, location),
            "AstStatForIn" => DeserializeStatForIn(element, location),
            "AstStatAssign" => DeserializeStatAssign(element, location),
            "AstStatCompoundAssign" => DeserializeStatCompoundAssign(element, location),
            "AstStatFunction" => DeserializeStatFunction(element, location),
            "AstStatLocalFunction" => DeserializeStatLocalFunction(element, location),
            "AstStatTypeAlias" => DeserializeStatTypeAlias(element, location),
            "AstStatDeclareFunction" => DeserializeStatDeclareFunction(element, location),
            "AstStatDeclareGlobal" => DeserializeStatDeclareGlobal(element, location),
            "AstStatDeclareClass" => DeserializeStatDeclareClass(element, location),
            _ => throw new NotSupportedException($"Unknown node type: {type}")
        };
    }

    private static AstStatError DeserializeStatError(JsonElement element, Location location)
    {
        var expressions = DeserializeExprList(element.GetProperty("expressions"));
        var statements = DeserializeStatList(element.GetProperty("statements"));
        return new AstStatError(location, expressions, statements);
    }

    private static AstStatBlock DeserializeStatBlock(JsonElement element, Location location)
    {
        var body = DeserializeStatList(element.GetProperty("body"));
        var hasEnd = element.GetProperty("hasEnd").GetBoolean();
        return new AstStatBlock(location, body, hasEnd);
    }

    private static AstStatExpr DeserializeStatExpr(JsonElement element, Location location)
    {
        var expression = DeserializeExpr(element.GetProperty("expr"));
        return new AstStatExpr(location, expression);
    }

    private static AstStatIf DeserializeStatIf(JsonElement element, Location location)
    {
        var condition = DeserializeExpr(element.GetProperty("condition"));
        var thenBody = (AstStatBlock)DeserializeNode(element.GetProperty("thenbody"));

        // elsebody can be AstStatBlock (plain else) or AstStatIf (elseif chain)
        AstStat? elseBody = null;
        if (element.TryGetProperty("elsebody", out var elseProp) && elseProp.ValueKind == JsonValueKind.Object)
        {
            elseBody = (AstStat)DeserializeNode(elseProp);
        }

        var hasThen = element.GetProperty("hasThen").GetBoolean();
        return new AstStatIf(location, condition, thenBody, elseBody, hasThen);
    }

    private static AstStatWhile DeserializeStatWhile(JsonElement element, Location location)
    {
        var condition = DeserializeExpr(element.GetProperty("condition"));
        var body = (AstStatBlock)DeserializeNode(element.GetProperty("body"));
        var hasDo = element.GetProperty("hasDo").GetBoolean();
        return new AstStatWhile(location, condition, body, hasDo);
    }

    private static AstStatRepeat DeserializeStatRepeat(JsonElement element, Location location)
    {
        var condition = DeserializeExpr(element.GetProperty("condition"));
        var body = (AstStatBlock)DeserializeNode(element.GetProperty("body"));
        return new AstStatRepeat(location, condition, body);
    }

    private static AstStatBreak DeserializeStatBreak(Location location) => new(location);

    private static AstStatContinue DeserializeStatContinue(Location location) => new(location);

    private static AstStatReturn DeserializeStatReturn(JsonElement element, Location location)
    {
        var list = DeserializeExprList(element.GetProperty("list"));
        return new AstStatReturn(location, list);
    }

    private static AstStatLocal DeserializeStatLocal(JsonElement element, Location location)
    {
        var vars = DeserializeLocalList(element.GetProperty("vars"));
        var values = DeserializeExprList(element.GetProperty("values"));
        return new AstStatLocal(location, vars, values);
    }

    private static AstStatFor DeserializeStatFor(JsonElement element, Location location)
    {
        var var_ = DeserializeLocal(element.GetProperty("var"));
        var from = DeserializeExpr(element.GetProperty("from"));
        var to = DeserializeExpr(element.GetProperty("to"));

        AstExpr? step = null;
        if (element.TryGetProperty("step", out var stepProp) && stepProp.ValueKind == JsonValueKind.Object)
        {
            step = DeserializeExpr(stepProp);
        }

        var body = (AstStatBlock)DeserializeNode(element.GetProperty("body"));
        var hasDo = element.GetProperty("hasDo").GetBoolean();
        return new AstStatFor(location, var_, from, to, step, body, hasDo);
    }

    private static AstStatForIn DeserializeStatForIn(JsonElement element, Location location)
    {
        var vars = DeserializeLocalList(element.GetProperty("vars"));
        var values = DeserializeExprList(element.GetProperty("values"));
        var body = (AstStatBlock)DeserializeNode(element.GetProperty("body"));
        var hasIn = element.GetProperty("hasIn").GetBoolean();
        var hasDo = element.GetProperty("hasDo").GetBoolean();
        return new AstStatForIn(location, vars, values, body, hasIn, hasDo);
    }

    private static AstStatAssign DeserializeStatAssign(JsonElement element, Location location)
    {
        var vars = DeserializeExprList(element.GetProperty("vars"));
        var values = DeserializeExprList(element.GetProperty("values"));
        return new AstStatAssign(location, vars, values);
    }

    private static AstStatCompoundAssign DeserializeStatCompoundAssign(JsonElement element, Location location)
    {
        var op = element.GetProperty("op").GetString()!;
        var var_ = DeserializeExpr(element.GetProperty("var"));
        var value = DeserializeExpr(element.GetProperty("value"));
        return new AstStatCompoundAssign(location, op, var_, value);
    }

    private static AstStatFunction DeserializeStatFunction(JsonElement element, Location location)
    {
        var name = DeserializeExpr(element.GetProperty("name"));
        var func = (AstExprFunction)DeserializeExpr(element.GetProperty("func"));
        return new AstStatFunction(location, name, func);
    }

    private static AstStatLocalFunction DeserializeStatLocalFunction(JsonElement element, Location location)
    {
        var name = DeserializeLocal(element.GetProperty("name"));
        var func = (AstExprFunction)DeserializeExpr(element.GetProperty("func"));
        return new AstStatLocalFunction(location, name, func);
    }

    private static AstStatTypeAlias DeserializeStatTypeAlias(JsonElement element, Location location)
    {
        var name = element.GetProperty("name").GetString()!;
        var generics = DeserializeGenericTypeList(element.GetProperty("generics"));
        var genericPacks = DeserializeGenericTypePackList(element.GetProperty("genericPacks"));
        var value = DeserializeType(element.GetProperty("value"));
        var exported = element.GetProperty("exported").GetBoolean();
        return new AstStatTypeAlias(location, name, generics, genericPacks, value, exported);
    }

    private static AstStatDeclareFunction DeserializeStatDeclareFunction(JsonElement element, Location location)
    {
        var attributes = DeserializeAttrList(element.GetProperty("attributes"));
        var name = element.GetProperty("name").GetString()!;
        var nameLocation = DeserializeLocation(element.GetProperty("nameLocation").GetString()!);
        var params_ = DeserializeTypeList(element.GetProperty("params"));
        var paramNames = DeserializeArgumentNameList(element.GetProperty("paramNames"));

        AstArgumentName? vararg = null;
        if (element.TryGetProperty("vararg", out var varargProp) && varargProp.ValueKind == JsonValueKind.Object)
        {
            vararg = DeserializeArgumentName(varargProp);
        }

        Location? varargLocation = null;
        if (element.TryGetProperty("varargLocation", out var varLocProp) && varLocProp.ValueKind == JsonValueKind.String)
        {
            varargLocation = DeserializeLocation(varLocProp.GetString()!);
        }

        var retTypes = DeserializeTypeList(element.GetProperty("retTypes"));
        var generics = DeserializeGenericTypeList(element.GetProperty("generics"));
        var genericPacks = DeserializeGenericTypePackList(element.GetProperty("genericPacks"));

        return new AstStatDeclareFunction(location, attributes, name, nameLocation, params_, paramNames,
            vararg, varargLocation, retTypes, generics, genericPacks);
    }

    private static AstStatDeclareGlobal DeserializeStatDeclareGlobal(JsonElement element, Location location)
    {
        var name = element.GetProperty("name").GetString()!;
        var nameLocation = DeserializeLocation(element.GetProperty("nameLocation").GetString()!);
        var type = DeserializeType(element.GetProperty("type"));
        return new AstStatDeclareGlobal(location, name, nameLocation, type);
    }

    private static AstStatDeclareClass DeserializeStatDeclareClass(JsonElement element, Location location)
    {
        var name = element.GetProperty("name").GetString()!;

        string? superName = null;
        if (element.TryGetProperty("superName", out var superProp) && superProp.ValueKind == JsonValueKind.String)
        {
            superName = superProp.GetString();
        }

        var props = DeserializeDeclaredClassPropList(element.GetProperty("props"));

        AstTableIndexer? indexer = null;
        if (element.TryGetProperty("indexer", out var idxProp) && idxProp.ValueKind == JsonValueKind.Object)
        {
            indexer = DeserializeTableIndexer(idxProp);
        }

        return new AstStatDeclareClass(location, name, superName, props, indexer);
    }

    private static AstType DeserializeType(JsonElement element)
    {
        var type = element.GetProperty("type").GetString()!;
        var location = DeserializeLocation(element.GetProperty("location").GetString()!);

        return type switch
        {
            "AstTypeReference" => DeserializeTypeReference(element, location),
            "AstTypeTable" => DeserializeTypeTable(element, location),
            "AstTypeFunction" => DeserializeTypeFunction(element, location),
            "AstTypeTypeof" => DeserializeTypeTypeof(element, location),
            "AstTypeOptional" => DeserializeTypeOptional(location),
            "AstTypeUnion" => DeserializeTypeUnion(element, location),
            "AstTypeIntersection" => DeserializeTypeIntersection(element, location),
            "AstTypeError" => DeserializeTypeError(element, location),
            "AstTypeGroup" => DeserializeTypeGroup(element, location),
            "AstTypeSingletonBool" => DeserializeTypeSingletonBool(element, location),
            "AstTypeSingletonString" => DeserializeTypeSingletonString(element, location),
            _ => throw new NotSupportedException($"Unknown type: {type}")
        };
    }

    // AstTypeOptional has no fields beyond location
    private static AstTypeOptional DeserializeTypeOptional(Location location) => new(location);

    private static AstTypeReference DeserializeTypeReference(JsonElement element, Location location)
    {
        string? prefix = null;
        if (element.TryGetProperty("prefix", out var prefixProp) && prefixProp.ValueKind == JsonValueKind.String)
            prefix = prefixProp.GetString();

        Location? prefixLocation = null;
        if (element.TryGetProperty("prefixLocation", out var prefLocProp) && prefLocProp.ValueKind == JsonValueKind.String)
        {
            prefixLocation = DeserializeLocation(prefLocProp.GetString()!);
        }

        var name = element.GetProperty("name").GetString()!;
        var nameLocation = DeserializeLocation(element.GetProperty("nameLocation").GetString()!);
        var parameters = DeserializeTypeOrPackList(element.GetProperty("parameters"));
        return new AstTypeReference(location, prefix, prefixLocation, name, nameLocation, parameters);
    }

    private static AstTypeTable DeserializeTypeTable(JsonElement element, Location location)
    {
        var props = DeserializeTablePropList(element.GetProperty("props"));

        AstTableIndexer? indexer = null;
        if (element.TryGetProperty("indexer", out var idxProp) && idxProp.ValueKind == JsonValueKind.Object)
        {
            indexer = DeserializeTableIndexer(idxProp);
        }

        return new AstTypeTable(location, props, indexer);
    }

    private static AstTypeFunction DeserializeTypeFunction(JsonElement element, Location location)
    {
        var attributes = DeserializeAttrList(element.GetProperty("attributes"));
        var generics = DeserializeGenericTypeList(element.GetProperty("generics"));
        var genericPacks = DeserializeGenericTypePackList(element.GetProperty("genericPacks"));
        var argTypes = DeserializeTypeList(element.GetProperty("argTypes"));
        var argNames = DeserializeArgumentNameList(element.GetProperty("argNames"));
        var returnTypes = DeserializeTypeList(element.GetProperty("returnTypes"));
        return new AstTypeFunction(location, attributes, generics, genericPacks, argTypes, argNames, returnTypes);
    }

    private static AstTypeTypeof DeserializeTypeTypeof(JsonElement element, Location location)
    {
        var expr = DeserializeExpr(element.GetProperty("expr"));
        return new AstTypeTypeof(location, expr);
    }

    // FIX: AstTypeUnion/Intersection/Error write "types" as a raw JSON array, not an AstTypeList object.
    // DeserializeTypeList() expects an object with a "types" key inside it, so it must not be used here.
    private static AstTypeUnion DeserializeTypeUnion(JsonElement element, Location location)
    {
        var types = new List<AstType>();
        foreach (var t in element.GetProperty("types").EnumerateArray())
            types.Add(DeserializeType(t));
        return new AstTypeUnion(location, types);
    }

    private static AstTypeIntersection DeserializeTypeIntersection(JsonElement element, Location location)
    {
        var types = new List<AstType>();
        foreach (var t in element.GetProperty("types").EnumerateArray())
            types.Add(DeserializeType(t));
        return new AstTypeIntersection(location, types);
    }

    private static AstTypeError DeserializeTypeError(JsonElement element, Location location)
    {
        var types = new List<AstType>();
        foreach (var t in element.GetProperty("types").EnumerateArray())
            types.Add(DeserializeType(t));
        var messageIndex = element.GetProperty("messageIndex").GetUInt32();
        return new AstTypeError(location, types, messageIndex);
    }

    private static AstTypeGroup DeserializeTypeGroup(JsonElement element, Location location)
    {
        var innerType = DeserializeType(element.GetProperty("inner"));
        return new AstTypeGroup(location, innerType);
    }

    private static AstTypeSingletonBool DeserializeTypeSingletonBool(JsonElement element, Location location)
    {
        var value = element.GetProperty("value").GetBoolean();
        return new AstTypeSingletonBool(location, value);
    }

    private static AstTypeSingletonString DeserializeTypeSingletonString(JsonElement element, Location location)
    {
        var value = element.GetProperty("value").GetString()!;
        return new AstTypeSingletonString(location, value);
    }

    // Type pack deserialization
    private static AstTypePack DeserializeTypePack(JsonElement element)
    {
        var type = element.GetProperty("type").GetString()!;
        var location = DeserializeLocation(element.GetProperty("location").GetString()!);

        return type switch
        {
            "AstTypePackExplicit" => DeserializeTypePackExplicit(element, location),
            "AstTypePackVariadic" => DeserializeTypePackVariadic(element, location),
            "AstTypePackGeneric" => DeserializeTypePackGeneric(element, location),
            _ => throw new NotSupportedException($"Unknown type pack: {type}")
        };
    }

    private static AstTypePackExplicit DeserializeTypePackExplicit(JsonElement element, Location location)
    {
        var typeList = DeserializeTypeList(element.GetProperty("typeList"));
        return new AstTypePackExplicit(location, typeList);
    }

    private static AstTypePackVariadic DeserializeTypePackVariadic(JsonElement element, Location location)
    {
        var variadicType = DeserializeType(element.GetProperty("variadicType"));
        return new AstTypePackVariadic(location, variadicType);
    }

    private static AstTypePackGeneric DeserializeTypePackGeneric(JsonElement element, Location location)
    {
        var genericName = element.GetProperty("genericName").GetString()!;
        return new AstTypePackGeneric(location, genericName);
    }

    // Helper deserialization methods
    private static AstTypeList DeserializeTypeList(JsonElement element)
    {
        var types = new List<AstType>();
        foreach (var type in element.GetProperty("types").EnumerateArray())
        {
            types.Add(DeserializeType(type));
        }

        AstTypePack? tailType = null;
        if (element.TryGetProperty("tailType", out var tailProp) && tailProp.ValueKind == JsonValueKind.Object)
        {
            tailType = DeserializeTypePack(tailProp);
        }

        return new AstTypeList(types, tailType);
    }

    private static AstTypeOrPack DeserializeTypeOrPack(JsonElement element)
    {
        if (element.TryGetProperty("type", out var typeProp))
        {
            var typeName = typeProp.GetString();
            if (typeName != null && IsAstType(typeName))
            {
                return new AstTypeOrPack(DeserializeType(element), null);
            }
            if (typeName != null && IsAstTypePack(typeName))
            {
                return new AstTypeOrPack(null, DeserializeTypePack(element));
            }
        }
        throw new NotSupportedException("Unknown AstTypeOrPack format");
    }

    private static bool IsAstType(string typeName) => typeName switch
    {
        "AstTypeReference" or "AstTypeTable" or "AstTypeFunction" or "AstTypeTypeof" or
        "AstTypeOptional" or "AstTypeUnion" or "AstTypeIntersection" or "AstTypeError" or
        "AstTypeGroup" or "AstTypeSingletonBool" or "AstTypeSingletonString" => true,
        _ => false
    };

    private static bool IsAstTypePack(string typeName) => typeName switch
    {
        "AstTypePackExplicit" or "AstTypePackVariadic" or "AstTypePackGeneric" => true,
        _ => false
    };

    private static AstTableProp DeserializeTableProp(JsonElement element)
    {
        var name = element.GetProperty("name").GetString()!;
        var location = DeserializeLocation(element.GetProperty("location").GetString()!);
        var propType = DeserializeType(element.GetProperty("propType"));
        return new AstTableProp(name, location, propType);
    }

    private static AstTableIndexer DeserializeTableIndexer(JsonElement element)
    {
        var location = DeserializeLocation(element.GetProperty("location").GetString()!);
        var indexType = DeserializeType(element.GetProperty("indexType"));
        var resultType = DeserializeType(element.GetProperty("resultType"));
        return new AstTableIndexer(location, indexType, resultType);
    }

    private static AstArgumentName DeserializeArgumentName(JsonElement element)
    {
        var name = element.GetProperty("name").GetString()!;
        var location = DeserializeLocation(element.GetProperty("location").GetString()!);
        return new AstArgumentName(name, location);
    }

    private static AstDeclaredClassProp DeserializeDeclaredClassProp(JsonElement element)
    {
        var name = element.GetProperty("name").GetString()!;
        var nameLocation = DeserializeLocation(element.GetProperty("nameLocation").GetString()!);
        var luauType = DeserializeType(element.GetProperty("luauType"));
        var location = DeserializeLocation(element.GetProperty("location").GetString()!);
        return new AstDeclaredClassProp(name, nameLocation, luauType, location);
    }

    private static AstGenericType DeserializeGenericType(JsonElement element)
    {
        var name = element.GetProperty("name").GetString()!;

        AstType? defaultValue = null;
        if (element.TryGetProperty("luauType", out var typeProp) && typeProp.ValueKind == JsonValueKind.Object)
        {
            defaultValue = DeserializeType(typeProp);
        }

        return new AstGenericType(name, defaultValue);
    }

    private static AstGenericTypePack DeserializeGenericTypePack(JsonElement element)
    {
        var name = element.GetProperty("name").GetString()!;

        AstTypePack? defaultValue = null;
        if (element.TryGetProperty("luauType", out var typeProp) && typeProp.ValueKind == JsonValueKind.Object)
        {
            defaultValue = DeserializeTypePack(typeProp);
        }

        return new AstGenericTypePack(name, defaultValue);
    }

    // List deserialization helpers
    private static List<AstExpr> DeserializeExprList(JsonElement element)
    {
        var list = new List<AstExpr>();
        foreach (var item in element.EnumerateArray())
        {
            list.Add(DeserializeExpr(item));
        }
        return list;
    }

    private static List<AstStat> DeserializeStatList(JsonElement element)
    {
        var list = new List<AstStat>();
        foreach (var item in element.EnumerateArray())
        {
            list.Add((AstStat)DeserializeNode(item));
        }
        return list;
    }

    private static List<AstLocal> DeserializeLocalList(JsonElement element)
    {
        var list = new List<AstLocal>();
        foreach (var item in element.EnumerateArray())
        {
            list.Add(DeserializeLocal(item));
        }
        return list;
    }

    private static List<AstAttr> DeserializeAttrList(JsonElement element)
    {
        var list = new List<AstAttr>();
        foreach (var item in element.EnumerateArray())
        {
            list.Add(DeserializeAttr(item));
        }
        return list;
    }

    private static List<AstGenericType> DeserializeGenericTypeList(JsonElement element)
    {
        var list = new List<AstGenericType>();
        foreach (var item in element.EnumerateArray())
        {
            list.Add(DeserializeGenericType(item));
        }
        return list;
    }

    private static List<AstGenericTypePack> DeserializeGenericTypePackList(JsonElement element)
    {
        var list = new List<AstGenericTypePack>();
        foreach (var item in element.EnumerateArray())
        {
            list.Add(DeserializeGenericTypePack(item));
        }
        return list;
    }

    private static List<AstArgumentName?> DeserializeArgumentNameList(JsonElement element)
    {
        var list = new List<AstArgumentName?>();
        foreach (var item in element.EnumerateArray())
        {
            if (item.ValueKind == JsonValueKind.Object)
            {
                list.Add(DeserializeArgumentName(item));
            }
            else
            {
                list.Add(null);
            }
        }
        return list;
    }

    private static List<AstTypeOrPack> DeserializeTypeOrPackList(JsonElement element)
    {
        var list = new List<AstTypeOrPack>();
        foreach (var item in element.EnumerateArray())
        {
            list.Add(DeserializeTypeOrPack(item));
        }
        return list;
    }

    private static List<AstTableProp> DeserializeTablePropList(JsonElement element)
    {
        var list = new List<AstTableProp>();
        foreach (var item in element.EnumerateArray())
        {
            list.Add(DeserializeTableProp(item));
        }
        return list;
    }

    private static List<AstDeclaredClassProp> DeserializeDeclaredClassPropList(JsonElement element)
    {
        var list = new List<AstDeclaredClassProp>();
        foreach (var item in element.EnumerateArray())
        {
            list.Add(DeserializeDeclaredClassProp(item));
        }
        return list;
    }

    // Location parsing
    private static Location DeserializeLocation(string raw)
    {
        var parts = raw.Split(" - ", StringSplitOptions.TrimEntries);
        return new Location(DeserializePosition(parts[0]), DeserializePosition(parts[1]));
    }

    private static Position DeserializePosition(string raw)
    {
        raw = raw.Trim();
        if (raw.StartsWith("(") && raw.EndsWith(")"))
            raw = raw[1..^1];
        var parts = raw.Split(',');
        return new Position(int.Parse(parts[0]), int.Parse(parts[1]));
    }
}