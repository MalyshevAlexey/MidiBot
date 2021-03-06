﻿namespace MidiBot.Views.DefaultForms
{
    partial class AudioWave
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
            this.pictureWave = new System.Windows.Forms.PictureBox();
            this.pictureFFT = new System.Windows.Forms.PictureBox();
            this.table = new System.Windows.Forms.TableLayoutPanel();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureWave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureFFT)).BeginInit();
            this.table.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureWave
            // 
            this.pictureWave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureWave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureWave.Location = new System.Drawing.Point(3, 3);
            this.pictureWave.Name = "pictureWave";
            this.pictureWave.Size = new System.Drawing.Size(878, 336);
            this.pictureWave.TabIndex = 0;
            this.pictureWave.TabStop = false;
            // 
            // pictureFFT
            // 
            this.pictureFFT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureFFT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureFFT.Location = new System.Drawing.Point(3, 345);
            this.pictureFFT.Name = "pictureFFT";
            this.pictureFFT.Size = new System.Drawing.Size(878, 336);
            this.pictureFFT.TabIndex = 0;
            this.pictureFFT.TabStop = false;
            // 
            // table
            // 
            this.table.ColumnCount = 1;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table.Controls.Add(this.pictureWave, 0, 0);
            this.table.Controls.Add(this.pictureFFT, 0, 1);
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.Location = new System.Drawing.Point(0, 0);
            this.table.Name = "table";
            this.table.RowCount = 2;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table.Size = new System.Drawing.Size(884, 684);
            this.table.TabIndex = 1;
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(194, 0);
            this.trackBar.Maximum = 22000;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(523, 45);
            this.trackBar.TabIndex = 2;
            this.trackBar.TickFrequency = 10;
            // 
            // label
            // 
            this.label.Location = new System.Drawing.Point(723, 0);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(87, 26);
            this.label.TabIndex = 3;
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AudioWave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 684);
            this.Controls.Add(this.label);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.table);
            this.Name = "AudioWave";
            this.Text = "AudioWave";
            ((System.ComponentModel.ISupportInitialize)(this.pictureWave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureFFT)).EndInit();
            this.table.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureWave;
        private System.Windows.Forms.PictureBox pictureFFT;
        private System.Windows.Forms.TableLayoutPanel table;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label label;
    }
}