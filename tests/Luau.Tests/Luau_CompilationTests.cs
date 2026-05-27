using Luau.Compilation;

namespace Luau.Tests
{
    public class Luau_CompilationTests
    {
        [Fact]
        public void Compile_ValidChunk_ReturnsBytecode()
        {
            string chunk = "return 123";

            LuauChunk bytecode = LuauCompiler.Compile(chunk);

            Assert.NotNull(bytecode);
            Assert.True(bytecode.Size > 0);
        }

        [Fact]
        public void Compile_EmptyChunk_DoesNotCrash()
        {
            var result = LuauCompiler.Compile("");
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("return 1 + 1")]
        [InlineData("local x = 10")]
        [InlineData("function f() return 1 end")]
        public void Compile_BasicChunks_DoNotThrow(string chunk)
        {
            var result = LuauCompiler.Compile(chunk);
            Assert.NotNull(result);
        }

        [Fact]
        public void Compile_SameInput_ProducesDeterministicOutput()
        {
            var a = LuauCompiler.Compile("return 1 + 1");
            var b = LuauCompiler.Compile("return 1 + 1");

            Assert.Equal(a.Size, b.Size);
        }

        [Fact]
        public void Compile_DifferentInput_ProducesDifferentBytecode()
        {
            var a = LuauCompiler.Compile("return 1");
            var b = LuauCompiler.Compile("return 2");

            Assert.NotEqual(
                a.ToByteArray(),
                b.ToByteArray()
            );
        }

        [Fact]
        public void Compile_ReturnedChunk_IsStillReadableAfterScope()
        {
            LuauChunk chunk;

            {
                chunk = LuauCompiler.Compile("return 1");
            }

            Assert.True(chunk.Size > 0);
        }

        [Fact]
        public void Chunk_Dispose_DoesNotCrash()
        {
            var chunk = LuauCompiler.Compile("return 1");

            chunk.Dispose();
            chunk.Dispose();
        }

        [Fact]
        public void Chunk_ToByteArray_AfterDispose_Throws()
        {
            var chunk = LuauCompiler.Compile("return 1");

            chunk.Dispose();

            Assert.Throws<ObjectDisposedException>(() =>
            {
                chunk.ToByteArray();
            });
        }

        [Fact]
        public void Chunk_AsSpan_AfterDispose_Throws()
        {
            var chunk = LuauCompiler.Compile("return 1");

            chunk.Dispose();

            Assert.Throws<ObjectDisposedException>(() =>
            {
                chunk.AsSpan();
            });
        }

        [Fact]
        public void Compile_LargeChunk_DoesNotCrash()
        {
            string chunk = string.Join(
                '\n',
                Enumerable.Repeat("local x = 1", 10000)
            );

            var result = LuauCompiler.Compile(chunk);

            Assert.True(result.Size > 0);
        }
        [Fact]
        public void Compile_EmptyChunk_ToByteArray_Works()
        {
            var chunk = LuauCompiler.Compile("");

            var bytes = chunk.ToByteArray();

            Assert.NotNull(bytes);
        }
    }
}