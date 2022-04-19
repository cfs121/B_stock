using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parameter
{
    public class Class_para
    {
    } 
    interface Client
    {
        int Intake();//入库操作
        int Output();//出库操作
        int LogData();//录入信息

        int UpData();//修改信息

    }
    /// <summary>
    /// 初始化系统的一些参数
    /// </summary>
    public  class Class_Parameter
    {
        private static int gap;

        public static int Gap//货物间隙
        {
            get { return gap; }
            set { gap = value; }
        }

        private static int rgv_speed;

        public static int RGV_speed//rgv移动速度
        {
            get { return rgv_speed; }
            set { rgv_speed = value; }
        }
        private static int track_length;

        public static int Track_length//轨道长度
        {
            get { return track_length; }
            set { track_length = value; }
        }
        private static int group_gap;

        public static int Group_gap
        {
            get { return group_gap; }
            set { group_gap = value; }
        }
        private static int shelf_gap;

        public static int Shelf_gap
        {
            get { return shelf_gap; }
            set { shelf_gap = value; }
        }

        private static string sever_addres;

        public static string Sever_addres
        {
            get { return sever_addres; }
            set { sever_addres = value; }
        }

        private static int com;

        public static int COM
        {
            get { return com; }
            set { com = value; }
        }





    }
    /// <summary>
    /// 用于记录操作员实例
    /// </summary>
    public class oper_emp
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string emp_number;

        public string Emp_number
        {
            get { return emp_number; }
            set { emp_number = value; }
        }
        private int level;

        public int Level
        {
            get { return level; }
            set { level = value; }
        }
    }
    /// <summary>
    /// 静态类，用于存储rgv以及其运行货物的信息
    /// </summary>
    public class RGV
    {
        private static string coods;

        public static string Coods
        {
            get { return coods; }
            set { coods = value; }
        }
        private static int start_location;

        /// <summary>
        /// 控件的起始位置的X坐标
        /// </summary>
        public static int Start_location
        {
            get { return start_location; }
            set { start_location = value; }
        }

        private static int now_shelf;

        public static int Now_shelf
        {
            get { return now_shelf; }
            set { now_shelf = value; }
        }
        private static int groal_shelf;

        public static int Groal_shelf
        {
            get { return groal_shelf; }
            set { groal_shelf = value; }
        }
        private static int start_shelf;

        public static int Start_shelf
        {
            get { return start_shelf; }
            set { start_shelf = value; }
        }


        private static bool start_steep1;
        /// <summary>
        /// RGV开始工作的标志位,PLC去取纸，D2000=778
        /// </summary>
        public static bool StartSteep1
        {
            get { return start_steep1; }
            set { start_steep1 = value; }
        }

        private static bool start_steep2;
        /// <summary>
        /// RGV开始工作的标志位,PLC去放纸，D2000=778
        /// </summary>
        public static bool StartSteep2
        {
            get { return start_steep2; }
            set { start_steep2 = value; }
        }
        private static bool take;
        /// <summary>
        /// 货物上了rgv---PLC完成接收纸堆，D2000=781
        /// </summary>
        public static bool Take
        {
            get { return take; }
            set { take = value; }
        }

        private static bool put;
        /// <summary>
        /// 货物下了rgv---PLC完成排出纸堆，D2000=785
        /// </summary>
        public static bool Put
        {
            get { return put; }
            set { put = value; }
        }


        private static bool sta_take;
        /// <summary>
        /// PLC开始接收纸堆，D2000=780
        /// </summary>
        public static bool sta_Take
        {
            get { return sta_take; }
            set { sta_take = value; }
        }

        private static bool sta_put;
        /// <summary>
        /// PLC开始排出纸堆，D2000=784
        /// </summary>
        public static bool sta_Put
        {
            get { return sta_put; }
            set { sta_put = value; }
        }
        private static bool  successed;
        /// <summary>
        /// PLC指令过程中一旦执行出错，该处置位为假
        /// </summary>
        public static bool  Successed
        {
            get { return successed; }
            set { successed = value; }
        }

        private static bool artfi;


        public  static bool Artfi
        {
            get { return artfi; }
            set { artfi = value; }
        }


        public static void clear()
        {
            Successed = true;
            Put = false;
            sta_Take = false;
            Take = false;
            sta_Put = false;
            StartSteep1 = false;
            StartSteep2 = false;
            Coods = "";
            Groal_shelf = 0;
            Start_shelf = 0;
            Artfi = false;
            
        }

        public static string stringlist()
        {

            string list ="cood: " +Coods + System.Environment.NewLine +
                "Start_location: " + Start_location.ToString() + System.Environment.NewLine +
               "Groal_shelf: " + Groal_shelf.ToString() + System.Environment.NewLine +
               "Start_shelf: " + Start_shelf.ToString() + System.Environment.NewLine +
               "start_steep1: " + start_steep1.ToString() + System.Environment.NewLine +
               "start_steep2: " + start_steep2.ToString() + System.Environment.NewLine +
               "sta_take: " + sta_Take.ToString() + System.Environment.NewLine +
               "Take: " + Take.ToString() + System.Environment.NewLine +
               "sat_put: " + sta_Put.ToString() + System.Environment.NewLine + 
               "Put: " + Put.ToString() + System.Environment.NewLine +
               "Successed: " + Successed.ToString() + System.Environment.NewLine +
               "Artfi: " + Artfi.ToString() ;
            return list;

        }



    }
    /// <summary>
    /// 存贮RGV被置为人工状态时指令状态
    /// </summary>

    public class TempRGV
    {
        private static string coods;

        public static string Coods
        {
            get { return coods; }
            set { coods = value; }
        }
       
        private static int groal_shelf;

        public static int Groal_shelf
        {
            get { return groal_shelf; }
            set { groal_shelf = value; }
        }
        private static int start_shelf;

        public static int Start_shelf
        {
            get { return start_shelf; }
            set { start_shelf = value; }
        }


        private static bool start_steep1;
        /// <summary>
        /// RGV开始工作的标志位,PLC去取纸，D2000=778
        /// </summary>
        public static bool StartSteep1
        {
            get { return start_steep1; }
            set { start_steep1 = value; }
        }

        private static bool start_steep2;
        /// <summary>
        /// RGV开始工作的标志位,PLC去放纸，D2000=778
        /// </summary>
        public static bool StartSteep2
        {
            get { return start_steep2; }
            set { start_steep2 = value; }
        }
        private static bool take;
        /// <summary>
        /// 货物上了rgv---PLC完成接收纸堆，D2000=780
        /// </summary>
        public static bool Take
        {
            get { return take; }
            set { take = value; }
        }

        private static bool put;
        /// <summary>
        /// 货物下了rgv---PLC完成排出纸堆，D2000=785
        /// </summary>
        public static bool Put
        {
            get { return put; }
            set { put = value; }
        }

        private static bool taked;
        /// <summary>
        /// 货物上了rgv---PLC完成接收纸堆，D2000=780
        /// </summary>
        public static bool Taked
        {
            get { return taked; }
            set { taked = value; }
        }

        private static bool puted;
        /// <summary>
        /// 货物下了rgv---PLC完成排出纸堆，D2000=785
        /// </summary>
        public static bool Puted
        {
            get { return puted; }
            set { puted = value; }
        }

        /// <summary>
        /// 清除全部字段的数据，准备接收下一次输入
        /// </summary>
        public static void clear()
        {
           
            Put = false;
            Puted = false;
            Take = false;
            Taked = false;
            StartSteep1 = false;
            StartSteep2 = false;
            Coods = "";
            Groal_shelf = 0;
            Start_shelf = 0;
        }
        /// <summary>
        /// 将RGV类的指令状态拷贝至此处准备存档
        /// </summary>
        public static void clone()
        {

            Put = RGV.sta_Put;
            Puted = RGV.Put;
            Take = RGV.sta_Take;
            Taked = RGV.Take;
            StartSteep1 = RGV.StartSteep1;
            StartSteep2 = RGV.StartSteep2;
            Coods = RGV.Coods;
            Groal_shelf = RGV.Groal_shelf;
            Start_shelf = RGV.Start_shelf;
        }

        public static bool Rebakced()
        {

            if
            (
                Put == false &&
                Take == false &&
                Taked==false&&
                Puted==false&&
                StartSteep1 == false &&
                StartSteep2 == false &&
                Coods == "" &&
                Groal_shelf == 0 &&
                Start_shelf == 0
            )
            {
                return true;
            }
            else return false;



        }

    }

    /// <summary>
    /// 该类用于记录具体的货架信息
    /// </summary>
    public class shelf
    {
        private int shelfNumber;

        public int ShelfNumber
        {
            get { return shelfNumber; }
            set { shelfNumber = value; }
        }

        private int lenght;

        public int Lenght
        {
            get { return lenght; }
            set { lenght = value; }
        }
        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        private bool able;

        public bool Able
        {
            get { return able; }
            set { able = value; }
        }

        private int hight;

        public int Hight
        {
            get { return hight; }
            set { hight = value; }
        }
        private int max_dis;

        public int Max_dis
        {
            get { return max_dis; }
            set { max_dis = value; }
        }
        private int min_dis;

        public int Min_dis
        {
            get { return min_dis ; }
            set { min_dis = value; }
        }




    }
    /// <summary>
    /// 图像框类，该类为静态类只实例化一次，用于记录相框的尺寸参数
    /// </summary>
    public class pictureBox
    {
        private static int lenght;

        public static int Lenght
        {
            get { return lenght; }
            set { lenght = value; }
        }
        private static int width;

        public static int Width
        {
            get { return width; }
            set { width = value; }
        }
    }
    /// <summary>
    ///商品类，封装了一些产品信息。
    /// </summary>
    public class des
    {
        private string des_number;

        public string Des_number
        {
            get { return des_number; }
            set { des_number = value; }
        }

        private string cust_number;

        public string Cust_number
        {
            get { return cust_number; }
            set { cust_number = value; }
        }
        private string des_name;

        public string Des_name
        {
            get { return des_name; }
            set { des_name = value; }
        }


        private int lenght;

        public int Lenght
        {
            get { return lenght; }
            set { lenght  = value; }
        }

        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private int hight;

        public int Hight
        {
            get { return hight; }
            set { hight = value; }
        }



    }

    public class des_queue
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }


        private string des_number;

        public string Des_number
        {
            get { return des_number; }
            set { des_number = value; }
        }

        private string order_class;

        public string Order_class
        {
            get { return order_class; }
            set { order_class = value; }
        }

        private int amount;

        public int  Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        private string mat_class;

        public string Mat_class
        {
            get { return mat_class; }
            set { mat_class = value; }
        }

        private string emp;

        public string Emp
        {
            get { return emp; }
            set { emp = value; }
        }

        private int priority;

        public int Priority 
        {
            get { return priority; }
            set { priority = value; }
        }






    }

    public class fail_queue
    {
        private string des_number;

        public string Des_number
        {
            get { return des_number; }
            set { des_number = value; }
        }
        private string order;

        public string Order
        {
            get { return order; }
            set { order = value; }
        }
        private string material;

        public string Material
        {
            get { return material; }
            set { material = value; }
        }


        private string fail_reason;

        public string Fail_reason
        {
            get { return fail_reason; }
            set { fail_reason = value; }
        }
        private int form_int;

        public int Form_int
        {
            get { return form_int; }
            set { form_int = value; }
        }

        private int plc_int;

        public int PLC_int
        {
            get { return plc_int; }
            set { plc_int = value; }
        }

        private int  amount;

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        private int idex;

        public int Idex
        {
            get { return idex; }
            set { idex = value; }
        }

        private int count;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        private string emp_name;

        public string Emp_name
        {
            get { return emp_name; }
            set { emp_name = value; }
        }

        public string list()
        {

            string reason = Des_number + "\n" +
                        Order + "\n" +
                        Material + "\n" +
                        Fail_reason + "\n" + 
                        Convert.ToString(Form_int) + "\n" +
                        Convert.ToString(PLC_int)+ "\n" +
                        Convert.ToString(Amount) + "\n" + 
                        Convert.ToString(Idex) + "\n" + 
                        Convert.ToString(Count) + "\n" + 
                        Emp_name;

            return reason;
        }

    }

    
}
