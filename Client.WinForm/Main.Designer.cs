namespace Client.WinForm
{
    partial class Main
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
            this.theButton = new System.Windows.Forms.Button();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // theButton
            // 
            this.theButton.Location = new System.Drawing.Point(49, 46);
            this.theButton.Name = "theButton";
            this.theButton.Size = new System.Drawing.Size(160, 77);
            this.theButton.TabIndex = 0;
            this.theButton.Text = "The Button!";
            this.theButton.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.messageTextBox.Location = new System.Drawing.Point(49, 149);
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "textBox1";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.Size = new System.Drawing.Size(722, 293);
            this.messageTextBox.TabIndex = 1;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(815, 491);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.theButton);
            this.Name = "Main";
            this.Text = "TheApp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button theButton;
        private System.Windows.Forms.TextBox messageTextBox;
    }
}

