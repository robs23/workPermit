﻿using System;
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
        public List<WorkPermit> Filtered { get; set; }

        public WorkPermitKeeper(List<WorkPermit> permits)
        {
            WorkPermits = new List<WorkPermit>();
            Filtered = null;
            WorkPermits = permits;
        }

        public WorkPermitKeeper()
        {
            WorkPermits = new List<WorkPermit>();
            Filtered = null;
            Download();
        }

        

        public void  Download()
        {
            WorkPermits.Clear();
            string sql = "SELECT * FROM tbWorkPermits ORDER BY date DESC;";

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
                                AuthorizingPPN = reader["authorizingPPN"].ToString(),
                                AuthorizingPZ = reader["authorizingPZ"].ToString(),
                                AuthorizingPNW = reader["authorizingPNW"].ToString(),
                                ControllerPPN = reader["controllerPPN"].ToString(),
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
                    case "AuthorizingPPN":
                    case "AuthorizingPZ":
                    case "AuthorizingPNW":
                    case "ControllerPPN":
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
                        if (!AcList.Contains(wp.AuthorizingPPN))
                        {
                            AcList.Add(wp.AuthorizingPPN);
                        }
                        if (!AcList.Contains(wp.AuthorizingPZ))
                        {
                            AcList.Add(wp.AuthorizingPZ);
                        }
                        if (!AcList.Contains(wp.AuthorizingPNW))
                        {
                            AcList.Add(wp.AuthorizingPNW);
                        }
                        if (!AcList.Contains(wp.ControllerPPN))
                        {
                            AcList.Add(wp.ControllerPPN);
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
            int currentYear = 0;
            int currentNumber = 0;
            Download();
            try
            {
                foreach (var wp in WorkPermits)
                //get the highest year and week
                {

                    var arr = wp.Number.Split('/');

                    if (arr.Length > 1)
                    {
                        //if not, then the format is definitely wrong
                        bool parsable = int.TryParse(arr[1], out currentYear);
                        if (parsable)
                        {
                            parsable = int.TryParse(arr[0], out currentNumber);
                            if (parsable)
                            {
                                if (currentYear >= highestYear)
                                {
                                    highestYear = currentYear;
                                    if (currentNumber > highestNumber)
                                    {
                                        highestNumber = currentNumber;
                                    }
                                }
                            }

                        }
                    }  
                }
                if (highestYear < DateTime.Now.Year)
                {
                    highestNumber = 0;
                }

                str = (highestNumber + 1).ToString() + "/" + DateTime.Now.Year.ToString();
                if (highestNumber < 10)
                {
                    str = "0" + str;
                }
            }catch(Exception ex)
            {
                MessageBox.Show($"Nie mogę wygenerować kolejnego numeru pozwolenia. Opis błędu: {ex.Message}", "Błąd numeru", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return str;
        }

    }
}
