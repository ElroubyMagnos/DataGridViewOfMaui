using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElroubyMauiLibrary.Interfaces;

namespace ElroubyMauiLibrary.Parts
{
    public class DGVCellButton : Button, IDGVCell {
        public bool IsReadOnly { get; set; } = false;
        public int Number { get; set; } = 0;
        public DGVCellButton() : base()
        {

        }
    }
}
