using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MySQL;
using Parameter;

namespace B_stock
{
    public partial class test : Form
    {

        Thread th1;
        Thread th2;


        public void read(int sp,int textbox)
        {
            MySQL.Select select = new Select();
            Device device = new Device();
            device.Device_number = "2";
          

            while (true)
            {
                string infro = "";
                DateTime dateTime = DateTime.Now;
                infro = infro + dateTime.ToString();
                infro = infro + "\n"+select.getQueueAfterInsert(device);
                if (textbox == 1)
                {
                    textBox1.Text = infro;
                }
                else
                {
                    textBox2.Text = infro;
                }
               

            }
            


        }

        public test()
        {
            InitializeComponent();
            Form.CheckForIllegalCrossThreadCalls = false;//允许其它线程
        }

        private void test_Load(object sender, EventArgs e)
        {
            th1 = new Thread(() => read(500,1));
            th2 = new Thread(() => read(500,2));
            th1.IsBackground = true;
            th2.IsBackground = true;
            th1.Start();
            th2.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySQL.Select select = new Select();
            Device device = new Device();
            device.Device_number = "8";
            string infro = "";
            
            DateTime dateTime = DateTime.Now;
            infro =  dateTime.ToString();
            string st2= select.getQueueAfterInsert(device);
            if (st2=="")
            {
                infro = infro + "\n" + "空";
            }
            textBox3.Text = infro;

            
        }

        private void test_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (th1.IsAlive)
            {
                th1.Abort();
            }
            if (th2.IsAlive)
            {
                th2.Abort();
            }
        }
    }
}
