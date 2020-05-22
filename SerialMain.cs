using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using CCWin;

namespace HSL采样
{
    public partial class SerialMain : Form
    {
        #region 初始化
        private StreamReader sRead;
        SerialPort sp = new SerialPort();
        byte[] sendReadMotorDistance = { 0X7E, 0X01, 0XAF};//读取电机行程
        byte[] sendGoToZeroPosition= { 0X7E, 0X01, 0X1C };//电机回原点
        byte[] sendInDebugCode = { 0X7E, 0X01, 0XAB };//进入调试模式编码
        byte[] receiveInDebugCode = { 0X7E, 0X01, 0XBA };//确认进入调试模式编码
        byte[] sendOutDebugCode = { 0X7E, 0X01, 0XCD };//退出调试模式编码
        byte[] receiveOutDebugCode = { 0X7E, 0X01, 0XDC };//确认退出调试模式编码
        byte[] sendReadDayFormCode = { 0X7E, 0X01, 0X1A };//发读日表命令
        byte[] sendReadNightFormCode = { 0X7E, 0X01, 0X2A };//发读夜表命令
        byte[] sendWriteDayFormCode = new byte[48];//发写日表命令 47
        byte[] sendWriteNightFormCode = new byte[48];//发写夜表命令47
        byte[] sendWriteMotorTargeValue = new byte[6];//发电机目标位置
        byte[] receiveByteData = { };//接收字节型数据
        byte[] sendByteData = { };//要发送的TXT框写入的数据转BYTE
        
        int[] sendIntData = new int[22];//要发送的TXT框写入的INT
        int[] receiveIntData = new int[22];//已经接收到的BYTE数据转INT
        int[] readIntData = new int[22];//读取文件中的值，写入到TEXT框中
        double[] xChart = { 15, 17, 20, 25, 30, 35, 40, 45, 50, 69, 70, 80, 90, 100, 125, 150, 175, 200, 250, 300, 350, 400 };
        int sumC = 0;
        Byte byteSumCheck = 0x00;//存放相加后的校验和
        Control[] readtext;
        Control[] writetext;
        Series series1 = new Series("");
        Series series= new Series("");
        private static StringBuilder sb = new StringBuilder();     //为了避免在接收处理函数中反复调用,依然声明为一个全局变量
        #endregion
        public SerialMain()
        {
            InitializeComponent();         
            // 设置显示范围
            ChartArea chartArea = chart1.ChartAreas[0];
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Maximum =400;
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Maximum = 5000;
            chart1.Series.Add(series1);
            chart1.Series.Add(series);
            series1.Points.AddXY(0, 0);//清空
        }
        #region 按钮相关 打开关闭发送
        private void SerialMain_Load(object sender, EventArgs e)
        {

            groupBox3.Enabled = false;
            groupBox2.Enabled = false;
            groupBox2.Visible = false;//发送管理不可见
            groupBox4.Enabled = false;
            //groupBox7.Enabled= false;//自测不可用
            btnReadDayForm.Enabled = false;
            btnReadNightForm.Enabled = false;
            btnIssueDayForm.Enabled = false;
            btnIssueNightForm.Enabled = false;
            this.toolStripStatusLabel1.Text = "端口号：端口未打开 | ";
            this.toolStripStatusLabel2.Text = "波特率：端口未打开 | ";
            this.toolStripStatusLabel3.Text = "数据位：端口未打开 | ";
            this.toolStripStatusLabel4.Text = "停止位：端口未打开 | ";
            this.toolStripStatusLabel5.Text = "";
            this.sp.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SP_DataReceived);
            txtRealTimeValue.Enabled = false;
            //txtRealTimeValue.Text = "ok";

            readtext = new Control[] { txtRead1, txtRead2, txtRead3, txtRead4, txtRead5, txtRead6, txtRead7, txtRead8, txtRead9, txtRead10, txtRead11,
            txtRead12, txtRead13, txtRead14, txtRead15, txtRead16, txtRead17, txtRead18, txtRead19, txtRead20, txtRead21,txtRead22};
            writetext = new Control[] { txtWrite1, txtWrite2, txtWrite3, txtWrite4, txtWrite5, txtWrite6, txtWrite7, txtWrite8, txtWrite9, txtWrite10, txtWrite11,
            txtWrite12, txtWrite13, txtWrite14, txtWrite15, txtWrite16, txtWrite17, txtWrite18, txtWrite19, txtWrite20, txtWrite21,txtWrite22};
            for (int i = 0; i < readtext.Length; i++)
            {
                readtext[i].Enabled = false;
                writetext[i].Enabled = false;
                readtext[i].Text= Convert.ToString(i);
                writetext[i].Text = Convert.ToString(i);

            }
        }

        private void BtnSetSP_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            sp.Close();
            SerialP p = new SerialP();
            if (p.ShowDialog()==DialogResult.OK)
            {
                try
                {
                    sp.PortName = ShareDataCom.strProtName;//将串口号写入
                    sp.BaudRate = int.Parse(ShareDataCom.strBaudRate);
                    sp.DataBits = int.Parse(ShareDataCom.strDataBits);
                    sp.StopBits = (StopBits)int.Parse(ShareDataCom.strStopBits);//注意强制转换
                    sp.ReadTimeout = 500;//读取数据超时时间,引发ReadExisting异常
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"22222222");
                    return;
                }
   
          
            }
        }
        //打开/关闭
        private void Button2_Click(object sender, EventArgs e)
        {
            if (button2.Text=="打开串口")
            {
                if (ShareDataCom.strProtName!=""&& ShareDataCom.strBaudRate!="" && ShareDataCom.strDataBits!="" && ShareDataCom.strStopBits!="")
                {
                    
                    try
                    {
                        //若原串口已打开，先关闭，后打开
                        if (sp.IsOpen)
                        {
                            sp.Close();
                            sp.Open();//打开串口
                         
                        }
                        else
                        {
                            sp.Open();                          
                        }
                        // MessageBox.Show("ok" + sp.BaudRate + sp.PortName + sp.DataBits);
                        button2.Text = "关闭串口";
                        //groupBox2.Enabled = true;
                        groupBox3.Enabled = true;
                        this.toolStripStatusLabel1.Text = "端口号：" + sp.PortName + " | ";
                        this.toolStripStatusLabel2.Text = "波特率：" + sp.BaudRate + " | ";
                        this.toolStripStatusLabel3.Text = "数据位：" + sp.DataBits + " | ";
                        this.toolStripStatusLabel4.Text = "停止位：" + sp.StopBits + " | ";
                        this.toolStripStatusLabel5.Text = "";

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("错误：" + ex.Message, "C#串口通信");
                        return;
             
                    }
                }
                else
                {
                    MessageBox.Show("请先设置串口！", "RS232串口通信");
                }
            }
            else
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
                button2.Text = "打开串口";

                if (sp.IsOpen)
                {
                    sp.Close();
                }
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;
                this.toolStripStatusLabel1.Text = "端口号：端口未打开 | ";
                this.toolStripStatusLabel2.Text = "波特率：端口未打开 | ";
                this.toolStripStatusLabel3.Text = "数据位：端口未打开 | ";
                this.toolStripStatusLabel4.Text = "停止位：端口未打开 | ";
                this.toolStripStatusLabel5.Text = "";
            }

        }
        //点击发送数据
        private void Button3_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                try
                {              
                    sp.Encoding = System.Text.Encoding.GetEncoding("GB2312");
                    sp.Write(richTextBox1.Text);
    
                }
                catch (Exception ex)
                {

                    MessageBox.Show("错误:" + ex.Message);
                    return;
                }
            }
            else
            {
                MessageBox.Show("请先打开串口:");
            }
        }
        //选择要发送的文件
        private void Button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "txt文件(*.txt)|*.txt";
            if (open.ShowDialog()==DialogResult.OK)
            {
                try
                {
                    if (open.OpenFile()!=null)
                    {
                        txtFileName.Text = open.FileName;
                    }
                }
                catch (Exception err1)
                {

                    MessageBox.Show("文件打开错误!  " + err1.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }
        //发送文件
        private void Button7_Click(object sender, EventArgs e)
        {
            string fileName = txtFileName.Text.Trim();
            if (fileName=="")
            {
                MessageBox.Show("请选择要发送的文件","Error");
                //return;
            }
            else
            {
                sRead = new StreamReader(fileName, Encoding.Default);
            }
            timer1.Start();
        }
        //发送文件时钟
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (txtFileName.Text!= "")
            {
                try
                {
                    string str1;
                    str1 = sRead.ReadLine();// to end?
                    if (str1 == null)
                    {
                        timer1.Stop();
                        sRead.Close();
                        MessageBox.Show("文件发送成功!", "C#串口通讯");
                        this.toolStripStatusLabel5.Text = "";
                        return;
                    }
                    byte[] data = Encoding.Default.GetBytes(str1);
                    sp.Write(data, 0, data.Length);
                    this.toolStripStatusLabel5.Text = "     文件发送中......";
                }
                catch (Exception ex)
                {
                    timer1.Enabled = false;
                    MessageBox.Show("错误:44" + ex.Message);
                    return;
                }

            }

        }
        // 接受数据
        #endregion
        #region 串口接受事件Sp_DataReceived

        private void SP_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;//允许跨线程
            Thread.Sleep(50);
           try
            {
                if (this.sp.BytesToRead > 0)
                {
                    receiveByteData = null;//清空数数区
                    toolStripStatusLabel5.Text = ("");
                    byte[] buffer = new byte[this.sp.BytesToRead];//创建接收字节数组
                    bool byteSumCheckWrong = false;
                    this.sp.Read(buffer, 0, buffer.Length);//将串口缓冲器中读取的数据放到数组中
                    //1125
                    if (buffer.Length == 3)
                    {
                        receiveByteData = buffer;
                    }
                    else if (buffer.Length > 3)
                    {
                        byteSumCheck = 0x00;
                        for (int i = 2; i < buffer.Length-1; i++)
                        {
                            byteSumCheck += buffer[i];
                        }
                        if (byteSumCheck == buffer[buffer.Length-1])
                        {
                            receiveByteData = buffer;//1125
                        }
                        else
                        {
                            byteSumCheck = 0x00;                           
                            sp.DiscardInBuffer();//清空SerialPort控件的Buffer     
                            byteSumCheckWrong = true;
                        }
                    }
                    //1125
                    //////-------------------------------------+++++
                    //receiveByteData = buffer;//1125
                    // received_count += buffer.Length;//增加接收计数
                    sp.DiscardInBuffer();//清空SerialPort控件的Buffer                    
                    string strRcv = null;
                   
                    if (checkBox1.Checked==false)
                    {
                        strRcv= Encoding.Default.GetString(buffer) + " ";//字符串形式显示16进制
                    }
                    else
                    {
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            strRcv += buffer[i].ToString("X2") + " ";//16进制显示 加空格                                                                   
                        }
                    }
                    richTextBox2.Text += strRcv+ "\r\n";
                    //textBox1.Text = byteToHexStr(receiveByteData);
                    timer2.Enabled = true;
                if (ShareDataCom.targetStatus == 1&& byteSumCheckWrong==false)///1126
                {
                    if (receiveByteData[0] == 0X7E && receiveByteData[1] == 0X03 && receiveByteData[2] == 0XFA)
                    {
                        txtRealTimeValue.Text = Convert.ToString(byteToInt(receiveByteData[3], receiveByteData[4]));                      
                    }
                    //下发日/夜表成功
                    if (receiveByteData[0] == 0X7E && receiveByteData[1] == 0X01 && receiveByteData[2] == 0X01)
                    {                      
                        toolStripStatusLabel5.Text = ("表格下发成功！");

                    }
                    //读取日表
                    if (receiveByteData[0] == 0X7E && receiveByteData[1] == 0X2D && receiveByteData[2] == 0X1A)
                    {
                        
                        if (receiveByteData.Length != 0)//判断串口是否有数据
                        {
                            //清空
                            for (int i = 0; i < receiveIntData.Length; i++)
                            {
                                receiveIntData[i] = 0;
                            }
                            //写入INT
                            byteToIntFor();
                            for (int i = 0; i < readtext.Length; i++)
                            {
                                readtext[i].Text = Convert.ToString(receiveIntData[i]);
                            }                           

                        }
                    }
                    //读取夜表
                    if (receiveByteData[0] == 0X7E && receiveByteData[1] == 0X2D && receiveByteData[2] == 0X2A)
                    {
                        if (receiveByteData.Length != 0)//判断串口是否有数据
                        {
                            //清空
                            for (int i = 0; i < receiveIntData.Length; i++)
                            {
                                receiveIntData[i] = 0;
                            }
                            //写入INT
                            byteToIntFor();
                            for (int i = 0; i < readtext.Length; i++)
                            {
                                readtext[i].Text = Convert.ToString(receiveIntData[i]);
                            }                            
                        }

                    }
                }
            }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "出错提示18888");
                richTextBox2.Text = "";
                return;
            }
        }
        #endregion
        #region 进入调试模式 退出调试模式 下发日夜表 读取日夜表 及各转换函数
        //进入调试模式
        private void BtnDebug_Click(object sender, EventArgs e)
        {
            try
            {
                sp.Write(sendInDebugCode, 0, sendInDebugCode.Length);
                ShareDataCom.targetStatus = 1;
                timer2.Enabled = true;
                btnDebug.Enabled = false;
                btnOutDebugModel.Enabled = true;
                if (sp.IsOpen)
                {
                    btnReadDayForm.Enabled = true;
                    btnReadNightForm.Enabled = true;
                    btnIssueDayForm.Enabled = true;
                    btnIssueNightForm.Enabled = true;
                }
                for (int i = 0; i < readtext.Length; i++)
                {
                    readtext[i].Enabled = true;
                    writetext[i].Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


        }
        //退出调试模式
        private void btnOutDebugModel_Click(object sender, EventArgs e)
        {
            try
            {
                sp.Write(sendOutDebugCode, 0, sendOutDebugCode.Length);
                ShareDataCom.targetStatus = 2;
                timer2.Enabled = true;
                btnDebug.Enabled = true;
                btnOutDebugModel.Enabled = false;
                btnReadDayForm.Enabled = false;
                btnReadDayForm.Enabled = false;
                btnReadNightForm.Enabled = false;
                btnIssueDayForm.Enabled = false;
                btnIssueNightForm.Enabled = false;
                for (int i = 0; i < readtext.Length; i++)
                {
                    readtext[i].Enabled = false;
                    writetext[i].Enabled = false;
                    readtext[i].Text = Convert.ToString(0);
                    writetext[i].Text = Convert.ToString(0);
                    txtRealTimeValue.Text = Convert.ToString(0);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }
        private void Timer2_Tick(object sender, EventArgs e)
        {
           
           try
            {
                if (ShareDataCom.targetStatus == 1 )
                {
                    

                    if (receiveByteData == receiveInDebugCode)
                    {                       
                        ShareDataCom.runStatus = 1;
                        ShareDataCom.targetStatus = 0;
                        receiveByteData =null;//清空数数区

                    }
                }
                if (ShareDataCom.targetStatus==2)
                {
                   
                    if (receiveByteData==receiveOutDebugCode)
                    {
                        ShareDataCom.runStatus = 0;
                        ShareDataCom.targetStatus = 0;
                        receiveByteData = null;//清空数数区
                    }

                }
                if (ShareDataCom.runStatus == 1)
                {


           }    }
            catch (Exception ex)
            {
                timer2.Enabled = false;
                MessageBox.Show(ex.Message, "出错提示22");
                return;
            }

            Thread.Sleep(10);
            timer2.Enabled = false;
        }

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        private void Button4_Click_1(object sender, EventArgs e)
        {
         
            try
            {
                if (this.sp.BytesToRead > 0)
                {
                    MessageBox.Show("3");
                    byte[] buffer = new byte[this.sp.BytesToRead];//创建接收字节数组

                    this.sp.Read(buffer, 0, buffer.Length);//将串口缓冲器中读取的数据放到数组中
                   // received_count += buffer.Length;//增加接收计数
                    sp.DiscardInBuffer();//清空SerialPort控件的Buffer
                    string strRcv = null;
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        strRcv += buffer[i].ToString("X2") + " ";//16进制显示 加空格                                                                      
                    }

                    richTextBox2.Text += strRcv + "\r\n";

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "出错提示11");
                richTextBox2.Text = "";
                return;
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
        }

        private void GroupBox4_Enter(object sender, EventArgs e)
        {

        }

        //下发日表
        private void btnIssueDayForm_Click(object sender, EventArgs e)
        {
            
            try
            {
                //将值TXT值写入sendIntData
                for (int i = 0; i < writetext.Length; i++)
                {
                    sendIntData[i] = int.Parse(writetext[i].Text);
                  //  MessageBox.Show(Convert.ToString(sendIntData[i]));
                }
                //将sendIntData 值转成BYTE型，并加文件头，发送
                for (int i = 0; i < sendIntData.Length; i++)
                {
                    byte[] bytes = BitConverter.GetBytes(sendIntData[i]);
                    sendWriteDayFormCode[2 * i + 3] = bytes[1];//《》《》《》《》《》《》《》《》《》《》《》
                    sendWriteDayFormCode[2 * i + 1 + 3] = bytes[0];//《》《》《》《》《》《》《》《》《》《》《》《》
                }
                sendWriteDayFormCode[0] = 0X7E;
                sendWriteDayFormCode[1] = 0X2D;
                sendWriteDayFormCode[2] = 0XA1;
                for (int i = 2; i < sendWriteDayFormCode.Length - 1; i++)
                {
                    sendWriteDayFormCode[sendWriteDayFormCode.Length - 1] += sendWriteDayFormCode[i];
                }
                sp.Write(sendWriteDayFormCode,0, sendWriteDayFormCode.Length);
                //清零
                for (int i = 0; i < sendWriteDayFormCode.Length; i++)
                {
                    sendWriteDayFormCode[i] = 0;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "错误232");
                return;
            }

        }
        //下发夜表
        private void btnIssueNightForm_Click(object sender, EventArgs e)
        {
           
            try
            {
                //将值TXT值写入sendIntData
                for (int i = 0; i < writetext.Length; i++)
                {
                    sendIntData[i] = int.Parse(writetext[i].Text);
                   // MessageBox.Show(Convert.ToString(sendIntData[i]));
                }
                //将sendIntData 值转成BYTE型，并加文件头，发送
                for (int i = 0; i < sendIntData.Length; i++)
                {
                    byte[] bytes = BitConverter.GetBytes(sendIntData[i]);
                    sendWriteNightFormCode[2 * i + 3] = bytes[1];//《》《》《》《》《》《》《》《》《》《》《》
                    sendWriteNightFormCode[2 * i + 1 + 3] = bytes[0];//《》《》《》《》《》《》《》《》《》《》《》《》
                }
                sendWriteNightFormCode[0] = 0X7E;
                sendWriteNightFormCode[1] = 0X2D;
                sendWriteNightFormCode[2] = 0XA2;
                for (int i = 2; i < sendWriteNightFormCode.Length-1; i++)
                {
                    sendWriteNightFormCode[sendWriteNightFormCode.Length - 1] += sendWriteNightFormCode[i];
                }
                sp.Write(sendWriteNightFormCode, 0, sendWriteNightFormCode.Length);
                //清零
                for (int i = 0; i < sendWriteNightFormCode.Length; i++)
                {
                    sendWriteNightFormCode[i] = 0;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "错误231");
                return;
            }
        }
        //读日表按钮
        private void btnReceiveDayForm_Click(object sender, EventArgs e)
        {
           
            sp.Write(sendReadDayFormCode, 0, sendReadDayFormCode.Length);
            timer2.Enabled = true;
        }
        //读夜表按钮
        private void btnReadNightForm_Click(object sender, EventArgs e)
        {
           
            sp.Write(sendReadNightFormCode,0, sendReadNightFormCode.Length);
            timer2.Enabled = true;
        }
        private int byteToInt(byte a, byte b)
        {
            sumC = (a << 8) | b;
            return sumC;
        }
        private void byteToIntFor()
        {
            try
            {
                
                for (int i = 1; i < (receiveByteData.Length-2)/2; i++)//(int i = 1; i < (receiveByteData.Length-1)/2; i++
                {
                    receiveIntData[i-1] = byteToInt(receiveByteData[2*i + 1], receiveByteData[2*i+2]);//《》《》《》《》《》《》《》《》《》《》《》《》注意前后顺序
                }             
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "错误 B");
                return;
            }
        }
        public static int IntToBitConverter(int num)
        {
            int temp = 0;
            byte[] bytes = BitConverter.GetBytes(num);//将int32转换为字节数组
            temp = BitConverter.ToInt32(bytes, 0);//将字节数组内容再转成int32类型
            return temp;
        }
        #endregion
        #region 自测按钮
        private void T1_Click(object sender, EventArgs e)
        {
            try
            {
                sp.Write(receiveInDebugCode, 0, receiveInDebugCode.Length);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "错误提示：");
                return;
            }
           
        }


        private void T4_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                byte[] a = { 0x7e, 0x01, 0x01 };
                sp.Write(a, 0, a.Length);
            }
                       
        }

        private void T5_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {

                byte[] a = { 0x7E, 0x2D, 0x1A, 0x0B, 0xB8, 0x00, 0x01, 0x0B, 0xB8, 0x00, 0x03, 0x0B, 0xB8, 0x00, 0x05, 0x0B, 0xB8, 0x00, 0x07, 0x0B, 0xB8, 0x00, 0x09, 0x0B, 0xB8, 0x00, 0x0B, 0x0B, 0xB8, 0x00, 0x0D, 0x0B, 0xB8, 0x00, 0x0F, 0x0B, 0xB8, 0x00, 0x11, 0x0B, 0xB8, 0x00, 0x13, 0x0B, 0xB8, 0x00, 0x15,0xF4 };//F4 校验和
                sp.Write(a, 0, a.Length);

            }
        }
       
        private void T6_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {

                byte[] a = { 0x7E, 0x2D, 0x2A, 0x0B, 0xB3, 0x00, 0xF2, 0x0B, 0xB8, 0x00, 0x03, 0x0B, 0xB8, 0x00, 0x05, 0x0B, 0xB8, 0x00, 0x07, 0x0B, 0xB8, 0x00, 0x09, 0x0B, 0xB8, 0x00, 0x0B, 0x0B, 0xB8, 0x00, 0x0D, 0x0B, 0xB8, 0x00, 0x0F, 0x0B, 0xB8, 0x00, 0x11, 0x0B, 0xB8, 0x00, 0x13, 0x0B, 0xB8, 0x00, 0x15,0xF0 };//F0校验和
                sp.Write(a, 0, a.Length);
            }
        }
        #endregion
        #region 文件存储 读取
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "数据文件(*.txt)|*.txt|All files(*.*)|*.*";//设置为数据文件或所有文件
            sfd.DefaultExt = "txt"; //设置默认扩展名为txt
            if (sfd.ShowDialog() == DialogResult.OK) //打开对话框 
            {
                try
                {
                    StreamWriter m_SW = new StreamWriter(sfd.FileName);// 实例化StreamWriter类 
                    for (int i = 0; i < receiveIntData.Length; i++)
                    {
                        m_SW.WriteLine(receiveIntData[i]);
                        
                    }
                    //m_SW.Write(DateTime.Now);
                    //m_SW.Write("  ActualCurrent:   "); // 写入数据 
                    //m_SW.Write(shareData.actualCurrent + 1); // 写入数据 
                    //m_SW.WriteLine(""); // 写入数据 
                    m_SW.Close();
                }// 关闭文件 } 
                catch (IOException ex)
                {
                    MessageBox.Show("写入出错！\n" + ex.Message);
                    return;
                }
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "数据文件(*.txt)|*.txt|All files(*.*)|*.*";//设置为数据文件或所有文件
            ofd.DefaultExt = "txt"; //设置默认扩展名为txt f
            if (ofd.ShowDialog() == DialogResult.OK) //打开对话框 
            {
                try
                {
                    StreamReader m_SW = new StreamReader(ofd.FileName);// 实例化StreamWriter类 
                    for (int i = 0; i < readIntData.Length; i++)
                    {
                        readIntData[i] =Convert.ToInt32( m_SW.ReadLine());
                    }                   
                    m_SW.Close();
                    for (int i = 0; i < writetext.Length; i++)
                    {
                        writetext[i].Text = Convert.ToString(readIntData[i]);
                    }
                }// 关闭文件 } 
                catch (IOException ex)
                {
                    MessageBox.Show("写入出错！\n" + ex.Message);
                    return;
                }
            }
        }
        #endregion
        #region 图表相关
        private void btnChart1_Click(object sender, EventArgs e)
        {
            series.Points.Clear();//用前必须先清空
            series.Enabled = true; ;
            // 画样条曲线（Spline）
            series.ChartType = SeriesChartType.Spline;
            // 线宽2个像素
            series.BorderWidth = 2;
            // 线的颜色：红色
            series.Color = System.Drawing.Color.Red;
            // 图示上的文字
            series.LegendText = "Value1";

            // 在chart中显示数据
            for (int i = 0; i < readtext.Length; i++)
            {
                series.Points.AddXY(xChart[i],Convert.ToInt32(readtext[i].Text));
            }
        }
       
        private void btnChart2_Click(object sender, EventArgs e)
        {
            series1.Points.Clear();//用前必要先清空
            series1.Enabled = true; ;

            //chart1.Series.Add(series1);
            // 画样条曲线（Spline）
            series1.ChartType = SeriesChartType.Spline;
            // 线宽2个像素
            series1.BorderWidth = 2;
            // 线的颜色：红色
            series1.Color = System.Drawing.Color.Blue;
            // 图示上的文字
            series1.LegendText = "Value2";
            //将值TXT值写入sendIntData
         
            //for (int i = 0; i < writetext.Length; i++)
            //{    
            //    sendIntData[i] = int.Parse(writetext[i].Text);                 
            //}
       
            // 在chart中显示数据          
            for (int i = 0; i < writetext.Length; i++)
            {
                series1.Points.AddXY(xChart[i],Convert.ToInt32(writetext[i].Text));
            }  
                          
        }

        private void btnChartClear_Click(object sender, EventArgs e)
        {
            series1.Points.Clear();
            series.Points.Clear();
            series1.Points.AddXY(0, 0);

        }

        private void btnChartSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.Filter = "JPEG文件|*.jpg";
                if (savefile.ShowDialog() == DialogResult.OK)
                {

                    chart1.SaveImage(savefile.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            catch (Exception ex )
            {

                MessageBox.Show(ex.Message, "错误提示");
                return;
            }

        }

        private void TxtRealTimeValue_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion
        #region 电机控制 原点 目标位置 上传电机行程  读取电机位置
        //电机回原点
        private void BtnGoToZeroPosition_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sp.Write(sendGoToZeroPosition, 0, sendGoToZeroPosition.Length);
            }
        }
        //电机移动到目标位置
        private void BtnMotorGoPosition_Click(object sender, EventArgs e)
        {
            try
            {
                int motorTargeValue = 0;
                motorTargeValue = int.Parse(txtMotorTargetValue.Text);
                byte[] bytes = BitConverter.GetBytes(motorTargeValue);
                sendWriteMotorTargeValue[3] = bytes[1];
                sendWriteMotorTargeValue[4] = bytes[0];
                sendWriteMotorTargeValue[0] = 0X7E;
                sendWriteMotorTargeValue[1] = 0X03;
                sendWriteMotorTargeValue[2] = 0X3E;
                for (int i = 2; i < sendWriteMotorTargeValue.Length - 1; i++)
                {
                    sendWriteMotorTargeValue[sendWriteMotorTargeValue.Length - 1] += sendWriteMotorTargeValue[i];
                }
                sp.Write(sendWriteMotorTargeValue, 0, sendWriteMotorTargeValue.Length);
                //清零
                for (int i = 0; i < sendWriteMotorTargeValue.Length; i++)
                {
                    sendWriteMotorTargeValue[i] = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误232");
                return;
            }
        }
        //实时电机位置命令
        private void T3_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sp.Write(sendReadMotorDistance, 0, sendReadMotorDistance.Length);
            }

        }
        //上传电机行程
        private void T10_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                byte[] a = { 0x7e, 0x03, 0xFA, 0x00,0x23,0x1D};
                sp.Write(a, 0, a.Length);
            }
        }

        private void GroupBox7_Enter(object sender, EventArgs e)
        {

        }
    }
    #endregion
    public static class ShareDataCom
    {
        //以下定义4个公有变量，用于参数传递
        public static string strProtName = "";
        public static string strBaudRate = "";
        public static string strDataBits = "";
        public static string strStopBits = "";
        public static int targetStatus = 0;//目标状态:   0初始状态 1进入调试状态 2退出调试状态  
        public static int runStatus = 0;//现在运行状态:  0初始状态 1进入调试状态 2退出调试状态  
    }

}
