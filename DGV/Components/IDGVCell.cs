using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElroubyMauiLibrary.Interfaces
{
    public interface IDGVCell
    {
        public string Text { get; set; }
        public Color TextColor { get; set; }
        public Color BackgroundColor { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
