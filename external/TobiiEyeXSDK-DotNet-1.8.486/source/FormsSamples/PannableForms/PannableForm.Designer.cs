namespace PannableForms
{
    partial class PannableForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PannableForm));
            this._behaviorMap = new EyeXFramework.Forms.BehaviorMap(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this._text = new PannableForms.ScrollTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(948, 47);
            this.label1.TabIndex = 2;
            this.label1.Text = "Look in the lower/upper part of the scrollable area and press and hold the Shift " +
    "key to scroll down/up.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _text
            // 
            this._text.AcceptsReturn = true;
            this._text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._text.Dock = System.Windows.Forms.DockStyle.Fill;
            this._text.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._text.Location = new System.Drawing.Point(0, 47);
            this._text.Margin = new System.Windows.Forms.Padding(4);
            this._text.Multiline = true;
            this._text.Name = "_text";
            this._text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._text.Size = new System.Drawing.Size(948, 600);
            this._text.TabIndex = 1;
            this._text.TabStop = false;
            this._text.Text = resources.GetString("_text.Text");
            // 
            // PannableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 647);
            this.Controls.Add(this._text);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PannableForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pannable Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private EyeXFramework.Forms.BehaviorMap _behaviorMap;
        private ScrollTextBox _text;
        private System.Windows.Forms.Label label1;
    }
}

