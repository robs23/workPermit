using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace workPermit
{
    public class AutoCompleteKeeper
    {
        public ListBox Output;
        public List<AutoComplete> AutoCompletes = new List<AutoComplete>();
        public AutoComplete Current;

        public AutoCompleteKeeper()
        {
            Output = new ListBox();
            Output.Name = "lstAutoComplete";
            Output.Click += (sender, e) =>
            {
                Current.ThisControl.Text = Output.GetItemText(Output.SelectedItem);
            };
        }
        public void Append(AutoComplete ac)
        {
            AutoCompletes.Add(ac);
        }

        public void Remove(AutoComplete ac)
        {
            foreach(AutoComplete AC in AutoCompletes)
            {
                if (AC.ThisControl.Name == ac.ThisControl.Name)
                {
                    AutoCompletes.Remove(ac);
                }
            }

        }

        public void Hide()
        {
            foreach(AutoComplete ac in AutoCompletes)
            {
                if (ac.isActive)
                {
                    ac.isActive = false;
                }
            }
        }
    }
}
