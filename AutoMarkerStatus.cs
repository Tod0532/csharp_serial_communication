using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HSL采样
{
    public partial class AutoMarkerStatus : Form
    {
        Random random = new Random();
        public AutoMarkerStatus()
        {
            InitializeComponent();
        }

        private void hslLanternSimple1_Load(object sender, EventArgs e)
        {
           


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (shareData.valueStatus)
            {
                case 0:
                    hslLanternSimple1.Text = "停止状态";
                    hslLanternSimple1.LanternBackground = Color.Yellow;
                    break;
                case 1:
                    hslLanternSimple1.Text = "电机启动状态";
                    hslLanternSimple1.LanternBackground = Color.Green;
                    break;
                case 2:
                    hslLanternSimple1.Text = "激光打标状态";
                    hslLanternSimple1.LanternBackground = Color.Blue;
                    break;
                default:
                    break;
            }
            textBox3.Text = shareData.valueCount.ToString();
            textBox1.Text = shareData.valueWait.ToString();
            textBox2.Text = shareData.valueRequest.ToString();
            textBox4.Text = shareData.test;
           // hslBarChart1.

        }

        private void AutoMarkerStatus_Load(object sender, EventArgs e)
        {

            //柱型图1
            hslBarChart1.AddLeftAuxiliary(50, Color.Yellow);//辅助线
            hslBarChart1.SetDataSource(new int[] { 22, 55, 77, 89, 33, 188 }, new string[] { "周一", "周二", "周三", "周四", "周五", "周五" });//设置内部数据及X轴内容


            //柱型图2
            List<int> month = new List<int>();
            List<string> day = new List<string>();
            for (int i = 1; i <=31; i++)
            {
                month.Add( random.Next(1500));
                day.Add(i.ToString());
            }
            hslBarChart2.AddLeftAuxiliary(1000, Color.Yellow);
            hslBarChart2.SetDataSource(month.ToArray(), day.ToArray(), month.Select(m => m < 1000 ? Color.Orchid : Color.Gold).ToArray());

        }

        private void HslBarChart1_Load(object sender, EventArgs e)
        {

        }
    }
}
