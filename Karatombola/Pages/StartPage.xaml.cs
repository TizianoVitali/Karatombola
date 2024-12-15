namespace Karatombola.Pages;

public partial class StartPage : ContentPage
{
	public StartPage()
	{
		InitializeComponent();
	}

	private async void OnBackClick(object sender, EventArgs e)
	{
		//await Shell.Current.GoToAsync("///MainPage");
		await Navigation.PopAsync();
	}

	private async void OnChoseClick(object sender, EventArgs e)
	{
		var button = (Button)sender;
		await Navigation.PushAsync(new PlayPage(button.StyleId));
	}

	private async void OnSongListClick(object sender, EventArgs e)
	{
		var button = (Button)sender;
		await Navigation.PushAsync(new SongListPage(button.StyleId));
	}
}
