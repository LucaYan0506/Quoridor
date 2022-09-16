
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
            this.gridPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.HorizontalLine = new System.Windows.Forms.PictureBox();
            this.VerticalLine = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pawnTurn = new Quoridor.PawnPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.HorizontalLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VerticalLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pawnTurn)).BeginInit();
            this.SuspendLayout();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(606, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Your turn";
            // 
            // HorizontalLine
            // 
            this.HorizontalLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(67)))), ((int)(((byte)(33)))));
            this.HorizontalLine.Location = new System.Drawing.Point(611, 109);
            this.HorizontalLine.Name = "HorizontalLine";
            this.HorizontalLine.Size = new System.Drawing.Size(134, 14);
            this.HorizontalLine.TabIndex = 0;
            this.HorizontalLine.TabStop = false;
            this.HorizontalLine.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Line_MouseDown);
            this.HorizontalLine.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Line_MouseMove);
            this.HorizontalLine.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Line_MouseUp);
            // 
            // VerticalLine
            // 
            this.VerticalLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(67)))), ((int)(((byte)(33)))));
            this.VerticalLine.Location = new System.Drawing.Point(611, 140);
            this.VerticalLine.Name = "VerticalLine";
            this.VerticalLine.Size = new System.Drawing.Size(13, 134);
            this.VerticalLine.TabIndex = 4;
            this.VerticalLine.TabStop = false;
            this.VerticalLine.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Line_MouseDown);
            this.VerticalLine.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Line_MouseMove);
            this.VerticalLine.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Line_MouseUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(630, 308);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "label2";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(629, 372);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pawnTurn
            // 
            this.pawnTurn.Location = new System.Drawing.Point(611, 37);
            this.pawnTurn.Name = "pawnTurn";
            this.pawnTurn.Size = new System.Drawing.Size(54, 54);
            this.pawnTurn.TabIndex = 3;
            this.pawnTurn.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 600);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.VerticalLine);
            this.Controls.Add(this.HorizontalLine);
            this.Controls.Add(this.pawnTurn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.HorizontalLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VerticalLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pawnTurn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel gridPanel;
        private System.Windows.Forms.Label label1;
        private PawnPictureBox pawnTurn;
        private System.Windows.Forms.PictureBox HorizontalLine;
        private System.Windows.Forms.PictureBox VerticalLine;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}