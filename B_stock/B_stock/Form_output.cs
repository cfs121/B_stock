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
using System.Net;
using System.Net.Sockets;
using System.Threading;


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
        /// <summary>
        /// 建立通讯的socket
        /// </summary>
        Socket socketSent;
        Thread socketTH;


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
            GDI.PrintWhitFont printWhitFont = new PrintWhitFont();
            select.setShelfList("Device_out_number",number,ref shelfList,ref shelfDic);
            pictureList.Add(pictureBox1);
            pictureList.Add(pictureBox2);
            pictureList.Add(pictureBox3);
            pictureList.Add(pictureBox4);
            labels.Add(label8);
            labels.Add(label10);
            labels.Add(label11);
            labels.Add(label12);
            for (int i = 0; i < 4; i++)
            {
                labels[i].Text = "";
            }
            List<string> coods = new List<string>();
            List<int> length = new List<int>();
            List<int> width = new List<int>();
            for (int i = 0; i < shelfDic.Count; i++)
            {
                coods.Clear();
                length.Clear();
                width.Clear();
                select.getStorageForPrint(shelfList[i],ref coods,ref length,ref width);
                //初始化抬头
                labels[i].Text = shelfDic[shelfList[i]].ShelfName;
                //初始化图像
                pictureList[i].Image= printWhitFont.StoreMap(shelfDic[shelfList[i]], 
                  pictureList[i],coods,length,width);
                
            }

        }


//***********************************************************************************************************************************//
//与stocket通讯相关的函数
        private void startSocket(string sever_addres,int COM)
        {
            //创建负责连接的socket
            try
            {

                socketSent = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //获取ip地址
                IPAddress ip = IPAddress.Parse(sever_addres);
                //创建端口号
                IPEndPoint point = new IPEndPoint(ip, COM);
                //设置远程服务器的IP地址和端口号
                socketSent.Connect(point);
                showstate("连接成功");

                socketTH = new Thread(recive);
                socketTH.IsBackground = true;
                socketTH.Start();


            }
            catch (Exception ex)
            {
                showstate("连接失败");
                MessageBox.Show(ex.ToString(),"错误");
                //记录错误信息

            }

        }
        private void recive()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    int i = socketSent.Receive(buffer);
                    if (i == 0)
                    {
                        break;
                    }
                    string str = Encoding.UTF8.GetString(buffer, 0, i);
                    
                }
            }
            catch
            {


            }

        }
        private void showstate(string text)
        {
            label14.Text = text;
        }





        //********************************************************************************************************************************//
        //各类界面事件


        private void box_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox sentPictureBox = sender as PictureBox;
            shelf shelf = new shelf();
            MySQL.Select select = new Select();
            int i = pictureList.FindIndex(item=>item.Equals(sentPictureBox));
            shelf = shelfDic[shelfList[i]];
            string strinlist = "";
            
            if (select.findForClick(ref strinlist, sentPictureBox, shelf, e.Y))
            {
                MessageBox.Show(strinlist);
            }
            
            

        }
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
            //连接主控端
            //string add = "";
            //int com=0;
            //select.getSeverInfor(ref add,ref com);
            //startSocket(add,com);

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
            loadList(de_.Device_number);

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
