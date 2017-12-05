using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainServer
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form1 = new Form1();
            Form2 form2 = new Form2();
            form2.setFrom(form1);
            form1.setForm(form2);
            form1.FormClosing += Form_FormClosing;
            form2.FormClosing += Form_FormClosing;
            form2.Show();
            Application.Run();
            //Application.Run(new Form2());
       
           
        }

        private static void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
