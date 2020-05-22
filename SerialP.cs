using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Threading;

namespace HSL采样
{
    public partial class SerialP : Form
    {
        public SerialP()
        {
            InitializeComponent();
        }

        private void SerialP_Load(object sender, EventArgs e)
        {
            //串口
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    cmbPort.Items.Add(port);
                }
                cmbPort.SelectedIndex = 0;
                //波特率
                cmbBaudRate.Items.Add("110");
                cmbBaudRate.Items.Add("300");
                cmbBaudRate.Items.Add("1200");
                cmbBaudRate.Items.Add("2400");
                cmbBaudRate.Items.Add("4800");
                cmbBaudRate.Items.Add("9600");
                cmbBaudRate.Items.Add("19200");
                cmbBaudRate.Items.Add("38400");
                cmbBaudRate.Items.Add("57600");
                cmbBaudRate.Items.Add("115200");
                cmbBaudRate.Items.Add("230400");
                cmbBaudRate.Items.Add("460800");
                cmbBaudRate.Items.Add("921600");
                cmbBaudRate.SelectedIndex = 5;

                //数据位
                cmbDataBits.Items.Add("5");
                cmbDataBits.Items.Add("6");
                cmbDataBits.Items.Add("7");
                cmbDataBits.Items.Add("8");
                cmbDataBits.SelectedIndex = 3;

                //停止位
                cmbStopBit.Items.Add("1");
                cmbStopBit.SelectedIndex = 0;

                //佼验位
                cmbParity.Items.Add("无");
                cmbParity.SelectedIndex = 0;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "错误");
                return;
            }
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ShareDataCom.strProtName = cmbPort.Text;
            ShareDataCom.strBaudRate = cmbBaudRate.Text;
            ShareDataCom.strDataBits = cmbDataBits.Text;
            ShareDataCom.strStopBits = cmbStopBit.Text;
            DialogResult = DialogResult.OK;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }

}