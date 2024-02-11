using ElroubyMauiLibrary.Components;
using ElroubyMauiLibrary.Interfaces;
using ElroubyMauiLibrary.Parts;
using Microsoft.Maui.Controls;
using System.Data;
using FastMember;
using System.Diagnostics;

namespace ElroubyMauiLibrary;

public partial class ElroubyDGV : ContentView, IBorderSize {
    public event EventHandler<EventArgs> SelectedRowChanged;
    Dictionary<string, string> ReplaceColumnName { get; set; } = new Dictionary<string, string>();
    List<string> ExcludeColumns { get; set; } = new List<string>();
    public void AddColumnNameToReplace(string First, string Second)
    {
        if (!ReplaceColumnName.Keys.Contains(First))
            ReplaceColumnName.Add(First, Second);
    }
    public void AddExcludedColumn(params string[] All)
    {
        ExcludeColumns.AddRange(All);
    }
    public Color DeleteButtonTextColor { get; set; } = Colors.Red;
    public Color AddButtonTextColor { get; set; } = Colors.Black;
    public Color AddButtonColor { get; set; } = Colors.BlueViolet;
    public Color CellColor { get; set; } = Colors.White;
    public Color CellTextColor { get; set; } = Colors.Black;
    /// <summary>
    /// Get current Row Cells in DGVRow
    /// </summary>
    public DGVRow SelectedRowCells 
    {
        get 
        {
            DGVRow Row = new DGVRow();

            for (int i = 0; i < Main.OfType<VerticalStackLayout>().Count(); i++) {
                if (Main.OfType<VerticalStackLayout>().ToList()[i].OfType<Border>().ToList()[SelectedRow + 1].Content.GetType() != typeof(DGVCellButton))
                    Row.Add(Main.OfType<VerticalStackLayout>().ToList()[i].OfType<Border>().ToList()[SelectedRow + 1].Content as DGVCell);
            }

            return Row;
        }
    }
    /// <summary>
    /// Get current Row Data in string
    /// </summary>
    public List<string> SelectedRowData
    {
        get
        {
            List<string> SRD = new List<string>();

            try
            {
                for (int i = 0; i < Main.OfType<VerticalStackLayout>().Count(); i++)
                {
                    if (Main.OfType<VerticalStackLayout>().ToList()[i].OfType<Border>().ToList()[SelectedRow + 1].Content.GetType() != typeof(DGVCellButton))
                        SRD.Add((Main.OfType<VerticalStackLayout>().ToList()[i].OfType<Border>().ToList()[SelectedRow + 1].Content as DGVCell).Text);
                }
            }
            catch { }

            return SRD;
        }
    }
    /// <summary>
    /// Show selected Row
    /// </summary>
    public bool ShowSelected { get; set; } = true;
    int _SelectedRow = 0;
    public int SelectedRow
    {
        get => _SelectedRow;
        set
        {
            if (_SelectedRow < (Main.Children[0] as VerticalStackLayout).Children.Count - 1)
            {
                for (int i = 0; i < Main.Children.Count; i++)
                {
                    if (Main.Children[i].GetType() == typeof(VerticalStackLayout))
                        ((Main.Children[i] as VerticalStackLayout)[_SelectedRow + 1] as Border).StrokeThickness = BorderSize;
                }
            }

            _SelectedRow = value;

            if (_SelectedRow < (Main.Children[0] as VerticalStackLayout).Children.Count - 1 && ShowSelected)
            {
                for (int i = 0; i < Main.Children.Count; i++)
                {
                    if (Main.Children[i].GetType() == typeof(VerticalStackLayout))
                        ((Main.Children[i] as VerticalStackLayout)[_SelectedRow + 1] as Border).StrokeThickness = BorderSize * 2;
                }
            }
        }
    }
    /// <summary>
    /// Edit all DataGridView Rows
    /// </summary>
    public DGVRows Rows 
    { 
        get 
        {
            DGVRows Rows = new DGVRows();
            Rows.Parent = this;
            List<VerticalStackLayout> VSL = Main.Children.OfType<VerticalStackLayout>().ToList();
            for (int i = 0; i < VSL.Count; i++) 
            {
                DGVRow Row = new DGVRow();
                Row.Parent = this;
                for (int i2 = 0; i2 < VSL[i].Count; i2++) 
                {
                    Row.Add(GetCell(i, i2));
                }
                if (!Row.Contains(null))
                    Rows.Add(Row);
            }

            return Rows;
        }
    }
    public int SelectedColumn { get; set; } = 0;
    /// <summary>
    /// Add the Add button with the Data Grid that will add new Row when click
    /// </summary>
    public bool Addedable { get; set; } = false;
    /// <summary>
    /// Add the Delete buttons for every Row when click on it the Row will delete
    /// </summary>
    public bool Deletable { get; set; } = false;
    public new LayoutOptions HorizontalOptions { get => Main.HorizontalOptions; set => Main.HorizontalOptions = value; }
    public new LayoutOptions VerticalOptions { get => Main.VerticalOptions; set => Main.VerticalOptions = value; }
    public HorizontalStackLayout Main = new HorizontalStackLayout() { HorizontalOptions = LayoutOptions.Center };
    public event EventHandler<TextChangedEventArgs> CellValueChanged;
    public event EventHandler<FocusEventArgs> CellFocused;
    public event EventHandler<FocusEventArgs> CellUnFocused;
    /// <summary>
    /// If the Cells is ReadOnly
    /// </summary>
    public bool IsReadOnly { get; set; } = true;
    /// <summary>
    /// Border size around the DataGridView
    /// </summary>
    public double BorderSize { get; set; } = 1;
    /// <summary>
    /// The upper Head of the DataGrid for each Column Size
    /// </summary>
    public int HeaderHeight { get; set; } = 50;
    /// <summary>
    /// Every Cell Width
    /// </summary>
    public int CellWidth { get; set; } = 100;
    /// <summary>
    /// Every Cell Height
    /// </summary>
    public int CellHeight { get; set; } = 35;
    /// <summary>
    /// Embed List as Data Source
    /// </summary>
    public void EmbedList<T>(List<T> List)
    {
        DataTable Data = new DataTable();
        Data.Load(ObjectReader.Create(List));

        DataSource = Data;
    }
    /// <summary>
    /// Embed Array as Data Source
    /// </summary>
    public void EmbedArray(object[] Array)
    {
        DataTable Data = new DataTable();
        Data.Load(ObjectReader.Create(Array));

        DataSource = Data;
    }

    DataTable DT = null;
    public DataTable DataSource
    {
        get => DT;
        set
        {
            Main.Clear();
            DT = value;
            if (DT != null)
            {
                for (int i = 0; i < DT.Columns.Count; i++)
                {
                    if (ExcludeColumns.Contains(DT.Columns[i].ColumnName))
                        continue;

                    DGVCell E = new DGVCell(BorderSize);
                    E.CurrentColumn = E.CurrentRow = -1;
                    E.IsReadOnly = true;
                    if (ReplaceColumnName.Keys.Contains(DT.Columns[i].ColumnName))
                    {
                        E.Text = ReplaceColumnName[DT.Columns[i].ColumnName];
                    }
                    else E.Text = DT.Columns[i].ColumnName;

                    E.CurrentColumnName = DT.Columns[i].ColumnName;

                    E.WidthRequest = CellWidth;
                    E.HeightRequest = HeaderHeight;
                    E.Focused += (s, e) => UpdateAsync();
                    E.BackgroundColor = CellColor;
                    E.TextColor = CellTextColor;

                    VerticalStackLayout VSL = new VerticalStackLayout
                    {
                        E.CellBorder
                    };

                    Main.Add(VSL);

                    for (int id = 0; id < DT.Rows.Count; id++)
                    {
                        DGVCell EX = new DGVCell(BorderSize);
                        EX.IsReadOnly = IsReadOnly;

                        EX.CurrentRow = id;
                        EX.CurrentColumn = i;
                        EX.CurrentColumnName = DT.Columns[i].ColumnName;
                        EX.Focused += (s, e) =>
                        {
                            UpdateAsync();
                            SelectedRow = EX.CurrentRow;
                            SelectedColumn = EX.CurrentColumn;
                        };

                        EX.WidthRequest = CellWidth;
                        EX.HeightRequest = CellHeight;
                        EX.Text = DT.Rows[id].ItemArray[i].ToString();

                        EX.TextChanged += CellValueChanged;
                        EX.Focused += CellFocused;
                        EX.Unfocused += CellUnFocused;

                        EX.BackgroundColor = CellColor;
                        EX.TextColor = CellTextColor;
                        VSL.Add(EX.CellBorder);
                    }
                }

                if (Deletable)
                {
                    VerticalStackLayout VSL = new VerticalStackLayout();
                    for (int i = 0; i < (Main.Children[0] as VerticalStackLayout).Children.Count; i++)
                    {
                        if (i == 0)
                        {
                            DGVCell DC = new DGVCell(BorderSize);
                            DC.IsReadOnly = true;
                            DC.Focused += (s, e) => UpdateAsync();

                            VSL.Add(DC.CellBorder);
                            DC.WidthRequest = CellWidth;
                            DC.HeightRequest = HeaderHeight;

                            DC.BackgroundColor = CellColor;
                            DC.TextColor = CellTextColor;
                            i++;
                            goto Again;
                        }
                    Again:
                        DGVCellButton E = new DGVCellButton();
                        E.Text = "Delete";
                        E.Number = i;
                        E.TextColor = Color.FromRgb(255, 0, 0);
                        E.WidthRequest = CellWidth;
                        E.HeightRequest = CellHeight;
                        E.BackgroundColor = CellColor;
                        E.TextColor = DeleteButtonTextColor;

                        E.Clicked += (s, e) =>
                        {
                            EClicked(E.Number, VSL);
                        };
                        Border border = new Border
                        {
                            Stroke = Color.FromArgb("#000000"),
                            StrokeThickness = BorderSize,
                            HorizontalOptions = LayoutOptions.Center,
                        };
                        border.Content = E;
                        VSL.Add(border);
                    }

                    Main.Add(VSL);
                }

                if (Addedable)
                {
                    IsReadOnly = false;
                    Button Adder = new Button();
                    Adder.WidthRequest = 75;
                    Adder.HeightRequest = 50;
                    Adder.Text = "Add";
                    Adder.BackgroundColor = AddButtonColor;
                    Adder.TextColor = AddButtonTextColor;

                    Adder.Clicked += (s, e) =>
                    {
                        Add();
                    };

                    Main.Add(Adder);
                }
            }
        }
    }

    /// <summary>
    /// Get Cell value by row and column Number
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public string GetValue(int row, int column)
    {
        foreach (VerticalStackLayout VSL in Main.Children.OfType<VerticalStackLayout>())
        {
            foreach (Border B in VSL.OfType<Border>())
            {
                if (B.Content.GetType() != typeof(DGVCellButton))
                {
                    DGVCell Cell = B.Content as DGVCell;
                    if (Cell.CurrentRow == row && Cell.CurrentColumn == column)
                        return Cell.Text;
                }
            }
        }

        return null;
    }
    /// <summary>
    /// Edit Cell by row and column Number
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="Data"></param>
    public void SetValue(int row, int column, string Data)
    {
        foreach (VerticalStackLayout VSL in Main.Children.OfType<VerticalStackLayout>())
        {
            foreach (Border B in VSL.OfType<Border>())
            {
                if (B.Content.GetType() != typeof(DGVCellButton))
                {
                    DGVCell Cell = B.Content as DGVCell;
                    if (Cell.CurrentRow == row && Cell.CurrentColumn == column)
                        Cell.Text = Data;
                }
            }
        }
    }
    /// <summary>
    /// Get DGV Cell Control by row and column Number
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public DGVCell GetCell(int row, int column) 
    {
        foreach (VerticalStackLayout VSL in Main.Children.OfType<VerticalStackLayout>()) {
            foreach (Border B in VSL.OfType<Border>()) {
                if (B.Content.GetType() != typeof(DGVCellButton)) {
                    DGVCell Cell = B.Content as DGVCell;
                    if (Cell.CurrentRow == row && Cell.CurrentColumn == column)
                        return Cell;
                }
            }
        }

        return null;
    }

    public ElroubyDGV()
    {
        Content = Main;
    }

    void EClicked(int Number, VerticalStackLayout VSL)
    {
        List<Border> Buttons = VSL.OfType<Border>().ToList();
        for (int i = 1; i < Buttons.Count(); i++)
        {
            (Buttons[i].Content as DGVCellButton).Number = i;
        }

        foreach (VerticalStackLayout VSL2 in Main.OfType<VerticalStackLayout>())
        {
            VSL2.RemoveAt(Number);
        }
    }
    /// <summary>
    /// Update Cells Numbers
    /// </summary>
    public void UpdateAsync()
    {
        List<VerticalStackLayout> TheVSL = Main.OfType<VerticalStackLayout>().ToList();

        for (int i = 1; i < TheVSL.Count; i++)
        {
            List<Border> TheBorders = TheVSL[i].OfType<Border>().ToList();
            for (int i2 = 1; i2 < TheBorders.Count; i2++)
            {
                if (TheBorders[i2].Content.GetType() != typeof(DGVCellButton))
                {
                    (TheBorders[i2].Content as DGVCell).CurrentRow = i2 - 1;
                    (TheBorders[i2].Content as DGVCell).CurrentColumn = i - 1;
                }
            }
        }
    }
    /// <summary>
    /// Add empty Row
    /// </summary>
    public void Add()
    {
        int i = 0;
        List<VerticalStackLayout> VSLList = Main.OfType<VerticalStackLayout>().ToList();
        int TheRow = VSLList[0].OfType<Border>().Count() - 1;
        foreach (VerticalStackLayout VSL in Main.OfType<VerticalStackLayout>())
        {
            if (i == VSLList.Count - 1 && Deletable)
            {
                DGVCellButton E = new DGVCellButton();
                E.Text = "Delete";
                E.Number = i;
                E.TextColor = Color.FromRgb(255, 0, 0);
                E.WidthRequest = CellWidth;
                E.HeightRequest = CellHeight;
                E.BackgroundColor = CellColor;
                E.TextColor = DeleteButtonTextColor;
                E.Clicked += (s, e) =>
                {
                    EClicked(E.Number, VSL);
                };
                Border border = new Border
                {
                    Stroke = Color.FromArgb("#000000"),
                    StrokeThickness = BorderSize,
                    HorizontalOptions = LayoutOptions.Center,
                };
                border.Content = E;
                VSL.Add(border);
            }
            else
            {
                DGVCell DC = new DGVCell(BorderSize);
                DC.IsReadOnly = false;
                DC.WidthRequest = CellWidth;
                DC.HeightRequest = CellHeight;
                DC.BackgroundColor = CellColor;
                DC.TextColor = CellTextColor;
                DC.CurrentRow = TheRow;
                DC.CurrentColumn = i;

                DC.Focused += (s, e) =>
                {
                    UpdateAsync();
                    SelectedRow = DC.CurrentRow;
                    SelectedColumn = DC.CurrentColumn;
                };

                VSL.Add(DC.CellBorder);
                i++;
            }
        }
    }
}