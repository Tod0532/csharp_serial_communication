using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using System.Threading;
using System.IO;
using CCWin;

namespace HSL采样
{
    //自定义一个公共类

    public partial class Form1 : CCSkinMain
    {
        SiemensS7Net siemens = new SiemensS7Net(SiemensPLCS.S1200, "192.168.0.1");
        //private Thread Job1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            if (!HslCommunication.Authorization.SetAuthorizationCode
("7a6b1f26-ae6e-4399-b2d4-e09246644028"))
            {
                Console.WriteLine("授权错误");
                MessageBox.Show("授权错误");
            }
            siemens.SetPersistentConnection();
            //Job1 = new Thread (MyJob);
            //Job1.Start();
           // new Thread(new ThreadStart(MyJob)) { IsBackground = true }.Start();
            
        }



        private void 保存数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "数据文件(*.dat)|*.dat|All files(*.*)|*.*";//设置为数据文件或所有文件
            sfd.DefaultExt = "dat"; //设置默认扩展名为dat 
            if (sfd.ShowDialog() == DialogResult.OK) //打开对话框 
            {
                try
                {
                    StreamWriter m_SW = new StreamWriter(sfd.FileName);// 实例化StreamWriter类 
                    m_SW.Write(DateTime.Now);
                    m_SW.Write("  ActualCurrent:   "); // 写入数据 
                    m_SW.Write(shareData.actualCurrent+1); // 写入数据 
                    m_SW.WriteLine(""); // 写入数据 
                    m_SW.Close();
                }// 关闭文件 } 
                catch (IOException ex)
                { MessageBox.Show("写入出错！\n" + ex.Message); }
            }

        }

        private void 读取数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "数据文件(*.dat)|*.dat|All files(*.*)|*.*";//设置为数据文件或所有文件
            ofd.DefaultExt = "dat"; //设置默认扩展名为dat f
            if (ofd.ShowDialog() == DialogResult.OK) //打开对话框 
            {
                try
                {
                    StreamReader m_SW = new StreamReader(ofd.FileName);// 实例化StreamWriter类 
                    shareData.test = m_SW.ReadLine();
                    m_SW.Close();
                }// 关闭文件 } 
                catch (IOException ex)
                { MessageBox.Show("写入出错！\n" + ex.Message); }
            }
        }

        private void 串口通讯ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SerialMain m = new SerialMain();
            m.MdiParent = this;
            m.Show();

        }

        private void 版本信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }
    }
    public class shareData
    {
        
        public static int valueStatus;
        public static long valueCount;
        public static int valueWait;
        public static int valueRequest;
        public static int panelCurrent;
        public static int actualCurrent;
        public static string test;
    }
}
