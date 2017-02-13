namespace GazeAwareForms
{
    partial class GazeAwareForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.userControl11 = new UserControl1();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.behaviorMap1 = new EyeXFramework.Forms.BehaviorMap(components);
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gold;
            this.panel1.Location = new System.Drawing.Point(61, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(266, 235);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Khaki;
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Location = new System.Drawing.Point(189, 139);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(257, 227);
            this.panel2.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Yellow;
            this.panel4.Location = new System.Drawing.Point(35, 107);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(103, 95);
            this.panel4.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Chartreuse;
            this.panel3.Location = new System.Drawing.Point(393, 96);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 168);
            this.panel3.TabIndex = 1;
            // 
            // userControl11
            // 
            this.userControl11.BackColor = System.Drawing.Color.PowderBlue;
            this.userControl11.Location = new System.Drawing.Point(519, 173);
            this.userControl11.Name = "userControl11";
            this.userControl11.Size = new System.Drawing.Size(387, 261);
            this.userControl11.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(61, 467);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(561, 27);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "The panels on this form respond to your gaze. The green one has a longer response" +
    " time than the others.";
            // 
            // behaviorMap1
            // 
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 506);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.userControl11);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Gaze-Aware Windows Forms Sample";
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private UserControl1 userControl11;
        private System.Windows.Forms.TextBox textBox1;
        private EyeXFramework.Forms.BehaviorMap behaviorMap1;
    }
}

