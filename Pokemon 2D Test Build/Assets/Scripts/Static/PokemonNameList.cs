﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PokemonNameList
{
    //Generates names which is fucking amazing
    //https://www.dragonflycave.com/resources/pokemon-list-generator?format=%22%25%5Bname%5D%25%22%2C&linebreaks=1&gens=1&order=national

    public static string[] PokemonName =
    {
        //Kanto
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

        //Johto
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

        //Hoenn
        "Treecko",
        "Grovyle",
        "Sceptile",
        "Torchic",
        "Combusken",
        "Blaziken",
        "Mudkip",
        "Marshtomp",
        "Swampert",
        "Poochyena",
        "Mightyena",
        "Zigzagoon",
        "Linoone",
        "Wurmple",
        "Silcoon",
        "Beautifly",
        "Cascoon",
        "Dustox",
        "Lotad",
        "Lombre",
        "Ludicolo",
        "Seedot",
        "Nuzleaf",
        "Shiftry",
        "Taillow",
        "Swellow",
        "Wingull",
        "Pelipper",
        "Ralts",
        "Kirlia",
        "Gardevoir",
        "Surskit",
        "Masquerain",
        "Shroomish",
        "Breloom",
        "Slakoth",
        "Vigoroth",
        "Slaking",
        "Nincada",
        "Ninjask",
        "Shedinja",
        "Whismur",
        "Loudred",
        "Exploud",
        "Makuhita",
        "Hariyama",
        "Azurill",
        "Nosepass",
        "Skitty",
        "Delcatty",
        "Sableye",
        "Mawile",
        "Aron",
        "Lairon",
        "Aggron",
        "Meditite",
        "Medicham",
        "Electrike",
        "Manectric",
        "Plusle",
        "Minun",
        "Volbeat",
        "Illumise",
        "Roselia",
        "Gulpin",
        "Swalot",
        "Carvanha",
        "Sharpedo",
        "Wailmer",
        "Wailord",
        "Numel",
        "Camerupt",
        "Torkoal",
        "Spoink",
        "Grumpig",
        "Spinda",
        "Trapinch",
        "Vibrava",
        "Flygon",
        "Cacnea",
        "Cacturne",
        "Swablu",
        "Altaria",
        "Zangoose",
        "Seviper",
        "Lunatone",
        "Solrock",
        "Barboach",
        "Whiscash",
        "Corphish",
        "Crawdaunt",
        "Baltoy",
        "Claydol",
        "Lileep",
        "Cradily",
        "Anorith",
        "Armaldo",
        "Feebas",
        "Milotic",
        "Castform",
        "Kecleon",
        "Shuppet",
        "Banette",
        "Duskull",
        "Dusclops",
        "Tropius",
        "Chimecho",
        "Absol",
        "Wynaut",
        "Snorunt",
        "Glalie",
        "Spheal",
        "Sealeo",
        "Walrein",
        "Clamperl",
        "Huntail",
        "Gorebyss",
        "Relicanth",
        "Luvdisc",
        "Bagon",
        "Shelgon",
        "Salamence",
        "Beldum",
        "Metang",
        "Metagross",
        "Regirock",
        "Regice",
        "Registeel",
        "Latias",
        "Latios",
        "Kyogre",
        "Groudon",
        "Rayquaza",
        "Jirachi",
        "Deoxys",

        //Sinnoh
        "Turtwig",
        "Grotle",
        "Torterra",
        "Chimchar",
        "Monferno",
        "Infernape",
        "Piplup",
        "Prinplup",
        "Empoleon",
        "Starly",
        "Staravia",
        "Staraptor",
        "Bidoof",
        "Bibarel",
        "Kricketot",
        "Kricketune",
        "Shinx",
        "Luxio",
        "Luxray",
        "Budew",
        "Roserade",
        "Cranidos",
        "Rampardos",
        "Shieldon",
        "Bastiodon",
        "Burmy",
        "Wormadam",
        "Mothim",
        "Combee",
        "Vespiquen",
        "Pachirisu",
        "Buizel",
        "Floatzel",
        "Cherubi",
        "Cherrim",
        "Shellos",
        "Gastrodon",
        "Ambipom",
        "Drifloon",
        "Drifblim",
        "Buneary",
        "Lopunny",
        "Mismagius",
        "Honchkrow",
        "Glameow",
        "Purugly",
        "Chingling",
        "Stunky",
        "Skuntank",
        "Bronzor",
        "Bronzong",
        "Bonsly",
        "Mime Jr.",
        "Happiny",
        "Chatot",
        "Spiritomb",
        "Gible",
        "Gabite",
        "Garchomp",
        "Munchlax",
        "Riolu",
        "Lucario",
        "Hippopotas",
        "Hippowdon",
        "Skorupi",
        "Drapion",
        "Croagunk",
        "Toxicroak",
        "Carnivine",
        "Finneon",
        "Lumineon",
        "Mantyke",
        "Snover",
        "Abomasnow",
        "Weavile",
        "Magnezone",
        "Lickilicky",
        "Rhyperior",
        "Tangrowth",
        "Electivire",
        "Magmortar",
        "Togekiss",
        "Yanmega",
        "Leafeon",
        "Glaceon",
        "Gliscor",
        "Mamoswine",
        "Porygon-Z",
        "Gallade",
        "Probopass",
        "Dusknoir",
        "Froslass",
        "Rotom",
        "Uxie",
        "Mesprit",
        "Azelf",
        "Dialga",
        "Palkia",
        "Heatran",
        "Regigigas",
        "Giratina",
        "Cresselia",
        "Phione",
        "Manaphy",
        "Darkrai",
        "Shaymin",
        "Arceus",

        //Unova
        "Victini",
        "Snivy",
        "Servine",
        "Serperior",
        "Tepig",
        "Pignite",
        "Emboar",
        "Oshawott",
        "Dewott",
        "Samurott",
        "Patrat",
        "Watchog",
        "Lillipup",
        "Herdier",
        "Stoutland",
        "Purrloin",
        "Liepard",
        "Pansage",
        "Simisage",
        "Pansear",
        "Simisear",
        "Panpour",
        "Simipour",
        "Munna",
        "Musharna",
        "Pidove",
        "Tranquill",
        "Unfezant",
        "Blitzle",
        "Zebstrika",
        "Roggenrola",
        "Boldore",
        "Gigalith",
        "Woobat",
        "Swoobat",
        "Drilbur",
        "Excadrill",
        "Audino",
        "Timburr",
        "Gurdurr",
        "Conkeldurr",
        "Tympole",
        "Palpitoad",
        "Seismitoad",
        "Throh",
        "Sawk",
        "Sewaddle",
        "Swadloon",
        "Leavanny",
        "Venipede",
        "Whirlipede",
        "Scolipede",
        "Cottonee",
        "Whimsicott",
        "Petilil",
        "Lilligant",
        "Basculin",
        "Sandile",
        "Krokorok",
        "Krookodile",
        "Darumaka",
        "Darmanitan",
        "Maractus",
        "Dwebble",
        "Crustle",
        "Scraggy",
        "Scrafty",
        "Sigilyph",
        "Yamask",
        "Cofagrigus",
        "Tirtouga",
        "Carracosta",
        "Archen",
        "Archeops",
        "Trubbish",
        "Garbodor",
        "Zorua",
        "Zoroark",
        "Minccino",
        "Cinccino",
        "Gothita",
        "Gothorita",
        "Gothitelle",
        "Solosis",
        "Duosion",
        "Reuniclus",
        "Ducklett",
        "Swanna",
        "Vanillite",
        "Vanillish",
        "Vanilluxe",
        "Deerling",
        "Sawsbuck",
        "Emolga",
        "Karrablast",
        "Escavalier",
        "Foongus",
        "Amoonguss",
        "Frillish",
        "Jellicent",
        "Alomomola",
        "Joltik",
        "Galvantula",
        "Ferroseed",
        "Ferrothorn",
        "Klink",
        "Klang",
        "Klinklang",
        "Tynamo",
        "Eelektrik",
        "Eelektross",
        "Elgyem",
        "Beheeyem",
        "Litwick",
        "Lampent",
        "Chandelure",
        "Axew",
        "Fraxure",
        "Haxorus",
        "Cubchoo",
        "Beartic",
        "Cryogonal",
        "Shelmet",
        "Accelgor",
        "Stunfisk",
        "Mienfoo",
        "Mienshao",
        "Druddigon",
        "Golett",
        "Golurk",
        "Pawniard",
        "Bisharp",
        "Bouffalant",
        "Rufflet",
        "Braviary",
        "Vullaby",
        "Mandibuzz",
        "Heatmor",
        "Durant",
        "Deino",
        "Zweilous",
        "Hydreigon",
        "Larvesta",
        "Volcarona",
        "Cobalion",
        "Terrakion",
        "Virizion",
        "Tornadus",
        "Thundurus",
        "Reshiram",
        "Zekrom",
        "Landorus",
        "Kyurem",
        "Keldeo",
        "Meloetta",
        "Genesect",

        //Kalos
        "Chespin",
        "Quilladin",
        "Chesnaught",
        "Fennekin",
        "Braixen",
        "Delphox",
        "Froakie",
        "Frogadier",
        "Greninja",
        "Bunnelby",
        "Diggersby",
        "Fletchling",
        "Fletchinder",
        "Talonflame",
        "Scatterbug",
        "Spewpa",
        "Vivillon",
        "Litleo",
        "Pyroar",
        "Flabébé",
        "Floette",
        "Florges",
        "Skiddo",
        "Gogoat",
        "Pancham",
        "Pangoro",
        "Furfrou",
        "Espurr",
        "Meowstic",
        "Honedge",
        "Doublade",
        "Aegislash",
        "Spritzee",
        "Aromatisse",
        "Swirlix",
        "Slurpuff",
        "Inkay",
        "Malamar",
        "Binacle",
        "Barbaracle",
        "Skrelp",
        "Dragalge",
        "Clauncher",
        "Clawitzer",
        "Helioptile",
        "Heliolisk",
        "Tyrunt",
        "Tyrantrum",
        "Amaura",
        "Aurorus",
        "Sylveon",
        "Hawlucha",
        "Dedenne",
        "Carbink",
        "Goomy",
        "Sliggoo",
        "Goodra",
        "Klefki",
        "Phantump",
        "Trevenant",
        "Pumpkaboo",
        "Gourgeist",
        "Bergmite",
        "Avalugg",
        "Noibat",
        "Noivern",
        "Xerneas",
        "Yveltal",
        "Zygarde",
        "Diancie",
        "Hoopa",
        "Volcanion",

        //Alola
        "Rowlet",
        "Dartrix",
        "Decidueye",
        "Litten",
        "Torracat",
        "Incineroar",
        "Popplio",
        "Brionne",
        "Primarina",
        "Pikipek",
        "Trumbeak",
        "Toucannon",
        "Yungoos",
        "Gumshoos",
        "Grubbin",
        "Charjabug",
        "Vikavolt",
        "Crabrawler",
        "Crabominable",
        "Oricorio",
        "Cutiefly",
        "Ribombee",
        "Rockruff",
        "Lycanroc",
        "Wishiwashi",
        "Mareanie",
        "Toxapex",
        "Mudbray",
        "Mudsdale",
        "Dewpider",
        "Araquanid",
        "Fomantis",
        "Lurantis",
        "Morelull",
        "Shiinotic",
        "Salandit",
        "Salazzle",
        "Stufful",
        "Bewear",
        "Bounsweet",
        "Steenee",
        "Tsareena",
        "Comfey",
        "Oranguru",
        "Passimian",
        "Wimpod",
        "Golisopod",
        "Sandygast",
        "Palossand",
        "Pyukumuku",
        "Type: Null",
        "Silvally",
        "Minior",
        "Komala",
        "Turtonator",
        "Togedemaru",
        "Mimikyu",
        "Bruxish",
        "Drampa",
        "Dhelmise",
        "Jangmo-o",
        "Hakamo-o",
        "Kommo-o",
        "Tapu Koko",
        "Tapu Lele",
        "Tapu Bulu",
        "Tapu Fini",
        "Cosmog",
        "Cosmoem",
        "Solgaleo",
        "Lunala",
        "Nihilego",
        "Buzzwole",
        "Pheromosa",
        "Xurkitree",
        "Celesteela",
        "Kartana",
        "Guzzlord",
        "Necrozma",
        "Magearna",
        "Marshadow",
        "Poipole",
        "Naganadel",
        "Stakataka",
        "Blacephalon",
        "Zeraora",
        "Meltan",
        "Melmetal",

        //Galar
        "Grookey",
        "Thwackey",
        "Rillaboom",
        "Scorbunny",
        "Raboot",
        "Cinderace",
        "Sobble",
        "Drizzile",
        "Inteleon",
        "Skwovet",
        "Greedent",
        "Rookidee",
        "Corvisquire",
        "Corviknight",
        "Blipbug",
        "Dottler",
        "Orbeetle",
        "Nickit",
        "Thievul",
        "Gossifleur",
        "Eldegoss",
        "Wooloo",
        "Dubwool",
        "Chewtle",
        "Drednaw",
        "Yamper",
        "Boltund",
        "Rolycoly",
        "Carkol",
        "Coalossal",
        "Applin",
        "Flapple",
        "Appletun",
        "Silicobra",
        "Sandaconda",
        "Cramorant",
        "Arrokuda",
        "Barraskewda",
        "Toxel",
        "Toxtricity",
        "Sizzlipede",
        "Centiskorch",
        "Clobbopus",
        "Grapploct",
        "Sinistea",
        "Polteageist",
        "Hatenna",
        "Hattrem",
        "Hatterene",
        "Impidimp",
        "Morgrem",
        "Grimmsnarl",
        "Obstagoon",
        "Perrserker",
        "Cursola",
        "Sirfetch’d",
        "Mr. Rime",
        "Runerigus",
        "Milcery",
        "Alcremie",
        "Falinks",
        "Pincurchin",
        "Snom",
        "Frosmoth",
        "Stonjourner",
        "Eiscue",
        "Indeedee",
        "Morpeko",
        "Cufant",
        "Copperajah",
        "Dracozolt",
        "Arctozolt",
        "Dracovish",
        "Arctovish",
        "Duraludon",
        "Dreepy",
        "Drakloak",
        "Dragapult",
        "Zacian",
        "Zamazenta",
        "Eternatus",
        "Kubfu",
        "Urshifu",
        "Zarude",
        "Regieleki",
        "Regidrago",
        "Glastrier",
        "Spectrier",
        "Calyrex",

        //Hisui
        "Wyrdeer",
        "Kleavor",
        "Ursaluna",
        "Basculegion",
        "Sneasler",
        "Overqwil",
        "Enamorus",

        //Custom
        "Alakarate",
        "Machost",
        "Gololium",
        "Haucker",
    };
    public static int[] PokemonDifferentGenderSprites =
    {
        //Kanto
        3,12,19,20,25,
        26,41,42,44,45,
        64,65,84,85,97,
        111,112,118,119,123,
        129,130,

        //Johto
        154,165,166,178,185,
        186,190,194,195,198,
        202,203,207,208,212,
        214,215,217,221,224,
        229,232,

        //Hoenn
        255,256,257,267,269,
        272,274,275,307,308,
        315,316,317,322,323,
        332,350,369,

        //Sinnoh

        //Custom
        906


    };
    public static string[] UnownForm =
    {
        "A","B","C","D","E",
        "F","G","H","I","J",
        "K","L","M","N","O",
        "P","Q","R","S","T",
        "U","V","W","X","Y",
        "Z","!","?",
    };
    static int[] PokemonHasUniqueEvolutionList =
    {
        236,265
    };

    /// <summary>
    /// Enter in the pokedex number to return the name
    /// </summary>
    /// <param name="pokedexNumber">ID #</param>
    public static string GetPokeDexName(int pokedexNumber)
    {
        pokedexNumber--;

        if(pokedexNumber < PokemonName.Length)
        {
            return PokemonName[pokedexNumber];
        }
        else
        {
            return "MissingNo";
        }
    }

    public static bool GenderExclusive(int pokedexNumber)
    {
        return PokemonDifferentGenderSprites.Contains(pokedexNumber);
    }
    /// <summary>
    /// Checks to see if this pokemon has a unique evolution
    /// </summary>
    /// <param name="pokedexNumber">Pokedex Number</param>
    /// <returns></returns>
    public static bool PokemonHasUniqueEvolution(int pokedexNumber)
    {
        return PokemonHasUniqueEvolutionList.Contains(pokedexNumber);
    }

    public static PokemonBase ReturnUniqueEvolution(Pokemon pokemon)
    {
        if(pokemon.pokemonBase.GetPokedexNumber() == 236)//Tyrogue
        {
            if(pokemon.attack > pokemon.defense)
            {
                return Resources.Load<PokemonBase>("Pokedex/Gen1/106 Hitmonlee");
            }
            else if(pokemon.attack < pokemon.defense)
            {
                return Resources.Load<PokemonBase>("Pokedex/Gen1/107 Hitmonchan");
            }
            return Resources.Load<PokemonBase>("Pokedex/Gen2/237 Hitmontop");
        }
        else if (pokemon.pokemonBase.GetPokedexNumber() == 265)// Wurmple
        {
            if(pokemon.personalityValue % 2 == 0)
            {
                return Resources.Load<PokemonBase>("Pokedex/Gen3/266 Silcoon");
            }
            else
            {
                return Resources.Load<PokemonBase>("Pokedex/Gen3/268 Cascoon");
            }
        }
        return null;
    }

    public static bool IsNincada(int pokedexNumber)
    {
        return (pokedexNumber == 290);
    }
}