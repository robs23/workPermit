using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace workPermit
{
    public partial class dataFiller : Form
    {
        WorkPermit wp;
        WorkPermitKeeper Keeper = new WorkPermitKeeper();
        AutoCompleteKeeper ACKeeper = new AutoCompleteKeeper();

        public dataFiller(WorkPermitKeeper keeper, int wpId=0, bool copy = false)
        {
            InitializeComponent();
            wp = new WorkPermit(wpId);
            Keeper = keeper;
            if (wp.WorkPermitId == 0)
            {
                this.Text = "Nowe pozwolenie";
                txtNumber.Text = wp.Number;
                createAutoComplete();
                this.Controls.Add(ACKeeper.Output);
            }
            else
            {
                this.Text = "Edycja pozwolenia";
                txtNumber.Text = wp.Number;
                txtDescription.Text = wp.Description;
                txtDepartment.Text = wp.Department;
                txtPlace.Text = wp.Place;
                txtCompany.Text = wp.CompanyName;
                txtPhone.Text = wp.CompanyPhone;
                txtApplicant.Text = wp.Applicant;
                txtHolder.Text = wp.Holder;
                dtDate.Value = wp.Date;
                txtUsers.Text = wp.GetUsers();
                txtAuthorizing.Text = wp.Authorizing;
                txtAuthorizingPPN.Text = wp.AuthorizingPPN;
                txtAuthorizingPZ.Text = wp.AuthorizingPZ;
                txtAuthorizingPNW.Text = wp.AuthorizingPNW;
                txtControllerPPN.Text = wp.ControllerPPN;
                cmbFrom.Text = wp.HourFrom;
                cmbTo.Text = wp.HourTo;
                if (copy)
                {
                    this.Text = "Nowe pozwolenie";
                    wp.Type = 1;
                    wp.WorkPermitId = 0;
                    wp.Date = DateTime.Now.Date;
                    dtDate.ResetText();
                    cmbFrom.ResetText();
                    cmbFrom.SelectedIndex = -1;
                    cmbTo.ResetText();
                    cmbTo.SelectedIndex = -1;
                    wp.getNewNumber();
                    txtNumber.Text = wp.Number;
                    createAutoComplete();
                    this.Controls.Add(ACKeeper.Output);
                }
            }
        }

        private void createAutoComplete()
        {
            AutoComplete AC = new AutoComplete(txtDepartment, Keeper.AutoCompleteCollection("Department"),ACKeeper);
            ACKeeper.Append(AC);
            AC = new AutoComplete(txtPlace, Keeper.AutoCompleteCollection("Place"), ACKeeper);
            ACKeeper.Append(AC);
            AC = new AutoComplete(txtCompany, Keeper.AutoCompleteCollection("CompanyName"), ACKeeper);
            ACKeeper.Append(AC);
            AC = new AutoComplete(txtApplicant, Keeper.AutoCompleteCollection("Applicant"), ACKeeper);
            ACKeeper.Append(AC);
            AC = new AutoComplete(txtAuthorizing, Keeper.AutoCompleteCollection("Authorizing"), ACKeeper);
            ACKeeper.Append(AC);
            AC = new AutoComplete(txtAuthorizingPPN, Keeper.AutoCompleteCollection("AuthorizingPPN"), ACKeeper);
            ACKeeper.Append(AC);
            AC = new AutoComplete(txtAuthorizingPZ, Keeper.AutoCompleteCollection("AuthorizingPZ"), ACKeeper);
            ACKeeper.Append(AC);
            AC = new AutoComplete(txtAuthorizingPNW, Keeper.AutoCompleteCollection("AuthorizingPNW"), ACKeeper);
            ACKeeper.Append(AC);
            AC = new AutoComplete(txtControllerPPN, Keeper.AutoCompleteCollection("ControllerPPN"), ACKeeper);
            ACKeeper.Append(AC);
            AC = new AutoComplete(txtHolder, Keeper.AutoCompleteCollection("Holder"), ACKeeper);
            ACKeeper.Append(AC);
            AC = new AutoComplete(txtUsers, Keeper.AutoCompleteCollection("Users"), ACKeeper,AutoCompleteType.append);
            ACKeeper.Append(AC);
        }

        private void Preview(object sender, EventArgs e)
        {
            UpdateWP();
            bool boo = true;
            if (wp.isDirty)
            {
                boo = false;
                DialogResult result = MessageBox.Show("Dane zostały zmienione. Aby przejść do podglądu, należy zapisać zmiany. Czy chcesz zapisać zmiany?","Zapisz zmiany",buttons: MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    boo = true;
                    wp.Save();
                }
            }
            if (boo)
            {
                frmP1 printDoc = new frmP1(wp);
                printDoc.Show();
            }
        }

        private void Save(object sender, EventArgs e)
        {
            UpdateWP();
            wp.Save();
        }

        private void UpdateWP()
        {
            wp.Number = txtNumber.Text;
            wp.Date = dtDate.Value.Date;
            wp.HourFrom = cmbFrom.Text;
            wp.HourTo = cmbTo.Text;
            wp.Applicant = txtApplicant.Text;
            wp.Authorizing = txtAuthorizing.Text;
            wp.AuthorizingPPN = txtAuthorizingPPN.Text;
            wp.AuthorizingPZ = txtAuthorizingPZ.Text;
            wp.AuthorizingPNW = txtAuthorizingPNW.Text;
            wp.ControllerPPN = txtControllerPPN.Text;
            wp.CompanyName = txtCompany.Text;
            wp.CompanyPhone = txtPhone.Text;
            wp.Department = txtDepartment.Text;
            wp.Description = txtDescription.Text;
            wp.Holder = txtHolder.Text;
            wp.Place = txtPlace.Text;
            wp.Users = txtUsers.Text.Split(',').ToList();
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
