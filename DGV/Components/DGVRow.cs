using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElroubyMauiLibrary.Parts;

namespace ElroubyMauiLibrary.Components
{
    public class DGVRow : List<DGVCell>
    {
        public ElroubyDGV Parent = null;
        public DGVRow() 
        { 

        }
    }
}
