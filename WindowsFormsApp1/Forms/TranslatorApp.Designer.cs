namespace WindowsFormsApp1.Forms
{
    partial class TranslatorApp
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtSource = new System.Windows.Forms.TextBox();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.btnTranslate = new System.Windows.Forms.Button();
            this.cbTargetLang = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();

            // txtSource
            this.txtSource.Location = new System.Drawing.Point(12, 40);
            this.txtSource.Multiline = true;
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(340, 150);

            // txtTarget
            this.txtTarget.Location = new System.Drawing.Point(360, 40);
            this.txtTarget.Multiline = true;
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.ReadOnly = true;
            this.txtTarget.Size = new System.Drawing.Size(340, 150);

            // btnTranslate
            this.btnTranslate.Location = new System.Drawing.Point(12, 200);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(120, 35);
            this.btnTranslate.Text = "Перевести";
            this.btnTranslate.UseVisualStyleBackColor = true;
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);

            // cbTargetLang
            this.cbTargetLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetLang.FormattingEnabled = true;
            this.cbTargetLang.Items.AddRange(new object[] { "Русский", "Английский" });
            this.cbTargetLang.Location = new System.Drawing.Point(360, 12);
            this.cbTargetLang.Name = "cbTargetLang";
            this.cbTargetLang.Size = new System.Drawing.Size(121, 21);
            this.cbTargetLang.SelectedIndex = 0;

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 250);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(86, 13);
            this.lblStatus.Text = "Статус: Ожидание";

            // progressBar
            this.progressBar.Location = new System.Drawing.Point(140, 210);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(212, 15);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.Visible = false;

            // TranslatorApp
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 280);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cbTargetLang);
            this.Controls.Add(this.btnTranslate);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.txtSource);
            this.Name = "TranslatorApp";
            this.Text = "Google Translator Pro";
            this.Load += new System.EventHandler(this.TranslatorApp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.Button btnTranslate;
        private System.Windows.Forms.ComboBox cbTargetLang;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}