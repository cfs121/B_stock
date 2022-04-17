using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace B_stock
{
    public partial class Form_major : Form
    {
        public void Showform(Form form)
        {
            //清除panel里面的其他窗体
            this.panel1.Controls.Clear();
            //将该子窗体设置成非顶级控件
            form.TopLevel = false;
            //将该子窗体的边框去掉
            form.FormBorderStyle = FormBorderStyle.None;
            //设置子窗体随容器大小自动调整
            form.Dock = DockStyle.Fill;
            //设置mdi父容器为当前窗口
            form.Parent = this.panel1;
            //子窗体显示
            form.Show();
        }

        public Form_major()
        {
            InitializeComponent();
        }

        private void 入库端ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_intake form_Intake = new Form_intake();
            Showform(form_Intake);
        }

        private void 出库端ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_output form_Output = new Form_output();
            Showform(form_Output);
        }

        private void 确认端ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_cheek form_Cheek = new Form_cheek();
            Showform(form_Cheek);
        }
    }
}
