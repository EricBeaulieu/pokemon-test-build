using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public bool testEXPGroup;
    public ExperienceGroup testExperienceGroup;
    public NatureBase testNature;
    public Sprite sprite;
    [SerializeField] PokeballItem pokeball;
    [SerializeField] List<Sprite> pokemonSprites = new List<Sprite>();


    public ElementType attacktype = ElementType.Bug;
    public ElementType defenderType = ElementType.Dark;
    public ElementType defenderType2 = ElementType.NA;

    public Pokemon[] pcPokemonTest;

    [Header("PokemonImages")]
    [SerializeField] PokemonBase currentPokemon;
    [SerializeField] bool showFemaleVariation;
    [SerializeField] Text pokemonText;
    [SerializeField] Text pokemonInfo;
    [SerializeField] Image front;
    [SerializeField] Image frontShiny;
    [SerializeField] Image back;
    [SerializeField] Image backShiny;
    [SerializeField] Image spriteA;
    [SerializeField] Sprite spriteNull;
    [SerializeField] Sprite standardNull;


    // Use this for initialization
    void Start()
    {

        if (testEXPGroup)
        {
            TestExpTable();
        }
        //TestNatureName(testNature);
        //TestTypeChart();

        //sprite = GameManager.instance.SpriteAtlas.GetSprite("075_Graveler_FrontB");

        //for (int i = 0; i < 255; i++)
        //{
        //    Debug.Log(PokemonNameList.GetPokeDexName(i));
        //}
        pokemonSprites = SpriteAtlas.pokemonSprites;
    }

    void TestNatureName(NatureBase currentNature)
    {
        Debug.Log(currentNature.natureName);
    }

    //void TestTypeChart()
    //{
    //    float effectiveness = DamageModifiers.TypeChartEffectiveness(defenderType, attacktype);
    //    Debug.Log($"{defenderType} DefenderType + {attacktype} AttackType = {effectiveness}");
    //}

    void TestExpTable()
    {
        for (int i = 1; i < 101; i++)
        {
            Debug.Log($"Level {i} EXP to level {ExperienceTable.ReturnExperienceRequiredForLevel(i, testExperienceGroup)}");
        }
    }

    public void SetAllTestPokemon()
    {
        PlayerController player = GameManager.instance.GetPlayerController;

        for (int i = 0; i < pcPokemonTest.Length; i++)
        {
            if (pcPokemonTest[i].pokemonBase == null)
            {
                pcPokemonTest[i] = null;
                continue;
            }
            pcPokemonTest[i].SetUpData();
            pcPokemonTest[i].Obtained(player, pokeball);
        }
    }

    public PokemonBase CurrentPokemon
    {
        get
        {
            return currentPokemon;
        }
        set
        {
            currentPokemon = value;
        }
    }

    public void UpdateInfo(PokemonBase pokemonBase)
    {
        //Debug.Log("Updating Info");
        int pokedexNumber = pokemonBase.GetPokedexNumber();
        PokeApi.PokemonData currentPokemon = null;

        front.sprite = standardNull;
        frontShiny.sprite = standardNull;
        back.sprite = standardNull;
        backShiny.sprite = standardNull;
        spriteA.sprite = spriteNull;

        if (PokemonNameList.PokemonDoesntHaveAnOriginalFormName(pokedexNumber) == false)
        {
            currentPokemon = PokeApi.APIHelper.GetPokemonData(pokedexNumber);

            if (string.IsNullOrEmpty(pokemonBase.GetForm()) == true)
            {
                currentPokemon = PokeApi.APIHelper.GetPokemonData(currentPokemon.name);
            }
            else
            {
                currentPokemon = PokeApi.APIHelper.GetPokemonData(currentPokemon.name);
                currentPokemon = PokeApi.APIHelper.GetPokemonData($"{currentPokemon.name}-{pokemonBase.GetForm()}");
            }
        }
        else
        {
            string currentLookup = $"{pokemonBase.GetPokedexName()}-{pokemonBase.GetForm()}".ToLower();
            currentPokemon = PokeApi.APIHelper.GetPokemonData(currentLookup);
        }

        pokemonText.text = $"#{currentPokemon.id} {currentPokemon.name.UpperFirstChar()}";

        pokemonInfo.text = $"back_default: {currentPokemon.sprites.back_default}\n" +
            $"back_female: {currentPokemon.sprites.back_female}\n" +
            $"back_shiny: {currentPokemon.sprites.back_shiny}\n" +
            $"back_shiny_female: {currentPokemon.sprites.back_shiny_female}\n" +
            $"front_default: {currentPokemon.sprites.front_default}\n" +
            $"front_female: {currentPokemon.sprites.front_female}\n" +
            $"front_shiny: {currentPokemon.sprites.front_shiny}\n" +
            $"front_shiny_female: {currentPokemon.sprites.front_shiny_female}";

        if (PokemonNameList.PokemonDoesntHaveAnOriginalFormName(pokedexNumber) == false)
        {
            if (PokemonNameList.GenderExclusive(pokedexNumber) == true && showFemaleVariation)
            {
                currentPokemon.sprites.back_default = ((string.IsNullOrEmpty(currentPokemon.sprites.back_female) == false ? currentPokemon.sprites.back_female : currentPokemon.sprites.back_default));
                currentPokemon.sprites.back_shiny = ((string.IsNullOrEmpty(currentPokemon.sprites.back_shiny_female) == false ? currentPokemon.sprites.back_shiny_female : currentPokemon.sprites.back_shiny));
                currentPokemon.sprites.front_default = ((string.IsNullOrEmpty(currentPokemon.sprites.front_female) == false ? currentPokemon.sprites.front_female : currentPokemon.sprites.front_default));
                currentPokemon.sprites.front_shiny = ((string.IsNullOrEmpty(currentPokemon.sprites.front_shiny_female) == false ? currentPokemon.sprites.front_shiny_female : currentPokemon.sprites.front_shiny));
            }
        }

        bool backTextureDoesntExist = false;

        if (string.IsNullOrEmpty(currentPokemon.sprites.back_default) == true)
        {
            back.rectTransform.localScale = new Vector3(-1, 1, 1);
            backShiny.rectTransform.localScale = new Vector3(-1, 1, 1);
            StartCoroutine(GetTexture(currentPokemon.sprites.front_default, back));
            backTextureDoesntExist = true;
        }
        else
        {
            back.rectTransform.localScale = new Vector3(1, 1, 1);
            backShiny.rectTransform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(GetTexture(currentPokemon.sprites.back_default, back));
            StartCoroutine(GetTexture(currentPokemon.sprites.back_shiny, backShiny));
        }

        if (string.IsNullOrEmpty(currentPokemon.sprites.front_default) == true)
        {
            string newURL = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{currentPokemon.id}.png";
            //Debug.Log(newURL);
            StartCoroutine(GetTexture(newURL, front, (backTextureDoesntExist == true ? back : null)));
        }
        else
        {
            StartCoroutine(GetTexture(currentPokemon.sprites.front_default, front, (backTextureDoesntExist == true ? back : null)));
        }

        if (string.IsNullOrEmpty(currentPokemon.sprites.front_shiny) == true)
        {
            string newURL = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/shiny/{currentPokemon.id}.png";
            //Debug.Log(newURL);
            StartCoroutine(GetTexture(newURL, frontShiny, (backTextureDoesntExist == true ? backShiny : null)));
        }
        else
        {
            StartCoroutine(GetTexture(currentPokemon.sprites.front_shiny, frontShiny, (backTextureDoesntExist == true ? backShiny : null)));
        }

        //

        string pokemonSpriteName = $"generation-vii/icons/";
        if (pokemonBase.GetForm().ToLower() == "galar" || pokemonBase.GetForm().ToLower() == "hisui" || (pokedexNumber >= 810 && pokedexNumber < 906))
        {
            pokemonSpriteName = $"generation-viii/icons/";
        }
        else if (pokemonBase.GetForm().ToLower() == "galar" || pokedexNumber >= 906)
        {
            //icons dont exist for this gen
        }

        if (string.IsNullOrEmpty(pokemonBase.GetForm()) == true)
        {
            pokemonSpriteName += $"{pokedexNumber}";
        }
        else
        {
            pokemonSpriteName += $"{pokedexNumber}-{pokemonBase.GetForm().ToLower()}";
        }

        string spriteURL = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/versions/{pokemonSpriteName}.png";
        StartCoroutine(GetTexture(spriteURL, spriteA));
    }

    IEnumerator GetTexture(string url, Image image, Image backImage = null)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture($"{url}");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            Debug.Log(url);
        }
        else
        {
            Texture2D recieved = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite = Sprite.Create(recieved, new Rect(0, 0, recieved.width, recieved.height), new Vector2(0.5f, 0.5f));
            image.sprite = sprite;
            if (backImage != null)
            {
                backImage.sprite = sprite;
            }
        }
    }

    //public void UpdateInfo(PokemonBase pokemonBase)
    //{
    //    //Debug.Log("Updating Info");
    //    int pokedexNumber = pokemonBase.GetPokedexNumber();
    //    PokeApi.PokemonData currentPokemon = null;

    //    front.sprite = standardNull;
    //    frontShiny.sprite = standardNull;
    //    back.sprite = standardNull;
    //    backShiny.sprite = standardNull;
    //    spriteA.sprite = spriteNull;

    //    if (PokemonNameList.PokemonDoesntHaveAnOriginalFormName(pokedexNumber) == false)
    //    {
    //        currentPokemon = PokeApi.APIHelper.GetPokemonData(pokedexNumber);

    //        if (string.IsNullOrEmpty(pokemonBase.GetForm()) == true)
    //        {
    //            currentPokemon = PokeApi.APIHelper.GetPokemonData(currentPokemon.name);
    //        }
    //        else
    //        {
    //            currentPokemon = PokeApi.APIHelper.GetPokemonData(currentPokemon.name);
    //            currentPokemon = PokeApi.APIHelper.GetPokemonData($"{currentPokemon.name}-{pokemonBase.GetForm()}");
    //        }
    //    }
    //    else
    //    {
    //        string currentLookup = $"{pokemonBase.GetPokedexName()}-{pokemonBase.GetForm()}".ToLower();
    //        currentPokemon = PokeApi.APIHelper.GetPokemonData(currentLookup);
    //    }

    //    pokemonText.text = $"#{currentPokemon.id} {currentPokemon.name.UpperFirstChar()}";

    //    pokemonInfo.text = $"back_default: {currentPokemon.sprites.back_default}\n" +
    //        $"back_female: {currentPokemon.sprites.back_female}\n" +
    //        $"back_shiny: {currentPokemon.sprites.back_shiny}\n" +
    //        $"back_shiny_female: {currentPokemon.sprites.back_shiny_female}\n" +
    //        $"front_default: {currentPokemon.sprites.front_default}\n" +
    //        $"front_female: {currentPokemon.sprites.front_female}\n" +
    //        $"front_shiny: {currentPokemon.sprites.front_shiny}\n" +
    //        $"front_shiny_female: {currentPokemon.sprites.front_shiny_female}";

    //    if (PokemonNameList.PokemonDoesntHaveAnOriginalFormName(pokedexNumber) == false)
    //    {
    //        if (PokemonNameList.GenderExclusive(pokedexNumber) == true && showFemaleVariation)
    //        {
    //            currentPokemon.sprites.back_default = ((string.IsNullOrEmpty(currentPokemon.sprites.back_female) == false ? currentPokemon.sprites.back_female : currentPokemon.sprites.back_default));
    //            currentPokemon.sprites.back_shiny = ((string.IsNullOrEmpty(currentPokemon.sprites.back_shiny_female) == false ? currentPokemon.sprites.back_shiny_female : currentPokemon.sprites.back_shiny));
    //            currentPokemon.sprites.front_default = ((string.IsNullOrEmpty(currentPokemon.sprites.front_female) == false ? currentPokemon.sprites.front_female : currentPokemon.sprites.front_default));
    //            currentPokemon.sprites.front_shiny = ((string.IsNullOrEmpty(currentPokemon.sprites.front_shiny_female) == false ? currentPokemon.sprites.front_shiny_female : currentPokemon.sprites.front_shiny));
    //        }
    //    }

    //    bool backTextureDoesntExist = false;

    //    if (string.IsNullOrEmpty(currentPokemon.sprites.back_default) == true)
    //    {
    //        back.rectTransform.localScale = new Vector3(-1, 1, 1);
    //        backShiny.rectTransform.localScale = new Vector3(-1, 1, 1);
    //        StartCoroutine(GetTexture(currentPokemon.sprites.front_default, back.sprite));
    //        backTextureDoesntExist = true;
    //    }
    //    else
    //    {
    //        back.rectTransform.localScale = new Vector3(1, 1, 1);
    //        backShiny.rectTransform.localScale = new Vector3(1, 1, 1);
    //        StartCoroutine(GetTexture(currentPokemon.sprites.back_default, back.sprite));
    //        StartCoroutine(GetTexture(currentPokemon.sprites.back_shiny, backShiny.sprite));
    //    }

    //    if (string.IsNullOrEmpty(currentPokemon.sprites.front_default) == true)
    //    {
    //        string newURL = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{currentPokemon.id}.png";
    //        Debug.Log(newURL);
    //        StartCoroutine(GetTexture(newURL, front.sprite, (backTextureDoesntExist == true ? back.sprite : null)));
    //    }
    //    else
    //    {
    //        StartCoroutine(GetTexture(currentPokemon.sprites.front_default, front.sprite, (backTextureDoesntExist == true ? back.sprite : null)));
    //    }

    //    if (string.IsNullOrEmpty(currentPokemon.sprites.front_shiny) == true)
    //    {
    //        string newURL = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/shiny/{currentPokemon.id}.png";
    //        Debug.Log(newURL);
    //        StartCoroutine(GetTexture(newURL, frontShiny.sprite, (backTextureDoesntExist == true ? backShiny.sprite : null)));
    //    }
    //    else
    //    {
    //        StartCoroutine(GetTexture(currentPokemon.sprites.front_shiny, frontShiny.sprite, (backTextureDoesntExist == true ? backShiny.sprite : null)));
    //    }



    //    string pokemonSpriteName = $"generation-vii/icons/";
    //    if (pokemonBase.GetForm().ToLower() == "galar" || pokemonBase.GetForm().ToLower() == "hisui" || (pokedexNumber >= 810 && pokedexNumber < 906))
    //    {
    //        pokemonSpriteName = $"generation-viii/icons/";
    //    }
    //    else if (pokemonBase.GetForm().ToLower() == "galar" || pokedexNumber >= 906)
    //    {
    //        icons dont exist for this gen
    //    }

    //    if (string.IsNullOrEmpty(pokemonBase.GetForm()) == true)
    //    {
    //        pokemonSpriteName += $"{pokedexNumber}";
    //    }
    //    else
    //    {
    //        pokemonSpriteName += $"{pokedexNumber}-{pokemonBase.GetForm().ToLower()}";
    //    }

    //    string spriteURL = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/versions/{pokemonSpriteName}.png";
    //    StartCoroutine(GetTexture(spriteURL, spriteA.sprite));
    //}

    //IEnumerator GetTexture(string url, Sprite image, Sprite backImage = null)
    //{
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture($"{url}");
    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log(www.error);
    //        Debug.Log(url);
    //    }
    //    else
    //    {
    //        Texture2D recieved = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        Sprite sprite = Sprite.Create(recieved, new Rect(0, 0, recieved.width, recieved.height), new Vector2(0.5f, 0.5f));
    //        image = sprite;
    //        if (backImage != null)
    //        {
    //            backImage = sprite;
    //        }
    //    }
    //}
}
