namespace ActivatableButtonsForms
{
    partial class ActivatableButtonsForm
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
            this.buttonHueDown = new System.Windows.Forms.Button();
            this.buttonHueUp = new System.Windows.Forms.Button();
            this.panelColorSample = new System.Windows.Forms.Panel();
            this.buttonBrightnessUp = new System.Windows.Forms.Button();
            this.buttonBrightnessDown = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.behaviorMap1 = new EyeXFramework.Forms.BehaviorMap(this.components);
            this.SuspendLayout();
            // 
            // buttonHueDown
            // 
            this.buttonHueDown.Location = new System.Drawing.Point(16, 167);
            this.buttonHueDown.Margin = new System.Windows.Forms.Padding(4);
            this.buttonHueDown.Name = "buttonHueDown";
            this.buttonHueDown.Size = new System.Drawing.Size(223, 127);
            this.buttonHueDown.TabIndex = 0;
            this.buttonHueDown.TabStop = false;
            this.buttonHueDown.Text = "< Hue";
            this.buttonHueDown.UseVisualStyleBackColor = true;
            this.buttonHueDown.Click += new System.EventHandler(this.buttonHueDown_Click);
            // 
            // buttonHueUp
            // 
            this.buttonHueUp.Location = new System.Drawing.Point(541, 167);
            this.buttonHueUp.Margin = new System.Windows.Forms.Padding(4);
            this.buttonHueUp.Name = "buttonHueUp";
            this.buttonHueUp.Size = new System.Drawing.Size(223, 127);
            this.buttonHueUp.TabIndex = 2;
            this.buttonHueUp.TabStop = false;
            this.buttonHueUp.Text = "Hue >";
            this.buttonHueUp.UseVisualStyleBackColor = true;
            this.buttonHueUp.Click += new System.EventHandler(this.buttonHueUp_Click);
            // 
            // panelColorSample
            // 
            this.panelColorSample.BackColor = System.Drawing.Color.SeaGreen;
            this.panelColorSample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColorSample.Location = new System.Drawing.Point(247, 149);
            this.panelColorSample.Margin = new System.Windows.Forms.Padding(4);
            this.panelColorSample.Name = "panelColorSample";
            this.panelColorSample.Size = new System.Drawing.Size(286, 163);
            this.panelColorSample.TabIndex = 4;
            // 
            // buttonBrightnessUp
            // 
            this.buttonBrightnessUp.Location = new System.Drawing.Point(279, 15);
            this.buttonBrightnessUp.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBrightnessUp.Name = "buttonBrightnessUp";
            this.buttonBrightnessUp.Size = new System.Drawing.Size(223, 127);
            this.buttonBrightnessUp.TabIndex = 1;
            this.buttonBrightnessUp.TabStop = false;
            this.buttonBrightnessUp.Text = "Lighter";
            this.buttonBrightnessUp.UseVisualStyleBackColor = true;
            this.buttonBrightnessUp.Click += new System.EventHandler(this.buttonBrightnessUp_Click);
            // 
            // buttonBrightnessDown
            // 
            this.buttonBrightnessDown.Location = new System.Drawing.Point(279, 320);
            this.buttonBrightnessDown.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBrightnessDown.Name = "buttonBrightnessDown";
            this.buttonBrightnessDown.Size = new System.Drawing.Size(223, 127);
            this.buttonBrightnessDown.TabIndex = 3;
            this.buttonBrightnessDown.TabStop = false;
            this.buttonBrightnessDown.Text = "Darker";
            this.buttonBrightnessDown.UseVisualStyleBackColor = true;
            this.buttonBrightnessDown.Click += new System.EventHandler(this.buttonBrightnessDown_Click);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(16, 471);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(748, 54);
            this.textBox1.TabIndex = 5;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "Click the buttons with your eyes! Look at a button and use the Shift key to trigg" +
    "er a direct click.";
            // 
            // ActivatableButtonsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 530);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonBrightnessDown);
            this.Controls.Add(this.buttonBrightnessUp);
            this.Controls.Add(this.panelColorSample);
            this.Controls.Add(this.buttonHueUp);
            this.Controls.Add(this.buttonHueDown);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ActivatableButtonsForm";
            this.Text = "Activatable Buttons Windows Forms Sample";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonHueDown;
        private System.Windows.Forms.Button buttonHueUp;
        private System.Windows.Forms.Panel panelColorSample;
        private System.Windows.Forms.Button buttonBrightnessUp;
        private System.Windows.Forms.Button buttonBrightnessDown;
        private System.Windows.Forms.TextBox textBox1;
        private EyeXFramework.Forms.BehaviorMap behaviorMap1;
    }
}

