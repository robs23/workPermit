using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace workPermit
{
    public partial class printController : Form
    {
        public int currentPage = 1;
        private frmP1 _parentForm;
        public printController(frmP1 parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        private void formLoaded(object sender, EventArgs e)
        {
            ToolTip tipper = new ToolTip();
            tipper.AutoPopDelay = 5000;
            tipper.InitialDelay = 1000;
            tipper.ReshowDelay = 500;
            tipper.ShowAlways = true;
            tipper.SetToolTip(btnNext, "Wyświetl następną stronę");
            tipper.SetToolTip(btnPrev, "Wyświetl poprzednią stronę");
            tipper.SetToolTip(btnPrint, "Drukuj dokument");
        }

        private void prevPage(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage -= 1;
                lblPage.Text = (currentPage).ToString();
            }
        }

        private void nextPage(object sender, EventArgs e)
        {
            if(currentPage < 3)
            {
                currentPage += 1;
                lblPage.Text = (currentPage).ToString();
            }
        }

        private void pageChanged(object sender, EventArgs e)
        {
            _parentForm.panelOnTop(Convert.ToInt32(lblPage.Text));
     
        }

        private void printDoc(object sender, EventArgs e)
        {
            _parentForm.printMe();
        }
    }
}
