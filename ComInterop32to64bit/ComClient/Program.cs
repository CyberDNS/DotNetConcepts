using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ComClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Type comType = Type.GetTypeFromCLSID(new Guid("EC23E47B-A6B4-4D83-B692-A68162DD3939"));
            object comObject = Activator.CreateInstance(comType);

            string result = (string)comType.InvokeMember("Test", BindingFlags.InvokeMethod, null, comObject, new object[] { "Hello World 64bit" });
            Console.WriteLine($"Result: {result}");

            comType.InvokeMember("Shutdown", BindingFlags.InvokeMethod, null, comObject, new object[0]);

            if (Marshal.IsComObject(comObject)) { Marshal.ReleaseComObject(comObject); }
        }
    }
}
