namespace Karatombola.Pages;

public partial class RulesPage : ContentPage
{
    public RulesPage()
    {
        InitializeComponent();
    }

    private async void OnBackClick(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
