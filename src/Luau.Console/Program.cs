using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Luau.Native;

class Program {
    static void Main(string[] args) {
        LuaState L = new(NativeMethods.luaL_newstate());

        try {
            L.OpenLibraries();

            // Awesome stuff here.
        } catch (Exception ex) {
            Console.WriteLine($"Error: {ex.Message}");
        } finally {
            L.Close();
        }
    }
};