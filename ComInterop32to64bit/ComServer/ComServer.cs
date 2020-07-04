using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ComServer
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("EC23E47B-A6B4-4D83-B692-A68162DD3939")]
    public class ComServer
    {
        public ComServer() { }

        public int Test(string text)
        {
            Debug.WriteLine("Hello World 64bit:\n" + text);

            List<List<string>> memory = new List<List<string>>();

            for (int j = 0; j <= 100000; j++)
            {
                List<string> memory2 = new List<string>();
                memory.Add(memory2);
                for (int i = 0; i <= 1000000; i++)
                {
                    memory2.Add("wefjhdsfkjghsdfjkghedfgkjhdfgkjdfhgkjfdgh!wefjhdsfkjghsdfjkghedfgkjhdfgkjdfhgkjfdgh!wefjhdsfkjghsdfjkghedfgkjhdfgkjdfhgkjfdgh!wefjhdsfkjghsdfjkghedfgkjhdfgkjdfhgkjfdgh!wefjhdsfkjghsdfjkghedfgkjhdfgkjdfhgkjfdgh!wefjhdsfkjghsdfjkghedfgkjhdfgkjdfhgkjfdgh!wefjhdsfkjghsdfjkghedfgkjhdfgkjdfhgkjfdgh!");
                }
            }

            return 6667;
        }
    }
}
