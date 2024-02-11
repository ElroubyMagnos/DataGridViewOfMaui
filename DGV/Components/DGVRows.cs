using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElroubyMauiLibrary.Components {
    public class DGVRows : List<DGVRow>
    {
        public ElroubyDGV Parent { get; set; }
        public DGVRows() {

        }
        /// <summary>
        /// Add One Empty Row
        /// </summary>
        public void Add() 
        {
            if (Parent != null) 
            {
                Parent.Add();
            }
        }

        public void Remove(int RowIndex) 
        {
            if (Parent != null) 
            {
                foreach (VerticalStackLayout VSL2 in Parent.Main.OfType<VerticalStackLayout>()) {
                    VSL2.RemoveAt(RowIndex);
                }

                Clear();
                foreach (DGVRow Row in Parent.Rows) {
                    Add(Row);
                }

                Parent.UpdateAsync();
            }
        }
    }
}
