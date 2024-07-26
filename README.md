# FFXIV Marketable Item List

A project the compiles a [CSV](output.csv) consisting of ID and the names in English, French, German and Japanese.

## How it works

The list is a compilation of two public API's:

-  [Universalis](https://docs.universalis.app/) for the list of item ids that can be listed on the market board.
-  [XIVApi](https://xivapi.com/) for the names associated with the ids.

This is a vanilla .Net 8 project with no external dependencies.

## Extension

Extension of the list is quite easy provided you stick to the JSON format returned from the XIVApi.
You should only need to modify the [item](Scraper/Item.cs) class to also include the fields you wish to include.
The overriden ToString method is then used when writing to the file.
