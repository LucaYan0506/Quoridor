﻿
namespace Quoridor
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.gridPanel = new System.Windows.Forms.Panel();
            this.pawnPictureBox1 = new Quoridor.PawnPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pawnPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(728, 338);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // gridPanel
            // 
            this.gridPanel.BackgroundImage = global::Quoridor.Properties.Resources.grid;
            this.gridPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gridPanel.Location = new System.Drawing.Point(0, 0);
            this.gridPanel.Name = "gridPanel";
            this.gridPanel.Size = new System.Drawing.Size(600, 603);
            this.gridPanel.TabIndex = 1;
            this.gridPanel.Click += new System.EventHandler(this.grid_Click);
            // 
            // pawnPictureBox1
            // 
            this.pawnPictureBox1.BackColor = System.Drawing.Color.White;
            this.pawnPictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pawnPictureBox1.BackgroundImage")));
            this.pawnPictureBox1.Location = new System.Drawing.Point(607, 274);
            this.pawnPictureBox1.Name = "pawnPictureBox1";
            this.pawnPictureBox1.Size = new System.Drawing.Size(54, 54);
            this.pawnPictureBox1.TabIndex = 3;
            this.pawnPictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 600);
            this.Controls.Add(this.pawnPictureBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gridPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pawnPictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel gridPanel;
        private System.Windows.Forms.Button button1;
        private PawnPictureBox pawnPictureBox1;
    }
}