using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElroubyMauiLibrary.Components;
using ElroubyMauiLibrary.Interfaces;

namespace ElroubyMauiLibrary.Parts
{
    public class DGVCell : Entry, IDGVCell {
        /// <summary>
        /// Border around the Cell
        /// </summary>
        public Border CellBorder;
        public int CurrentRow { get; set; } = 0;
        public int CurrentColumn { get; set; } = 0;
        public string CurrentColumnName { get; set; }
        public DGVCell(double BorderSize) : base()
        {
            IsReadOnly = true;
            TextColor = Color.FromRgb(0, 0, 0);
            WidthRequest = HeightRequest = 100;

            CellBorder = new Border
            {
                Stroke = Color.FromArgb("#000000"),
                StrokeThickness = BorderSize,
                HorizontalOptions = LayoutOptions.Center,
            };

            CellBorder.Content = this;
        }
    }
}
