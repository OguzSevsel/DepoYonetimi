using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balya_Yerleştirme.Models
{
    public partial class Layout
    {
        public Layout() { }

        public Layout(string name, string description, int lastclosed) 
        { 
            Name = name;
            Description = description;
            LastClosedLayout = lastclosed;
        }

        public int LayoutId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LastClosedLayout { get; set; }
    }
}
