using Squirrel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace workPermit
{
    public partial class appStarter : Form
    {
        WorkPermitKeeper Keeper = new WorkPermitKeeper();
        public appStarter()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void fillTable(List<WorkPermit>Permits = null)
        {
            
            if (Permits == null)
            {
                dgWorkPermits.DataSource = null;
                dgWorkPermits.Rows.Clear();
                dgWorkPermits.Refresh();
                Permits = Keeper.WorkPermits;
            }
            dgWorkPermits.DataSource = Permits;
            dgWorkPermits.Columns[0].HeaderText = "ID";
            dgWorkPermits.Columns[1].HeaderText = "Data";
            dgWorkPermits.Columns[2].Visible = false;
            dgWorkPermits.Columns[3].HeaderText = "Od";
            dgWorkPermits.Columns[4].HeaderText = "Do";
            dgWorkPermits.Columns[5].HeaderText = "Opis";
            dgWorkPermits.Columns[6].HeaderText = "Dział";
            dgWorkPermits.Columns[7].HeaderText = "Miejsce";
            dgWorkPermits.Columns[8].HeaderText = "Firma";
            dgWorkPermits.Columns[9].Visible = false;
            dgWorkPermits.Columns[10].HeaderText = "Wnioskujący";
            dgWorkPermits.Columns[11].HeaderText = "Posiadacz";
            dgWorkPermits.Columns[12].Visible = false;
            dgWorkPermits.Columns[13].HeaderText = "Dodano";
            dgWorkPermits.DefaultCellStyle.Font = new Font("Arial", 12);
            dgWorkPermits.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12);
            dgWorkPermits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void formLoaded(object sender, EventArgs e)
        {
            fillTable();
            txtSearch.Text = "Szukaj..";
            txtSearch.ForeColor = Color.Gray;
            txtSearch.BorderStyle = BorderStyle.Fixed3D;
        }
        
        private void UpdateSearch(string str = "")
        {
            if  (!str.Contains("Szukaj"))
            {
                if (str.Length > 1)
                {
                    List<WorkPermit> Permits = new List<WorkPermit>();
                    var pers = Keeper.WorkPermits.Where(c => c.Applicant.ToLower().Contains(str) || c.Authorizing.ToLower().Contains(str) || c.CompanyName.ToLower().Contains(str) || c.CompanyPhone.ToLower().Contains(str) || c.Date.ToString().Contains(str) || c.Place.ToLower().Contains(str) || c.Holder.ToLower().Contains(str) || c.Department.ToLower().Contains(str) || c.Description.ToLower().Contains(str) || c.Number.ToLower().Contains(str) || c.GetUsers().Contains(str));
                    foreach (var p in pers)
                    {
                        Permits.Add(p);
                    }
                    fillTable(Permits);
                }
                else
                {
                    fillTable();
                }
            }
            
        }


        private void addPermit(object sender, EventArgs e)
        {
            dataFiller df = new dataFiller(Keeper);
            df.Show();
        }

        private void searchActive(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            txtSearch.ForeColor = Color.Black;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
        }

        private void searchInactive(object sender, EventArgs e)
        {
            txtSearch.Text = "Szukaj..";
            txtSearch.ForeColor = Color.Gray;
            txtSearch.BorderStyle = BorderStyle.Fixed3D;
        }

        private void bringWorkPermit(object sender, DataGridViewCellEventArgs e)
        {
            int wpId = Convert.ToInt32(dgWorkPermits.Rows[dgWorkPermits.CurrentCell.RowIndex].Cells[0].Value);
            dataFiller df = new dataFiller(Keeper,wpId);
            df.Show();
        }

        private void Refresh(object sender, EventArgs e)
        {
            Keeper.Download();
            fillTable();
        }

        private void UpdateSearch(object sender, EventArgs e)
        {
            UpdateSearch(txtSearch.Text);
        }

        private void FormKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.F)
            {
                txtSearch.Focus();
            }
            else if(e.KeyCode==Keys.Escape){
                UpdateSearch();
                dgWorkPermits.Focus();
            }
        }

        private void Remove(object sender, EventArgs e)
        {
            if(dgWorkPermits.SelectedRows.Count == 0)
            {
                MessageBox.Show("Żaden wiersz nie jest zaznaczony. Aby usunąć wybrane wiersze, najpierw zaznacz je kliknięciem po ich lewej stronie.");
            }
            else
            {
                List<int> SelectedRows = new List<int>();
                for (int i = 0; i < dgWorkPermits.SelectedRows.Count; i++)
                {
                    SelectedRows.Add((int)dgWorkPermits.SelectedRows[i].Cells[0].Value);
                }
                Keeper.Remove(SelectedRows);
                fillTable();
            }
        }

        void menuClicked(object sender, EventArgs e)
        {
            if (dgWorkPermits.SelectedRows.Count == 1)
            {
                int wpId = (int)dgWorkPermits.SelectedRows[0].Cells[0].Value;
                createFrom(wpId);
            }
        }

        public void createFrom(int wpId)
        {
            dataFiller df = new dataFiller(Keeper, wpId, true);
            df.Show();
        }

        private void rightClicked(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            var dg = sender as DataGridView;
            if (dg != null)
            {
                if (e.RowIndex != -1)
                {
                    dg.ClearSelection();
                    dg.Rows[e.RowIndex].Selected = true;
                    ContextMenu cm = new ContextMenu();
                    cm.MenuItems.Add(new MenuItem("Utwórz z..", menuClicked));
                    cm.Show(dg, dg.PointToClient(Cursor.Position));
                }
            }
        }

        private async void appStarter_Shown(object sender, EventArgs e)
        {
            this.Text = "WorkPermit v." + System.Windows.Forms.Application.ProductVersion;
#if (DEBUG == false)
            ReleaseEntry release = null;
            string path = string.Empty;
            if (Directory.Exists(Static.Secrets.SquirrelAbsoluteUpdatePath))
            {
                path = Static.Secrets.SquirrelAbsoluteUpdatePath;
            }
            else if (Directory.Exists(Static.Secrets.SquirrelUpdatePath))
            {
                path = Static.Secrets.SquirrelUpdatePath;
            }
            if (!string.IsNullOrWhiteSpace(path))
            {
                using (var mgr = new UpdateManager(path))
                {
                    //SquirrelAwareApp.HandleEvents(
                    //onInitialInstall: v =>
                    //{
                    //    mgr.CreateShortcutForThisExe();
                    //    mgr.CreateRunAtWindowsStartupRegistry();
                    //},
                    //onAppUninstall: v =>
                    //{
                    //    mgr.RemoveShortcutForThisExe();
                    //    mgr.RemoveRunAtWindowsStartupRegistry();
                    //});
                    release = await mgr.UpdateApp();
                }
                if (release != null)
                {
                    MessageBox.Show("Aplikacja została zaktualizowana do nowszej wersji. Naciśnij OK aby zrestartować aplikację", "Aktualizacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //force app restart
                    UpdateManager.RestartApp();
                }
            }
            
#endif
        }
    }
}
