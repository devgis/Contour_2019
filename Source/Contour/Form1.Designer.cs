namespace DEVGIS.Contour {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_DrawContourLine = new System.Windows.Forms.Button();
            this.btn_Test = new System.Windows.Forms.Button();
            this.btn_MarkEdge = new System.Windows.Forms.Button();
            this.btn_MarkTriangle = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_DrawTriangle = new System.Windows.Forms.Button();
            this.btn_OpenFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(457, 353);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Move += new System.EventHandler(this.pictureBox1_Move);
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // btn_DrawContourLine
            // 
            this.btn_DrawContourLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_DrawContourLine.Location = new System.Drawing.Point(20, 114);
            this.btn_DrawContourLine.Name = "btn_DrawContourLine";
            this.btn_DrawContourLine.Size = new System.Drawing.Size(75, 20);
            this.btn_DrawContourLine.TabIndex = 3;
            this.btn_DrawContourLine.Text = "Draw CLine";
            this.btn_DrawContourLine.UseVisualStyleBackColor = true;
            this.btn_DrawContourLine.Click += new System.EventHandler(this.btn_DrawContourLine_Click);
            // 
            // btn_Test
            // 
            this.btn_Test.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Test.Location = new System.Drawing.Point(20, 143);
            this.btn_Test.Name = "btn_Test";
            this.btn_Test.Size = new System.Drawing.Size(75, 20);
            this.btn_Test.TabIndex = 4;
            this.btn_Test.Text = "Fill CLine";
            this.btn_Test.UseVisualStyleBackColor = true;
            this.btn_Test.Click += new System.EventHandler(this.btn_Test_Click);
            // 
            // btn_MarkEdge
            // 
            this.btn_MarkEdge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_MarkEdge.Location = new System.Drawing.Point(20, 201);
            this.btn_MarkEdge.Name = "btn_MarkEdge";
            this.btn_MarkEdge.Size = new System.Drawing.Size(75, 20);
            this.btn_MarkEdge.TabIndex = 5;
            this.btn_MarkEdge.Text = "Mark Edge";
            this.btn_MarkEdge.UseVisualStyleBackColor = true;
            this.btn_MarkEdge.Click += new System.EventHandler(this.btn_MarkEdge_Click);
            // 
            // btn_MarkTriangle
            // 
            this.btn_MarkTriangle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_MarkTriangle.Location = new System.Drawing.Point(20, 230);
            this.btn_MarkTriangle.Name = "btn_MarkTriangle";
            this.btn_MarkTriangle.Size = new System.Drawing.Size(75, 20);
            this.btn_MarkTriangle.TabIndex = 6;
            this.btn_MarkTriangle.Text = "Mark Tri";
            this.btn_MarkTriangle.UseVisualStyleBackColor = true;
            this.btn_MarkTriangle.Click += new System.EventHandler(this.btn_MarkTriangle_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btn_OpenFile);
            this.groupBox1.Controls.Add(this.btn_DrawTriangle);
            this.groupBox1.Controls.Add(this.btn_MarkTriangle);
            this.groupBox1.Controls.Add(this.btn_DrawContourLine);
            this.groupBox1.Controls.Add(this.btn_MarkEdge);
            this.groupBox1.Controls.Add(this.btn_Test);
            this.groupBox1.Location = new System.Drawing.Point(486, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(118, 270);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Option Control";
            // 
            // btn_DrawTriangle
            // 
            this.btn_DrawTriangle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_DrawTriangle.Location = new System.Drawing.Point(20, 172);
            this.btn_DrawTriangle.Name = "btn_DrawTriangle";
            this.btn_DrawTriangle.Size = new System.Drawing.Size(75, 20);
            this.btn_DrawTriangle.TabIndex = 7;
            this.btn_DrawTriangle.Text = "Draw Tri";
            this.btn_DrawTriangle.UseVisualStyleBackColor = true;
            this.btn_DrawTriangle.Click += new System.EventHandler(this.btn_DrawTriangle_Click);
            // 
            // btn_OpenFile
            // 
            this.btn_OpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OpenFile.Location = new System.Drawing.Point(20, 28);
            this.btn_OpenFile.Name = "btn_OpenFile";
            this.btn_OpenFile.Size = new System.Drawing.Size(75, 23);
            this.btn_OpenFile.TabIndex = 8;
            this.btn_OpenFile.Text = "Open File";
            this.btn_OpenFile.UseVisualStyleBackColor = true;
            this.btn_OpenFile.Click += new System.EventHandler(this.btn_OpenFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "--- MouStudio ---";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Location = new System.Drawing.Point(487, 288);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(114, 77);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 372);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(587, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "Author:YANG Qing.Department of Geophysics,Yangtze University,Jingzhou,Hubei.43402" +
                "3.mou_yq@126.com";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 387);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "- MouStudio -等值线绘制测试";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_DrawContourLine;
        private System.Windows.Forms.Button btn_Test;
        private System.Windows.Forms.Button btn_MarkEdge;
        private System.Windows.Forms.Button btn_MarkTriangle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_DrawTriangle;
        private System.Windows.Forms.Button btn_OpenFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label2;
    }
}

