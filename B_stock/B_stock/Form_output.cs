﻿using System;
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
using System.Reflection;

namespace B_stock
{
    public partial class Form_output : Form
    {
        //界面初始化所用的系列函数

          //全局通用的变量
        oper_emp oper_Emp = new oper_emp();
        Device nowDevice = new Device();
         
        List<int> shelfList = new List<int>();
        Dictionary<int, shelf> shelfDic = new Dictionary<int, shelf>();

        List<PictureBox> pictureList = new List<PictureBox>();
        List<int> InquiredShelf = new List<int>();
        private static bool inquired;
        private static bool Inquired
        {
            get { return inquired; }
            set { inquired = value; }
        }
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

        /// <summary>
        /// 重新绘画对应存储情况
        /// </summary>
        /// <param name="shelf_number"></param>
        public void rePicture(int shelf_number)
        {
            MySQL.Select select = new Select();
            GDI.PrintWhitFont printWhitFont = new PrintWhitFont();
            

            List<string> coods = new List<string>();
            List<int> length = new List<int>();
            List<int> width = new List<int>();
            int index = shelfList.FindIndex(item => item.Equals(shelf_number));
               
            select.getStorageForPrint(shelfList[index], ref coods, ref length, ref width);
            
            pictureList[index].Image = printWhitFont.StoreMap(shelfDic[shelfList[index]],
                pictureList[index], coods, length, width);

            
        }

        internal void loadList()
        {
            pictureList.Add(pictureBox1);
            pictureList.Add(pictureBox2);
            pictureList.Add(pictureBox3);
            pictureList.Add(pictureBox4);
            labels.Add(label8);
            labels.Add(label10);
            labels.Add(label11);
            labels.Add(label12);
            for (int i = 0; i < labels.Count; i++)
            {
                labels[i].Text = "";
            }
        }

       

        private void loadEvent()
        {
            this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
            
            for (int i = 0; i < shelfList.Count; i++)
            {
                pictureList[i].MouseClick+= new System.Windows.Forms.MouseEventHandler(this.box_MouseClick);
            }
        }
        private void reEvent()
        {

            for (int i = 0; i < pictureList.Count; i++)
            {
                ClearEvent(pictureList[i], "MouseClick");
            }
            for (int i = 0; i < shelfList.Count; i++)
            {
                pictureList[i].MouseClick += new System.Windows.Forms.MouseEventHandler(this.box_MouseClick);
            }
        }

        public void reSetShelf()
        {

            MySQL.Select select = new Select();
            
            shelfDic.Clear();
            shelfList.Clear();
            select.setShelfList("Device_out_number", nowDevice.Device_number, ref shelfList, ref shelfDic);
        }
        public void initPicture()
        {
            MySQL.Select select = new Select();
            GDI.PrintWhitFont printWhitFont = new PrintWhitFont();
          
            
            List<string> coods = new List<string>();
            List<int> length = new List<int>();
            List<int> width = new List<int>();
            //for (int i = 0; i < labels.Count; i++)
            //{
            //    labels[i].Text = "";
            //}
            for (int i = 0; i <pictureList.Count; i++)
            {
               


                if (i>=shelfList.Count)
                {
                    //不存在的货架控件不予显示
                    pictureList[i].Visible = false;
                    labels[i].Text = "";
                    
                }
                else
                {
                    coods.Clear();
                    length.Clear();
                    width.Clear();
                    select.getStorageForPrint(shelfList[i], ref coods, ref length, ref width);
                    //初始化抬头
                    labels[i].Text = shelfDic[shelfList[i]].ShelfName;
                    //初始化图像
                    pictureList[i].Visible = true;
                    pictureList[i].Image = printWhitFont.StoreMap(shelfDic[shelfList[i]],
                      pictureList[i], coods, length, width);
                }
            }

        }
        public void initDataGrid()
        {
            DataGridViewCheckBoxColumn ChCol = new DataGridViewCheckBoxColumn();
            ChCol.Name = "CheckBoxRow";
            ChCol.HeaderText = "选择";
            ChCol.Width = 50;
            ChCol.TrueValue = "1";
            ChCol.FalseValue = "0";
            dataGridView1.Columns.Insert(0, ChCol);


        }
        public void setDataGrid()
        {

            MySQL.Select select = new Select();
            DataSet dataSet = new DataSet();
            if (select.takeOrders(nowDevice, "out", out dataSet))
            {
                dataGridView1.DataSource = dataSet.Tables[0];

            }
            else
            {
                MessageBox.Show("获取排单信息失败", "提示");
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

        //*****************************************************************************************************************************************//
        //辅助函数
        /// <summary>
        /// 删除指定控件的指定事件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="eventname"></param>
        public void ClearEvent(System.Windows.Forms.Control control, string eventname)
        {
            if (control == null) return;
            if (string.IsNullOrEmpty(eventname)) return;

            BindingFlags mPropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
            BindingFlags mFieldFlags = BindingFlags.Static | BindingFlags.NonPublic;
            Type controlType = typeof(System.Windows.Forms.Control);
            PropertyInfo propertyInfo = controlType.GetProperty("Events", mPropertyFlags);
            EventHandlerList eventHandlerList = (EventHandlerList)propertyInfo.GetValue(control, null);
            FieldInfo fieldInfo = (typeof(System.Windows.Forms.Control)).GetField("Event" + eventname, mFieldFlags);
            Delegate d = eventHandlerList[fieldInfo.GetValue(control)];

            if (d == null) return;
            EventInfo eventInfo = controlType.GetEvent(eventname);

            foreach (Delegate dx in d.GetInvocationList())
                eventInfo.RemoveEventHandler(control, dx);

        }



        //********************************************************************************************************************************//
        //各类界面事件

 /// <summary>
 /// pictureBOX的通用点击事件
 /// </summary>
 /// <param name="sender"></param>
 /// <param name="e"></param>
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
            nowDevice = de_;
            //初始化三个列表并初始化储位图形
            Inquired = false;
            loadList();
            reSetShelf();
            initPicture();

            //列表初始化后再增加事件函数
            loadEvent();
            //初始化排单
            initDataGrid();
            //填入数据
            setDataGrid();
            //初始化队列信息
            textBox2.Text = select.getqueue(nowDevice);

        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            MySQL.Select select = new Select();
            Device de_ = new Device();
            de_.Device_number = comboBox1.Text;
            select.getDeviceInfor(de_.Device_number, ref de_);
            setDevice(de_);
            nowDevice = de_;
            //初始化三个列表并初始化储位图形
            reSetShelf();
            initPicture();
          
            reEvent();  //重新初始化事件函数
            //初始化排单
            setDataGrid();
            //清空之前留下的信息
            textBox3.Text = "";
            textBox4.Text = "";
            //初始化队列信息
            textBox2.Text = select.getqueue(nowDevice);


        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox4.Text=="")
            {
                MessageBox.Show("订单信息不能为空！！！","错误");
                return;
            }
            MySQL.Select select = new Select();
            GDI.PrintWhitFont print = new PrintWhitFont();
            string inf = "";
            string deviceIn = "";
            string desviceOut = "";
            //验证信息是否正确
            if (select.FindOrderInfro(textBox4.Text, ref inf, ref deviceIn, ref desviceOut))
            {
                if (desviceOut == nowDevice.Device_number)
                {
                    textBox3.Text = inf;
                    //标记对应图片
                    List<int> s_list = new List<int>();
                    if (select.findAll(textBox4.Text, ref s_list) == false)
                    {
                        MessageBox.Show("目前登录的设备号所属的存储区内，暂无该单号的货物" +
                            "\n" +
                            "请检查单号是否正确或者原料还未生产完成", "提示");
                        return;
                    }


                    

                }
                else
                {
                    MessageBox.Show("该订单不是该设备生产，请检查您是否输入错误" +
                    "\n" +
                    "或咨询主管是否数据录入错误", "提示");
                    return;
                }

            }
            else
            {
                MessageBox.Show("数据库中无此单号的订单，请检查您是否输入错误" +
                    "\n" +
                    "或咨询主管是否数据录入错误", "提示");
                return;
            }

            //请求写入数据库
            MySQL.Insert insert = new Insert();
            if (insert.insertToEnqueue(nowDevice, textBox4.Text, "out", 3, oper_Emp))
            {
                MessageBox.Show("信息进入队列成功","提示");
                textBox2.Text = select.getqueue(nowDevice);
            }
            else
            {
                MessageBox.Show("信息进入队列失败", "提示");
            }

            
            //发送请求


        }
        /// <summary>
        /// 查询货物位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            MySQL.Select select = new Select();
            GDI.PrintWhitFont print = new PrintWhitFont();
            string inf = "";
            string deviceIn="";
            string desviceOut="";
            if (Inquired == false)
            {
                if (select.FindOrderInfro(textBox4.Text, ref inf, ref deviceIn, ref desviceOut))
                {
                    if (desviceOut == nowDevice.Device_number)
                    {
                        textBox3.Text = inf;
                        //标记对应图片
                        List<int> s_list = new List<int>();
                        if (select.findAll(textBox4.Text,ref s_list)==false)
                        {
                            MessageBox.Show("目前登录的设备号所属的存储区内，暂无该单号的货物" +
                                "\n" +
                                "请检查单号是否正确或者原料还未生产完成","提示");
                        }
                        InquiredShelf = s_list;
                        Inquired = true;

                        List<string> coods = new List<string>();
                        List<int> length = new List<int>();
                        List<int> width = new List<int>();
                        for (int i = 0; i < s_list.Count; i++)
                        {
                            coods.Clear();
                            length.Clear();
                            width.Clear();
                            select.getStorageForPrint(s_list[i], ref coods, ref length, ref width);

                            int index = shelfList.FindIndex(item => item.Equals(s_list[i]));
                            //初始化图像
                            pictureList[index].Image = print.StoreMap(shelfDic[shelfList[index]],pictureList[index],textBox4.Text,coods,length,width);

                        }

                    }
                    else
                    {
                        MessageBox.Show("该订单不是该设备生产，请检查您是否输入错误" +
                        "\n" +
                        "或咨询主管是否数据录入错误", "提示");
                    }

                }
                else
                {
                    MessageBox.Show("数据库中无此单号的订单，请检查您是否输入错误" +
                        "\n" +
                        "或咨询主管是否数据录入错误", "提示");
                }
            }
            else
            {
                foreach (var item in InquiredShelf)
                {
                    rePicture(item);
                }
                InquiredShelf = null;
                Inquired = false;
                textBox3.Text = ""  ;
            }

           
        }
        /// <summary>
        /// 订单号输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')//扫描枪键入或者手工输入直至回车
            {
                e.Handled = true;//执行该事件
                MySQL.Select select = new Select();
                string inf = "";
                if (select.FindOrderInfro(textBox4.Text, ref inf))
                {
                    textBox3.Text = inf;
                }
                else
                {
                    MessageBox.Show("数据库中无此单号的订单，请检查您是否输入错误" +
                        "\n" +
                        "或咨询主管是否数据录入错误","提示");
                }

            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MySQL.Select select = new Select();
            string inf = "";
            if (select.FindOrderInfro(this.dataGridView1.CurrentRow.Cells[1].Value.ToString(), ref inf))
            {
                textBox4.Text = this.dataGridView1.CurrentRow.Cells[1].Value.ToString();
                textBox3.Text = inf;
            }
            else
            {
                MessageBox.Show("数据库中无此单号的订单，请检查您是否输入错误" +
                    "\n" +
                    "或咨询主管是否数据录入错误", "提示");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            setDataGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> order_list = new List<string>();
            List<string> des_name = new List<string>();
            string st_infro = "";
            //得到勾选序列
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((bool)dataGridView1.Rows[i].Cells[0].EditedFormattedValue == true)
                {
                    order_list.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    des_name.Add(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    st_infro = st_infro + "订单尾号后4位： " +Convert.ToString(order_list[i]).Substring(order_list[i].Length- 4, 4)+
                      "   "+ "品名： "+ des_name[i]+"\n";
                    dataGridView1.Rows[i].Cells[0].Value = false;
                }
            }
            if (order_list.Count==0)
            {
                MessageBox.Show("请至少在排单表中勾选一种订单号！！！","提示");
                return;
            }
            //再次确认
            DialogResult result= MessageBox.Show("您选择的是："+"\n"+st_infro,"提示",MessageBoxButtons.OKCancel);
            if (result== DialogResult.OK)
            {

            }
            else
            {
                return;
            }
            //后台确认可行序列
            string inf = "";
            string deviceIn = "";
            string desviceOut = "";
            st_infro = "";
            List<int> err_lisr_index = new List<int>();
            MySQL.Select select = new Select();
            for (int i = 0; i < order_list.Count; i++)
            {
               
                //验证信息是否正确
                if (select.FindOrderInfro(order_list[i], ref inf, ref deviceIn, ref desviceOut))
                {
                    if (desviceOut == nowDevice.Device_number)
                    {
                        
                        //标记对应图片
                        List<int> s_list = new List<int>();
                        if (select.findAll(order_list[i], ref s_list) == false)
                        {
                            st_infro=st_infro+ order_list[i]+": "+"目前登录的设备号所属的存储区内，暂无该单号的货物" +
                                "\n" +
                                "请检查单号是否正确或者原料还未生产完成"+"\n";
                            err_lisr_index.Add(i);
                            
                        }


                    }
                    else
                    {
                        st_infro = st_infro + order_list[i] + ": "+"该订单不是该设备生产，请检查您是否输入错误" +
                        "\n" +
                        "或咨询主管是否数据录入错误"+"\n";
                        err_lisr_index.Add(i);
                    }

                }
                else
                {
                    st_infro = st_infro + order_list[i] + ": "+"数据库中无此单号的订单，请检查您是否输入错误" +
                        "\n" +
                        "或咨询主管是否数据录入错误"+"\n";
                    err_lisr_index.Add(i);

                }
            }
            //请求写入数据库
            MySQL.Insert insert = new Insert();
            for (int i = 0; i < order_list.Count; i++)
            {
                if (err_lisr_index.Exists(item=>item==i))
                {
                    continue;
                }
                if (insert.insertToEnqueue(nowDevice, order_list[i], "out", 3, oper_Emp))
                {
                    st_infro = st_infro + order_list[i] + ": "+"信息进入队列成功" +"\n";
                }
                else
                {
                    st_infro = st_infro + order_list[i] + ": " + "信息进入队列失败" + "\n";
                }
            }
            MessageBox.Show(st_infro,"提示");
            textBox2.Text = select.getqueue(nowDevice);

            //发送请求


        }
    }

   
}
