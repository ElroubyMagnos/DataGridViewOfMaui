using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElroubyMauiLibrary.Interfaces {
    public interface IDGV {
        /// <summary>
        /// Control Content
        /// </summary>
        public VerticalStackLayout Body { get; set; }
        public double BorderSize { get; set; }
    }
}
