using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HslControls;
namespace HSL采样
{
    public partial class Sample : Form
    {
        public Sample()
        {
            InitializeComponent();
        }

        private void Sample_Load(object sender, EventArgs e)
        {
            if (!HslControls.Authorization.SetAuthorizationCode
("9dc51fce-98dc-48a8-9b8e-1a12f4b3b091"))
            {
                Console.WriteLine("授权错误");
                MessageBox.Show("授权错误");
            }

            hslCurve1.SetLeftCurve("仪表电流", null, Color.Blue,true);    // 设置曲线的名称，数据 空，颜色为蓝色            
            hslCurve1.SetLeftCurve("实际电流", null, Color.Red,true);    // 设置曲线的名称，数据空， 颜色为红色 
            hslCurve1.AddLeftAuxiliary(60f, Color.Green);           //设置辅助线
            random = new Random( );            
            timer1 = new Timer( );          
            timer1.Interval = 100;     // 每隔500毫秒触发一次 15.             
            timer1.Tick += timer1_Tick;           
            timer1.Start( ); 
        }
        //做一个随机函数
        Random random = new Random();
        private Timer timer1;
        private int timeTick = 0;
        private float[] GetRandomData(int length,int max)
        {
            float[] buffer = new float[length];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = random.Next(max);
            }
            return buffer;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float value1 = (float)(Math.Sin(2 * Math.PI * timeTick / 100) * 40 + 40);
            float value2 = (float)(Math.Sin(2 * Math.PI * timeTick / 100) * 10 + 20);
            //float value1 = (float)shareData.panelCurrent;
            //float value2 = (float)shareData.actualCurrent;
            hslCurve1.AddCurveData(
            DateTime.Now.ToString(),
            new string[] { "仪表电流", "实际电流" },
            new float[] { value1, value2 });
            timeTick++;

        }

        private void hslCurve1_Load(object sender, EventArgs e)
        {

        }

        private void HslCurve1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
