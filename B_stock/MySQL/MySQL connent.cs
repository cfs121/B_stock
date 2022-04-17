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
            string mysqlStr = string.Format("select Password,Emp_number,Name,Level from operator_table where Account_number='{0}'", Account);

            GetConnection();//打开通讯通道
            value = new Dictionary<string, string>();

            try//利用try块语句捕捉通讯中可能出现的异常
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                if (mysqldr.Read() == true)
                {
                    //赋值操作员的一些具体信息
                    value.Add("Name", mysqldr[2].ToString());
                    value.Add("Emp_number", mysqldr[1].ToString());
                    value.Add("Level", mysqldr[3].ToString());
                    if (mysqldr[0].ToString() == Password)//密码正确返回值0
                    {

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

        public void findForClick(ref string des_number, ref string inf, PictureBox box, shelf shelf, int y)
        {
            decimal ratioY = (decimal)y / box.Size.Height;
            decimal ratioDesTop;
            decimal ratioDesBow;


            int toLength;//起始位置
            int maxLenght = shelf.Lenght;
            bool empty = true;//记录是否为空货架

            int Gap = Parameter.Class_Parameter.Gap;

            string mysqlStr = string.Format("select * from storage_position where Shelf_number={0} order by Allocation", shelf.ShelfNumber);
            Open();//打开通讯通道
            MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
            MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();
            try
            {


                if (shelf.ShelfNumber / 10 % 2 == 0)//在左边，偶数区
                {
                    toLength = 0;
                    while (mysqldr.Read())//读一行
                    {
                        empty = false;
                        ratioDesTop = (decimal)(toLength + (Int32)mysqldr[5]) / maxLenght;
                        ratioDesBow = (decimal)toLength / maxLenght;
                        if (ratioDesTop >= ratioY && ratioDesBow <= ratioY)
                        {
                            des_number = (string)mysqldr[1];
                            for (int i = 7; i < mysqldr.FieldCount; i++)
                            {
                                inf = inf + mysqldr.GetName(i) + ": " + Convert.ToString(mysqldr[i]) + "\n";
                            }

                        }
                        toLength = toLength + Gap + (Int32)mysqldr[5];

                    }


                }
                else//在右侧，奇数区
                {
                    empty = false;
                    toLength = shelf.Lenght;

                    while (mysqldr.Read())//读一行
                    {

                        empty = false;
                        ratioDesTop = (decimal)(toLength - (Int32)mysqldr[5]) / maxLenght;
                        ratioDesBow = (decimal)toLength / maxLenght;
                        if (ratioDesTop <= ratioY && ratioDesBow >= ratioY)
                        {
                            des_number = (string)mysqldr[1];
                            for (int i = 7; i < mysqldr.FieldCount; i++)
                            {
                                inf = inf + mysqldr.GetName(i) + ": " + Convert.ToString(mysqldr[i]) + "\n";
                            }
                        }
                        toLength = toLength - Gap - (Int32)mysqldr[5];
                    }

                }

                if (empty)
                {
                    MessageBox.Show("该货架为空货架，若有图形显示，请检查程序"
                        + "\n" + "若无图形显示，请误乱点", "提示");
                    Close();
                    return;
                }

                mysqldr.Close();

                mysqlStr = string.Format("select * from description_infro where Description_number='{0}'", des_number);
                using (mySqlCommand = new MySqlCommand(mysqlStr, mycon))
                    mysqldr = mySqlCommand.ExecuteReader();
                if (mysqldr.Read() == true)
                {
                    for (int i = 2; i < mysqldr.FieldCount; i++)//从第二个字段开始读起
                    {
                        inf = inf + mysqldr.GetName(i) + ": " + Convert.ToString(mysqldr[i]) + "\n";
                    }
                }
                else
                {
                    MessageBox.Show("存在storage_position表中有description_infro表中不存在的编号", "错误");
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

        public bool findForDesNameToNumber(string name_str, ref List<string> number)
        {
            string mysqlStr = string.Format("select Description_number from description_infro where Description_name like '%{0}%'", name_str);
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())
                {
                    number.Add((string)mysqldr[0]);
                }
                if (number.Count != 0)
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
                return false;
            }
        }

        public bool findForOutput(int shelf_number, ref string des_number, ref int lenght, ref int width)
        {
            string mysqlStr = string.Format("select Description_number,Length,Width from storage_position where Shelf_number={0} and Allocation=1", shelf_number);
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();
                if (mysqldr.Read())
                {
                    des_number = (string)mysqldr[0];
                    lenght = (Int32)mysqldr[1];
                    width = (Int32)mysqldr[2];
                    Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("存储区内该货架号上无货物？" + "'/n'" + mysqlStr, "提示");
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

        public bool ifFindInPosition(string des_number)
        {
            string mysqlStr = string.Format("select * from storage_position where Description_number='{0}'", des_number);
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();
                if (mysqldr.Read())
                {

                    Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("存储区内该货架号上无货物？" + "'/n'" + mysqlStr, "提示");
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
        /// <summary>
        /// 查询任务队列中是否有任务未被执行
        /// </summary>
        /// <param name="des"></param>
        /// <returns></returns>
        public bool findForQueue(ref des_queue des)
        {
            string mysqlStr = string.Format("select * from enqueue order by Priority,ID");
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();
                if (mysqldr.Read())
                {
                    des.ID = (Int32)mysqldr[0];
                    des.Des_number = (string)mysqldr[1];
                    des.Order_class = (string)mysqldr[2];
                    des.Priority = (Int32)mysqldr[3];
                    des.Mat_class = (string)mysqldr[4];
                    des.Amount = (Int32)mysqldr[5];
                    des.Emp = (string)mysqldr[6];
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
        public int LocationLayout(string Des_number, ref int out_shelf)
        {
            List<int> des_shelf = new List<int>();
            List<int> capac_shelf = new List<int>();
            List<int> empt_shelf = new List<int>();

            string mysqlStr = string.Format("select a.Shelf_number from storage_position a join storage_status b on a.Shelf_number=b.Shelf_number where a.Description_number='{0}'" +
                "and(b.Used_capacity + a.Length+{1}) < b.Max_length and b.Able = '1' order by a.Shelf_number", Des_number, Parameter.Class_Parameter.Gap);


            Open();//打开通讯通道

            try//利用try块语句捕捉通讯中可能出现的异常
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                //同品名的，能够存放得下的货架
                if (mysqldr.Read() == true)
                {
                    out_shelf = (Int32)mysqldr[0];
                    return 0;
                }
                mysqldr.Close();


                mysqlStr = string.Format("select distinct Shelf_number from storage_position where Description_number='{0}'", Des_number);
                mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                mysqldr = mySqlCommand.ExecuteReader();
                //得到同品名货架,去除重复记录
                while (mysqldr.Read())
                {
                    des_shelf.Add((Int32)mysqldr[0]);
                }
                mysqldr.Close();


                mysqlStr = string.Format("select Shelf_number from storage_status where Used_capacity=0 and Total_storage=0 and Able = '1' order by Shelf_number");
                mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                mysqldr = mySqlCommand.ExecuteReader();
                //得到空货架
                while (mysqldr.Read())
                {
                    empt_shelf.Add((Int32)mysqldr[0]);
                }
                mysqldr.Close();


                if (empt_shelf.Count != 0)//有空货架
                {

                    if (des_shelf.Count == 0)//没有同品名货架
                    {
                        out_shelf = empt_shelf[0];
                        return 0;
                    }
                    else//有同品名货架
                    {
                        int minshelf = 10000;
                        int mindis = 10000;
                        int tempdis = 0;
                        //求距离最小
                        foreach (var desShelf in des_shelf)
                        {
                            foreach (var emptShelf in empt_shelf)
                            {
                                tempdis = Convert.ToInt32(Math.Pow(desShelf / 10 - emptShelf / 10, 2.0) + Math.Pow(desShelf % 10 - emptShelf % 10, 2.0) * 2);
                                if (tempdis < mindis)
                                {
                                    minshelf = emptShelf;
                                    mindis = tempdis;
                                }
                            }
                        }
                        out_shelf = minshelf;
                        return 0;
                    }

                }


                mysqlStr = string.Format("select Shelf_number from storage_status where " +
                    "Used_capacity+(select Net_length from description_infro where Description_number='{0}')+{1}<Max_length " +
                    "and Able = '1' order by Shelf_number",
                    Des_number, Parameter.Class_Parameter.Gap);
                mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                mysqldr = mySqlCommand.ExecuteReader();
                //得到能放的下得货架
                while (mysqldr.Read())
                {
                    capac_shelf.Add((Int32)mysqldr[0]);
                }
                mysqldr.Close();


                if (capac_shelf.Count != 0)//有放的下得货架
                {

                    if (des_shelf.Count == 0)//没有同品名货架
                    {
                        out_shelf = capac_shelf[0];
                        return 0;
                    }
                    else//有同品名货架
                    {
                        int minshelf = 10000;
                        int mindis = 10000;
                        int tempdis = 0;
                        //求距离最小
                        foreach (var desShelf in des_shelf)
                        {
                            foreach (var capaShelf in capac_shelf)
                            {
                                tempdis = Convert.ToInt32(Math.Pow(desShelf / 10 - capaShelf / 10, 2.0) + Math.Pow(desShelf % 10 - capaShelf % 10, 2.0) * 2);
                                if (tempdis < mindis)
                                {
                                    minshelf = capaShelf;
                                    mindis = tempdis;
                                }
                            }
                        }
                        out_shelf = minshelf;

                        return 0;
                    }

                }

                //没有适合存放的货位  
                return 1;


            }
            catch (Exception ex)//捕捉到了异常
            {

                MessageBox.Show(ex.ToString());
                Close();
                return -1;
            }



        }

        public string getqueue()
        {
            string mysqlStr = string.Format("select Description_number as '编号',Class as '类型',Priority as '优先级' from enqueue order by Priority,ID");
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
        public string getQueueAfterInsert()
        {
            string mysqlStr = string.Format("select Description_number as '编号',Class as '类型',Priority as '优先级' from enqueue order by ID");
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

        public bool getScheduNumber(ref  List<string> number)
        {

            string mysqlStr = string.Format("select distinct Schedu_number from schedu_list");
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())
                {
                    number.Add(Convert.ToString(mysqldr[0]));
                }
                if (number.Count!=0)
                {
                    return true;
                }
                else
                {
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
        public bool getScheduDes(string sch_number,ref  List<string> des_)
        {
            string mysqlStr = string.Format("select Description_number from schedu_list where Schedu_number='{0}'",sch_number);
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())
                {
                    des_.Add(Convert.ToString(mysqldr[0]));
                }
                if (des_.Count != 0)
                {
                    return true;
                }
                else
                {
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

        public bool getShelfNumber(ref List<int> list)
        {

            string mysqlStr = string.Format("select Shelf_number from storage_status");
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                while (mysqldr.Read())
                {
                    list.Add(Convert.ToInt32(mysqldr[0]));
                }
                if (list.Count != 0)
                {
                    return true;
                }
                else
                {
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
        public DataTable dt()
        {
            string mysqlStr = string.Format("select Description_number '商品编号',Customer_number '客户编号',Description_name '商品名称'," +
                "Net_length '长',Net_width '宽',Net_hight '高'from description_infro ");
            Open();
            MySqlDataAdapter da = new MySqlDataAdapter(mysqlStr, mycon);
            DataTable dt = new DataTable();

            da.Fill(dt);
            Close();
            return dt;

        }
      

    }

    public class Insert : Connection
    {
        public bool insertToQueue(des_queue des_)
        {
            string mysqlStr = string.Format("insert into enqueue(Description_number,Class,Material_class,Amount,Operator,Priority) " +
                    "values('{0}','{1}','{2}',{3},'{4}',{5})",des_.Des_number,des_.Order_class,des_.Mat_class,des_.Amount,des_.Emp,des_.Priority);
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
                return false;
            }
        }

        public bool inserToFail(fail_queue fail_ )
        {
            string mysqlStr = string.Format("insert into fail_queue" +
                "(Description_number,Class,Materail_class,Fail_reason,Form_int,Plc_int,Amount,Idex,Count,Operator) " +
                   "values('{0}','{1}','{2}','{3}',{4},{5},{6},{7},{8},'{9}')"
                   ,fail_.Des_number,fail_.Order,fail_.Material,fail_.Fail_reason,fail_.Form_int,fail_.PLC_int,fail_.Amount,fail_.Amount
                   ,fail_.Idex,fail_.Count , fail_.Emp_name);
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
                    MessageBox.Show("数据库fail_enqueue写入数据失败，0行成功" + "'/n'" + mysqlStr, "错误");
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

        public bool insertToTemp_RGV()
        {
            string mysqlStr = string.Format("insert into temp_rgv(Description_number,Start_shelf,End_shelf,Steep1,Steep2,Take,Taked,Put,Puted) " +
                    "values('{0}',{1},{2},{3},{4},{5},{6},{7},{8})",
                    Parameter.TempRGV.Coods, Parameter.TempRGV.Start_shelf,
                    Parameter.TempRGV.Groal_shelf, Parameter.TempRGV.StartSteep1,
                    Parameter.TempRGV.StartSteep2,Parameter.TempRGV.Take,
                    Parameter.TempRGV.Taked,Parameter.TempRGV.Put,
                    Parameter.TempRGV.Puted);
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
                    MessageBox.Show("数据库temp_RGV写入数据失败，0行成功" + "'/n'" + mysqlStr, "错误");
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

        public void update_sql(DataTable dt)
        {
            string mysqlStr = string.Format("select Description_number '商品编号',Customer_number '客户编号',Description_name '商品名称'," +
                "Net_length '长',Net_width '宽',Net_hight '高'from description_infro ");
            Open();
            MySqlDataAdapter da = new MySqlDataAdapter(mysqlStr, mycon);
            MySqlCommandBuilder ms_CB = new MySqlCommandBuilder(da);

            da.Update(dt);
        }
        public void insert2(string bianhao, string kehuhao, string name, int length, int width, int hight, string caozuo)
        {
            //Open();//打开通讯通道
            string mysqlStr = string.Format("insert into description_infro(Description_number,Customer_number,Description_name,Net_length,Net_width,Net_hight) " +
                    "values('{0}','{1}','{2}',{3},{4},{5})",
                    bianhao, kehuhao, name, length, width, hight);
            MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
            using (mySqlCommand = new MySqlCommand(mysqlStr, mycon))
                if (mySqlCommand.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show(caozuo + "成功");
                    Close();
                }
        }

    }
    

    public class Update : Connection
    {

    }

    public class Delet : Connection
    {
        public bool deletToQueue(des_queue des_)
        {
            string mysqlStr = string.Format("delete from enqueue where Description_number={0} and ID={1}",
                des_.Des_number, des_.ID);
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
                    MessageBox.Show("数据库enqueue删除失败，0行成功" + "'/n'" + mysqlStr, "错误");
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
        public void delete(string bianhao)
        {
            Open();//打开通讯通道
            string mysqlStr = string.Format("delete from description_infro  where Description_number='{0}'", bianhao);
            MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
            using (mySqlCommand = new MySqlCommand(mysqlStr, mycon))
                if (mySqlCommand.ExecuteNonQuery() > 0)
                {
                    //MessageBox.Show("删除成功");
                    Close();
                }
        }
    }
    /// <summary>
    /// 混合类的SQL
    /// </summary>
    public class MIXSQL : Connection
    {
        /// <summary>
        /// 向数据库storage_position表中插入数据对应更改
        /// </summary>
        /// <param name="des_number"></param>
        /// <param name="shelf"></param>
        /// <param name="goods_length"></param>
        /// <param name="goods_width"></param>
        /// <param name="count"></param>
        /// <param name="oper_Emp"></param>
        public bool insertIntake(string des_number, int shelf, int goods_length, int goods_width, int count, oper_emp oper_Emp)
        {
            string mysqlStr = string.Format("select * from storage_status where Shelf_number={0}", shelf);
            Open();//打开通讯通道
            MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
            MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();
            try
            {
                if (mysqldr.Read() == true)
                {
                    if ((Int32)mysqldr[2] + goods_length + Parameter.Class_Parameter.Gap >= (Int32)mysqldr[4])//检查容量是否允许
                    {
                        MessageBox.Show("目标货架剩余容量无法存放该货物，请更换目标货架。", "错误");
                        mysqldr.Close();
                        Close();
                        return false;
                    }
                    if ((Int32)mysqldr[3] == 0)//没有该货架号，不用改动其它货位
                    {
                        mysqldr.Close();
                    }
                    else//原有货位+1
                    {

                        mysqldr.Close();
                        mysqlStr = string.Format("update storage_position set Allocation=Allocation+1 where Shelf_number={0}", shelf);
                        mySqlCommand = new MySqlCommand(mysqlStr, mycon);

                        if (mySqlCommand.ExecuteNonQuery() > 0)
                        {

                        }
                        else
                        {
                            MessageBox.Show("数据库更改失败，0行成功" + "'/n'" + mysqlStr, "错误");
                            Close();
                            return false;
                        }

                    }

                }

                mysqlStr = string.Format("insert into storage_position(Description_number,Length,Width,Amount,Operator,Shelf_number,Allocation) " +
                    "values('{0}',{1},{2},{3},'{4}',{5},{6})",
                    des_number, goods_length, goods_width, count, oper_Emp.Name, shelf, 1);

                using (mySqlCommand = new MySqlCommand(mysqlStr, mycon))

                if (mySqlCommand.ExecuteNonQuery() > 0)
                {

                }
                else
                {
                    MessageBox.Show("数据库插入失败，0行成功" + "'/n'" + mysqlStr, "错误");
                    Close();
                    return false;
                }


                mysqlStr = string.Format("update storage_status set Used_capacity=Used_capacity+{0}+{1},Total_storage=Total_storage+1 where Shelf_number={2}",
                    goods_length, Parameter.Class_Parameter.Gap, shelf);

                using (mySqlCommand = new MySqlCommand(mysqlStr, mycon))

                if (mySqlCommand.ExecuteNonQuery() > 0)
                {
                    Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("数据库更改失败，0行成功" + "'/n'" + mysqlStr, "错误");
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

        public bool updateOutput(int shelf, oper_emp oper_Emp)
        {
            string mysqlStr = string.Format("select * from storage_position where Shelf_number={0} and Allocation=1", shelf);
            int des_lenght = 0;

            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();
                //查询数据是否存在
                if (mysqldr.Read())
                {
                    des_lenght = Convert.ToInt32(mysqldr[5]);
                }
                else
                {
                    MessageBox.Show("该货架无该货物" + "'/n'" + mysqlStr, "提示");
                    mysqldr.Close();
                    Close();
                    return false;

                }
                mysqldr.Close();
                //删除数据
                mysqlStr = string.Format("delete from storage_position where Shelf_number={0} and Allocation=1", shelf);
                using (mySqlCommand = new MySqlCommand(mysqlStr, mycon))

                    if (mySqlCommand.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {
                        MessageBox.Show("数据库删除失败，0行成功" + "'/n'" + mysqlStr, "错误");
                        Close();
                        return false;
                    }


                

                //检查是不是第一个
                mysqlStr = string.Format("select * from storage_status where Shelf_number={0}", shelf);
                using (mySqlCommand = new MySqlCommand(mysqlStr, mycon))
                using (mysqldr = mySqlCommand.ExecuteReader())
                if (mysqldr.Read())
                {
                    if ((Int32)mysqldr[3] == 1)
                    {
                        //该货架原本只有一个货，不用该其它货位
                        mysqldr.Close();
                    }
                    else
                    {

                        mysqldr.Close();

                        //原有货位-1


                            mysqlStr = string.Format("update storage_position set Allocation=Allocation-1 where Shelf_number={0}", shelf);
                        using (mySqlCommand = new MySqlCommand(mysqlStr, mycon))

                        if (mySqlCommand.ExecuteNonQuery() > 0)
                        {

                        }
                        else
                        {
                            MessageBox.Show("数据库货位更新失败，0行成功" + "'/n'" + mysqlStr, "错误");
                            Close();
                            return false;
                        }

                    }
                       
                }

                


                //status表更新
                mysqlStr = string.Format("update storage_status set Used_capacity=Used_capacity-{0}-{1},Total_storage=Total_storage-1 where Shelf_number={2}",
                    des_lenght, Parameter.Class_Parameter.Gap, shelf);

                using (mySqlCommand = new MySqlCommand(mysqlStr, mycon))

                    if (mySqlCommand.ExecuteNonQuery() > 0)
                    {
                        Close();
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("数据库更改失败，0行成功" + "'/n'" + mysqlStr, "错误");
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

        public bool updateEXSteep1(int shelf_start, int shelf_end)
        {
            string mysqlStr = string.Format("select * from storage_position where Shelf_number={0} and Allocation=1", shelf_start);
            int des_lenght = 0;
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();
                //查询数据是否存在
                if (mysqldr.Read())
                {
                    des_lenght = Convert.ToInt32(mysqldr[5]);
                }
                else
                {
                    MessageBox.Show("该货架无该货物" + "'/n'" + mysqlStr, "提示");
                    mysqldr.Close();
                    Close();
                    return false;

                }
                mysqldr.Close();


                //更改起始货架货物货架,将货位置为0作为一种中间状态 
                mysqlStr = string.Format("update storage_position set Shelf_number={0},Allocation=0 where Shelf_number={1} and Allocation=1",
                    shelf_end, shelf_start);
                mySqlCommand = new MySqlCommand(mysqlStr, mycon);

                    if (mySqlCommand.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {
                        MessageBox.Show("数据库货位更改失败，0行成功" + "'/n'" + mysqlStr, "错误");
                        Close();
                        return false;
                    }

                //起始货架原有货位-1
                mysqlStr = string.Format("select * from storage_status where Shelf_number={0}", shelf_start);
                mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                mysqldr = mySqlCommand.ExecuteReader();
                if (mysqldr.Read())
                {
                    if ((Int32)mysqldr[3] == 1)
                    {
                        //该货架原本只有一个货，不用该其它货位
                        
                    }
                    else
                    {
                        mysqldr.Close();
                        //原有货位-1
                        mysqlStr = string.Format("update storage_position set Allocation=Allocation-1 where Shelf_number={0}", shelf_start);
                        using (mySqlCommand = new MySqlCommand(mysqlStr, mycon))

                        if (mySqlCommand.ExecuteNonQuery() > 0)
                        {

                        }
                        else
                        {
                            MessageBox.Show("数据库货位更新失败，0行成功" + "'/n'" + mysqlStr, "错误");
                            Close();
                            return false;
                        }

                    }
                }
                mysqldr.Close();

                //起始货架status表更新
                mysqlStr = string.Format("update storage_status set Used_capacity=Used_capacity-{0}-{1},Total_storage=Total_storage-1 where Shelf_number={2}",
                    des_lenght, Parameter.Class_Parameter.Gap, shelf_start);

                mySqlCommand = new MySqlCommand(mysqlStr, mycon);

                if (mySqlCommand.ExecuteNonQuery() > 0)
                {
                    Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("数据库更改失败，0行成功" + "'/n'" + mysqlStr, "错误");
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
        public bool updateEXSteep2(int shelf_end)
        {
            string mysqlStr = string.Format("select * from storage_position where Shelf_number={0} and Allocation=0", shelf_end);
            int des_lenght = 0;
            Open();//打开通讯通道
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();
                //查询数据是否存在
                if (mysqldr.Read())
                {
                    des_lenght = Convert.ToInt32(mysqldr[5]);//得到尺寸
                }
                else
                {
                    MessageBox.Show("该货架无该货物" + "'/n'" + mysqlStr, "提示");
                    mysqldr.Close();
                    Close();
                    return false;

                }
                mysqldr.Close();

                //新货架货位+1
                mysqlStr = string.Format("update storage_position set Allocation=Allocation+1 where Shelf_number={0}", shelf_end);
                mySqlCommand = new MySqlCommand(mysqlStr, mycon);

                if (mySqlCommand.ExecuteNonQuery() > 0)
                {

                }
                else
                {
                    MessageBox.Show("数据库货位更新失败，0行成功" + "'/n'" + mysqlStr, "错误");
                    Close();
                    return false;
                } 
                //目标货架更新
                mysqlStr = string.Format("update storage_status set Used_capacity=Used_capacity+{0}+{1},Total_storage=Total_storage+1 where Shelf_number={2}",
                    des_lenght, Parameter.Class_Parameter.Gap, shelf_end);

                mySqlCommand = new MySqlCommand(mysqlStr, mycon);

                if (mySqlCommand.ExecuteNonQuery() > 0)
                {
                    Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("数据库更改失败，0行成功" + "'/n'" + mysqlStr, "错误");
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


        public int  outPutLayout(string des_,ref List<int> exlist,ref int all_shelf)
        {

            try
            {
                //复制缓冲表格

                Open();//打开通讯通道
                int star_shelf = 0;//规划的起始货架
                int star_all = 0;//规划的起始位置
                List<int> capa_shelf = new List<int>();//记录所有
                MySqlCommand mySqlCommand = new MySqlCommand(
                    "truncate temp_pos;" +
                    "truncate temp_stu;" +
                    "insert temp_pos select * from storage_position;" +
                    "insert temp_stu select * from storage_status", mycon);
                if (mySqlCommand.ExecuteNonQuery() > 0)
                {

                }
                else
                {
                    Close();
                    return 2;
                }


                //开始规划
                string mysqlStr = string.Format("select Shelf_number,Allocation from temp_pos where Description_number='{0}'" +
               " order by Allocation,Shelf_number", des_);

                mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                MySqlDataReader mysqldr = mySqlCommand.ExecuteReader();

                if (mysqldr.Read())
                {
                    if ((Int32)mysqldr[1] == 1)//有不用移库的位置，直接返回
                    {
                        all_shelf = (Int32)mysqldr[0];
                        return 0;
                    }
                    else
                    {
                        star_shelf = (Int32)mysqldr[0];//给定要移库的货架
                        all_shelf = star_shelf;//给定定位
                        star_all = (Int32)mysqldr[1];//给定要移库的货物位置
                    }
                }
                mysqldr.Close();

                for (int i = 0; i < star_all - 1; i++)//开始规划移库位置
                {
                    //查找能放的下目标货架最外面货物的货架,不会找起始货架
                    mysqlStr = string.Format("select Shelf_number from temp_stu where Used_capacity+(select Length from temp_pos where Shelf_number={0} and Allocation=1)+{1}<Max_length and Shelf_number!={2}"
                        , star_shelf, Parameter.Class_Parameter.Gap, star_shelf);
                    capa_shelf.Clear();//先清空一次
                    mySqlCommand = new MySqlCommand(mysqlStr, mycon);
                    mysqldr = mySqlCommand.ExecuteReader();

                    while (mysqldr.Read())//得到可行货架序列
                    {
                        capa_shelf.Add((Int32)mysqldr[0]);
                    }
                    mysqldr.Close();
                    if (capa_shelf.Count == 0)//没有可行货架
                    {
                        Close();

                        return 1;
                    }

                    int minshelf = 10000;
                    int mindis = 10000;
                    int tempdis = 0;
                    //求距离最小
                    foreach (var capaShelf in capa_shelf)
                    {
                        tempdis = Convert.ToInt32(Math.Pow(star_shelf / 10 - capaShelf / 10, 2.0) + Math.Pow(star_shelf % 10 - capaShelf % 10, 2.0) * 2);
                        if (tempdis < mindis)
                        {
                            minshelf = capaShelf;
                            mindis = tempdis;
                        }
                    }

                    //开始更改货架状态

                    //更改起始货架货物货架,将货位置为0作为一种中间状态 
                    mysqlStr = string.Format("update temp_pos set Shelf_number={0},Allocation=0 where Shelf_number={1} and Allocation=1",
                        minshelf, star_shelf);
                    mySqlCommand = new MySqlCommand(mysqlStr, mycon);

                    if (mySqlCommand.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {

                        Close();
                        return 2;
                    }
                    //原有货位-1
                    mysqlStr = string.Format("update temp_pos set Allocation=Allocation-1 where Shelf_number={0}", star_shelf);
                    using (mySqlCommand = new MySqlCommand(mysqlStr, mycon))

                        if (mySqlCommand.ExecuteNonQuery() > 0)
                        {

                        }
                        else
                        {

                            Close();
                            return 2;
                        }

                    //目标货架status表更新
                    mysqlStr = string.Format("update temp_stu set Used_capacity=Used_capacity+{0}+" +
                        "(select Length from temp_pos where Shelf_number={1} and Allocation=0),Total_storage=Total_storage+1 " +
                        "where Shelf_number={1}",
                        Parameter.Class_Parameter.Gap, minshelf);

                    mySqlCommand = new MySqlCommand(mysqlStr, mycon);

                    if (mySqlCommand.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {

                        Close();
                        return 2;
                    }

                    //目标货架原有货位+1
                    mysqlStr = string.Format("update temp_pos set Allocation=Allocation+1 where Shelf_number={0}", minshelf);
                    mySqlCommand = new MySqlCommand(mysqlStr, mycon);

                    if (mySqlCommand.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {

                        Close();
                        return 2;
                    }

                    exlist.Add(minshelf);
                }


                if (exlist.Count == star_all - 1)//得到全部
                {

                    Close();
                    return 0;
                }
                else
                {

                    Close();
                    return 1;
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                Close();
                return -1;


            }
            
           
        }

    }



    
    /// <summary>
    /// 用于通讯的建立，关闭，状态检查
    /// </summary>
    public class Connection
    {

        //构建数据库连接字符串
        protected string M_str_sqlcon = "server=localhost;user id=root;password=12345678;database=deehero"; //根据自己的设置
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
