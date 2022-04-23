using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parameter;

namespace B_stock
{
    public partial class Form_major : Form
    {
        oper_emp oper_ = new oper_emp();
        public void Showform(Form form)
        {
            //清除panel里面的其他窗体
            this.panel1.Controls.Clear();
            //将该子窗体设置成非顶级控件
            form.TopLevel = false;
            //将该子窗体的边框去掉
            form.FormBorderStyle = FormBorderStyle.None;
            //设置子窗体随容器大小自动调整
            form.Dock = DockStyle.None;
            //设置mdi父容器为当前窗口
            form.Parent = this.panel1;
            //子窗体显示
            form.Show();
        }

        public Form_major(oper_emp login_emp)
        {
            oper_ = login_emp;
            InitializeComponent();
        }

        private void 入库端ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (oper_.Level>3&&oper_.Level!=4)
            {
                MessageBox.Show("抱歉，您没有操作该界面的权限，请通知主管修改您的权限或者更换账号。","提示");
                return;
            }
            Form_intake form_Intake = new Form_intake(oper_);
            Showform(form_Intake);
        }

        private void 出库端ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (oper_.Level > 3 && oper_.Level != 5)
            {
                MessageBox.Show("抱歉，您没有操作该界面的权限，请通知主管修改您的权限或者更换账号。", "提示");
                return;
            }
            Form_output form_Output = new Form_output(oper_ );
            Showform(form_Output);
        }

        private void 确认端ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (oper_.Level >3 && oper_.Level != 3)
            {
                MessageBox.Show("抱歉，您没有操作该界面的权限，请通知主管修改您的权限或者更换账号。", "提示");
                return;
            }
            Form_cheek form_Cheek = new Form_cheek();
            Showform(form_Cheek);
        }

        private void Form_major_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
