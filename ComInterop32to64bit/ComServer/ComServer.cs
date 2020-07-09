using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ComServer
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("EC23E47B-A6B4-4D83-B692-A68162DD3939")]
    public class ComServer
    {
        public bool Boot()
        {
            MessageBox.Show("Boot");

            if (Application.Current == null)
            {
                Thread thread = new Thread(() => StaBoot());
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }

            return SpinWait.SpinUntil(() => Application.Current != null, TimeSpan.FromSeconds(10));
        }

        private void StaBoot()
        {
            if (Application.Current == null)
            {
                Application currentApplication = new Application();
                currentApplication.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                currentApplication.Run();
            }
        }



        public string Call(string jsonArgs)
        {
            return Application.Current.Dispatcher.Invoke(() => StaCall(jsonArgs));
        }

        private string StaCall(string jsonArgs)
        {
            MessageBox.Show(Thread.CurrentThread.GetApartmentState().ToString());

            return Thread.CurrentThread.GetApartmentState().ToString();
        }



        public void Shutdown()
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
            }
        }


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
                    memory2.Add("abcdefghijklmnopqrstuvwxyz!abcdefghijklmnopqrstuvwxyz!abcdefghijklmnopqrstuvwxyz!abcdefghijklmnopqrstuvwxyz!abcdefghijklmnopqrstuvwxyz!abcdefghijklmnopqrstuvwxyz!abcdefghijklmnopqrstuvwxyz!abcdefghijklmnopqrstuvwxyz!abcdefghijklmnopqrstuvwxyz!");
                }
            }

            return 6667;
        }
    }
}
