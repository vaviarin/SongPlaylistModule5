using System;
using System.Collections.Generic;
using System.Linq;

// A record compares songs by their values.
public record Song
{
    public string Title { get; init; }
    public string Artist { get; init; }
    public int DurationSeconds { get; init; }

    public Song(string title, string artist, int durationSeconds)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(artist);
        ArgumentOutOfRangeException.ThrowIfNegative(durationSeconds);

        Title = title;
        Artist = artist;
        DurationSeconds = durationSeconds;
    }
}

public class Playlist
{
    // The list is private, so it can be changed only through this class.
    private readonly List<Song> _songs = new();

    public int Count => _songs.Count;

    // Calculate the total duration from the songs in the list.
    public int TotalDuration => _songs.Sum(song => song.DurationSeconds);

    // Get a song by its position.
    public Song this[int index]
    {
        get
        {
            if (index < 0 || index >= _songs.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return _songs[index];
        }
    }

    // Add an existing Song.
    public void Add(Song song)
    {
        ArgumentNullException.ThrowIfNull(song);
        ArgumentException.ThrowIfNullOrWhiteSpace(song.Title);
        ArgumentException.ThrowIfNullOrWhiteSpace(song.Artist);
        ArgumentOutOfRangeException.ThrowIfNegative(song.DurationSeconds);

        _songs.Add(song);
    }

    // Overload: create a Song from its values and add it.
    public void Add(string title, string artist, int durationSeconds)
    {
        Add(new Song(title, artist, durationSeconds));
    }
}

public static class Program
{
    // Main calls the methods that run the examples.
    public static void Main()
    {
        ShowSongs();

        Playlist list = MakeList();
        ShowList(list);
    }

    private static void ShowSongs()
    {
        Song song1 = new Song("Northern Lights", "Mira Sol", 214);
        Song song2 = new Song("Northern Lights", "Mira Sol", 214);

        Console.WriteLine("SONG RECORDS");
        Console.WriteLine(
            $"Two songs with the same values are equal: {song1 == song2}");

        // Make a copy with a different title.
        Song copy = song1 with { Title = "Northern Lights (Acoustic)" };

        Console.WriteLine($"Copy with a changed title: {copy.Title}");
        Console.WriteLine($"Copy equals the original: {song1 == copy}");
    }

    private static Playlist MakeList()
    {
        Playlist list = new Playlist();

        // Use both Add methods.
        list.Add("Northern Lights", "Mira Sol", 214);
        list.Add(new Song("Paper Planes", "The Daydreamers", 197));
        list.Add("Golden Hour", "June Harbor", 246);

        return list;
    }

    private static void ShowList(Playlist list)
    {
        ArgumentNullException.ThrowIfNull(list);

        Console.WriteLine("\nPLAYLIST");
        Console.WriteLine($"Song count: {list.Count}");

        for (int i = 0; i < list.Count; i++)
        {
            Song song = list[i];

            Console.WriteLine(
                $"{i + 1}. {song.Title} by {song.Artist} " +
                $"({GetTime(song.DurationSeconds)})");
        }

        Console.WriteLine($"Total duration: {GetTime(list.TotalDuration)}");
    }

    // Overload for a duration stored as seconds.
    private static string GetTime(int seconds)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(seconds);
        return GetTime(TimeSpan.FromSeconds(seconds));
    }

    // Overload for a TimeSpan.
    private static string GetTime(TimeSpan time)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(time.Ticks);
        return $"{(int)time.TotalMinutes}:{time.Seconds:D2}";
    }
}