using UnityEngine;
using System.Net;
using System.IO;

namespace PokeApi
{
    public static class APIHelper
    {
        public static PokemonData GetPokemonData(int idNumber)
        {
            if (idNumber <= 0)
            {
                return null;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://pokeapi.co/api/v2/pokemon/{idNumber}");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return JsonUtility.FromJson<PokemonData>(json);
        }

        public static PokemonData GetPokemonData(string pokemonName)
        {
            if (string.IsNullOrEmpty(pokemonName))
            {
                return null;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://pokeapi.co/api/v2/pokemon/{pokemonName.ToLower()}");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return JsonUtility.FromJson<PokemonData>(json);
        }

        public static PokemonSpecies GetPokemonSpecies(int idNumber)
        {
            if (idNumber <= 0)
            {
                return null;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://pokeapi.co/api/v2/pokemon-species/{idNumber}");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return JsonUtility.FromJson<PokemonSpecies>(json);
        }

        //public static Texture GetPokemonSprite(string url)
        //{
        //    if (string.IsNullOrEmpty(url))
        //    {
        //        return null;
        //    }
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{url}");
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    StreamReader reader = new StreamReader(response.GetResponseStream());
        //    string json = reader.ReadToEnd();
        //    return JsonUtility.FromJson<Texture>(json);
        //}
    }
}

