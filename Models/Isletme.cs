using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balya_Yerleştirme.Models
{
    public partial class Isletme
    {
        public Isletme() { }

        public Isletme(string name, string description, int lastclosed)
        {
            Name = name;
            Description = description;
            LastClosedIsletme = lastclosed;
        }

        public int IsletmeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LastClosedIsletme { get; set; }
    }
}
