using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class SettingManager : MonoBehaviour
{
    public enum Settings { City, Forest, Food, Mountain/*, Water*/};
    [SerializeField] private bool startRandom;
    [SerializeField] private Settings startSetting;
    private Settings currentSetting; //For chunks
    private Settings nextSetting; //For picture wall

    [SerializeField] private GameObject[] cityChunks;
    [SerializeField] private GameObject[] forestChunks;
    [SerializeField] private GameObject[] foodChunks;
    //[SerializeField] private GameObject[] waterChunks;
    [SerializeField] private GameObject[] mountainChunks;

    [Space]

    // SHOULD BE IN PICTURE MANAGER?
    private Sprite[] allCityPictures = new Sprite[225];  //original order
    private Sprite[] allForestPictures = new Sprite[225];//original order
    private Sprite[] allFoodPictures = new Sprite[225];//original order
    private Sprite[] allWaterPictures = new Sprite[225];//original order
    private Sprite[] allMountainPictures = new Sprite[225];//original order

    private Sprite[] cityPicturesInSort;  //Sorted always differently
    private Sprite[] forestPicturesInSort;//Sorted always differently
    private Sprite[] foodPicturesInSort;//Sorted always differently
    //private Sprite[] waterPicturesInSort;//Sorted always differently
    private Sprite[] mountainPicturesInSort;//Sorted always differently

    [SerializeField] private string citySortsLocationPics = "SortPics/city";
    [SerializeField] private string natureSortsLocationPics = "SortPics/nature";
    [SerializeField] private string foodSortsLocationPics = "SortPics/food";

    [Space]

    //[SerializeField] private string citySortsLocation = Application.dataPath + "/StreamingAssets/SortTxt/city";
    //[SerializeField] private string natureSortsLocation = Application.dataPath + "/StreamingAssets/SortTxt/nature";
    //[SerializeField] private string foodSortsLocation = Application.dataPath + "/StreamingAssets/SortTxt/food";

    [SerializeField]  private TextAsset[] citySorts;
    [SerializeField]  private TextAsset[] forestSorts;
    [SerializeField]  private TextAsset[] foodSorts;
    private ImageLoader imageLoaderManager;

    //private TextAsset[] waterSorts;
    private TextAsset[] mountainSorts;

    [Space]

    [SerializeField] private GameObject directionalLight;
    [SerializeField] private Vector3 defaultLightRot;
    [SerializeField] private Vector3 cityLightRot;
    [SerializeField] private Vector3 foodLightRot;
    [SerializeField] private Vector3 forestLightRot;

    public bool useSorts;
    private int sortQuality;
    private DifficultyManager difficultyManager;

    void Awake()
    {
        // Load resources

        //var sort = Resources.Load<TextAsset>("SortTxt/city_hq");   citySortsLocation
        //citySorts.Add(sort);

        //citySorts = Resources.LoadAll<TextAsset>(citySortsLocation);
        //forestSorts = Resources.LoadAll<TextAsset>(natureSortsLocation);
        //mountainSorts = Resources.LoadAll<TextAsset>(natureSortsLocation);
        //foodSorts = Resources.LoadAll<TextAsset>(foodSortsLocation);

        //allCityPictures = Resources.LoadAll<Sprite>(citySortsLocationPics);
        //allForestPictures = Resources.LoadAll<Sprite>(natureSortsLocationPics);
        //allMountainPictures = Resources.LoadAll<Sprite>(natureSortsLocationPics);
        //allFoodPictures = Resources.LoadAll<Sprite>(foodSortsLocationPics);

        // Sort properly so pic_01 is at array position 0, etc
        //allCityPictures = allCityPictures.OrderBy(obj => obj.name, new AlphanumComparatorFast()).ToArray();
        //allForestPictures = allForestPictures.OrderBy(obj => obj.name, new AlphanumComparatorFast()).ToArray();
        //allMountainPictures = allMountainPictures.OrderBy(obj => obj.name, new AlphanumComparatorFast()).ToArray();
        //allFoodPictures = allFoodPictures.OrderBy(obj => obj.name, new AlphanumComparatorFast()).ToArray();

        ////print(allCityPictures.Length);
        //foreach (var s in citySortsTEST)
        //    Debug.Log(s.name);


        imageLoaderManager = FindObjectOfType<ImageLoader>();

        allCityPictures = imageLoaderManager.getCityPics();
        allForestPictures = imageLoaderManager.getNaturePics();
        allMountainPictures = imageLoaderManager.getNaturePics();
        allFoodPictures = imageLoaderManager.getFoodPics();


        //ImageLoader imageLoader = new ImageLoader(allForestPictures, allCityPictures, allFoodPictures);
        //imageLoader.loadJSON();
        //imageLoaderManager = FindObjectOfType<ImageLoader>();
        //imageLoaderManager.loadJSON();

        Debug.Log(imageLoaderManager);
        difficultyManager = GetComponent<DifficultyManager>();

        if (startRandom) currentSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);
        else currentSetting = startSetting;

        //Get random next setting
        nextSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);
        while (currentSetting == nextSetting)
            nextSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);


        setLightSource(currentSetting);

        //cityPicturesInSort = (Sprite[])allCityPictures.Clone();
        //forestPicturesInSort = (Sprite[])allForestPictures.Clone();


        //randomSortForSetting(currentSetting);
        //randomSortForSetting(nextSetting);
    }


    void Update()
    {

    }
    public (Sprite[], Sprite[], Sprite[]) getPictureArrays()
    {
        return (allForestPictures, allCityPictures, allFoodPictures);
    }

    public void setPictureArrays(Sprite[] nature, Sprite[] city, Sprite[]food)
    {
        allForestPictures = nature;
        allCityPictures = city;
        allFoodPictures = food;
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
        int sortIndex = Random.Range(0, settingSorts.Length);
        var randSort = settingSorts[sortIndex]; //Get a random sort (good/ bad atm)

        sortQuality = sortIndex; //sortIndex == 0 ? true : false;

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
        //if (s == Settings.Water) return waterSorts;
        if (s == Settings.Mountain) return mountainSorts;

        return null;
    }

    private Sprite[] getAllPicturesOrg(Settings s)
    {
        if (s == Settings.City) return allCityPictures;
        if (s == Settings.Forest) return allForestPictures;
        if (s == Settings.Food) return allFoodPictures;
        //if (s == Settings.Water) return allWaterPictures;
        if (s == Settings.Mountain) return allMountainPictures;

        return null;
    }



    public Sprite[] getAllPicturesInSort(Settings s)
    {
        if (s == Settings.City) return cityPicturesInSort;
        if (s == Settings.Forest) return forestPicturesInSort;
        if (s == Settings.Food) return foodPicturesInSort;
        //if (s == Settings.Water) return waterPicturesInSort;
        if (s == Settings.Mountain) return mountainPicturesInSort;

        return null;
    }
    public void setAllPicturesInSort(Settings s, Sprite[] spr)
    {
        if (s == Settings.City) cityPicturesInSort = spr;
        if (s == Settings.Forest) forestPicturesInSort = spr;
        if (s == Settings.Food) foodPicturesInSort = spr;
        //if (s == Settings.Water) waterPicturesInSort = spr;
        if (s == Settings.Mountain) mountainPicturesInSort = spr;
    }


    public GameObject getRandomChunkCurrSetting()
    {
        if (currentSetting == Settings.City) return cityChunks[Random.Range(0, cityChunks.Length)];
        if (currentSetting == Settings.Forest) return forestChunks[Random.Range(0, forestChunks.Length)];
        if (currentSetting == Settings.Food) return foodChunks[Random.Range(0, foodChunks.Length)];
        //if (currentSetting == Settings.Water) return waterChunks[Random.Range(0, waterChunks.Length)];
        if (currentSetting == Settings.Mountain) return mountainChunks[Random.Range(0, mountainChunks.Length)];

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

        setLightSource(currentSetting);
        
        //Get random setting for next
        nextSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);
        while (currentSetting == nextSetting)
            nextSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);
    }


    private void setLightSource(Settings current)
    {
        if (current == Settings.Forest)
            directionalLight.transform.eulerAngles = forestLightRot;
        else if(current == Settings.City)
            directionalLight.transform.eulerAngles = cityLightRot;
        else if (current == Settings.Food)
            directionalLight.transform.eulerAngles = foodLightRot;
        else
            directionalLight.transform.eulerAngles = defaultLightRot;
    }


    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        System.Array.Reverse(charArray);
        return new string(charArray);
    }
    
    public int getQuality()
    {

        
        return sortQuality;
   

    }

}



