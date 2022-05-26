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
        public FormToChangeQueue(Device device )
        {
            nowDevice = device;
            InitializeComponent();
        }

        private void FormToChangeQueue_Load(object sender, EventArgs e)
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
    }
}
