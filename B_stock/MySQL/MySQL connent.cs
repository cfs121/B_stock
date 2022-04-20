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
                    shelf.Max_dis = (Int32)mysqldr[7];
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

        /// <summary>
        /// 查询可用的货架，将其记录在序列中
        /// </summary>
        /// <param name="shelfList">用于记录序列的动态数组</param>
        /// <returns>可用货架的总数</returns>
        public int getAbleShelfCount(ArrayList shelfList)
        {
            string mysqlStr = "select * from storage_status";
            int count = 0;
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())//读一行
                {
                    if ((string)mysqldr[9] == "1")
                    {
                        shelfList.Add(mysqldr[1]);
                        count++;
                    }

                }
                Close();
                return count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return 0;
            }
        }
        /// <summary>
        /// 根据数据库的情况设置对应的shelf类
        /// </summary>
        /// <param name="shelf">需要设置的shelf类参数</param>
        public void SetShelf(shelf shelf)
        {
            string mysqlStr = string.Format("select * from storage_status where Shelf_number={0}", shelf.ShelfNumber);
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())//读一行
                {
                    shelf.Lenght = (Int32)mysqldr[4];
                    shelf.Width = (Int32)mysqldr[5];
                    shelf.Hight = (Int32)mysqldr[6];
                    shelf.Max_dis = Convert.ToInt32( mysqldr[7]);
                    shelf.Min_dis= Convert.ToInt32(mysqldr[8]); 

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
            string mysqlStr = string.Format("select Description_number,Length,Width from storage_position where Shelf_number={0} order by Allocation", shelf_number);
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

    }
    

    public class Update : Connection
    {

    }

    public class Delet : Connection
    {

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
