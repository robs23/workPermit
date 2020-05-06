using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace workPermit.Models
{
    public class WorkPermitCheckKeeper
    {
        public List<WorkPermitCheck> Items { get; set; }
        public WorkPermitCheckKeeper()
        {
            Items = new List<WorkPermitCheck>();
        }

        public void Download(int WorkPermitId = 0)
        {
            Items.Clear();
            string sql;

            if (WorkPermitId > 0)
            {
                sql = $"SELECT * FROM tbWorkPermitChecks WHERE WorkPermitId={WorkPermitId}";
            }
            else
            {
                sql = "SELECT * FROM tbWorkPermitChecks ORDER BY CreatedOn DESC";
            }
            

            try
            {
                using (SqlConnection conn = new SqlConnection(Variables.npdConnectionString))
                {

                    conn.Open();
                    SqlCommand sqlComand;
                    sqlComand = new SqlCommand(sql, conn);
                    using (SqlDataReader reader = sqlComand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            WorkPermitCheck wp = new WorkPermitCheck();
                            wp.WorkPermitCheckId = reader.GetInt32(reader.GetOrdinal("WorkPermitCheckId"));
                            wp.WorkPermitId = reader.GetInt32(reader.GetOrdinal("WorkPermitId"));
                            wp.Name = reader.GetString(reader.GetOrdinal("Name"));
                            wp.Page = reader.GetInt32(reader.GetOrdinal("Page"));
                            wp.XPoint = reader.GetInt32(reader.GetOrdinal("XPoint"));
                            wp.YPoint = reader.GetInt32(reader.GetOrdinal("YPoint"));
                            wp.CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn"));
                            Items.Add(wp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                //MessageBox.Show(String.Format("Nie udało się nawiązać połączenia z bazą danych", "Błąd połączenia z bazą danych"));
            }
        }

        public void Remove(List<int> ids)
        {
            DialogResult result = MessageBox.Show("Czy jesteś pewien, że chcesz usunąć " + ids.Count.ToString() + " zaznaczone wiersze?", "Potwierdź usunięcie", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                string sql = "";
                foreach (int id in ids)
                {
                    sql = sql + id.ToString() + ",";
                }
                if (sql.Length > 0)
                {
                    sql = sql.Substring(0, sql.Length - 1);
                    sql = "DELETE FROM tbWorkPermitChecks WHERE workPermitId IN (" + sql + ");";
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(Variables.npdConnectionString))
                        {
                            SqlCommand sqlComand;
                            using (sqlComand = new SqlCommand(sql, conn))
                            {
                                conn.Open();
                                sqlComand.ExecuteNonQuery();
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                    Download();
                }
            }
        }

        public bool IsEqual(WorkPermitCheckKeeper other)
        {
            bool _Result = true; //assume they are equal

            if (other != null)
            {
                if(this.Items.Count != other.Items.Count)
                {
                    _Result = false;
                }
                else
                {
                    //we'll have to check it manually
                    foreach(WorkPermitCheck i in this.Items)
                    {
                        if(!other.Items.Any(c=>c.XPoint==i.XPoint && c.YPoint == i.YPoint))
                        {
                            _Result = false;
                            break;
                        }
                    }
                    if (_Result)
                    {
                        //if still true then go the other way around
                        foreach(WorkPermitCheck c in other.Items)
                        {
                            if(!this.Items.Any(i=>i.XPoint==c.XPoint && i.YPoint == c.YPoint))
                            {
                                _Result = false;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                _Result = false;
            }

            return _Result;
        }
    }
}
