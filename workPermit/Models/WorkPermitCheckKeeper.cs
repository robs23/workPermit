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
                sql = $"SELECT * FROM tbWorkPermitChecks WHERE WorkPermitId={WorkPermitId} ORDER BY CreatedOn DESC;";
            }
            else
            {
                sql = "SELECT * FROM tbWorkPermitChecks ORDER BY CreatedOn DESC;";
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
                            WorkPermitCheck wp = new WorkPermitCheck
                            {
                                WorkPermitCheckId = reader.GetInt32(reader.GetOrdinal("workPermitCheckId")),
                                WorkPermitId = reader.GetInt32(reader.GetOrdinal("workPermitId")),
                                Page = reader.GetInt32(reader.GetOrdinal("Page")),
                                XPoint = reader.GetInt32(reader.GetOrdinal("XPoint")),
                                YPoint = reader.GetInt32(reader.GetOrdinal("YPoint")),
                                CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn"))
                            };
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
    }
}
