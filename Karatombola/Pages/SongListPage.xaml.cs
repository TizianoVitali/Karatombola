namespace Karatombola.Pages;

public partial class SongListPage : ContentPage
{
    public SongListPage(string kType)
    {
        InitializeComponent();

        string kFileName = string.Empty, kTypeName = string.Empty;
        switch (kType)
        {
            case "btn_listk80":
                kFileName = "k_80.csv";
                kTypeName = "'80"; break;
            case "btn_listk90":
                kFileName = "k_90.csv";
                kTypeName = "'90"; break;
            case "btn_listk2000":
                kFileName = "k_2000.csv";
                kTypeName = "2000"; break;
            case "btn_listkdisney":
                kFileName = "k_disney.csv";
                kTypeName = "Disney"; break;
        }
        lbl_type.Text = kTypeName;
        SongUtil.ReadSongList(kFileName);
        FillTable(SongUtil.songDictionary);
    }

    private async void OnBackClick(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OpenUrl(string url)
    {
        try
        {
            await Browser.Default.OpenAsync(new Uri(url), BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }

    }

    private void FillTable(Dictionary<int, Song> songDictionary)
    {
        SongTable.ColumnDefinitions.Add(new ColumnDefinition { Width = 80 });
        SongTable.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        SongTable.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        //SongTable.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        for (int i = 1; i <= 90; i++)
        {
            SongTable.Add(new Label { Text = i.ToString() + " -", FontSize = 35 }, 0, i);
            SongTable.Add(new Label { Text = songDictionary[i].Title, FontSize = 35 }, 1, i);
            SongTable.Add(new Label { Text = songDictionary[i].Artist, FontSize = 35 }, 2, i);

            // var linkButton = new Button
            // {
            //     Text = "Link YouTube",
            //     Command = new Command(() => OpenUrl(songDictionary[i].YTLink))
            // };
            // linkButton.FontSize = 30;
            // SongTable.Add(linkButton, 3, i);
        }
    }
}