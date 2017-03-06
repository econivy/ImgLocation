using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgLocation.Models
{
    public class Project
    {
        public int id { get; set; }
        public string TITLE { get; set; }
        public string PATH { get; set; }
        public string LASTDATAID { get; set; }
        public DateTime ADDDATE { get; set; }
    }
}
