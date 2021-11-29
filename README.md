# AverageWordCountCalculator 

## Key Features

This console app, when given the name of an artist, will produce the average
(mean) number of words in their songs.

## APIs used:
1. [MusicBrainz](https://musicbrainz.org/doc/MusicBrainz_API): Used for acquiring artist song titles based on the artist name provided by user.
2. [Lyrics.ovh](https://lyricsovh.docs.apiary.io/#reference): Used for acquiring song lyrics based on song titles acquired from MusicBrainz with given artist name.

## Build and run

To build and run the app, change to the /AvgWordCountCalculator directory and execute the following command:

`dotnet run`

Here you will be asked to enter an artist's name, the app will find any songs available and return an average word count across the songs it found.
The app will notify the the user if lyrics for a particular song were not found or if there were no results found.
The app will then give the user a choice to searhc again or exit the app.

To run the tests, change to the AvgWordCountCalculator directory and execute the following command:

`dotnet build AvgWordCountCalculator.sln`

`dotnet test AvgWordCountCalculator.sln`

## Limitations

- Lyrics are not always available for all songs
- Artist names may vary & lyrics may be linked to different entity 
- Language/format of artst or song title may effect search results
- Due to above limitations, averages may sometimes be inconsistent or innacurate  

## Potential improvements

- Linking data where artist may be stored under multiple names.  
- Implementing other APIs & resources for optimal results.
- Caching previous search results to improve performance. 

