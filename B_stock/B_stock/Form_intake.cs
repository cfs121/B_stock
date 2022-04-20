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
    public partial class Form_intake : Form
    {
        oper_emp oper_Emp = new oper_emp();

        public void setComblist(List<string> list)
        {
            foreach (var item in list)
            {
                comboBox1.Items.Add(item);
            }
            comboBox1.Text = Convert.ToString(comboBox1.Items[0]);
        }
        public Form_intake(oper_emp login_emp)
        {
            oper_Emp = login_emp;
            InitializeComponent();
            
        }

        private void Form_intake_Load(object sender, EventArgs e)
        {
            Form.CheckForIllegalCrossThreadCalls = false;//允许其它线程
            MySQL.Select select = new Select();
            label_emp.Text = oper_Emp.Emp_number;
            label_device.Text = "";
            select.set();//设置各类基本参数
            List<string> device = new List<string>();
            select.getDeviceNumber("in",oper_Emp.Emp_number,ref device);
            setComblist(device);//初始化设备选择下拉餐单
            //初始化设备信息
            //初始化储位图形
        }
    }
}
