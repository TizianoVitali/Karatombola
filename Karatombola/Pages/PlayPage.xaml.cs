namespace Karatombola.Pages;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Plugin.Maui.Audio;

public partial class PlayPage : ContentPage
{
    public Dictionary<int, string> playedSongList = new Dictionary<int, string>();
    int actualDraw = 0;
    string kFileName = string.Empty, kTypeName = string.Empty;
    int counter = 1;
    Song song = null;
    public double Duration => player?.Duration ?? 1;
    private bool isUpdatingSlider = false;
    IAudioPlayer player;

    public PlayPage(string kType)
    {
        InitializeComponent();

        btn_start.IsVisible = true;

        switch (kType)
        {
            case "btn_k80":
                kFileName = "k_80.csv";
                kTypeName = "'80"; break;
            case "btn_k90":
                kFileName = "k_90.csv";
                kTypeName = "'90"; break;
            case "btn_k2000":
                kFileName = "k_2000.csv";
                kTypeName = "2000"; break;

            case "btn_kdisney":
                kFileName = "k_disney.csv";
                kTypeName = "Disney";
                btn_start.FontFamily = "Disney";
                lbl_counter.FontFamily = "Disney";
                lbl_type.FontFamily = "Disney";
                break;

        }
        lbl_type.Text = kTypeName;
        SongUtil.ReadSongList(kFileName);
    }


    private async void OnStartClick(object sender, EventArgs e)
    {
        btn_start.IsVisible = false;
        lbl_counter.IsVisible = true;

        string songPath = $"Raw/songs/countdown.mp3";
        var path = await FileSystem.Current.OpenAppPackageFileAsync(songPath);
        player = AudioManager.Current.CreatePlayer(path);
        player.Play();

        lbl_counter.Text = "3";
        await Task.Delay(1000);
        lbl_counter.Text = "2";
        await Task.Delay(1000);
        lbl_counter.Text = "1";
        await Task.Delay(1000);
        lbl_counter.IsVisible = false;
        grid_play.IsVisible = true;

        // 1à estrazione
        drawSong();
    }

    private async void OnOpenModalClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ModalPage(playedSongList));
    }

    private async void OnPauseClick(object sender, EventArgs e)
    {
        if (player.IsPlaying)
        {
            player.Pause();
            isUpdatingSlider = false;
            btn_pause.IsVisible = false;
            btn_play.IsVisible = true;
        }
    }

    private async void OnPlayClick(object sender, EventArgs e)
    {
        if (!player.IsPlaying)
        {
            player.Play();
            btn_pause.IsVisible = true;
            btn_play.IsVisible = false;
            await UpdateSliderAsync();
        }
    }

    private async void OnNextClick(object sender, EventArgs e)
    {
        btn_pause.IsVisible = true;
        btn_play.IsVisible = false;
        player.Stop();
        drawSong();
        isUpdatingSlider = true;
    }

    private async void OnLinkClick(object sender, EventArgs e)
    {
        player.Pause();
        btn_pause.IsVisible = false;
        btn_play.IsVisible = true;
        await Launcher.OpenAsync(new Uri(song.YTLink));
    }

    private async void OnWinClick(object sender, EventArgs e)
    {
        string prizeType = "";
        int nType = 0;
        var button = (ImageButton)sender;

        player.Volume = 0.3;

        switch (button.StyleId)
        {
            case "btn_ambo": prizeType = "Ambo"; nType = 1; break;
            case "btn_terno": prizeType = "Terno"; nType = 2; break;
            case "btn_quaterna": prizeType = "Quaterna"; nType = 3; break;
            case "btn_cinquina": prizeType = "Cinquina"; nType = 4; break;
            case "btn_tombola": prizeType = "Tombola"; nType = 5; break;
        }

        bool answer = false;

        if (button.StyleId.Equals("btn_tombola"))
            answer = await DisplayAlert($"Qualcuno ha fatto {prizeType}?", "Premendo \"Si\" terminerai la partita!", "Si", "No");
        else
            answer = await DisplayAlert($"Qualcuno ha fatto {prizeType}?", "Se c'è stata una vincita premi Si", "Si", "No");

        if (answer)
        {
            switch (nType)
            {
                case 1:
                    btn_ambo.IsVisible = false;
                    btn_terno.IsVisible = true;
                    player.Volume = 1;
                    break;
                case 2:
                    btn_terno.IsVisible = false;
                    btn_quaterna.IsVisible = true;
                    player.Volume = 1;
                    break;
                case 3:
                    btn_quaterna.IsVisible = false;
                    btn_cinquina.IsVisible = true;
                    player.Volume = 1;
                    break;
                case 4:
                    btn_cinquina.IsVisible = false;
                    btn_tombola.IsVisible = true;
                    player.Volume = 1;
                    break;
                case 5:
                    btn_tombola.IsVisible = false;
                    await Navigation.PopAsync();
                    break;
            }
        }
        else
            player.Volume = 1;
    }

    private async void OnBackClick(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Uscire dalla partita?", "Se esci perderai i progressi!", "Si", "No");

        if (answer)
        {
            if (player == null)
            {
                await Navigation.PopAsync();
            }
            else
            {
                player.Stop();
                player.Dispose();
                await Navigation.PopAsync();
            }
        }
    }

    private int drawNumber()
    {
        Random rnd = new Random();
        int rndNumber;

        do
        {
            rndNumber = rnd.Next(1, 91);
        } while (playedSongList.ContainsKey(rndNumber));

        return rndNumber;
    }

    private async void drawSong()
    {
        if (counter <= 90)
        {
            actualDraw = drawNumber();
            song = SongUtil.songDictionary[actualDraw];
            lbl_title.Text = song.Title;

            lbl_artist.Text = song.Artist.Replace(",", ", ");
            lbl_drawCounter.Text = "Estrazione n. " + counter++.ToString();
            playedSongList[actualDraw] = song.Title;

            string songPath = $"Raw/songs/{kFileName.Replace(".csv", "")}/{song.FileName}";
            var path = await FileSystem.Current.OpenAppPackageFileAsync(songPath);

            player = AudioManager.Current.CreatePlayer(path);

            song_slider.Value = player.CurrentPosition;
            song_slider.Maximum = Duration;
            lbl_songDuration.Text = SecondsToStringConverter.Convert(player.Duration).ToString();

            player.Play();
            await UpdateSliderAsync();
        }
        else
        {
            bool answer = await DisplayAlert("Estrazioni terminate!", "Sono state estratte tutte e 90 le canzoni, gioco finito.", "Daje", "Torna alla pagina principale");
            if (!answer)
            {
                player.Dispose();
                await Navigation.PopAsync();
            }
        }
    }

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (Math.Abs(e.NewValue - player.CurrentPosition) > 0.5)
        {
            player.Seek(e.NewValue);
            lbl_songCurrentPosition.Text = SecondsToStringConverter.Convert(e.NewValue).ToString();
        }
    }

    private async Task UpdateSliderAsync()
    {
        isUpdatingSlider = true;
        while (player.IsPlaying && isUpdatingSlider)
        {
            song_slider.Value = player.CurrentPosition;
            lbl_songCurrentPosition.Text = SecondsToStringConverter.Convert(player.CurrentPosition).ToString();

            if (player.CurrentPosition >= player.Duration)
            {
                isUpdatingSlider = false;
                break;
            }

            await Task.Delay(500); // Aggiorna ogni 500ms
        }
    }


}