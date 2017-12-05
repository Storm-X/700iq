using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainServer
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class Form2 : Form
    {
        Form1 form1;
        static public List<Role> roleList = new List<Role>();
        private bool flag = false;
        string roleName;
        public Form2()
        {
            InitializeComponent();
            textBox1.PasswordChar = '*';
        }

        public void flush()
        {
            flag = false;
            textBox1.Text = "";
            roleName = "";     
        }
        public void setFrom(Form1 form1)
        {
            this.form1 = form1;
        }

        private void Form2_Load(object sender, EventArgs e)
        { 
            roleList.Add(new Role("Administrator", "admin700iq"));
            roleList.Add(new Role("Manager", "manager700iq"));
            foreach(Role role in roleList)
            {
                comboBox1.Items.Add(role.Login());
            }
            this.Visible = true;
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Role role in roleList)
            {
                if (String.Compare(comboBox1.Text, role.Login(),true) == 0 && String.Compare(textBox1.Text, role.Password()) == 0)
                {
                    flag = true;
                    roleName = role.Login();
                    form1.setRole(roleName);
                    break;
                }
                                               
            }

            if (flag)
            {

                form1.Show();
                this.Hide();
            }
            else MessageBox.Show("Неверный пароль");

        }
    }
}
