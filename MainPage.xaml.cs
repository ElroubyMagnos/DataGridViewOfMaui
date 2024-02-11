namespace ElroubyOldDGV
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void DGV_Loaded(object sender, EventArgs e)
        {
            DGV.EmbedList(new List<TestGrid>
            {
                new TestGrid
                {
                    ID = 0,
                    Address = "TestOne",
                    Name = "Elrouby",
                    Phone = 01208033237//Call me in Egypt
                },
                new TestGrid
                {
                    ID = 0,
                    Address = "TestOne",
                    Name = "Elrouby",
                    Phone = 01208033237//Call me in Egypt
                },
                new TestGrid
                {
                    ID = 0,
                    Address = "TestOne",
                    Name = "Elrouby",
                    Phone = 01208033237//Call me in Egypt
                }
            });
        }
    }
}
