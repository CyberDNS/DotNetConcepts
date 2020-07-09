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
            Type ComType = Type.GetTypeFromCLSID(new Guid("EC23E47B-A6B4-4D83-B692-A68162DD3939"));
            object ComObject = Activator.CreateInstance(ComType);

            bool bootResult = (bool)ComType.InvokeMember("Boot", BindingFlags.InvokeMethod, null, ComObject, new object[0]);
            Console.WriteLine($"Boot: {bootResult}");

            string result = (string)ComType.InvokeMember("Call", BindingFlags.InvokeMethod, null, ComObject, new object[] { "" });
            Console.WriteLine($"Result: {result}");

            ComType.InvokeMember("Shutdown", BindingFlags.InvokeMethod, null, ComObject, new object[0]);

            if (Marshal.IsComObject(ComObject))
                Marshal.ReleaseComObject(ComObject);
        }
    }
}
