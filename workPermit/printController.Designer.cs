namespace workPermit
{
    partial class printController
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
            this.lblPage = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnDeleteChecks = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblPage.Location = new System.Drawing.Point(38, 13);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(16, 16);
            this.lblPage.TabIndex = 0;
            this.lblPage.Text = "1";
            this.lblPage.TextChanged += new System.EventHandler(this.pageChanged);
            // 
            // btnPrev
            // 
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPrev.Location = new System.Drawing.Point(8, 9);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(26, 23);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "<";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.prevPage);
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnNext.Location = new System.Drawing.Point(58, 9);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(26, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.nextPage);
            // 
            // btnPrint
            // 
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPrint.Location = new System.Drawing.Point(143, 7);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 26);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "Drukuj";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.printDoc);
            // 
            // btnDeleteChecks
            // 
            this.btnDeleteChecks.Image = global::workPermit.Properties.Resources.rubber_16;
            this.btnDeleteChecks.Location = new System.Drawing.Point(98, 9);
            this.btnDeleteChecks.Name = "btnDeleteChecks";
            this.btnDeleteChecks.Size = new System.Drawing.Size(24, 23);
            this.btnDeleteChecks.TabIndex = 4;
            this.btnDeleteChecks.UseVisualStyleBackColor = true;
            this.btnDeleteChecks.Click += new System.EventHandler(this.btnDeleteChecks_Click);
            // 
            // printController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 36);
            this.ControlBox = false;
            this.Controls.Add(this.btnDeleteChecks);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lblPage);
            this.MaximizeBox = false;
            this.Name = "printController";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kontroler";
            this.Load += new System.EventHandler(this.formLoaded);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnDeleteChecks;
    }
}