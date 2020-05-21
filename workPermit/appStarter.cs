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
using System.Linq.Dynamic;
using System.Threading;

namespace workPermit
{
    public partial class appStarter : Form
    {
        WorkPermitKeeper Keeper = new WorkPermitKeeper();
        bool IsSearched = false;
        Task SearchTask = null;
        CancellationTokenSource Cts;
        SynchronizationContext SyncContext;
        public appStarter()
        {
            InitializeComponent();
            SyncContext = SynchronizationContext.Current;
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
            
        }

        private void Beautify()
        {
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
            dgWorkPermits.Columns[12].HeaderText = "Zatwierdził";
            dgWorkPermits.Columns[13].HeaderText = "Dodał";
            dgWorkPermits.Columns[14].Visible = false;
            dgWorkPermits.Columns[15].Visible = false;
            dgWorkPermits.Columns[16].Visible = false;
            dgWorkPermits.Columns[17].HeaderText = "Data dodania";
            dgWorkPermits.Columns[18].Visible = false;
            dgWorkPermits.Columns[19].Visible = false;
            dgWorkPermits.Columns[20].Visible = false;
            dgWorkPermits.Columns[21].Visible = false;
            dgWorkPermits.Columns[22].Visible = false;
            dgWorkPermits.DefaultCellStyle.Font = new Font("Arial", 12);
            dgWorkPermits.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12);
            dgWorkPermits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dgWorkPermits.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgWorkPermits.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgWorkPermits.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgWorkPermits.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgWorkPermits.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        private void formLoaded(object sender, EventArgs e)
        {
            fillTable();
            SetupDataBinding();
            Beautify();
            txtSearch.Text = "Szukaj..";
            txtSearch.ForeColor = Color.Gray;
            txtSearch.BorderStyle = BorderStyle.Fixed3D;
        }

        private void SetupDataBinding()
        {
            Keeper.Filtered = Keeper.WorkPermits;

            dgWorkPermits.DataBindings.Clear();
            dgWorkPermits.DataSource = null;
            dgWorkPermits.DataSource = Keeper.WorkPermits;
            dgWorkPermits.AutoResizeRows();
        }

        private void UpdateSearch(string str = "")
        {
            if  (!str.Contains("Szukaj"))
            {
                if (str.Length > 0)
                {
                    IsSearched = true;
                    if (SearchTask != null)
                    {
                        //there's active task, lets cancel it before triggering new one
                        Cts.Cancel();
                    }
                    Cts = new CancellationTokenSource();
                    SearchTask = Search(str, Cts.Token);
                }
                else
                {
                    if (IsSearched)
                    {
                        fillTable();
                        SetupDataBinding();
                        Beautify();
                        IsSearched = false;
                    }
                }
                
            }
            
        }

        private async Task Search(string str, CancellationToken token)
        {
            await Task.Delay(1000);
            if (!token.IsCancellationRequested)
            {
                //SynchronizationContext.SetSynchronizationContext(SyncContext);
                var pers = Keeper.WorkPermits.Where(c => c.Applicant.ToLower().Contains(str) || c.Authorizing.ToLower().Contains(str) || c.CompanyName.ToLower().Contains(str) || c.CompanyPhone.ToLower().Contains(str) || c.Date.ToString().Contains(str) || c.Place.ToLower().Contains(str) || c.Holder.ToLower().Contains(str) || c.Department.ToLower().Contains(str) || c.Description.ToLower().Contains(str) || c.Number.ToLower().Contains(str));
                List<WorkPermit> Permits = new List<WorkPermit>(pers);
                fillTable(Permits);
                //SetupDataBinding();
                Beautify();
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
            WorkPermit wp = Keeper.WorkPermits.Where(i => i.WorkPermitId == wpId).FirstOrDefault();
            dataFiller df = new dataFiller(Keeper,wpId);

            df.Show();
        }

        private void Refresh(object sender, EventArgs e)
        {
            Keeper.Download();
            fillTable();
            SetupDataBinding();
            Beautify();
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

        private void dgWorkPermits_SortStringChanged(object sender, Zuby.ADGV.AdvancedDataGridView.SortEventArgs e)
        {
            ApplySort();
        }

        private void ApplySort()
        {
            try
            {
                if (string.IsNullOrEmpty(dgWorkPermits.SortString) == true)
                    return;

                var sortStr = dgWorkPermits.SortString.Replace("[", "").Replace("]", "");

                if (string.IsNullOrEmpty(dgWorkPermits.FilterString) == true)
                {
                    // the grid is not filtered!
                    Keeper.WorkPermits = Keeper.WorkPermits.OrderBy(sortStr).ToList();
                    dgWorkPermits.DataSource = Keeper.WorkPermits;
                }
                else
                {
                    // the grid is filtered!
                    Keeper.Filtered = Keeper.Filtered.OrderBy(sortStr).ToList();
                    dgWorkPermits.DataSource = Keeper.Filtered;
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void dgWorkPermits_FilterStringChanged(object sender, Zuby.ADGV.AdvancedDataGridView.FilterEventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            try
            {
                if (string.IsNullOrEmpty(dgWorkPermits.FilterString) == true)
                {
                    dgWorkPermits.DataSource = Keeper.WorkPermits;
                    Keeper.Filtered = Keeper.WorkPermits;
                }
                else
                {
                    var listfilter = FilterStringConverter(dgWorkPermits.FilterString);

                    Keeper.Filtered = Keeper.Filtered.Where(listfilter).ToList();

                    dgWorkPermits.DataSource = Keeper.Filtered;

                }
            }
            catch (Exception ex)
            {
            }
        }

        private string FilterStringConverter(string filter)
        {
            string newColFilter = "";

            filter = filter.Replace("(", "").Replace(")", "");

            var colFilterList = filter.Split(new string[] { "AND" }, StringSplitOptions.None);

            string andOperator = "";

            foreach (var colFilter in colFilterList)
            {
                newColFilter += andOperator;

                var colName = "";

                // Step 1: BOOLEAN Check 
                if (colFilter.Contains(" IN ") == false && colFilter.Split('=').Length == 2)
                {
                    // if the filter string is in the form "ColumnName=value". example = "(InAlarm != null && (InAlarm == true))";
                    colName = colFilter.Split('=')[0];
                    var booleanVal = colFilter.Split('=')[1];

                    newColFilter += $"({colName} != null && ({colName} == {booleanVal}))";

                    continue;
                }

                // Step 2: NUMBER (int/decimal/double/etc) and STRING Check
                if (colFilter.Contains(" IN ") == true)
                {
                    var temp1 = colFilter.Trim().Split(new string[] { "IN" }, StringSplitOptions.None);

                    colName = GetStringBetweenChars(temp1[0], '[', ']');

                    var filterValsList = temp1[1].Split(',');

                    newColFilter += string.Format("({0} != null && (", colName);

                    string orOperator = "";

                    foreach (var filterVal in filterValsList)
                    {
                        double tempNum = 0;
                        if (Double.TryParse(filterVal, out tempNum))
                            newColFilter += string.Format("{0} {1} = {2}", orOperator, colName, filterVal.Trim());
                        else
                            newColFilter += string.Format("{0} {1}.Contains({2})", orOperator, colName, filterVal.Trim());

                        orOperator = " OR ";
                    }

                    newColFilter += "))";
                }

                // Step 3: DATETIME Check
                if (colFilter.Contains(" LIKE ") == true && colFilter.Contains("Convert[") == true)
                {
                    // first of all remove the cast
                    var colFilterNoCast = colFilter.Replace("Convert", "").Replace(", 'System.String'", "");

                    var filterValsList = colFilterNoCast.Trim().Split(new string[] { "OR" }, StringSplitOptions.None);

                    colName = GetStringBetweenChars(filterValsList[0], '[', ']');

                    newColFilter += string.Format("({0} != null && (", colName);

                    string orOperator = "";

                    foreach (var filterVal in filterValsList)
                    {
                        var v = GetStringBetweenChars(filterVal, '%', '%');

                        newColFilter += string.Format("{0} {1}.Date = DateTime.Parse('{2}')", orOperator, colName, v.Trim());

                        orOperator = " OR ";
                    }

                    newColFilter += "))";
                }

                andOperator = " AND ";
            }

            return newColFilter.Replace("'", "\"");
        }

        private string GetStringBetweenChars(string input, char startChar, char endChar)
        {
            string output = input.Split(startChar, endChar)[1];
            return output;
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            dgWorkPermits.CleanFilter();
        }
    }
}
