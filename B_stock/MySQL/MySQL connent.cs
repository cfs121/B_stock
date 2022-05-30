using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using Parameter;
using System.Data;

namespace MySQL
{



    public class Select : Connection
    {
        public void set()
        {
            string mysqlStr = string.Format("select * from Basic_infor");

            Open();//打开通讯通道

            try//利用try块语句捕捉通讯中可能出现的异常
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                if (mysqldr.Read() == true)
                {
                    Parameter.Class_Parameter.Gap = (Int32)mysqldr[1];
                    Parameter.Class_Parameter.RGV_speed = (Int32)mysqldr[2];
                    Parameter.Class_Parameter.Track_length = (Int32)mysqldr[3];
                    Parameter.Class_Parameter.Group_gap = (Int32)mysqldr[5];
                    Parameter.Class_Parameter.Shelf_gap = (Int32)mysqldr[6];
                    Parameter.RGV.Start_location = (Int32)mysqldr[4];
                    Parameter.Class_Parameter.Sever_addres = mysqldr[7].ToString();
                    Parameter.Class_Parameter.COM = (Int32)mysqldr[8];
                }


            }
            catch (Exception ex)//捕捉到了异常
            {

                MessageBox.Show(ex.ToString());

            }
            Close();
            return;

        }
        /// <summary>
        /// 查询密码是否正确
        /// </summary>
        /// <param name="Account">账号</param>
        /// <param name="Password">密码</param>
        /// <returns>图形</returns>
        public int log_in_cheek(string Account, string Password, ref Dictionary<string, string> value)
        {
            string mysqlStr = string.Format("select Password,Emp_number,Name,Level from operator_account where Account_number='{0}'", Account);

            GetConnection();//打开通讯通道
            value = new Dictionary<string, string>();

            try//利用try块语句捕捉通讯中可能出现的异常
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                if (mysqldr.Read() == true)
                {
                    
                   
                    if (mysqldr[0].ToString() == Password)//密码正确返回值0
                    {
                        //赋值操作员的一些具体信息
                        value.Add("Name", mysqldr[2].ToString());
                        value.Add("Emp_number", mysqldr[1].ToString());
                        value.Add("Level", mysqldr[3].ToString());
                        Close();
                        return 0;
                    }
                    else
                    {
                        Close();
                        return 1;
                    }
                }
                else
                {
                    MessageBox.Show("该账号不存在！请检查账号输入是否正确。", "错误");
                    Close();
                    return 2;
                }

            }
            catch (Exception ex)//捕捉到了异常
            {

                MessageBox.Show(ex.ToString());
                Close();
                return 2;
            }





        }

        public void getDeviceNumber(string device_class,string emp_number,ref List<string> device_list)
        {
            string mysqlStr =string.Format("select a.Device_number from operator_table a join device_table b on a.Device_number=b.Device_number " +
                "where a.Emp_number='{0}' and b.Device_class='{1}'",emp_number,device_class);
            
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())//读一行
                {
                    device_list.Add(mysqldr[0].ToString());

                }
                Close();
                return ;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return ;
            }
        }

        public void getDeviceInfor(string de_number,ref Parameter.Device device)
        {
            string mysqlStr = string.Format("select * from device_table where Device_number='{0}'",de_number);

            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())//读一行
                {
                    device.Device_number = mysqldr[1].ToString();
                    device.Access =(Int32) mysqldr[2];
                    device.Name = mysqldr[4].ToString();



                }
                Close();
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return;
            }
        }

        public void setShelfList(string device_class,string de_number,ref List<int> shellist,ref Dictionary<int,shelf> shelf_dic)
        {

            string mysqlStr = string.Format("select * from shelf_parameter where {0}='{1}' order by Shelf_number",device_class, de_number);

            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())//读一行
                 {
                    shelf shelf = new shelf();
                    //得到各项参数
                    shelf.ShelfNumber = (Int32)mysqldr[1];
                    shelf.ShelfName = mysqldr[2].ToString();
                    shelf.Lenght = (Int32)mysqldr[5];
                    shelf.Width=(Int32)mysqldr[6];
                    shelf.Max_dis = (Int32) mysqldr[7];
                    shelf.Min_dis = (Int32)mysqldr[8];
                    //进入集合
                    shellist.Add(shelf.ShelfNumber);
                    shelf_dic.Add(shelf.ShelfNumber,shelf);




                }
                Close();
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return;
            }
        }

        public void getSeverInfor(ref string sever_add,ref int com)
        {
            string mysqlStr = string.Format("select socket,COM from dasic_infor");

            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())//读一行
                {
                    sever_add = mysqldr[0].ToString();
                    com = (Int32)mysqldr[1];
                }
                Close();
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return;
            }
        }
        /// <summary>
        /// 得到做图形信息
        /// </summary>
        /// <param name="shelf_number">目标货架号</param>
        /// <param name="c_oods">编号序列</param>
        /// <param name="l_enght">长度尺寸序列</param>
        /// <param name="w_idth">宽度尺寸序列</param>
        public void getStorageForPrint(int shelf_number, ref List<string> c_oods, ref List<int> l_enght, ref List<int> w_idth)
        {
            string mysqlStr = string.Format("select st.Order_number,des.Net_length,des.Net_width from storage_position st join order_table ord on st.Order_number=ord.Order_number join description_table des"+ 
                " on ord.Description_number = des.Description_number"+
                 " where st.Shelf_number ={0} order by st.Allocation", shelf_number);
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())//读一行
                {
                    c_oods.Add((string)mysqldr[0]);
                    l_enght.Add((Int32)mysqldr[1]);
                    w_idth.Add((Int32)mysqldr[2]);

                }
                Close();
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return;
            }

        }

        public void getStorageNumber(Device device,string des_number,ref  string mes)
        {
            string mysqlStr = string.Format("select count(sp.Order_number),sum(sp.amount) from storage_position sp join shelf_parameter spa " +
                "on sp.Shelf_number=spa.Shelf_number where spa.Device_out_number='{0}' and sp.Order_number='{1}'", device.Device_number,des_number);
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                if(mysqldr.Read())//读一行
                {
                    mes = mes + "库存垛数： " + mysqldr[0].ToString()+System.Environment.NewLine;
                    mes = mes + "库存总数： " + mysqldr[1].ToString();

                }
                Close();
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return;
            }

        }

        public bool findForClick( ref string inf, PictureBox box, shelf shelf, int y)
        {
            decimal ratioY = (decimal)y / box.Size.Height;
            decimal ratioDesTop;
            decimal ratioDesBow;


            int toLength;//起始位置
            int maxLenght = shelf.Lenght;
            bool empty = true;//记录是否为空货架
            string des_number = "";
            string order_number = "";
            int Gap = Parameter.Class_Parameter.Gap;
            bool whileFlage = false;

            string mysqlStr = string.Format("select st.Order_number as 订单编号,ord.Description_number as 品名编号,des.Net_length from storage_position st join order_table ord " +
                "on st.Order_number=ord.Order_number join description_table des on ord.Description_number=des.Description_number " +
                "where st.Shelf_number={0} order by st.Allocation;", shelf.ShelfNumber);
            Open();//打开通讯通道
            MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
            MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();
            try
            {


                if (true)//在左边，偶数区
                {
                    toLength = 0;
                    while (mysqldr.Read())//读一行
                    {
                        empty = false;
                        ratioDesTop = (decimal)(toLength + (Int32)mysqldr[2]) / maxLenght;
                        ratioDesBow = (decimal)toLength / maxLenght;
                        if (ratioDesTop >= ratioY && ratioDesBow <= ratioY)
                        {
                            whileFlage = true;
                            order_number = mysqldr[0].ToString();
                            des_number = (string)mysqldr[1];
                            for (int i = 0; i < mysqldr.FieldCount; i++)
                            {
                                inf = inf + mysqldr.GetName(i) + ": " + Convert.ToString(mysqldr[i]) + "\n";
                            }

                        }
                        toLength = toLength + Gap + (Int32)mysqldr[2];

                    }


                }
                else//在右侧，奇数区
                {
                    empty = false;
                    toLength = shelf.Lenght;

                    while (mysqldr.Read())//读一行
                    {

                        empty = false;
                        ratioDesTop = (decimal)(toLength - (Int32)mysqldr[2]) / maxLenght;
                        ratioDesBow = (decimal)toLength / maxLenght;
                        if (ratioDesTop <= ratioY && ratioDesBow >= ratioY)
                        {
                            whileFlage = true;
                            order_number = mysqldr[0].ToString();
                            des_number = (string)mysqldr[1];
                            for (int i = 0; i < mysqldr.FieldCount; i++)
                            {
                                inf = inf + mysqldr.GetName(i) + ": " + Convert.ToString(mysqldr[i]) + "\n";
                            }
                        }
                        toLength = toLength - Gap - (Int32)mysqldr[2];
                    }

                }

                if (empty)
                {
                    MessageBox.Show("该货架为空货架，若有图形显示，请检查程序"
                        + "\n" + "若无图形显示，请误乱点", "提示");
                    Close();
                    return false;
                }
                if (whileFlage==false)
                {
                    MessageBox.Show("您是否在点击空白区域，或者图形边缘" + "\n"+
                        "（图形显示有一定误差，请尽量点击图形的中心区域）", "提示");
                    Close();
                    return false;
                }

                mysqldr.Close();

                mysqlStr = string.Format("select cus.Customer_name as 客户名,des.Description_name as 品名,des.Net_length as 净长,des.Net_width as 净宽 " +
                    "from description_table des join customer_infor cus " +
                    "on des.Customer_number=cus.Customer_number where des.Description_number={0}", des_number);
                using (mySqlCommand = new MySqlCommand(mysqlStr, mycon))
                mysqldr = mySqlCommand.ExecuteReader();
                if (mysqldr.Read() == true)
                {
                    for (int i = 0; i < mysqldr.FieldCount; i++)//从第二个字段开始读起
                    {
                        inf = inf + mysqldr.GetName(i) + ": " + Convert.ToString(mysqldr[i]) + "\n";
                    }
                }
                else
                {
                    MessageBox.Show("存在storage_position表中有description_infro表中不存在的编号", "错误");
                }

                Close();
                return true;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return false;
            }

        }

        public bool FindOrderInfro(string order_number,ref string infro,ref string de_in,ref string de_out)
        {
            string mysqlStr = string.Format("select * from order_table where Order_number={0}",order_number);
            string des_number = "";
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                if (mysqldr.Read() == true)//读一行
                {
                    des_number = mysqldr[2].ToString();
                    de_in = mysqldr[4].ToString();
                    de_out = mysqldr[5].ToString();
                    mysqldr.Close();
                    mysqlStr = string.Format("select cus.Customer_name as 客户名,des.Description_name as 品名,des.Net_length as 净长," +
                    "des.Net_width as 净宽,des.Enter_time as 录入时间 " +
                    "from description_table des join customer_infor cus " +
                    "on des.Customer_number=cus.Customer_number where des.Description_number={0}", des_number);
                    using (mySqlCommand = new MySqlCommand(mysqlStr, mycon))
                        mysqldr = mySqlCommand.ExecuteReader();
                    if (mysqldr.Read() == true)
                    {
                        for (int i = 0; i < mysqldr.FieldCount; i++)
                        {
                            infro = infro + mysqldr.GetName(i) + ": " + Convert.ToString(mysqldr[i]) + Environment.NewLine;
                        }
                    }
                    else
                    {
                        MessageBox.Show("存在Order_table表中有description_infro表中不存在的编号", "错误");
                    }

                    Close();
                    return true;
                }
                else
                {
                    Close();
                    return false;
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return true;
            }
        }
        public bool FindOrderInfro(string order_number, ref string infro)
        {
            string mysqlStr = string.Format("select * from order_table where Order_number={0}", order_number);
            string des_number = "";
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                if (mysqldr.Read() == true)//读一行
                {
                    des_number = mysqldr[2].ToString();
                    
                    mysqldr.Close();
                    mysqlStr = string.Format("select cus.Customer_name as 客户名,des.Description_name as 品名,des.Net_length as 净长," +
                    "des.Net_width as 净宽,des.Enter_time as 录入时间 " +
                    "from description_table des join customer_infor cus " +
                    "on des.Customer_number=cus.Customer_number where des.Description_number={0}", des_number);
                    using (mySqlCommand = new MySqlCommand(mysqlStr, mycon))
                        mysqldr = mySqlCommand.ExecuteReader();
                    if (mysqldr.Read() == true)
                    {
                        for (int i = 0; i < mysqldr.FieldCount; i++)
                        {
                            infro = infro + mysqldr.GetName(i) + ": " + Convert.ToString(mysqldr[i]) + Environment.NewLine;
                        }
                    }
                    else
                    {
                        MessageBox.Show("存在Order_table表中有description_infro表中不存在的编号", "错误");
                    }

                    Close();
                    return true;
                }
                else
                {
                    Close();
                    return false;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return true;
            }
        }

        public bool findAll(string order_number,ref List<int>list)
        {
            string mysqlStr = string.Format("select distinct Shelf_number from storage_position where Order_number={0}", order_number);
            
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())
                {
                    list.Add((Int32)mysqldr[0]);
                }
                if (list.Count != 0)
                {
                    Close();
                    return true;
                }
                else
                {
                    Close();
                    return false;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return true;
            }
        }


        public bool takeOrders(Device device,string dev_class,out DataSet data)
        {
            string de_class = "";
            string table = "";
            string name = "";
            //分辨设备类型，SQL语句中有区分
            if (dev_class == "out" || dev_class == "in")
            {
                if (dev_class=="out")
                {
                    de_class ="Device_out_number";
                    table = "output_table";
                    name = "已出库";
                }
                else
                {
                    de_class = "Device_in_number";
                    table = "input_table";
                    name = "已入库";
                }
            }
            else
            {
                MessageBox.Show("在读取排单状态时有未知的设备类型","错误");
                data = null;
                return false;
            }

            string mysqlStr = string.Format("select ord.Order_number as 订单号,des.Description_name as 品名,cus.Customer_name as 顾客名,ord.Total as 总数,ifnull(enqueue.sum,0) as 已请求,ifnull(cou.sum,0) as {0}  " +
                "from order_table ord join description_table des on ord.Description_number=des.Description_number " +
                "join customer_infor cus on des.Customer_number=cus.Customer_number " +
                "join (select Order_number,Device_number,count(Order_number) as sum from enqueue group by Order_number) enqueue on ord.Order_number=enqueue.Order_number " +
                "left join (select Order_number,sum(Count) as sum from {1} group by Order_number) cou " +
                "on ord.Order_number=cou.Order_number where ord.{2}={3};", name, table, de_class, device.Device_number);

            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataAdapter mySqlData = new MySqlDataAdapter(mySqlCommand);
                DataSet ds = new DataSet();
                mySqlData.Fill(ds ,"排单表");
                data = ds;
                

                Close();
                return true;

              

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                data = null;
                return false;
            }
        }
        public bool getQueue(Device device, string dev_class, out DataSet data)
        {
            string de_class = "";
            string table = "";
            //分辨设备类型，SQL语句中有区分
            if (dev_class == "out" || dev_class == "in")
            {
                if (dev_class == "out")
                {
                    de_class = "Device_out_number";
                    table = "output_table";
                }
                else
                {
                    de_class = "Device_in_number";
                    table = "input_table";
                }
            }
            else
            {
                MessageBox.Show("在读取排单状态时有未知的设备类型", "错误");
                data = null;
                return false;
            }
            string mysqlStr = string.Format("select ord.Order_number as 订单号,des.Description_name as 品名,cus.Customer_name as 顾客名,ord.Total as 总数,ifnull(enqueue.sum,0) as 已请求,ifnull(cou.sum,0) as 已入库 " +
                 "from order_table ord join description_table des on ord.Description_number=des.Description_number " +
                 "join customer_infor cus on des.Customer_number=cus.Customer_number " +
                 "join (select Order_number,count(Order_number) as sum from enqueue group by Order_number) enqueue on ord.Order_number=enqueue.Order_number " +
                 "left join (select Order_number,sum(Count) as sum from {0} group by Order_number) cou " +
                 "on ord.Order_number=cou.Order_number where ord.{1}={2};", table, de_class, device.Device_number);

            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataAdapter mySqlData = new MySqlDataAdapter(mySqlCommand);
                DataSet ds = new DataSet();
                mySqlData.Fill(ds, "排单表2");
                data = ds;


                Close();
                return true;



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                data = null;
                return false;
            }
        }

        public string getqueue(Device device)
        {
            string mysqlStr = string.Format("select Order_number as 订单编号,Class as 类型,Priority as 优先级 from enqueue where Device_number={0} and Status=0 order by Priority,ID",device.Device_number);
            Open();//打开通讯通道
            //创建DataSet类的对象
            string queue = "";
            try
            {

                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())
                {
                    queue = queue + "编号： " + Convert.ToString(mysqldr[0]) + System.Environment.NewLine +
                          "类型： " + Convert.ToString(mysqldr[1]) + System.Environment.NewLine +
                          "优先级" + Convert.ToString(mysqldr[2]) + System.Environment.NewLine + System.Environment.NewLine;

                }
                Close();
                return queue;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return queue;
            }

        }
        public string getqueue2(Device device)
        {
            string mysqlStr = string.Format("select Order_number as 订单编号,Priority as 优先级 from enqueue where Device_number={0} and Status=0 order by Priority,ID", device.Device_number);
            Open();//打开通讯通道
            //创建DataSet类的对象
            string queue = "";
            try
            {

                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())
                {
                    queue = queue + "编号： " + Convert.ToString(mysqldr[0]) + System.Environment.NewLine +
                       
                       
                          //"类型： " + Convert.ToString(mysqldr[1]) + System.Environment.NewLine +
                          "优先级" + Convert.ToString(mysqldr[1]) + System.Environment.NewLine + System.Environment.NewLine;

                }
                Close();
                return queue;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return queue;
            }

        }
        public bool getqueue(Device device, out DataSet data)
        {
            string mysqlStr = string.Format("select ID as 主键,Order_number as 订单编号,Class as 类型,Priority as 优先级,Status as 状态 from enqueue where Device_number={0} " +
                "and Status!=2 order by Priority,ID", device.Device_number);

            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataAdapter mySqlData = new MySqlDataAdapter(mySqlCommand);
                DataSet ds = new DataSet();
                mySqlData.Fill(ds, "队列表");
                data = ds;


                Close();
                return true;



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                data = null;
                return false;
            }

        }
        /// <summary>
        /// 插单操作后的表单更新，防止干扰第一个指令
        /// </summary>
        /// <returns></returns>
        public string getQueueAfterInsert(Device device)
        {
            string mysqlStr = string.Format("select Order_number as 编号,Class as 类型,Priority as 优先级 from enqueue where Device_number={0} order by ID",device.Device_number);
            Open();//打开通讯通道
            //创建DataSet类的对象
            string queue = "";
            try
            {

                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())
                {
                    queue = queue + "编号： " + Convert.ToString(mysqldr[0]) + System.Environment.NewLine +
                           "类型： " + Convert.ToString(mysqldr[1]) + System.Environment.NewLine +
                           "优先级" + Convert.ToString(mysqldr[2]) + System.Environment.NewLine + System.Environment.NewLine;
                }
                Close();
                return queue;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return queue;
            }

        }

        public int getOneQueue(Device device,ref Order order )
        {
            string mysqlStr = string.Format("select Order_number,Class,ID from enqueue where Device_number={0} order by Priority,ID ", device.Device_number);
            Open();//打开通讯通道
            //创建DataSet类的对象
           
            try
            {

                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                if (mysqldr.Read())//读到一条
                {
                    order.Id =(Int32) mysqldr[2];
                    order.Class_ = (string)mysqldr[1];
                    order.orderNumber = (string)mysqldr[0];
                    Close();
                    return 0;
                }
                else//没有读到
                {
                    order.Id = 0;
                    order.Class_ = "";
                    order.orderNumber = "";

                    Close();
                    return 1;
                }
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                order.Id = 0;
                order.Class_ = "";
                order.orderNumber = "";
                Close();
                return 2;
            }
        }

        public string getAllQueue()
        {

            string mysqlStr = string.Format("select Order_number as 订单编号,Class as 类型,Priority as 优先级 from enqueue order by Priority,ID");
            Open();//打开通讯通道
            //创建DataSet类的对象
            string queue = "";
            try
            {

                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())
                {
                    queue = queue + "编号： " + Convert.ToString(mysqldr[0]) + System.Environment.NewLine +
                          "类型： " + Convert.ToString(mysqldr[1]) + System.Environment.NewLine +
                          "优先级" + Convert.ToString(mysqldr[2]) + System.Environment.NewLine + System.Environment.NewLine;

                }
                Close();
                return queue;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return queue;
            }
        }

        public bool getTask(out DataSet dataSet)
        {
            string mysqlStr = "select ord.Order_number as 订单号,des.Description_number as 品名,cus.Customer_name as 顾客名,ord.Total as 总数,ifnull(couout.sum,0) as 已叫,ifnull(couin.sum,0) as 已入 from order_table ord " +
                "join description_table des on ord.Description_number=des.Description_number " +
                "join customer_infor cus on des.Customer_number=cus.Customer_number left join " +
                "(select Order_number,sum(Count) as sum from output_table group by Order_number) " +
                "couout on ord.Order_number=couout.Order_number left join (select Order_number,sum(Count) " +
                "as sum from input_table group by Order_number) couin on ord.Order_number=couin.Order_number";
            Open();//打开通讯通道
            
          
            try
            {

                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataAdapter mySqlData = new MySqlDataAdapter(mySqlCommand);
                DataSet ds = new DataSet();
                mySqlData.Fill(ds, "排单表");
                dataSet = ds;


                Close();
                return true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                dataSet = null;
                return false;
            }

        }

        public bool getSocket(out string address,out int com,out int block)
        {
            string mysqlStr = string.Format("select socket,COM,block from basic_infor");
            Open();//打开通讯通道
            //创建DataSet类的对象
            string queue = "";
            try
            {

                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                if (mysqldr.Read())
                {
                    address = mysqldr[0].ToString();
                    com = (int)mysqldr[1];
                    block = (int)mysqldr[2];
                    Close();
                    return true;

                }
                else
                {
                    address = "";
                    com = 0;
                    block = 0;
                    Close();
                    return false;
                }
               

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                address = "";
                com = 0;
                block = 0;
                return false;
            }

        }



        /// <summary>
        /// 查询该品名是否存在
        /// </summary>
        /// <param name="des_number">品名编号</param>
        /// <returns>存在返回真，不存在或者运行失败返回假</returns>
        public bool findInDes(ref des des)
        {

            string mysqlStr = string.Format("select * from description_infro where Description_number='{0}'", des.Des_number);
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                if (mysqldr.Read() == true)//读到数据,存在该数据
                {
                    des.Des_number = (string)mysqldr[1];
                    des.Cust_number = (string)mysqldr[2];
                    des.Des_name = (string)mysqldr[3];
                    des.Lenght = (Int32)mysqldr[4];
                    des.Width = (Int32)mysqldr[5];
                    des.Hight = (Int32)mysqldr[6];

                    Close();
                    return true;
                }
                else
                {
                    Close();
                    return false;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return false;
            }
        }
        public bool findInDes(string coods, ref int length, ref int width)
        {
            string mysqlStr = string.Format("select * from description_infro where Description_number='{0}'", coods);
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                if (mysqldr.Read() == true)//读到数据,存在该数据
                {
                    length = (Int32)mysqldr[4];
                    width = (Int32)mysqldr[5];
                    Close();
                    return true;
                }
                else
                {
                    Close();
                    return false;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return false;
            }
        }

        public bool StatisticalCapacity(ref int used, ref int total_stack, ref int total_capacity)
        {

            string mysqlStr = string.Format("select sum(Used_capacity),sum(Total_storage),sum(Max_length) from storage_status");
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                if (mysqldr.Read() == true)//读到数据,存在该数据
                {
                    used = Convert.ToInt32(mysqldr[0]);
                    total_stack = Convert.ToInt32(mysqldr[1]);
                    total_capacity = Convert.ToInt32(mysqldr[2]);
                    Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("查询容量情况失败" + "'/n'" + mysqlStr, "提示");
                    Close();
                    return false;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return false;
            }


        }
        public bool findForInquire(string coods, ref List<int> list_shelf)
        {
            string mysqlStr = string.Format("select distinct Shelf_number from storage_position where Description_number='{0}' order by Shelf_number", coods);
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                if (mysqldr.Read() == true)//读到数据,存在该数据
                {
                    list_shelf.Add((Int32)mysqldr[0]);
                    while (mysqldr.Read())//read()执行一次读一行
                    {
                        list_shelf.Add((Int32)mysqldr[0]);
                    }
                    Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("存储区内未查询到该编号的物品" + "'/n'" + "请检查编号是否正确，或该货物并为入库"
                        + "'/n'" + mysqlStr, "提示");
                    Close();
                    return false;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return false;
            }
        }

      

    }

    public class Insert : Connection
    {
        public bool insertToEnqueue(Device device,string order_number,string class_,int priority,oper_emp _Emp)
        {
            string mysqlStr = string.Format("insert into enqueue(Order_number,Device_number,Class,Priority,Operator) " +
                    "values('{0}','{1}','{2}',{3},'{4}')", order_number,device.Device_number,class_,priority,_Emp.Name);
            
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);



                if (mySqlCommand.ExecuteNonQuery() > 0)
                {
                    Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("数据库enqueue写入数据失败，0行成功" + "'/n'" + mysqlStr, "错误");
                    Close();
                    return false;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return true;
            }
        }
    }
    

    public class Update : Connection
    {

    }

    public class Delet : Connection
    {
        public bool  de_queue(int id)
        {
            string mysqlStr = string.Format("select Status from enqueue where ID={0}", id);
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                if (mysqldr.Read() == true)//读到数据,存在该数据
                {
                    if ((Int32)mysqldr[0]==2)
                    {
                        MessageBox.Show("该指令正在执行","提示");
                        return false;
                    }
                    if ((Int32)mysqldr[0] == 1)
                    {
                        MessageBox.Show("该指令已经执行完成", "提示");
                        return false;
                    }
                    mysqldr.Close();

                    mysqlStr = string.Format("delete from enqueue where ID={0}",id );
                    mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                    if (mySqlCommand.ExecuteNonQuery() > 0)
                    {
                        Close();
                        return true ;
                    }
                    else
                    {
                        MessageBox.Show("数据库enqueue删除数据失败，0行成功" + "'/n'" + mysqlStr, "错误");
                        Close();
                        return false  ;
                    }
                }
                else
                {
                    MessageBox.Show("队列表中无该ID主键的指令", "提示");
                    Close();
                    return false ;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return false ;
            }
        }
    }
    /// <summary>
    /// 混合类的SQL
    /// </summary>
    public class MIXSQL : Connection
    {
       

    }



    
    /// <summary>
    /// 用于通讯的建立，关闭，状态检查
    /// </summary>
    public class Connection
    {

        //构建数据库连接字符串
        protected string M_str_sqlcon = "server=localhost;user id=root;password=12345678;database=b_stock"; //根据自己的设置
                                                                                                            //创建数据库连接对象
        protected MySqlConnection mycon = new MySqlConnection();


        /// <summary>
        /// 用于打开与数据库的连接
        /// </summary>
        public void Open()
        {

            mycon.ConnectionString = M_str_sqlcon;

            try
            {
                //打开数据库连接
                mycon.Open();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        /// <summary>
        /// 用于关闭与数据库的连接
        /// </summary>
        public void Close()
        {

            try
            {
                //关闭数据库连接
                mycon.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }
        /// <summary>
        /// 查询当前通讯连接的状态，看是否需要修正
        /// </summary>
        internal void GetConnection()//查询连接状态,中途可能会被打断
        {
            mycon.ConnectionString = M_str_sqlcon;
            if (mycon.State == System.Data.ConnectionState.Closed)// 连接未开启
            {
                mycon.Open();
            }
            if (mycon.State == System.Data.ConnectionState.Broken)//连接被打断，只有在被打开过的情况下会出现
            {
                mycon.Close();
                mycon.Open();
            }
            if (mycon.State == System.Data.ConnectionState.Open)
            {
                return;
            }

        }


    }


}
