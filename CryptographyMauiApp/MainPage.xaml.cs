namespace CryptographyMauiApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_PolybiusSquare_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PolybiusSquare());
        }
    }

}
