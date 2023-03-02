
namespace PokeApi
{
    [System.Serializable]
    public class PokemonData
    {
        public int id;
        public string name;
        public int base_experience;
        public int height;
        public int weight;
        public Stats[] stats;
        public PokemonType[] types;
        public PokemonSpecies species;
        public Sprites sprites;
        public PokemonAbility[] abilities;
    }

    [System.Serializable]
    public class Stats
    {
        public Stat stat;
        public int effort;
        public int base_stat;
    }

    [System.Serializable]
    public class Stat
    {
        public int id;
        public string name;
    }

    [System.Serializable]
    public class PokemonType
    {
        public int slot;
        public Type type;
    }

    [System.Serializable]
    public class Type
    {
        public int id;
        public string name;
    }

    [System.Serializable]
    public class PokemonSpecies
    {
        public int gender_rate;
        public int capture_rate;
        public int base_happiness;
        public bool is_baby;
        public bool is_legendary;
        public bool is_mythical;
        public int hatch_counter;
        public bool has_gender_differences;
        public GrowthRate growth_rate;
        public EggGroup[] egg_groups;
        public FlavorText[] flavor_text_entries;
        public Genera[] genera;
    }

    [System.Serializable]
    public class GrowthRate
    {
        public int id;
        public string name;
    }

    [System.Serializable]
    public class EggGroup
    {
        public string name;
    }

    [System.Serializable]
    public class PokemonAbility
    {
        public bool is_hidden;
        public int slot;
        public Ability ability;
    }

    [System.Serializable]
    public class Ability
    {
        public string name;
    }

    [System.Serializable]
    public class FlavorText
    {
        public string flavor_text;
        public Language language;
    }

    [System.Serializable]
    public class Language
    {
        public string name;
    }

    [System.Serializable]
    public class Genera
    {
        public string genus;
        public Language language;
    }

    [System.Serializable]
    public class Sprites
    {
        public string back_default;
        public string back_female;
        public string back_shiny;
        public string back_shiny_female;
        public string front_default;
        public string front_female;
        public string front_shiny;
        public string front_shiny_female;
    }
}


