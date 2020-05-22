namespace HSL采样
{
    partial class AutoMarkerStatus
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.hslLanternSimple1 = new HslControls.HslLanternSimple();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.hslBarChart1 = new HslControls.HslBarChart();
            this.hslBarChart2 = new HslControls.HslBarChart();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "运行状态:";
            // 
            // hslLanternSimple1
            // 
            this.hslLanternSimple1.Location = new System.Drawing.Point(160, 26);
            this.hslLanternSimple1.Name = "hslLanternSimple1";
            this.hslLanternSimple1.Size = new System.Drawing.Size(81, 75);
            this.hslLanternSimple1.TabIndex = 2;
            this.hslLanternSimple1.Load += new System.EventHandler(this.hslLanternSimple1_Load);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "待打区物料数：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 186);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "要求打物料数：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 248);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "累计物料数：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(160, 122);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(160, 177);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 21);
            this.textBox2.TabIndex = 3;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(160, 239);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 21);
            this.textBox3.TabIndex = 3;
            // 
            // hslBarChart1
            // 
            this.hslBarChart1.BackColor = System.Drawing.Color.Black;
            this.hslBarChart1.BarBackColor = System.Drawing.Color.DarkOrange;
            this.hslBarChart1.ColorDashLines = System.Drawing.Color.DimGray;
            this.hslBarChart1.ColorLinesAndText = System.Drawing.Color.WhiteSmoke;
            this.hslBarChart1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.hslBarChart1.ForeColor = System.Drawing.Color.White;
            this.hslBarChart1.Location = new System.Drawing.Point(299, 26);
            this.hslBarChart1.Name = "hslBarChart1";
            this.hslBarChart1.ShowBarValueFormat = "{0}";
            this.hslBarChart1.Size = new System.Drawing.Size(517, 234);
            this.hslBarChart1.TabIndex = 4;
            this.hslBarChart1.Text = "hslBarChart1";
            this.hslBarChart1.Title = "周产量表";
            this.hslBarChart1.UseGradient = true;
            this.hslBarChart1.ValueSegment = 10;
            this.hslBarChart1.Load += new System.EventHandler(this.HslBarChart1_Load);
            // 
            // hslBarChart2
            // 
            this.hslBarChart2.BackColor = System.Drawing.Color.Black;
            this.hslBarChart2.BarBackColor = System.Drawing.Color.DarkOrange;
            this.hslBarChart2.ColorDashLines = System.Drawing.Color.DimGray;
            this.hslBarChart2.ColorLinesAndText = System.Drawing.Color.WhiteSmoke;
            this.hslBarChart2.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.hslBarChart2.ForeColor = System.Drawing.Color.White;
            this.hslBarChart2.Location = new System.Drawing.Point(40, 317);
            this.hslBarChart2.Name = "hslBarChart2";
            this.hslBarChart2.ShowBarValueFormat = "{0}";
            this.hslBarChart2.Size = new System.Drawing.Size(776, 234);
            this.hslBarChart2.TabIndex = 4;
            this.hslBarChart2.Text = "hslBarChart1";
            this.hslBarChart2.Title = "月产量";
            this.hslBarChart2.UseGradient = true;
            this.hslBarChart2.ValueSegment = 10;
            this.hslBarChart2.Load += new System.EventHandler(this.HslBarChart1_Load);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(160, 279);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 21);
            this.textBox4.TabIndex = 5;
            // 
            // AutoMarkerStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 592);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.hslBarChart2);
            this.Controls.Add(this.hslBarChart1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.hslLanternSimple1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AutoMarkerStatus";
            this.Text = "AutoMarkerStatus";
            this.Load += new System.EventHandler(this.AutoMarkerStatus_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private HslControls.HslLanternSimple hslLanternSimple1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private HslControls.HslBarChart hslBarChart1;
        private HslControls.HslBarChart hslBarChart2;
        private System.Windows.Forms.TextBox textBox4;
    }
}