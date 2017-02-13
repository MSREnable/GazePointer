namespace ActivatableNotesForms
{
    partial class ActivatableNotesForm
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
            this.moreWisdomButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.behaviorMap1 = new EyeXFramework.Forms.BehaviorMap(this.components);
            this.SuspendLayout();
            // 
            // moreWisdomButton
            // 
            this.moreWisdomButton.Location = new System.Drawing.Point(16, 20);
            this.moreWisdomButton.Margin = new System.Windows.Forms.Padding(4);
            this.moreWisdomButton.Name = "moreWisdomButton";
            this.moreWisdomButton.Size = new System.Drawing.Size(221, 98);
            this.moreWisdomButton.TabIndex = 1;
            this.moreWisdomButton.TabStop = false;
            this.moreWisdomButton.Text = "More wisdom!";
            this.moreWisdomButton.UseVisualStyleBackColor = true;
            this.moreWisdomButton.Click += new System.EventHandler(this.moreWisdomButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(16, 172);
            this.closeButton.Margin = new System.Windows.Forms.Padding(4);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(221, 98);
            this.closeButton.TabIndex = 2;
            this.closeButton.TabStop = false;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // ActivatableNotesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 821);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.moreWisdomButton);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ActivatableNotesForm";
            this.Text = "Activatable Notes Sample";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button moreWisdomButton;
        private System.Windows.Forms.Button closeButton;
        private EyeXFramework.Forms.BehaviorMap behaviorMap1;
    }
}

