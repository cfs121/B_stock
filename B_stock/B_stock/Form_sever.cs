using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySQL;
using Parameter;

namespace B_stock
{
    public partial class Form_sever : Form
    {
        //全局通用的变量
        oper_emp oper_Emp = new oper_emp();//全局通用员工属性
        string severAddres;
        int COM;
        int backLog;
        Socket socketReceive;
        //存入连接上的客户端间的socket
        Dictionary<string, Socket> clientSocket = new Dictionary<string, Socket>();
        Dictionary<string, string> deviceClient = new Dictionary<string, string>(); 
        //存入连接上的客户端监听的线程
        Dictionary<string, Thread> clientThread = new Dictionary<string, Thread>();
        List<string> test = new List<string>();



        public void getQUEUE()
        {
            MySQL.Select select = new Select();
            textBox2.Text= select.getAllQueue();

        }
        public void getTest()
        {
            DataSet data = new DataSet();
            MySQL.Select select = new Select();
            if (select.getTask(out data))
            {
                dataGridView1.DataSource = data.Tables[0];
            }

        }
        private void showMes(string mes)
        {
            this.textBox4.AppendText(mes + Environment.NewLine);
        }
        private void socketAccept(object o)
        {
            Socket socketWatch = o as Socket;
            while (true)
            {
                try
                {
                    socketReceive = socketWatch.Accept();
                    clientSocket.Add(socketReceive.RemoteEndPoint.ToString(), socketReceive);
                    comboBox1.Items.Add(socketReceive.RemoteEndPoint.ToString());
                    test.Add(socketReceive.RemoteEndPoint.ToString());
                    showMes(socketReceive.RemoteEndPoint.ToString() + ": 连接成功");
                    Thread th = new Thread(socketSent);
                    th.IsBackground = true;
                    th.Start(socketReceive);
                }
                catch
                { }

            }
        }
        private void socketSent(object o)
        {
            Socket socketReceive = o as Socket;
            while (true)
            {
                try
                {
                    //接收发来的信息
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    int i = socketReceive.Receive(buffer);
                    if (i == 0)
                    {
                        break;
                    }
                    string strReceive = Encoding.UTF8.GetString(buffer, 0, i);
                    showMes("接收 " + socketReceive.RemoteEndPoint.ToString() + " : " + strReceive);
                    analy(strReceive, socketReceive.RemoteEndPoint.ToString());

                }
                catch (Exception ex)
                {


                }


            }
        }

        private void  socketSend(string mes,string device_number)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(mes);
            clientSocket[deviceClient[device_number]].Send(buffer);
            showMes("发送给" +device_number+" "+ clientSocket[deviceClient[device_number]].RemoteEndPoint.ToString() + " : " + mes);
            
        }

        private void check()
        {
            Socket tempSocket;

            while (true)
            {
                try
                {

                    for (int i = 0; i < test.Count; i++)
                    {
                        tempSocket = clientSocket[test[i]];
                        //连接是否断开
                        if (tempSocket.Poll(10000, SelectMode.SelectRead))
                        {
                            //断开
                            try
                            {
                                byte[] data = new byte[1024 * 1024 * 2];

                                if (tempSocket.Receive(data) == 0)
                                {
                                    //正常断开连接；
                                    showMes(tempSocket.RemoteEndPoint.ToString() + ": 正常断开连接");
                                    comboBox1.Items.Remove(tempSocket.RemoteEndPoint.ToString());
                                    clientSocket.Remove(test[i]);
                                    test.Remove(test[i]);
                                }
                            }
                            catch
                            {
                                showMes(tempSocket.RemoteEndPoint.ToString() + ": 异常断开连接");
                                comboBox1.Items.Remove(tempSocket.RemoteEndPoint.ToString());
                                clientSocket.Remove(test[i]);
                                test.Remove(test[i]);
                            }
                        }
                    }



                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }

            }
        }

        private void analy(string mes,string address)
        {
            string[] conet = mes.Split('+');
            switch (conet[0])
            {
                case "1"://指定设备号，用作初始socket建立时
                    deviceClient.Add(conet[1],address);
                    return;
                default:
                    break;
            }

        }








        public Form_sever(oper_emp login_emp)
        {
            InitializeComponent();
            oper_Emp = login_emp;
            //允许调用线程
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void Form_sever_Load(object sender, EventArgs e)
        {
            getQUEUE();
            getTest();
            //得到IP和端口号
            MySQL.Select select = new Select();
            if (select.getSocket(out severAddres,out COM,out backLog))//成功得到socket参数
            {
                //创建监听socket
                try
                {
                    //当点击开始监听的时候 在服务器端创建一个负责监听IP地址和端口号的Socket
                    Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //获取ip地址
                    IPAddress ip = IPAddress.Parse(this.severAddres.Trim());
                    //创建端口号
                    IPEndPoint point = new IPEndPoint(ip, COM);
                    //绑定IP地址和端口号
                    socketWatch.Bind(point);
                    //开始监听:设置最大可以同时连接多少个请求
                    socketWatch.Listen(backLog);
                    textBox5.Text = "正在监听";
                    Thread lisenStocket = new Thread(socketAccept);
                    lisenStocket.IsBackground = true;
                    lisenStocket.Start(socketWatch);
                    Thread checkStocket = new Thread(check);
                    checkStocket.IsBackground = true;
                    checkStocket.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());

                }
            }
           
        }
    }
}
