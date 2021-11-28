# AverageWordCountCalculator 

## Key Features

This console app, when given the name of an artist, will produce the average
(mean) number of words in their songs.

## Build and run

To build and run the app, change to the AireLogic/AvgWordCountCalculator directory and execute the following command:

`dotnet run`

Here you will be asked to enter an artist's name, the app will find any songs available and return an average word count across the songs it found.
The app will notify the the user if lyrics for a particular song were not found or if there were no results found.
The app will then give the user a choice to searhc again or exit the app.

To run the tests, change to the AireLogic/AvgWordCountCalculator directory and execute the following command:

`dotnet build AireLogic.sln`

`dotnet test AireLogic.sln`



