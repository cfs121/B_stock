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
            //初始一下子窗体大小
            form.Size = this.panel1.Size;
            //子窗体显示
            form.Show();
        }

        public Form_major(oper_emp login_emp)
        {
            oper_ = login_emp;
            InitializeComponent();
            //获得当前窗口大小信息
            x = this.Width;
            y = this.Height;
            setTag(this);
        }
        #region 控件大小随窗体大小等比例缩放
        private float x;//定义当前窗体的宽度
        private float y;//定义当前窗体的高度
        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }
        private void setControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                //获取控件的Tag属性值，并分割后存储字符串数组
                if (con.Tag != null)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
                    //根据窗体缩放的比例确定控件的值
                    con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);//宽度
                    con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);//高度
                    con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);//左边距
                    con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);//顶边距
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / x;
            float newy = (this.Height) / y;
            setControls(newx, newy, this);
        }

        #endregion
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

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Form test = new test();
            test.Show();
        }

        private void 服务端ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_sever form_Sever = new Form_sever(oper_);
            Showform(form_Sever);
        }

        private void Form_major_Load(object sender, EventArgs e)
        {
            //窗体最大化
            
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;  //显示任务栏
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
