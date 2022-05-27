using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parameter;

namespace B_stock
{
    public partial class FormToChangeQueue : Form
    {
        Device nowDevice;

        public void reSetQueue()
        {
            MySQL.Select select = new MySQL.Select();
            DataSet dataSet = new DataSet();
            if (select.getqueue(nowDevice, out dataSet))
            {
                dataGridView1.DataSource = dataSet.Tables[0];

            }
            else
            {
                MessageBox.Show("获取排单信息失败", "提示");
            }


        }
        public FormToChangeQueue(Device device )
        {
            nowDevice = device;
            InitializeComponent();
        }

        private void FormToChangeQueue_Load(object sender, EventArgs e)
        {
            reSetQueue();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            reSetQueue();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string mess = "";
            if (dataGridView1.SelectedRows[0].Index == 0)
            {
                result = MessageBox.Show("不建议选择排序较前的指令，因为该指令可能已在执行\n为防止删除已在执行的指令，请确定该指令的" +
                    "状态或者执行删除后，后台会进行检测是否适合删除\n 点击确认继续执行，点击取消撤回该操作", "提示", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                   
                }
                else
                {
                    return;
                }
            }

            
            mess = mess +"主键： "+ dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[0].Value.ToString()+"\n"
                +"编号： "+dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[1].Value.ToString() + "\n";


            result = MessageBox.Show("您要删除"+"\n"+mess, "提示", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                //删除队列
                MySQL.Delet delet = new MySQL.Delet();
                if (delet.de_queue(Convert .ToInt32 (dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[0].Value)))
                {
                    MessageBox.Show("删除成功","提示");
                    reSetQueue();
                }

                else MessageBox.Show("删除失败", "提示"); 



            }
            else
            {
                return;
            }

        }
    }
}
