using CsvHelper.Configuration.Attributes;

public class Song
{
    [Name("Title")]
    public string Title { get; set; }

    [Name("Artist")]
    public string Artist { get; set; }

    [Name("FileName")]
    public string FileName { get; set; }

    public string YTLink { get { return "https://www.youtube.com/results?search_query=" + this.Title.Replace(' ', '+') + "+karaoke"; } }
}