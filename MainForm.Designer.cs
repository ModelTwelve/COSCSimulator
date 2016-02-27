namespace COSCSimulator
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.xyAxisPanel = new System.Windows.Forms.Panel();
            this.simulationPictureBox = new System.Windows.Forms.PictureBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.zAxisBoard = new System.Windows.Forms.Panel();
            this.zAxisPictureBox = new System.Windows.Forms.PictureBox();
            this.goButton = new System.Windows.Forms.Button();
            this.countDropDown = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.resultsListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.speedTrackBar = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.xyAxisPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.simulationPictureBox)).BeginInit();
            this.zAxisBoard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zAxisPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speedTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // simulationBoard
            // 
            this.xyAxisPanel.AutoScroll = true;
            this.xyAxisPanel.AutoScrollMinSize = new System.Drawing.Size(5400, 5400);
            this.xyAxisPanel.Controls.Add(this.simulationPictureBox);
            this.xyAxisPanel.Location = new System.Drawing.Point(12, 12);
            this.xyAxisPanel.Name = "simulationBoard";
            this.xyAxisPanel.Size = new System.Drawing.Size(879, 544);
            this.xyAxisPanel.TabIndex = 1;
            // 
            // simulationPictureBox
            // 
            this.simulationPictureBox.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.simulationPictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("simulationPictureBox.BackgroundImage")));
            this.simulationPictureBox.Location = new System.Drawing.Point(12, 15);
            this.simulationPictureBox.Name = "simulationPictureBox";
            this.simulationPictureBox.Size = new System.Drawing.Size(5280, 5280);
            this.simulationPictureBox.TabIndex = 0;
            this.simulationPictureBox.TabStop = false;
            this.simulationPictureBox.Click += new System.EventHandler(this.xy_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.Location = new System.Drawing.Point(12, 578);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(555, 31);
            this.infoLabel.TabIndex = 2;
            this.infoLabel.Text = "Get Started";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // zAxisBoard
            // 
            this.zAxisBoard.Controls.Add(this.zAxisPictureBox);
            this.zAxisBoard.Location = new System.Drawing.Point(908, 12);
            this.zAxisBoard.Name = "zAxisBoard";
            this.zAxisBoard.Size = new System.Drawing.Size(89, 544);
            this.zAxisBoard.TabIndex = 3;
            // 
            // simulationZAxisPictureBox
            // 
            this.zAxisPictureBox.BackColor = System.Drawing.Color.SkyBlue;
            this.zAxisPictureBox.Location = new System.Drawing.Point(4, 4);
            this.zAxisPictureBox.Name = "simulationZAxisPictureBox";
            this.zAxisPictureBox.Size = new System.Drawing.Size(82, 537);
            this.zAxisPictureBox.TabIndex = 0;
            this.zAxisPictureBox.TabStop = false;
            this.zAxisPictureBox.Click += new System.EventHandler(this.z_Click);
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(1040, 332);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(92, 23);
            this.goButton.TabIndex = 6;
            this.goButton.Text = "GO!";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // countDropDown
            // 
            this.countDropDown.DisplayMember = "1";
            this.countDropDown.FormattingEnabled = true;
            this.countDropDown.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.countDropDown.Location = new System.Drawing.Point(1040, 302);
            this.countDropDown.Name = "countDropDown";
            this.countDropDown.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.countDropDown.Size = new System.Drawing.Size(92, 21);
            this.countDropDown.TabIndex = 7;
            this.countDropDown.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1037, 286);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Number of Objects";
            // 
            // resultsListBox
            // 
            this.resultsListBox.FormattingEnabled = true;
            this.resultsListBox.Location = new System.Drawing.Point(1019, 396);
            this.resultsListBox.Name = "resultsListBox";
            this.resultsListBox.Size = new System.Drawing.Size(154, 160);
            this.resultsListBox.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1016, 373);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Results (in ft)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1015, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "COSC 636 Spring 2016";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Impact", 20.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1014, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 34);
            this.label4.TabIndex = 12;
            this.label4.Text = "Simulation";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1003, 94);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(178, 172);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // speedTrackBar
            // 
            this.speedTrackBar.LargeChange = 1;
            this.speedTrackBar.Location = new System.Drawing.Point(912, 578);
            this.speedTrackBar.Minimum = 1;
            this.speedTrackBar.Name = "speedTrackBar";
            this.speedTrackBar.Size = new System.Drawing.Size(266, 45);
            this.speedTrackBar.TabIndex = 14;
            this.speedTrackBar.Value = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(1056, 562);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Simulation Speed";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 636);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.speedTrackBar);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.resultsListBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.countDropDown);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.zAxisBoard);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.xyAxisPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.xyAxisPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.simulationPictureBox)).EndInit();
            this.zAxisBoard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.zAxisPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speedTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel xyAxisPanel;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Panel zAxisBoard;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.ComboBox countDropDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox resultsListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox simulationPictureBox;
        private System.Windows.Forms.PictureBox zAxisPictureBox;
        private System.Windows.Forms.TrackBar speedTrackBar;
        private System.Windows.Forms.Label label5;
    }
}

