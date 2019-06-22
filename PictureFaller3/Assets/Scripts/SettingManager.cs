using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class SettingManager : MonoBehaviour
{
    public enum Settings { City, Forest, Food, Water };
    [SerializeField] private bool startRandom;
    [SerializeField] private Settings startSetting;
    private Settings currentSetting; //For chunks
    private Settings nextSetting; //For picture wall

    [SerializeField] private GameObject[] cityChunks;
    [SerializeField] private GameObject[] forestChunks;
    [SerializeField] private GameObject[] foodChunks;
    [SerializeField] private GameObject[] waterChunks;

    [Space]

    // SHOULD BE IN PICTURE MANAGER?
    [SerializeField] private Sprite[] allCityPictures;  //original order
    [SerializeField] private Sprite[] allForestPictures;//original order
    [SerializeField] private Sprite[] allFoodPictures;//original order
    [SerializeField] private Sprite[] allWaterPictures;//original order

    private Sprite[] cityPicturesInSort;  //Sorted always differently
    private Sprite[] forestPicturesInSort;//Sorted always differently
    private Sprite[] foodPicturesInSort;//Sorted always differently
    private Sprite[] waterPicturesInSort;//Sorted always differently

    [Space]

    [SerializeField] private TextAsset[] citySorts;
    [SerializeField] private TextAsset[] forestSorts;
    [SerializeField] private TextAsset[] foodSorts;
    [SerializeField] private TextAsset[] waterSorts;

    public bool useSorts;
    private DifficultyManager difficultyManager;

    public void setPicture()
    {
        var ImageLoader = new ImageLoader();


    // automatically called when game started
}
    void Start()
    {
        ImageLoader imageLoader = new ImageLoader(allForestPictures, allCityPictures, allFoodPictures);
        StartCoroutine(LoadFromLikeCoroutine()); // execute the section independently

       
    }

    // this section will be run independently
    private IEnumerator LoadFromLikeCoroutine()
    {
        string url = "http://localhost:8000/nature_255/nature_9.jpg";

        Debug.Log("Loading ....");
        WWW wwwLoader = new WWW(url);   // create WWW object pointing to the url
        yield return wwwLoader;         // start loading whatever in that url ( delay happens here )

        Debug.Log("Loaded");
        // set white
        Sprite pic = Sprite.Create(wwwLoader.texture, new Rect(0.0f, 0.0f, wwwLoader.texture.width, wwwLoader.texture.height), new Vector2(0.5f, 0.5f));

        allForestPictures[1] = pic;  // set loaded image
    }
    void Awake()
    {
        setPicture();
        difficultyManager = GetComponent<DifficultyManager>();
        
        if(startRandom) currentSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);
        else currentSetting = startSetting;

        //Get random next setting
        nextSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);
        while (currentSetting == nextSetting)
            nextSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);


        //cityPicturesInSort = (Sprite[])allCityPictures.Clone();
        //forestPicturesInSort = (Sprite[])allForestPictures.Clone();


        //randomSortForSetting(currentSetting);
        //randomSortForSetting(nextSetting);
    }


    void Update()
    {
        
    }


    public void randomSortForSetting(Settings set)
    {
        if (!useSorts) return;

        var settingAllPictures = getAllPicturesOrg(set);
        var settinPicturesInSort = getAllPicturesInSort(set);

        int size = difficultyManager.getDim() * difficultyManager.getDim();
        settinPicturesInSort = new Sprite[size];

        var settingSorts = getSorts(set);

        //A single txt with sort in it
        var randSort = settingSorts[Random.Range(0, settingSorts.Length)]; //Get a random sort (good/ bad atm)

        string sort = randSort.text;
        char[] sortChars = sort.ToCharArray();

        //Random jumble (with reoccurances)
        //for (int i = 0; i < settingAllPictures.Length; i++)
        //settinPicturesInSort[i] = settingAllPictures[Random.Range(0, settingAllPictures.Length)];



        //Get the relevant sort (by grid size)
        for (int c = 0; c < sortChars.Length; c++)
        {
            if (sortChars[c].Equals(':'))
            {
                //check the numbers before it
                string sortSize = sortChars[c - 1].ToString();
                if (c > 2)
                {
                    if (int.TryParse(sortChars[c - 2].ToString(), out int n)) sortSize += sortChars[c - 2];
                    if (int.TryParse(sortChars[c - 3].ToString(), out int m)) sortSize += sortChars[c - 3];
                }

                sortSize = Reverse(sortSize);

                //Found correct size sort 
                if (int.Parse(sortSize) == size)
                {
                    //Search the end
                    var len = 0;
                    for (int c2 = 0; c2 < sortChars.Length; c2++)
                    {
                        if (sortChars[c + c2].Equals('-'))
                        {
                            len = c2;
                            break;
                        }
                    }
                    // take the next "size" lines as new sortChars
                    sort = sort.Substring(c + 1, len);
                    sortChars = sort.ToCharArray();
                    break;
                }
            }
        }


        var index = 0;

        //Look at every second character in txt
        for (int c = 0; c < sortChars.Length; c++)
        {
            if (sortChars[c].Equals('_'))
            {

                //Which picture to go at this pos
                string sortId = sortChars[c + 1].ToString();
                if (int.TryParse(sortChars[c + 2].ToString(), out int n)) sortId += sortChars[c + 2]; //Bad implementation 
                if (int.TryParse(sortChars[c + 3].ToString(), out int m)) sortId += sortChars[c + 3];


                var id = int.Parse(sortId) - 1;
                if (id < settingAllPictures.Length)
                {
                    settinPicturesInSort[index] = settingAllPictures[id];
                    index++;
                }
            }

            //if (string.Compare(sortString, "_157", true))
        }


        //Apply sort changes
        setAllPicturesInSort(set, settinPicturesInSort); // WHY IS THIS NECESSARY ?????
    }







    private TextAsset[] getSorts(Settings s)
    {
        if (s == Settings.City) return citySorts;
        if (s == Settings.Forest) return forestSorts;
        if (s == Settings.Food) return foodSorts;
        if (s == Settings.Water) return waterSorts;

        return null;
    }

    private Sprite[] getAllPicturesOrg(Settings s)
    {
        if (s == Settings.City) return allCityPictures;
        if (s == Settings.Forest) return allForestPictures;
        if (s == Settings.Food) return allFoodPictures;
        if (s == Settings.Water) return allWaterPictures;

        return null;
    }



    public Sprite[] getAllPicturesInSort(Settings s)
    {
        if (s == Settings.City) return cityPicturesInSort;
        if (s == Settings.Forest) return forestPicturesInSort;
        if (s == Settings.Food) return foodPicturesInSort;
        if (s == Settings.Water) return waterPicturesInSort;

        return null;
    }
    public void setAllPicturesInSort(Settings s, Sprite[] spr)
    {
        if (s == Settings.City) cityPicturesInSort = spr;
        if (s == Settings.Forest) forestPicturesInSort = spr;
        if (s == Settings.Food) foodPicturesInSort = spr;
        if (s == Settings.Water) waterPicturesInSort = spr;
    }


    public GameObject getRandomChunkCurrSetting()
    {
        if (currentSetting == Settings.City) return cityChunks[Random.Range(0, cityChunks.Length)];
        if (currentSetting == Settings.Forest) return forestChunks[Random.Range(0, forestChunks.Length)];
        if (currentSetting == Settings.Food) return foodChunks[Random.Range(0, foodChunks.Length)];
        if (currentSetting == Settings.Water) return waterChunks[Random.Range(0, waterChunks.Length)];

        return null;
    }



    public Settings getNextSetting()
    {
        return nextSetting;
    }


    /*public void changeSettingTo(Settings sett)
    {
        currentSetting = sett;
    }*/

    /*
public void changeSettingRandomly()
{
    currentSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);
}*/

    /*
    public void changeSettingToDifferentOne()
    {
        var nextSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);

        while(currentSetting == nextSetting)
            nextSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);

        currentSetting = nextSetting;
    }*/

    public void changeSettingToPictures()
    {
        currentSetting = nextSetting;

        //Get random setting for next
        nextSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);
        while (currentSetting == nextSetting)
            nextSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);
    }


    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        System.Array.Reverse(charArray);
        return new string(charArray);
    }
}



