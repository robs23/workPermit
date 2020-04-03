using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace workPermit
{
    public class AutoComplete
    {
        public Control ThisControl = new Control();
        public List<string> Values = new List<string>();
        public List<string> Filtered = new List<string>();
        public Form ParentForm { get; set; }
        private bool IsActive;
        public AutoCompleteType Type;
        public bool isActive {
            get
            {
                return IsActive;
            }
            set
            {
                if(value == true)
                {
                    IsActive = true;
                    Keeper.Current = this;
                    this.Show();
                }else
                {
                    IsActive = false;
                    this.Hide();
                }
            }
        }
        public AutoCompleteKeeper Keeper = new AutoCompleteKeeper();

        public AutoComplete(AutoCompleteKeeper Keeper, AutoCompleteType type = AutoCompleteType.replace)
        {
            this.Keeper = Keeper;
            ParentForm = ThisControl.FindForm();
            Type = type;
            AddEvents();
        }

        public AutoComplete(Control ctrl, AutoCompleteKeeper Keeper, AutoCompleteType type = AutoCompleteType.replace)
        {
            ThisControl = ctrl;
            this.Keeper = Keeper;
            ParentForm = ThisControl.FindForm();
            Type = type;
            AddEvents();
        }

        public AutoComplete(Control ctrl, List<string> values, AutoCompleteKeeper Keeper, AutoCompleteType type = AutoCompleteType.replace)
        {
            ThisControl = ctrl;
            this.Keeper = Keeper;
            ParentForm = ThisControl.FindForm();
            Values = values;
            Type = type;
            AddEvents();
        }

        public void Append(string value)
        {
            Values.Add(value);
        }

        public void Update(string str = "")
        {
            List<string> results = new List<string>();
            if(str.Length>0 && str.Contains(","))
            {
                string[] arr = str.Split(',');
                if (arr.Length > 0)
                {
                    str = arr[arr.Length - 1];
                }
                else
                {
                    str = arr[0];
                }  
            }
            if (str.Length == 0)
            {
                Filtered = Values;
            }
            else
            {
                foreach (string item in Values)
                {
                    if (item.ToLower().Contains(str.ToLower()))
                    {
                        results.Add(item);
                    }
                }
                Filtered = results;
            }
            Keeper.Output.DataSource = null;
            if (Filtered.Count > 0)
            {
                Keeper.Output.DataSource = Filtered;
                Keeper.Output.Visible = true;
            }else
            {
                Keeper.Output.Visible = false;
            }
        }

        public void Show()
        {
            Update();
            Keeper.Output.Location = new System.Drawing.Point(this.ThisControl.Left, this.ThisControl.Top + this.ThisControl.Height);
            Keeper.Output.Size = new System.Drawing.Size(this.ThisControl.Width, 50);
            Keeper.Output.Visible = true;
            Keeper.Output.BringToFront();

        }

        public void Hide()
        {
            Keeper.Output.Visible = false;
        }

        private void AddEvents()
        {
            ThisControl.Enter += (sender, e) =>
            {
                this.isActive = true;
            };
            ThisControl.LostFocus += (sender, e) =>
            {
                this.isActive = false;
            };
            ThisControl.KeyUp += (sender, e) =>
            {
                Update(ThisControl.Text);
                if (e.KeyCode == Keys.Enter || (e.KeyCode == Keys.Tab && ThisControl.Text.Length>0))
                {
                    if(this.Type == AutoCompleteType.replace)
                    {
                        ThisControl.Text = Keeper.Output.GetItemText(Keeper.Output.SelectedItem);
                    }else
                    {
                        string[] str = ThisControl.Text.Split(',');
                        if (str.Length > 0)
                        {
                            string nString = "";
                            for (int i = 0; i < str.Length-1; i++)
                            {
                                nString += str[i] + ", ";
                            }
                            ThisControl.Text = nString + Keeper.Output.GetItemText(Keeper.Output.SelectedItem);
                        }
                        else
                        {
                            ThisControl.Text =  Keeper.Output.GetItemText(Keeper.Output.SelectedItem);
                        }
                    }
                    
                    try
                    {
                        TextBox tb = (TextBox)ThisControl;
                        tb.SelectionStart = tb.Text.Length;
                        tb.SelectionLength = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    this.Hide();
                }
                else if (e.KeyCode == Keys.Down && this.isActive)
                {
                    for (int i = 0; i < Keeper.Output.Items.Count; i++)
                    {
                        if (Keeper.Output.Items[i] == Keeper.Output.SelectedItem)
                        {
                            if (i + 1 < Keeper.Output.Items.Count)
                            {
                                Keeper.Output.SetSelected(i + 1, true);
                                break;
                            }
                        }
                    }
                }
                else if (e.KeyCode == Keys.Up && this.isActive)
                {
                    for (int i = 0; i < Keeper.Output.Items.Count; i++)
                    {
                        if (Keeper.Output.Items[i] == Keeper.Output.SelectedItem)
                        {
                            if (i - 1 >= 0)
                            {
                                Keeper.Output.SetSelected(i - 1, true);
                                break;
                            }
                        }
                    }
                }else if(e.KeyCode==Keys.Escape && this.isActive)
                {
                    Hide();
                }else if (e.KeyCode == Keys.Oemcomma)
                {
                    Show();
                }
            };

            ParentForm.PreviewKeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Tab)
                {
                    e.IsInputKey = true;
                }
            };
        }


    }
    public enum AutoCompleteType
    {
        replace,
        append
    }
}
