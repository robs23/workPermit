using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace workPermit.Models
{
    public class WorkPermitCheck
    {
        public int WorkPermitCheckId { get; set; }
        public int WorkPermitId { get; set; }
        public int Page { get; set; }
        public int XPoint { get; set; }
        public int YPoint { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public PictureBox Picture { get; set; }
        public Point Location
        {
            get
            {
                return new Point(XPoint, YPoint);
            }
        }
    }
}
