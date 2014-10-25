using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unused_Classes
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        //if (MemoryApi.GetAsyncKeyState((int)MemoryApi.KeyCode.KEY_A) != 0)
        //{
        //    Console.WriteLine("Stopped Manual - Keypress");
        //    MemoryHandler.PressKey(MemoryApi.KeyCode.KEY_A, false);
        //}
    }
}
