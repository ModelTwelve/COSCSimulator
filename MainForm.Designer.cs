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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.simulationBoard = new System.Windows.Forms.Panel();
            this.infoLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.zAxisBoard = new System.Windows.Forms.Panel();
            this.goButton = new System.Windows.Forms.Button();
            this.countDropDown = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.resultsListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // simulationBoard
            // 
            this.simulationBoard.Location = new System.Drawing.Point(12, 12);
            this.simulationBoard.Name = "simulationBoard";
            this.simulationBoard.Size = new System.Drawing.Size(879, 544);
            this.simulationBoard.TabIndex = 1;
            this.simulationBoard.Click += new System.EventHandler(this.simulationBoard_Click);
            this.simulationBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.simulationBoard_Paint);
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
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // zAxisBoard
            // 
            this.zAxisBoard.Location = new System.Drawing.Point(908, 12);
            this.zAxisBoard.Name = "zAxisBoard";
            this.zAxisBoard.Size = new System.Drawing.Size(89, 544);
            this.zAxisBoard.TabIndex = 3;
            this.zAxisBoard.Click += new System.EventHandler(this.zAxisBoard_Click);
            this.zAxisBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.zAxisBoard_Paint);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 627);
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
            this.Controls.Add(this.simulationBoard);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel simulationBoard;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel zAxisBoard;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.ComboBox countDropDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox resultsListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

