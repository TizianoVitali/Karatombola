namespace Karatombola.Pages;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnStartClick(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new StartPage());
	}

	private async void OnRulesClick(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new RulesPage());
	}
}

