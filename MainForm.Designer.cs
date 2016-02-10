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
            this.simulationBoard = new System.Windows.Forms.Panel();
            this.infoLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.zAxisBoard = new System.Windows.Forms.Panel();
            this.CurrentPositionLabel = new System.Windows.Forms.Label();
            this.GoalPositionLabel = new System.Windows.Forms.Label();
            this.goButton = new System.Windows.Forms.Button();
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
            // CurrentPositionLabel
            // 
            this.CurrentPositionLabel.AutoSize = true;
            this.CurrentPositionLabel.Location = new System.Drawing.Point(14, 559);
            this.CurrentPositionLabel.Name = "CurrentPositionLabel";
            this.CurrentPositionLabel.Size = new System.Drawing.Size(41, 13);
            this.CurrentPositionLabel.TabIndex = 4;
            this.CurrentPositionLabel.Text = "Current";
            // 
            // GoalPositionLabel
            // 
            this.GoalPositionLabel.AutoSize = true;
            this.GoalPositionLabel.Location = new System.Drawing.Point(905, 559);
            this.GoalPositionLabel.Name = "GoalPositionLabel";
            this.GoalPositionLabel.Size = new System.Drawing.Size(29, 13);
            this.GoalPositionLabel.TabIndex = 5;
            this.GoalPositionLabel.Text = "Goal";
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(816, 586);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(75, 23);
            this.goButton.TabIndex = 6;
            this.goButton.Text = "GO!";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 627);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.GoalPositionLabel);
            this.Controls.Add(this.CurrentPositionLabel);
            this.Controls.Add(this.zAxisBoard);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.simulationBoard);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "Simulation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel simulationBoard;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel zAxisBoard;
        private System.Windows.Forms.Label CurrentPositionLabel;
        private System.Windows.Forms.Label GoalPositionLabel;
        private System.Windows.Forms.Button goButton;
    }
}

