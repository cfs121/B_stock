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
    public partial class Form_sever : Form
    {
        //全局通用的变量
        oper_emp oper_Emp = new oper_emp();//全局通用员工属性
        public void getQUEUE()
        {
            MySQL.Select select = new Select();
            textBox2.Text= select.getAllQueue();

        }
        public void getTest()
        {
            MySQL.Select select = new Select();
            textBox4.Text = select.getTask();

        }
        public Form_sever(oper_emp login_emp)
        {
            InitializeComponent();
            oper_Emp = login_emp;

        }

        private void Form_sever_Load(object sender, EventArgs e)
        {
            getQUEUE();
            getTest();
        }
    }
}
