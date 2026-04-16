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
            this.txtSource.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.txtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSource.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtSource.ForeColor = System.Drawing.Color.Gainsboro;
            this.txtSource.Location = new System.Drawing.Point(21, 70);
            this.txtSource.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtSource.Multiline = true;
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(508, 264);
            this.txtSource.TabIndex = 5;
            this.txtSource.Text = "Введите текст";
            this.txtSource.TextChanged += new System.EventHandler(this.txtSource_TextChanged);
            // 
            // txtTarget
            // 
            this.txtTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTarget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.txtTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTarget.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtTarget.ForeColor = System.Drawing.Color.Gainsboro;
            this.txtTarget.Location = new System.Drawing.Point(540, 70);
            this.txtTarget.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtTarget.Multiline = true;
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.ReadOnly = true;
            this.txtTarget.Size = new System.Drawing.Size(508, 264);
            this.txtTarget.TabIndex = 4;
            this.txtTarget.Text = "Перевод";
            // 
            // btnTranslate
            // 
            this.btnTranslate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTranslate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnTranslate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTranslate.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnTranslate.Location = new System.Drawing.Point(868, 371);
            this.btnTranslate.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(180, 62);
            this.btnTranslate.TabIndex = 3;
            this.btnTranslate.Text = "Перевести";
            this.btnTranslate.UseVisualStyleBackColor = false;
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
            // 
            // cbTargetLang
            // 
            this.cbTargetLang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTargetLang.BackColor = System.Drawing.Color.White;
            this.cbTargetLang.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.cbTargetLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetLang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbTargetLang.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
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
            this.cbTargetLang.Location = new System.Drawing.Point(540, 19);
            this.cbTargetLang.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.cbTargetLang.Name = "cbTargetLang";
            this.cbTargetLang.Size = new System.Drawing.Size(508, 31);
            this.cbTargetLang.TabIndex = 2;
            this.cbTargetLang.SelectedIndexChanged += new System.EventHandler(this.cbTargetLang_SelectedIndexChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatus.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblStatus.Location = new System.Drawing.Point(18, 443);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(152, 23);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Статус: Ожидание";
            this.lblStatus.Click += new System.EventHandler(this.lblStatus_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.progressBar.Location = new System.Drawing.Point(21, 346);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1027, 14);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 0;
            this.progressBar.Visible = false;
            this.progressBar.Click += new System.EventHandler(this.progressBar_Click);
            // 
            // TranslatorApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(1072, 496);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cbTargetLang);
            this.Controls.Add(this.btnTranslate);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.txtSource);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "TranslatorApp";
            this.Text = "DeepL Translator";
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