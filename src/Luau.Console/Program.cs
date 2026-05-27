using System;
using System.Text;
using Luau;

class Program
{
    static void Main(string[] args)
    {
        using var state = new Luau.Luau();

        try
        {
            state.OpenLibraries();
            state.State.Sandbox();

            state.DoString(File.ReadAllText("input.luau"));

        }
        catch (Luau.LuauException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}