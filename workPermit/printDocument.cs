﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace workPermit
{
    public partial class frmP1 : Form
    {
        PrintDocument printDocument = new PrintDocument();
        PrinterSettings pSettings = new PrinterSettings();
        Panel pnl = new Panel();
        printController pc;
        WorkPermit thisPermit = new WorkPermit();
        public frmP1(WorkPermit wp)
        {
            thisPermit = wp;
            InitializeComponent();
        }

        private void formLoaded(object sender, EventArgs e)
        {
            txtDepartment.Text = thisPermit.Department;
            txtLocal.Text = thisPermit.Place;
            //txtDate.Text = DateTime.Today.ToString("dd.MM.yyyy");
            txtDate.Text = thisPermit.Date.ToShortDateString();
            txtHourFrom.Text = thisPermit.HourFrom;
            txtHourTo.Text = thisPermit.HourTo;
            txtApplicant.Text = thisPermit.Applicant;
            txtHolderCompany.Text = thisPermit.CompanyName;
            txtApplicantPhone.Text = "";
            txtApplicant3.Text = thisPermit.Applicant;
            txtAuthorizing3.Text = thisPermit.Authorizing;
            txtHolder3.Text = thisPermit.Holder;
            txtHolderName.Text = thisPermit.Holder;
            txtHolderPhone.Text = thisPermit.CompanyPhone;
            txtPerminNumber.Text = thisPermit.Number;
            txtWorkDescription.Text = thisPermit.Description;
            txtUser1.Text = "";
            txtUser2.Text = "";
            txtUser3.Text = "";
            txtUser4.Text = "";
            txtUser5.Text = "";
            txtUser6.Text = "";
            if (thisPermit.Users.Any())
            {
                int i = 0;
                foreach(var u in thisPermit.Users)
                {
                    i++;
                    pnlP1.Controls["txtUser" + i.ToString()].Text = u;
                }
            }
            pc = new printController(this);
            pc.StartPosition = FormStartPosition.CenterParent;
            pc.Show(this);

        }

        public void printMe()
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.PrinterSettings.Duplex = Duplex.Default;
            printDialog.PrinterSettings.DefaultPageSettings.PaperSize = new PaperSize("PaperA4", 840, 1180);
            printDialog.PrinterSettings.DefaultPageSettings.Landscape = false;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pSettings = printDialog.PrinterSettings;
                int activePnl = pc.currentPage;
                for (int i = 0; i < 2; i++)
                {
                    panelOnTop(i + 1);
                    pnl = (Panel)this.Controls["pnlP" + (i + 1).ToString()];
                    printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
                    printDocument.PrinterSettings = pSettings;
                    printDocument.Print();
                }
                pc.currentPage = activePnl;
                panelOnTop(activePnl);
            }
         }
            

        public void panelOnTop(int panelId)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.GetType() == typeof(Panel) && ctrl.Name.Substring(ctrl.Name.Length - 1, 1) == panelId.ToString())
                {
                    ctrl.Visible = true;
                    this.Text = "Strona " + panelId.ToString();
                }
                else
                {
                    ctrl.Visible = false;
                }
            }
        }


        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap memoryImage = new Bitmap(pnl.Width, pnl.Height);
            pnl.DrawToBitmap(memoryImage, new Rectangle(0, 0, pnl.Width, pnl.Height));
            foreach(Control c in pnl.Controls)
            {
                if (c.Name != "pBox" && c.Name != "pBox2")//don't print the pricturebox
                {
                    c.DrawToBitmap(memoryImage, new Rectangle(c.Left, c.Top,c.Width,c.Height));
                } 
            }
            //string projectPath =  Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            //projectPath += "\\photo.png";
            //memoryImage.Save(projectPath, System.Drawing.Imaging.ImageFormat.Png);
            RectangleF bounds = e.PageSettings.PrintableArea;
            float factor = ((float)memoryImage.Height/(float)memoryImage.Width);
            e.Graphics.DrawImage(memoryImage, bounds.Left, bounds.Top, bounds.Width, factor * bounds.Width);
        }

        private void pBox2_Click(object sender, EventArgs e)
        {

        }
    }
}