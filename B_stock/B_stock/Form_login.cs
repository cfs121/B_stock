using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySQL;
using Parameter;

namespace B_stock
{
    public partial class Form_login : Form
    {
        public Form_login()
        {
            InitializeComponent();
            textBox1.Text = "2112002329";
            textBox2.Text = "12345678";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            cheek();
        }
        internal void cheek()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            Select sql = new MySQL.Select();
            int rest = sql.log_in_cheek(textBox1.Text, textBox2.Text, ref list);
            if (rest == 0)
            {
                oper_emp oper_Emp = new oper_emp();
                oper_Emp.Emp_number = list["Emp_number"];
                oper_Emp.Name = list["Name"];
                oper_Emp.Level = Convert.ToInt32(list["Level"]);
                Form_major form_Major = new Form_major (oper_Emp);
                form_Major.Show();//打开窗体
                this.Visible = false;//隐藏该窗体
            }
            if (rest == 1)
            {
                MessageBox.Show("密码错误！", "提示");
                textBox1.Text = "";
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (textBox1.Text == null || textBox1.Text.Length == 0)
                {
                    MessageBox.Show("账号不能为空！", "错误");
                }
                else
                {
                    int l = textBox1.Text.Length;
                    if (l > 15)
                    {

                        MessageBox.Show("账号不能长于15位", "提示");
                    }
                    else textBox2.Focus();
                }

            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox2.Text == null || textBox2.Text.Length == 0)
                {
                    MessageBox.Show("密码不能为空！", "错误");
                }
                else
                {
                    int l = textBox2.Text.Length;
                    if (l > 15)
                    {
                        MessageBox.Show("密码不能长于15位", "提示");
                    }
                    else cheek();
                }


            }
        }

        
    }
}
