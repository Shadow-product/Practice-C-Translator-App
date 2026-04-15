using System;
using System.Windows.Forms; // элементы WinForms: Form, Button, Label

using WindowsFormsApp1.Models; 
using WindowsFormsApp1.Services;
using WindowsFormsApp1.Repositories;

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
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(16, 49);
            this.txtSource.Margin = new System.Windows.Forms.Padding(4);
            this.txtSource.Multiline = true;
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(452, 184);
            this.txtSource.TabIndex = 5;
            // 
            // txtTarget
            // 
            this.txtTarget.Location = new System.Drawing.Point(480, 49);
            this.txtTarget.Margin = new System.Windows.Forms.Padding(4);
            this.txtTarget.Multiline = true;
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.ReadOnly = true;
            this.txtTarget.Size = new System.Drawing.Size(452, 184);
            this.txtTarget.TabIndex = 4;
            // 
            // btnTranslate
            // 
            this.btnTranslate.Location = new System.Drawing.Point(772, 258);
            this.btnTranslate.Margin = new System.Windows.Forms.Padding(4);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(160, 43);
            this.btnTranslate.TabIndex = 3;
            this.btnTranslate.Text = "Перевести";
            this.btnTranslate.UseVisualStyleBackColor = true;
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
            // 
            // cbTargetLang
            // 
            this.cbTargetLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetLang.FormattingEnabled = true;
            this.cbTargetLang.Items.AddRange(new object[] {
            "Русский",
            "Английский",
            "Казахский",
            "Немецкий",
            "Японский",
            "Французский",
            "Испанский",
            "Польский"});
            this.cbTargetLang.SelectedIndex = 1; // Английский язык по умолчанию
            this.cbTargetLang.Location = new System.Drawing.Point(480, 17);
            this.cbTargetLang.Margin = new System.Windows.Forms.Padding(4);
            this.cbTargetLang.Name = "cbTargetLang";
            this.cbTargetLang.Size = new System.Drawing.Size(160, 24);
            this.cbTargetLang.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(16, 308);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(126, 16);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Статус: Ожидание";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(481, 241);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(283, 18);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 0;
            this.progressBar.Visible = false;
            // 
            // TranslatorApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 345);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cbTargetLang);
            this.Controls.Add(this.btnTranslate);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.txtSource);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TranslatorApp";
            this.Text = "DeepL Translator Pro";
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