using System;
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
using workPermit.Models;

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
            txtAuthorizing4.Text = thisPermit.Authorizing;
            txtControllerPPN.Text = thisPermit.ControllerPPN;
            txtAuthorizingPNW1.Text = thisPermit.AuthorizingPNW;
            txtAuthorizingPPN.Text = thisPermit.AuthorizingPPN;
            txtAuthorizingPPN1.Text = thisPermit.AuthorizingPPN;
            txtAuthorizingPZ.Text = thisPermit.AuthorizingPZ;
            txtAuthorizingPZ1.Text = thisPermit.AuthorizingPZ;
            txtHolder3.Text = thisPermit.Holder;
            txtHolderName.Text = thisPermit.Holder;
            txtHolderName2.Text = thisPermit.Holder;
            txtApplicant1.Text = thisPermit.Applicant;
            txtDate1.Text = thisPermit.Date.ToShortDateString();
            txtDate2.Text = thisPermit.Date.ToShortDateString();
            txtHolderPhone.Text = thisPermit.CompanyPhone;
            txtPerminNumber.Text = thisPermit.Number;
            txtWorkDescription.Text = thisPermit.Description;
            SetUsers();
            pc = new printController(this);
            pc.StartPosition = FormStartPosition.CenterParent;
            RecreateChecks(1);
            pc.Show(this);

        }

        private void SetUsers()
        {
            txtUser1.Text = "";
            txtUser2.Text = "";
            txtUser3.Text = "";
            txtUser4.Text = "";
            txtUser5.Text = "";
            txtUser6.Text = "";
            bool insertComa = false;
            if (thisPermit.Users.Any())
            {
                int i = 0;
                foreach (var u in thisPermit.Users)
                {
                    i++;
                    if (i % 9 == 0)
                    {
                        insertComa = true;
                        i = 1;
                    }
                    if (insertComa)
                    {
                        pnlP1.Controls["txtUser" + i.ToString()].Text += "," + u.Trim();
                    }
                    else
                    {
                        pnlP1.Controls["txtUser" + i.ToString()].Text += u.Trim();
                    }
                    
                }
            }
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

        public void RemoveAllChecks()
        {
            if (thisPermit.CheckKeeper.Items.Any())
            {
                foreach(WorkPermitCheck c in thisPermit.CheckKeeper.Items)
                {
                    if (c.Page == 1)
                    {
                        pBox.Controls.Remove(pBox.Controls[c.Name]);
                    }
                    else
                    {
                        pBox2.Controls.Remove(pBox2.Controls[c.Name]);
                    }
                   
                }
                thisPermit.CheckKeeper.Items.Clear();
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
                    //RecreateChecks(panelId);
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

        public void RecreateChecks(int page)
        {
            //Recreates checks for specified page of work permit
            if (thisPermit.CheckKeeper.Items.Any(i => i.Page == page))
            {
                foreach(WorkPermitCheck c in thisPermit.CheckKeeper.Items.Where(i => i.Page == page))
                {
                    PictureBox pb;
                    PictureBox parent;

                    if (c.Page == 1)
                    {
                        parent = pBox;
                    }
                    else
                    {
                        parent = pBox2;
                    }
                    pb = PaintCheck(new Point(c.XPoint, c.YPoint), parent, c);
                    
                    c.Picture = pb;
                }
            }
        }

        private void pBox2_Click(object sender, EventArgs e)
        {
            Point p = ((MouseEventArgs)e).Location;
            CreateCheck(p, pBox2);
        }

        private void txtLocal_TextChanged(object sender, EventArgs e)
        {

        }

        private PictureBox PaintCheck(Point ClickPoint, PictureBox Parent, WorkPermitCheck check =null)
        {

            PictureBox pb = new PictureBox();
            

            if (check == null)
            {
                //it has just been checked with mouse
                pb.Location = new Point(ClickPoint.X-5, ClickPoint.Y-5);
                pb.Name = $"Pb_X{pb.Location.X}_Y{pb.Location.Y}";
            }
            else
            {
                //it's been restored from memory 
                pb.Location = ClickPoint;
                pb.Name = check.Name;
            }
            pb.Size = new Size(10, 10);
            pb.Image = workPermit.Properties.Resources.X_mark_16;
            
            Parent.Controls.Add(pb);
            pb.SizeMode = PictureBoxSizeMode.CenterImage;
            pb.BringToFront();
            pb.Click += pb_Click;
            return pb;
        }

        protected override Point ScrollToControl(Control activeControl)
        {
            Point pt = this.AutoScrollPosition;
            return pt;
        }

        private void CreateCheck(Point ClickPoint, PictureBox pic)
        {
            PictureBox pb = PaintCheck(ClickPoint, pic);
            pb.Parent = pic;
            
            WorkPermitCheck wpc = new WorkPermitCheck()
            {
                Page = pc.currentPage,
                WorkPermitId = thisPermit.WorkPermitId,
                XPoint = ClickPoint.X - (pb.Width / 2),
                YPoint = ClickPoint.Y - (pb.Height / 2),
                CreatedOn = DateTime.Now,
                Name = pb.Name,
                Picture = pb
            };
            thisPermit.CheckKeeper.Items.Add(wpc);
        }

        private void pBox_Click(object sender, EventArgs e)
        {
            Point p = ((MouseEventArgs)e).Location;
            CreateCheck(p, pBox);
        }

        private void pb_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            if(thisPermit.CheckKeeper.Items.Any(i => i.Name == pb.Name))
            {
                thisPermit.CheckKeeper.Items.Remove(thisPermit.CheckKeeper.Items.Where(i => i.Name == pb.Name).FirstOrDefault());
            }
            if (pc.currentPage == 1)
            {
                pBox.Controls.Remove(pBox.Controls[pb.Name]);
            }
            else
            {
                pBox2.Controls.Remove(pBox2.Controls[pb.Name]);
            }
            
        }
    }
}
