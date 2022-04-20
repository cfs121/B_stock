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
using GDI;


namespace B_stock
{
    public partial class Form_output : Form
    {
        //界面初始化所用的系列函数
        oper_emp oper_Emp = new oper_emp();

        List<int> shelfList = new List<int>();
        Dictionary<int, shelf> shelfDic = new Dictionary<int, shelf>();

        List<PictureBox> pictureList = new List<PictureBox>();


        List<Label> labels = new List<Label>();
        
        public void setComblist(List<string> list)
        {
            foreach (var item in list)
            {
                comboBox1.Items.Add(item);
            }
            comboBox1.Text =Convert.ToString( comboBox1.Items[0]);
        }
        public void setDevice(Device device)
        {
            label_access.Text = device.Access.ToString();
            label_device.Text = device.Name;
             
        }

        public void rePicture()
        {

        }
        public void loadList(string number)
        {
            MySQL.Select select = new Select();
            GDI.Print print = new Print();
            select.setShelfList("Device_out_number",number,ref shelfList,ref shelfDic);
            pictureList.Add(pictureBox1);
            pictureList.Add(pictureBox2);
            pictureList.Add(pictureBox3);
            pictureList.Add(pictureBox4);
            labels.Add(label8);
            labels.Add(label10);
            labels.Add(label11);
            labels.Add(label12);
            for (int i = 0; i < shelfDic.Count; i++)
            {
                //初始化图像
            }

        }








        //********************************************************************************************************************************//
        //各类界面事件
        public Form_output(oper_emp login_emp)
        {
            oper_Emp = login_emp;
            InitializeComponent();
        }

        private void Form_output_Load(object sender, EventArgs e)
        {
            Form.CheckForIllegalCrossThreadCalls = false;//允许其它线程
            MySQL.Select select = new Select();
            select.set();//设置各类基本参数
            label_emp.Text = oper_Emp.Emp_number;
            //初始化设备选择下拉餐单
            List<string> device = new List<string>();
            select.getDeviceNumber("out", oper_Emp.Emp_number, ref device);
            setComblist(device);
            //初始化设备信息
            Device de_ = new Device();
            de_.Device_number = device[0];
            select.getDeviceInfor(de_.Device_number,ref de_);
            setDevice(de_);
            //初始化三个列表并初始化储位图形
            

        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            MySQL.Select select = new Select();
            Device de_ = new Device();
            de_.Device_number = comboBox1.Text;
            select.getDeviceInfor(de_.Device_number, ref de_);
            setDevice(de_);
        }
    }
}
