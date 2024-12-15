using Microsoft.Maui.Controls;

namespace Karatombola.Pages
{
    public partial class ModalPage : ContentPage
    {
        public ModalPage(Dictionary<int, string> playedSongList)
        {
            InitializeComponent();

            var songList = playedSongList.Select(ps => new PlayedSong
            {
                Id = ps.Key,
                Title = ps.Value
            }).ToList();

            PlayedSongsCollectionView.ItemsSource = songList;
        }

        private async void OnCloseModalClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }

    public class PlayedSong
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
