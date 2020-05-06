using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;
using workPermit.Models;

namespace workPermit
{
    public class WorkPermit
    {
        public int WorkPermitId { get; set; }
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string HourFrom { get; set; }
        public string HourTo { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string Place { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhone { get; set; }
        public string Applicant { get;set;}
        public string Holder { get; set; }
        public List<string> Users { get; set; }
        public string Authorizing { get; set; }
        public string AuthorizingPPN { get; set; }
        public string AuthorizingPZ { get; set; }
        public string AuthorizingPNW { get; set; }
        public string ControllerPPN { get; set; }
        public DateTime DateAdded { get; set; }

        public WorkPermitCheckKeeper CheckKeeper { get; set; }
        public WorkPermitCheckKeeper InitialChecks { get; set; }
        public int Type { get; set; }   //1-new, 2-existent
        public bool isDirty { get
            {
                bool boo = false;
                if (initialState != null)
                {
                    if (!Comparison.AreEqual(this.Number, initialState.Number)
                        || this.Date!= initialState.Date
                        || !Comparison.AreEqual(this.HourFrom, initialState.HourFrom)
                        || !Comparison.AreEqual(this.HourTo, initialState.HourTo)
                        || !Comparison.AreEqual(this.Applicant, initialState.Applicant)
                        || !Comparison.AreEqual(this.Holder, initialState.Holder)
                        || !Comparison.AreEqual(this.Authorizing, initialState.Authorizing)
                        || !Comparison.AreEqual(this.AuthorizingPPN, initialState.AuthorizingPPN)
                        || !Comparison.AreEqual(this.AuthorizingPZ, initialState.AuthorizingPZ)
                        || !Comparison.AreEqual(this.AuthorizingPNW, initialState.AuthorizingPNW)
                        || !Comparison.AreEqual(this.ControllerPPN, initialState.ControllerPPN))
                    {
                        boo = true;
                    }
                }
                if (boo == false)
                {
                    //if not yet dirty, check checks
                    if (InitialChecks != null)
                    {
                        if (!this.CheckKeeper.IsEqual(InitialChecks))
                        {
                            boo = true;
                        }
                    }
                }
                return boo;
            }
        }
        public WorkPermit initialState { get; set; }
        public WorkPermit(int Id = 0)
        {
            this.WorkPermitId = Id;
            if (Id == 0)
            {
                this.Date = DateTime.Now.Date;
                this.Type = 1;
                CheckKeeper = new WorkPermitCheckKeeper();
                getNewNumber();
            }else
            {
                this.Type = 2;
                BringDetails();
                BringChecks();
            }
            initialState = this.Clone();
            InitialChecks = this.CheckKeeper;
        }

        public WorkPermit()
        {
            CheckKeeper = new WorkPermitCheckKeeper();
        }

        public string GetUsers()
        {
            string str = "";
            if(Users != null)
            {
                for (int i = 0; i < Users.Count; i++)
                {
                    str += Users[i] + ", ";
                }
                if (str.Length > 0)
                {
                    str = str.Substring(0, str.Length - 2);
                }
            }
            return str;
        }

        public void BringDetails()
        {
            string sql = "SELECT * FROM tbWorkPermits WHERE workPermitId = " + this.WorkPermitId.ToString() ;

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
                            Date = reader.GetDateTime(reader.GetOrdinal("date"));
                            Number = reader["number"].ToString();
                            HourFrom = reader["hourFrom"].ToString();
                            HourTo = reader["hourTo"].ToString();
                            Description = reader["description"].ToString();
                            Department = reader["department"].ToString();
                            Place = reader["place"].ToString();
                            CompanyName = reader["company"].ToString();
                            CompanyPhone = reader["companyPhone"].ToString();
                            Applicant = reader["applicant"].ToString();
                            Holder = reader["holder"].ToString();
                            Authorizing = reader["authorizing"].ToString();
                            AuthorizingPPN = reader["authorizingPPN"].ToString();
                            AuthorizingPZ = reader["authorizingPZ"].ToString();
                            AuthorizingPNW = reader["authorizingPNW"].ToString();
                            ControllerPPN = reader["controllerPPN"].ToString();
                            DateAdded = reader.GetDateTime(reader.GetOrdinal("dateAdded"));
                            string users = reader["users"].ToString();
                            Users = users.Split(',').ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message.ToString());
                //MessageBox.Show(String.Format("Nie udało się nawiązać połączenia z bazą danych", "Błąd połączenia z bazą danych"));
            }
        }

        public void BringChecks()
        {
            try
            {
                CheckKeeper = new WorkPermitCheckKeeper();
                CheckKeeper.Download(this.WorkPermitId);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                //MessageBox.Show(String.Format("Nie udało się nawiązać połączenia z bazą danych", "Błąd połączenia z bazą danych"));
            }
        }

        public WorkPermit Clone()
        {
            WorkPermit newWp = new WorkPermit();
            newWp.Date = this.Date;
            newWp.HourFrom = this.HourFrom;
            newWp.HourTo = this.HourTo;
            newWp.Number = this.Number;
            newWp.Place = this.Place;
            newWp.Type = this.Type;
            newWp.WorkPermitId = this.WorkPermitId;
            newWp.Department = this.Department;
            newWp.Description = this.Description;
            newWp.CompanyName = this.CompanyName;
            newWp.CompanyPhone = this.CompanyPhone;
            newWp.Applicant = this.Applicant;
            newWp.Authorizing = this.Authorizing;
            newWp.AuthorizingPPN = this.AuthorizingPPN;
            newWp.AuthorizingPZ = this.AuthorizingPZ;
            newWp.AuthorizingPNW = this.AuthorizingPNW;
            newWp.ControllerPPN = this.ControllerPPN;
            newWp.Holder = this.Holder;
            newWp.Users = this.Users;
            newWp.CheckKeeper = new WorkPermitCheckKeeper();
            newWp.CheckKeeper = this.CheckKeeper.CloneJson<WorkPermitCheckKeeper>();
            return newWp;
        }

        public void Save()
        {
            string sql = "";
            if (this.Type == 1)
            {
                sql = @"INSERT INTO tbWorkPermits (number, date, hourFrom, hourTo, description, department, place, company, companyPhone, applicant, holder, users, authorizing, authorizingPPN, authorizingPZ, authorizingPNW, controllerPPN, dateAdded) 
                        output INSERTED.WorkPermitId 
                        VALUES (@number, @date, @hourFrom, @hourTo, @description, @department, @place, @company, @companyPhone, @applicant, @holder, @users, @authorizing, @authorizingPPN, @authorizingPZ, @authorizingPNW, @controllerPPN, @dateAdded)";
            }else if (this.Type == 2)
            {
                sql = @"UPDATE tbWorkPermits SET number=@number, date=@date, hourFrom=@hourFrom, hourTo=@hourTo, description=@description, department=@department, place=@place, company=@company, companyPhone=@companyPhone, applicant=@applicant, holder=@holder, users=@users, authorizing=@authorizing, authorizingPPN=@authorizingPPN, authorizingPZ=@authorizingPZ, authorizingPNW=@authorizingPNW, controllerPPN=@controllerPPN
                                                      WHERE workPermitId=@id";
            }
            
            try
            {
                using (SqlConnection conn = new SqlConnection(Variables.npdConnectionString))
                {
                    SqlCommand sqlComand;
                    using (sqlComand = new SqlCommand(sql, conn))
                    {
                        sqlComand.Parameters.AddWithValue("@number", this.Number);
                        sqlComand.Parameters.AddWithValue("@date", this.Date.ToString());
                        sqlComand.Parameters.AddWithValue("@hourFrom", this.HourFrom);
                        sqlComand.Parameters.AddWithValue("@hourTo", this.HourTo);
                        sqlComand.Parameters.AddWithValue("@description", this.Description);
                        sqlComand.Parameters.AddWithValue("@department", this.Department);
                        sqlComand.Parameters.AddWithValue("@place", this.Place);
                        sqlComand.Parameters.AddWithValue("@company", this.CompanyName);
                        sqlComand.Parameters.AddWithValue("@companyPhone", this.CompanyPhone);
                        sqlComand.Parameters.AddWithValue("@applicant", this.Applicant);
                        sqlComand.Parameters.AddWithValue("@holder", this.Holder);
                        sqlComand.Parameters.AddWithValue("@users", this.GetUsers());
                        sqlComand.Parameters.AddWithValue("@authorizing", this.Authorizing);
                        sqlComand.Parameters.AddWithValue("@authorizingPPN", this.AuthorizingPPN);
                        sqlComand.Parameters.AddWithValue("@authorizingPZ", this.AuthorizingPZ);
                        sqlComand.Parameters.AddWithValue("@authorizingPNW", this.AuthorizingPNW);
                        sqlComand.Parameters.AddWithValue("@controllerPPN", this.ControllerPPN);
                        if (this.Type == 1)
                        {
                            sqlComand.Parameters.AddWithValue("@dateAdded", DateTime.Now);
                            conn.Open();
                            this.WorkPermitId = (int)sqlComand.ExecuteScalar();
                        }
                        else
                        {
                            sqlComand.Parameters.AddWithValue("@id", this.WorkPermitId);
                            conn.Open();
                            sqlComand.ExecuteNonQuery();
                        }
                        
                        
                    }
                }
                SaveChecks();
                MessageBox.Show("Zapis zakończony powodzeniem!");
            }
            catch(Exception ex)
            { 
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                initialState = this.Clone();
                InitialChecks = this.CheckKeeper;
            }
        }

        public void SaveChecks()
        {
            string dSql = "DELETE FROM tbWorkPermitChecks WHERE WorkPermitId=@WorkPermitId";
            string iSql = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(Variables.npdConnectionString))
                {
                    SqlCommand sqlComand;
                    using (sqlComand = new SqlCommand(dSql, conn))
                    {
                        sqlComand.Parameters.AddWithValue("@WorkPermitId", this.WorkPermitId);
                        conn.Open();
                        sqlComand.ExecuteNonQuery();
                    }

                    SqlCommand iSqlCommand;
                    foreach(WorkPermitCheck c in CheckKeeper.Items)
                    {
                        iSql = "INSERT INTO tbWorkPermitChecks (WorkPermitId, Name, Page, XPoint, YPoint, CreatedOn)" +
                            " VALUES (@WorkPermitId, @Name, @Page, @XPoint, @YPoint, @CreatedOn)";
                        using(iSqlCommand = new SqlCommand(iSql, conn))
                        {
                            iSqlCommand.Parameters.AddWithValue("@WorkPermitId", this.WorkPermitId);
                            iSqlCommand.Parameters.AddWithValue("@Name", c.Name);
                            iSqlCommand.Parameters.AddWithValue("@Page", c.Page);
                            iSqlCommand.Parameters.AddWithValue("@XPoint", c.XPoint);
                            iSqlCommand.Parameters.AddWithValue("@YPoint", c.YPoint);
                            iSqlCommand.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                            iSqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void Remove()
        {
            string sql = "DELETE FROM tbWorkPermits WHERE workPermitId=@id";

            try
            {
                using (SqlConnection conn = new SqlConnection(Variables.npdConnectionString))
                {
                    SqlCommand sqlComand;
                    using (sqlComand = new SqlCommand(sql, conn))
                    {
                        sqlComand.Parameters.AddWithValue("@id", this.WorkPermitId);
                        conn.Open();
                        sqlComand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        public void getNewNumber()
        {
            WorkPermitKeeper keeper = new WorkPermitKeeper();
            this.Number= keeper.getNewNumber();
        }

    }
}
