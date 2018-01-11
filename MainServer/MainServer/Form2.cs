using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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
        MySqlConnection mycon;
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
            string myConnectionString = "Data Source=178.172.150.251; Port=27999; Database=iqseven_test; UserId=700iqby; Password=uLCUrohCLoPUcedI; Character Set=utf8;";
            mycon = new MySqlConnection(myConnectionString);
            mycon.Open();
            MySqlCommand cm = new MySqlCommand("SELECT login FROM role", mycon);
            MySqlDataReader rd = cm.ExecuteReader();
            DataTable rol = new DataTable();
            using (rd)
            {
                if (rd.HasRows) rol.Load(rd);
            }
            //var stringArr = rol.AsEnumerable().Select(r => r.Field<string>("login")).ToArray();
            comboBox1.DataSource = rol; //stringArr;
            comboBox1.DisplayMember = "login";
            this.Visible = true;
            this.Show();
        }
        static string getSHAHash(string input)//хеш пароля
        {
            SHA256 md5Hasher = SHA256.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommand cm = new MySqlCommand("SELECT * FROM role Where password = '" + getSHAHash(textBox1.Text) + "' and login = '" + comboBox1.Text +"'", mycon);
            MySqlDataReader rd = cm.ExecuteReader();
            DataTable rol = new DataTable();
            using (rd)
            {
                if (rd.HasRows)
                {
                    flag = true;
                    roleName = comboBox1.Text;
                    form1.setRole(roleName);
                }
            }
            if (flag)
            {
                form1.Show();
                this.Hide();
            }
            else MessageBox.Show("Неверный пароль");
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.SendWait("{TAB}");
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1.PerformClick();
        }
    }
}
