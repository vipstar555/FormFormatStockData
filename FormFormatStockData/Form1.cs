using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormFormatStockData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dateTimePicker2_CloseUp(object sender, EventArgs e)
        {            
            dataGridView1.DataSource = YahooFInanceConToViewData.TradeIndexsWhereDatetime(dateTimePicker2.Value);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Length != 4)
            {
                return;
            }
            if(int.TryParse(textBox1.Text, out int code))
            {
                dataGridView1.DataSource = YahooFInanceConToViewData.TradeIndexsWhereCode(code);
            }
            else
            {
                MessageBox.Show("数字を入力してください");
            }
        }
    }
}
