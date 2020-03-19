using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace workPermit
{
    public class WorkPermitKeeper
    {
        public List<WorkPermit> WorkPermits { get; set; }

        public WorkPermitKeeper(List<WorkPermit> permits)
        {
            WorkPermits = new List<WorkPermit>();
            WorkPermits = permits;
        }

        public WorkPermitKeeper()
        {
            WorkPermits = new List<WorkPermit>();
            Download();
        }

        

        public void Download()
        {
            WorkPermits.Clear();
            string sql = "SELECT * FROM tbWorkPermits ORDER BY dateAdded DESC;";

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
                            WorkPermit wp = new WorkPermit
                            {
                                WorkPermitId = reader.GetInt32(reader.GetOrdinal("workPermitId")),
                                Date = reader.GetDateTime(reader.GetOrdinal("date")),
                                Number = reader["number"].ToString(),
                                HourFrom = reader["hourFrom"].ToString(),
                                HourTo = reader["hourTo"].ToString(),
                                Description = reader["description"].ToString(),
                                Department = reader["department"].ToString(),
                                Place = reader["place"].ToString(),
                                CompanyName = reader["company"].ToString(),
                                CompanyPhone = reader["companyPhone"].ToString(),
                                Applicant = reader["applicant"].ToString(),
                                Holder = reader["holder"].ToString(),
                                Authorizing = reader["authorizing"].ToString(),
                                DateAdded = reader.GetDateTime(reader.GetOrdinal("dateAdded")),
                                Users = reader["users"].ToString().Split(',').ToList()

                            };
                            WorkPermits.Add(wp);
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
                    sql = "DELETE FROM tbWorkPermits WHERE workPermitId IN (" + sql + ");";
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

        public List<string>AutoCompleteCollection(string fldName)
        {
            List<string> AcList = new List<string>();
            foreach(var wp in WorkPermits)
            {
                switch (fldName)
                {
                    case "Department":
                        if (!AcList.Contains(wp.Department))
                        {
                            AcList.Add(wp.Department);
                        }
                        break;
                    case "Place":
                        if (!AcList.Contains(wp.Place))
                        {
                            AcList.Add(wp.Place);
                        }
                        break;
                    case "Description":
                        if (!AcList.Contains(wp.Description))
                        {
                            AcList.Add(wp.Description);
                        }
                        break;
                    case "CompanyName":
                        if (!AcList.Contains(wp.CompanyName))
                        {
                            AcList.Add(wp.CompanyName);
                        }
                        break;
                    case "Applicant":
                    case "Authorizing":
                    case "Holder":
                    case "Users":
                        if (!AcList.Contains(wp.Applicant))
                        {
                            AcList.Add(wp.Applicant);
                        }
                        if (!AcList.Contains(wp.Authorizing))
                        {
                            AcList.Add(wp.Authorizing);
                        }
                        if (!AcList.Contains(wp.Holder))
                        {
                            AcList.Add(wp.Holder);
                        }
                        foreach(var u in wp.GetUsers())
                        {
                            if (!AcList.Contains(u.ToString()))
                            {
                                AcList.Add(u.ToString());
                            }
                        }
                        break;
                }
            }
            return AcList;
        }

        public string getNewNumber()
        {
            string str = "";
            int highestYear=0;
            int highestNumber = 0;
            Download();
            
            foreach(var wp in WorkPermits)
            //get the highest year and week
            {
                var arr = wp.Number.Split('/');

                if (Convert.ToInt32(arr[1]) >= highestYear)
                {
                    highestYear = Convert.ToInt32(arr[1]);
                    if (Convert.ToInt32(arr[0]) > highestNumber)
                    {
                        highestNumber = Convert.ToInt32(arr[0]);
                    }
                }
            }
            if (highestYear < DateTime.Now.Year)
            {
                highestNumber = 0;
            }

            str = (highestNumber+1).ToString() + "/" + DateTime.Now.Year.ToString() ;
            if (highestNumber < 10)
            {
                str = "0" + str;
            }
            return str;
        }

    }
}
