using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
    public partial class Form_intake : Form
    {
        //界面初始化所用的系列函数

        //全局通用的变量
        oper_emp oper_Emp = new oper_emp();//全局通用员工属性
        Device nowDevice = new Device();//全局通用设备属性
        //存储货架的基本信息
        List<int> shelfList = new List<int>();
        Dictionary<int, shelf> shelfDic = new Dictionary<int, shelf>();
        //与图形化显示相关
        List<PictureBox> pictureList = new List<PictureBox>();
        List<int> InquiredShelf = new List<int>();
        //记录已经被点击到的订单号
        List<string> orderNumber_list = new List<string>();
        DataTable waitSetTable = new DataTable();
        //用于查询的注销
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
            comboBox1.Text = Convert.ToString(comboBox1.Items[0]);
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
            pictureList.Add(pictureBox5);
            pictureList.Add(pictureBox6);
            pictureList.Add(pictureBox7);
            pictureList.Add(pictureBox8);
            pictureList.Add(pictureBox9);
            pictureList.Add(pictureBox10);
            pictureList.Add(pictureBox11);
            pictureList.Add(pictureBox12);
         
            labels.Add(label8);
            labels.Add(label9);
            labels.Add(label10);
            labels.Add(label11);
            labels.Add(label12);
            labels.Add(label13);
            labels.Add(label14);
            labels.Add(label15);
            labels.Add(label16);
            labels.Add(label17);
            labels.Add(label18);
            labels.Add(label19);

            for (int i = 0; i < labels.Count; i++)
            {
                labels[i].Text = "";
            }
        }
        private void loadEvent()
        {
            this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);

            for (int i = 0; i < shelfList.Count; i++)
            {
                pictureList[i].MouseClick += new System.Windows.Forms.MouseEventHandler(this.box_MouseClick);
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
            select.setShelfList("Device_in_number", nowDevice.Device_number, ref shelfList, ref shelfDic);
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
            for (int i = 0; i < pictureList.Count; i++)
            {

                if (i >= shelfList.Count)
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
        //***********************************************************************************************************************************//
        //与stocket通讯相关的函数
        private void startSocket(string sever_addres, int COM)
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
                label20.Text = "连接成功";

                socketTH = new Thread(recive);
                socketTH.IsBackground = true;
                socketTH.Start();

            }
            catch (Exception ex)
            {
                label20.Text = "连接失败";
                MessageBox.Show(ex.ToString(), "错误");
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

        //*********************************************************************************************************************************************//
        //线程中用到的函数
        //public int race()
        //{
        //    int res = plc_con.isEmpty();//防止在进入该函数前有设备写入设备号
        //    if (res == 0)//再次检查是否为空
        //    {
        //        plc_con.Write();//写入设备号
        //        //延时1秒
        //        res = plc_con.Read();//再次读取设备号，看是否一致
        //        if (true)
        //        {
        //            return 0;
        //        }
        //        else
        //        {
        //            return 1;
        //        }

        //    }
        //    else if (res == 1)
        //    {
        //        return 1;
        //    }
        //    else
        //    {
        //        return res;
        //    }





        //}
        //public int raceToControl()
        //{
        //    MySQL.Select select = new Select();


        //    while (true)
        //    {
        //        Order order_ = new Border();
        //        if (select.getOneQueue(nowDevice, ref order_) == 0 &&
        //          plc_con.isEmpty() == 0 && order.IsAlive)//队列为空，PLC不为空，线程order在执行
        //        {
        //            //挂起30秒
        //            continue;
        //        }
        //        else//尝试抢占通道
        //        {
        //            if (true)//抢占成功
        //            {
        //                int res = race();

        //                if (res == 0)
        //                {

        //                }
        //                else if (res == 1) continue;
        //                else
        //                {
        //                    //显示错误代码
        //                }
        //                order = new Thread(() => { });
        //                order.IsBackground = true;
        //                order.Start();
        //            }
        //            else//抢占失败
        //            {
        //                //挂起30秒   
        //                continue;
        //            }

        //        }

        //    }
        //}

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
            int i = pictureList.FindIndex(item => item.Equals(sentPictureBox));
            shelf = shelfDic[shelfList[i]];
            string strinlist = "";

            if (select.findForClick(ref strinlist, sentPictureBox, shelf, e.Y))
            {
                MessageBox.Show(strinlist);
            }



        }

        public Form_intake(oper_emp login_emp)
        {
            oper_Emp = login_emp;
            InitializeComponent();
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
        private void Form_intake_Load(object sender, EventArgs e)
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
            select.getDeviceNumber("in", oper_Emp.Emp_number, ref device);
            setComblist(device);
            //初始化设备信息
            Device de_ = new Device();
            de_.Device_number = device[0];
            select.getDeviceInfor(de_.Device_number, ref de_);
            setDevice(de_);
            nowDevice = de_;
            //初始化三个列表并初始化储位图形
            Inquired = false;
            loadList();
            reSetShelf();
            initPicture();
            //第二次更改
            //列表初始化后再增加事件函数
            loadEvent();
            //初始化排单
            initDataGrid();
            //填入数据
            setDataGrid();
            //初始化队列信息
            textBox2.Text = select.getqueue2(nowDevice);
        }
        public void initDataGrid()
        {
            //增加单选列表
            DataGridViewCheckBoxColumn ChCol = new DataGridViewCheckBoxColumn();
            ChCol.Name = "CheckBoxRow";
            ChCol.HeaderText = "选择";
            ChCol.Width = 65;
            ChCol.TrueValue = "1";
            ChCol.FalseValue = "0";
            dataGridView1.Columns.Insert(0, ChCol);
            //初始化数据表

            waitSetTable.Columns.Add("订单号", typeof(string));
            waitSetTable.Columns.Add("品名号", typeof(string));
            waitSetTable.Clear();
            dataGridView2.DataSource = waitSetTable;

            DataGridViewComboBoxColumn boxColumn = new DataGridViewComboBoxColumn();
            boxColumn.Name = "count";
            boxColumn.HeaderText = "数量/（垛）";
            boxColumn.Items.Add("1");
            boxColumn.Items.Add("2");
            boxColumn.Items.Add("3");
            boxColumn.Items.Add("4");
            boxColumn.Items.Add("5");
            dataGridView2.Columns.Add(boxColumn);
        }
        public void setDataGrid()
        {

            MySQL.Select select = new Select();
            DataSet dataSet = new DataSet();
            if (select.getQueue(nowDevice, "in", out dataSet))
            {
                dataGridView1.DataSource = dataSet.Tables[0];
                
            }
            else
            {
                MessageBox.Show("获取排单信息失败", "提示");
            }
            
        }
        private void label1_Click(object sender, EventArgs e)
        {
            setDataGrid();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySQL.Select select = new Select();
            Device de_ = new Device();
            de_.Device_number = comboBox1.Text;
            select.getDeviceInfor(de_.Device_number, ref de_);
            setDevice(de_);
            nowDevice = de_;
            //初始化列表并初始化储位图形
            reSetShelf();
            initPicture();

            //reEvent();  //重新初始化事件函数
            //初始化排单
            setDataGrid();
            waitSetTable.Clear();
            //清空之前留下的信息
            textBox3.Text = "";
            textBox4.Text = "";
            //初始化队列信息
            textBox2.Text = select.getqueue2(nowDevice);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text.Trim()))
            {
                MessageBox.Show("单号不能为空");
            }
            else
            {
                MySQL.Select select = new Select();
                GDI.PrintWhitFont print = new PrintWhitFont();
                string inf = "";
                string deviceIn = "";
                string desviceOut = "";
                if (select.FindOrderInfro(textBox4.Text, ref inf, ref deviceIn, ref desviceOut))
                {
                    //请求写入数据库
                    MySQL.Insert insert = new Insert();
                    if (insert.insertToEnqueue(nowDevice, textBox4.Text, "in", 3, oper_Emp))
                    {
                        MessageBox.Show("信息进入队列成功", "提示");
                        textBox2.Text = select.getqueue2(nowDevice);
                    }

                    //发送请求

                }
                else
                {
                    MessageBox.Show("数据库中无此单号的订单，请检查您是否输入错误" +
                        "\n" +
                        "或咨询主管是否数据录入错误", "提示");
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox4.Text.Trim() != "")
            {
                MySQL.Select select = new Select();
                GDI.PrintWhitFont print = new PrintWhitFont();
                string inf = "";
                string deviceIn = "";
                string desviceOut = "";
                if (Inquired == false)
                {
                    if (select.FindOrderInfro(textBox4.Text, ref inf, ref deviceIn, ref desviceOut))
                    {

                        textBox3.Text = inf;
                        //标记对应图片
                        List<int> s_list = new List<int>();
                        if (select.findAll(textBox4.Text, ref s_list) == false)
                        {
                            MessageBox.Show("目前登录的设备号所属的存储区内，暂无该单号的货物", "提示");
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
                            pictureList[index].Image = print.StoreMap(shelfDic[shelfList[index]], pictureList[index], textBox4.Text, coods, length, width);

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
                    textBox3.Text = "";
                }
            }
            else
            {
                MessageBox.Show("单号不能为空");
            }

        }

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
                        "或咨询主管是否数据录入错误", "提示");
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> order_list = new List<string>();
            List<string> des_name = new List<string>();
            List<int> des_count = new List<int>();
            string st_infro = "";
            //得到勾选序列
            int n = 0;
            for (int i = 0; i < waitSetTable.Rows.Count; i++)
            {
                order_list.Add(waitSetTable.Rows[i].ItemArray[0].ToString());
                des_name.Add(waitSetTable.Rows[i].ItemArray[1].ToString());
                if (dataGridView2.Rows[i].Cells[0].Value == null)
                {
                    MessageBox.Show("有货物没选择需要出库的数量", "提示！");
                    return;
                }
                des_count.Add(Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value));
                st_infro = st_infro + "订单尾号后4位： " + Convert.ToString(order_list[i]).Substring(order_list[i].Length - 4, 4) +
                    "   " + "品名： " + des_name[i] + "  " + "数量/（垛）" + des_count[i] + "\n";
                dataGridView1.Rows[i].Cells[0].Value = false;

                //if ((bool)dataGridView1.Rows[i].Cells[0].EditedFormattedValue == true)
                //{
                //    order_list.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
                //    des_name.Add(dataGridView1.Rows[i].Cells[2].Value.ToString());

                //    st_infro = st_infro + "订单尾号后4位： " + Convert.ToString(order_list[n]).Substring(order_list[n].Length - 4, 4) +
                //      "   " + "品名： " + des_name[n] + "\n";
                //    dataGridView1.Rows[i].Cells[0].Value = false;
                //    n++;
                //}
            }
            if (order_list.Count == 0)
            {
                MessageBox.Show("请至少在排单表中勾选一种订单号！！！", "提示");
                return;
            }
            //再次确认
            DialogResult result = MessageBox.Show("您选择的是：" + "\n" + st_infro, "提示", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {

            }
            else
            {
                return;
            }


            //请求写入数据库
            MySQL.Insert insert = new Insert();
            MySQL.Select select = new Select();
            for (int i = 0; i < order_list.Count; i++)
            {

                for (int j = 0; j < des_count[i]; j++)
                {
                    if (insert.insertToEnqueue(nowDevice, order_list[i], "in", 3, oper_Emp))
                    {
                        st_infro = st_infro + order_list[i] + ": " + "信息进入队列成功" + "\n";
                    }
                    else
                    {
                        st_infro = st_infro + order_list[i] + ": " + "信息进入队列失败" + "\n";
                    }
                }

            }
            MessageBox.Show(st_infro, "提示");
            textBox2.Text = select.getqueue2(nowDevice);
            //清空勾选框和列表
            waitSetTable.Clear();
            orderNumber_list.Clear();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = false;
            }
            //发送请求
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //判断是否是单选框被改变值
            if (e.ColumnIndex != 0)
            {
                return;
            }
            if ((bool)this.dataGridView1.CurrentRow.Cells[0].EditedFormattedValue == true)//被选
            {
                if (orderNumber_list.Exists(p => p == this.dataGridView1.CurrentRow.Cells[1].Value.ToString()))
                {
                    return;
                }
                else
                {
                    orderNumber_list.Add(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                    DataRow dataRow = waitSetTable.NewRow();

                    for (int i = 0; i < 2; i++)
                    {

                        dataRow[i] = dataGridView1.CurrentRow.Cells[i + 1].Value.ToString();
                    }

                    waitSetTable.Rows.Add(dataRow.ItemArray);

                }
            }
            else//不被勾选 
            {
                if (orderNumber_list.Exists(p => p == this.dataGridView1.CurrentRow.Cells[1].Value.ToString()))
                {

                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        if (Convert.ToString(waitSetTable.Rows[i].ItemArray[0]) == dataGridView1.CurrentRow.Cells[1].Value.ToString())
                        {
                            waitSetTable.Rows.RemoveAt(i);


                        }
                    }
                    orderNumber_list.Remove(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                }
                else return;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            waitSetTable.Clear();
            orderNumber_list.Clear();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = true;
                orderNumber_list.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
                DataRow dataRow = waitSetTable.NewRow();

                for (int j = 0; j < 2; j++)
                {

                    dataRow[j] = dataGridView1.Rows[i].Cells[j + 1].Value.ToString();
                }

                waitSetTable.Rows.Add(dataRow.ItemArray);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            waitSetTable.Clear();
            orderNumber_list.Clear();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                //防止没有选中行
                if (dataGridView2.SelectedRows.Count == 0)
                {
                    MessageBox.Show("没有选中行");
                    return;
                }

                int index = dataGridView2.SelectedRows[0].Index;//获取当前选中行的索引
                if (index == dataGridView2.RowCount - 1)
                {
                    MessageBox.Show("请勿选择空白行");
                    return;
                }
                if (index > 0)//如果该行不是第一行
                {

                    DataRow row = waitSetTable.NewRow();
                    row.ItemArray = waitSetTable.Rows[index - 1].ItemArray;
                    waitSetTable.Rows[index - 1].ItemArray = waitSetTable.Rows[index].ItemArray;
                    waitSetTable.Rows[index].ItemArray = row.ItemArray;
                    dataGridView2.Rows[index - 1].Selected = true;
                    dataGridView2.Rows[index].Selected = false;



                }
                else
                {
                    MessageBox.Show("已是首行！", "提示");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedRows.Count == 0)
                {
                    MessageBox.Show("没有选中行");
                    return;
                }
                int index = dataGridView2.SelectedRows[0].Index;//获取当前选中行的索引
                if ((dataGridView2.RowCount - 2) != index)//如果该行不是最后一行
                {
                    DataRow row = waitSetTable.NewRow();
                    row.ItemArray = waitSetTable.Rows[index + 1].ItemArray;
                    waitSetTable.Rows[index + 1].ItemArray = waitSetTable.Rows[index].ItemArray;
                    waitSetTable.Rows[index].ItemArray = row.ItemArray;
                    dataGridView2.Rows[index + 1].Selected = true;
                    dataGridView2.Rows[index].Selected = false;
                }
                else
                {
                    MessageBox.Show("已是最后一行！", "提示");
                }





            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Form formtochang = new FormToChangeQueue(nowDevice);
            formtochang.TopMost = true;
            formtochang.Show();
        }
    }
}