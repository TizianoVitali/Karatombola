using System.Globalization;
using CsvHelper;

namespace Karatombola
{
    public static class SongUtil
    {
        public static Dictionary<int, Song> songDictionary { get; set; }

        public async static void ReadSongList(string fileName)
        {
            List<Song> songList = new List<Song>();
            int i = 1;

            try
            {
                using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync("Raw/csv/" + fileName);
                using StreamReader reader = new StreamReader(fileStream);

                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                try
                {
                    songList = csv.GetRecords<Song>().ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Errore durante la lettura del file: {ex.Message}");
                }

                songDictionary = new Dictionary<int, Song>(90);

                foreach (Song song in songList)
                {
                    songDictionary[i] = song;
                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante la lettura del file: {ex.Message}");
            }
        }



    }
}