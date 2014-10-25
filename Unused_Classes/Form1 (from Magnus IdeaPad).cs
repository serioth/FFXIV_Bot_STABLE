using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unused_Classes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            StructExample s;
            s.price = 55.5M;
            Console.WriteLine("STRUCT: " + s.price);

            TestClass c = new TestClass();
            c.author = "me";
            Console.WriteLine("CLASS: " + c.author);

            int i = new int();
        }
    }
}
