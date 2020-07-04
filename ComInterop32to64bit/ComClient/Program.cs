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
            Type ComType = Type.GetTypeFromProgID("ComServer.ComServer");

            object ComObject = Activator.CreateInstance(ComType);

            object[] methodArgs = new object[1];
            methodArgs[0] = "Test Arg";
            int result = (int)ComType.InvokeMember("Test",
                                                   BindingFlags.InvokeMethod, null,
                                                   ComObject, methodArgs);
            Console.WriteLine("Result: " + result.ToString());

            if (Marshal.IsComObject(ComObject))
                Marshal.ReleaseComObject(ComObject);
        }
    }
}
