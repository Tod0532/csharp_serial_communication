namespace HSL采样
{
    partial class Sample
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
            this.hslCurve1 = new HslControls.HslCurve();
            this.label1 = new System.Windows.Forms.Label();
            this.hslCurveHistory1 = new HslControls.HslCurveHistory();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // hslCurve1
            // 
            this.hslCurve1.CurveNameWidth = 150;
            this.hslCurve1.Location = new System.Drawing.Point(29, 45);
            this.hslCurve1.Name = "hslCurve1";
            this.hslCurve1.Size = new System.Drawing.Size(823, 152);
            this.hslCurve1.TabIndex = 2;
            this.hslCurve1.TextAddFormat = "HH:mm:ss";
            this.hslCurve1.Load += new System.EventHandler(this.HslCurve1_Load_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(378, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "实时监控";
            // 
            // hslCurveHistory1
            // 
            this.hslCurveHistory1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.hslCurveHistory1.DashCoordinateColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.hslCurveHistory1.Location = new System.Drawing.Point(59, 223);
            this.hslCurveHistory1.MarkTextColor = System.Drawing.Color.Yellow;
            this.hslCurveHistory1.Name = "hslCurveHistory1";
            this.hslCurveHistory1.Size = new System.Drawing.Size(757, 222);
            this.hslCurveHistory1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(378, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "历史记录";
            // 
            // Sample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 494);
            this.Controls.Add(this.hslCurveHistory1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hslCurve1);
            this.Name = "Sample";
            this.Load += new System.EventHandler(this.Sample_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private HslControls.HslCurve hslCurve1;
        private System.Windows.Forms.Label label1;
        private HslControls.HslCurveHistory hslCurveHistory1;
        private System.Windows.Forms.Label label2;
    }
}