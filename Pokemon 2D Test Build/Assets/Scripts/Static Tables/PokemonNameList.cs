﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PokemonNameList
{
    //Generates names which is fucking amazing
    //https://www.dragonflycave.com/resources/pokemon-list-generator?format=%22%25%5Bname%5D%25%22%2C&linebreaks=1&gens=1&order=national

    public static string[] PokemonNameKanto1to151 =
    {
        "Bulbasaur",
        "Ivysaur",
        "Venusaur",
        "Charmander",
        "Charmeleon",
        "Charizard",
        "Squirtle",
        "Wartortle",
        "Blastoise",
        "Caterpie",
        "Metapod",
        "Butterfree",
        "Weedle",
        "Kakuna",
        "Beedrill",
        "Pidgey",
        "Pidgeotto",
        "Pidgeot",
        "Rattata",
        "Raticate",
        "Spearow",
        "Fearow",
        "Ekans",
        "Arbok",
        "Pikachu",
        "Raichu",
        "Sandshrew",
        "Sandslash",
        "Nidoran♀",
        "Nidorina",
        "Nidoqueen",
        "Nidoran♂",
        "Nidorino",
        "Nidoking",
        "Clefairy",
        "Clefable",
        "Vulpix",
        "Ninetales",
        "Jigglypuff",
        "Wigglytuff",
        "Zubat",
        "Golbat",
        "Oddish",
        "Gloom",
        "Vileplume",
        "Paras",
        "Parasect",
        "Venonat",
        "Venomoth",
        "Diglett",
        "Dugtrio",
        "Meowth",
        "Persian",
        "Psyduck",
        "Golduck",
        "Mankey",
        "Primeape",
        "Growlithe",
        "Arcanine",
        "Poliwag",
        "Poliwhirl",
        "Poliwrath",
        "Abra",
        "Kadabra",
        "Alakazam",
        "Machop",
        "Machoke",
        "Machamp",
        "Bellsprout",
        "Weepinbell",
        "Victreebel",
        "Tentacool",
        "Tentacruel",
        "Geodude",
        "Graveler",
        "Golem",
        "Ponyta",
        "Rapidash",
        "Slowpoke",
        "Slowbro",
        "Magnemite",
        "Magneton",
        "Farfetch’d",
        "Doduo",
        "Dodrio",
        "Seel",
        "Dewgong",
        "Grimer",
        "Muk",
        "Shellder",
        "Cloyster",
        "Gastly",
        "Haunter",
        "Gengar",
        "Onix",
        "Drowzee",
        "Hypno",
        "Krabby",
        "Kingler",
        "Voltorb",
        "Electrode",
        "Exeggcute",
        "Exeggutor",
        "Cubone",
        "Marowak",
        "Hitmonlee",
        "Hitmonchan",
        "Lickitung",
        "Koffing",
        "Weezing",
        "Rhyhorn",
        "Rhydon",
        "Chansey",
        "Tangela",
        "Kangaskhan",
        "Horsea",
        "Seadra",
        "Goldeen",
        "Seaking",
        "Staryu",
        "Starmie",
        "Mr. Mime",
        "Scyther",
        "Jynx",
        "Electabuzz",
        "Magmar",
        "Pinsir",
        "Tauros",
        "Magikarp",
        "Gyarados",
        "Lapras",
        "Ditto",
        "Eevee",
        "Vaporeon",
        "Jolteon",
        "Flareon",
        "Porygon",
        "Omanyte",
        "Omastar",
        "Kabuto",
        "Kabutops",
        "Aerodactyl",
        "Snorlax",
        "Articuno",
        "Zapdos",
        "Moltres",
        "Dratini",
        "Dragonair",
        "Dragonite",
        "Mewtwo",
        "Mew",
    };
    public static int[] PokemonKantoDifferentGenderSprites =
    {
        3,12,19,20,25,
        26,41,42,44,45,
        64,65,84,85,97,
        111,112,118,119,123,
        129,130
    };

    public static string[] PokemonNameJohto152to251 =
    {
        "Chikorita",
        "Bayleef",
        "Meganium",
        "Cyndaquil",
        "Quilava",
        "Typhlosion",
        "Totodile",
        "Croconaw",
        "Feraligatr",
        "Sentret",
        "Furret",
        "Hoothoot",
        "Noctowl",
        "Ledyba",
        "Ledian",
        "Spinarak",
        "Ariados",
        "Crobat",
        "Chinchou",
        "Lanturn",
        "Pichu",
        "Cleffa",
        "Igglybuff",
        "Togepi",
        "Togetic",
        "Natu",
        "Xatu",
        "Mareep",
        "Flaaffy",
        "Ampharos",
        "Bellossom",
        "Marill",
        "Azumarill",
        "Sudowoodo",
        "Politoed",
        "Hoppip",
        "Skiploom",
        "Jumpluff",
        "Aipom",
        "Sunkern",
        "Sunflora",
        "Yanma",
        "Wooper",
        "Quagsire",
        "Espeon",
        "Umbreon",
        "Murkrow",
        "Slowking",
        "Misdreavus",
        "Unown",
        "Wobbuffet",
        "Girafarig",
        "Pineco",
        "Forretress",
        "Dunsparce",
        "Gligar",
        "Steelix",
        "Snubbull",
        "Granbull",
        "Qwilfish",
        "Scizor",
        "Shuckle",
        "Heracross",
        "Sneasel",
        "Teddiursa",
        "Ursaring",
        "Slugma",
        "Magcargo",
        "Swinub",
        "Piloswine",
        "Corsola",
        "Remoraid",
        "Octillery",
        "Delibird",
        "Mantine",
        "Skarmory",
        "Houndour",
        "Houndoom",
        "Kingdra",
        "Phanpy",
        "Donphan",
        "Porygon2",
        "Stantler",
        "Smeargle",
        "Tyrogue",
        "Hitmontop",
        "Smoochum",
        "Elekid",
        "Magby",
        "Miltank",
        "Blissey",
        "Raikou",
        "Entei",
        "Suicune",
        "Larvitar",
        "Pupitar",
        "Tyranitar",
        "Lugia",
        "Ho-Oh",
        "Celebi",
    };
    public static int[] PokemonJohtoDifferentGenderSprites =
    {
        154,165,166,178,185,
        186,190,194,195,198,
        202,203,207,208,212,
        214,215,217,221,224,
        229,232
    };

    //public static string[] PokemonNameSpecialized1000Up =
    
    private enum PokemonNameSpecialized1000Up
    {
        Alakarate = 1065,
        Machost = 1068,
        Gololium = 1076,
        Haucker = 1094
    }
    static PokemonNameSpecialized1000Up specializedName;
    public static int[] PokemonSpecializedDifferentGenderSprites =
    {
        1065
    };
    /// <summary>
    /// Enter in the pokedex number to return the name
    /// </summary>
    /// <param name="pokedexNumber">ID #</param>
    public static string GetPokeDexName(int pokedexNumber)
    {
        pokedexNumber--;

        switch (pokedexNumber)
        {
            case int n when (n >= 0 && n < PokemonNameKanto1to151.Length):
                return PokemonNameKanto1to151[pokedexNumber];
            case int n when (n >= PokemonNameKanto1to151.Length && n < PokemonNameKanto1to151.Length + PokemonNameJohto152to251.Length):
                pokedexNumber -= PokemonNameKanto1to151.Length;
                return PokemonNameJohto152to251[pokedexNumber];
            case int n when System.Enum.TryParse<PokemonNameSpecialized1000Up>((n+1).ToString(),out specializedName):
                return specializedName.ToString();
            default:
                return "MissingNo";
        }
    }

    public static bool GenderExclusive(int pokedexNumber)
    {
        switch (pokedexNumber)
        {
            case int n when (n >= 0 && n < PokemonNameKanto1to151.Length):
                return PokemonKantoDifferentGenderSprites.Contains(pokedexNumber);
            case int n when (n >= PokemonNameKanto1to151.Length && n < PokemonNameKanto1to151.Length + PokemonNameJohto152to251.Length):
                return PokemonJohtoDifferentGenderSprites.Contains(pokedexNumber);
            case int n when (n >= 1000):
                return PokemonSpecializedDifferentGenderSprites.Contains(pokedexNumber);
            default:
                return false;
        }
    }
}