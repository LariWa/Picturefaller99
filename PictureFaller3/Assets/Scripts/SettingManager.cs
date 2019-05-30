using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class SettingManager : MonoBehaviour
{
    private enum Settings { City, Forest };
    [SerializeField] private Settings startSetting;
    private Settings currentSetting;
    private Settings nextSetting;

    [SerializeField] private GameObject[] cityChunks;
    [SerializeField] private GameObject[] forestChunks;

    [Space]

    // SHOULD BE IN PICTURE MANAGER?
    [SerializeField] private Sprite[] allCityPictures; //original order
    [SerializeField] private Sprite[] allForestPictures; //original order

    private Sprite[] cityPicturesInSort;
    private Sprite[] forestPicturesInSort;

    [Space]

    [SerializeField] private TextAsset[] citySorts;
    [SerializeField] private TextAsset[] forestSorts;

    public bool useSorts;

    void Awake()
    {
        currentSetting = startSetting;

        //Get random next setting
        nextSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);
        while (currentSetting == nextSetting)
            nextSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);


        cityPicturesInSort = (Sprite[])allCityPictures.Clone();
        forestPicturesInSort = (Sprite[])allForestPictures.Clone();

        randomSortForNextSetting();
    }


    void Update()
    {
        
    }


    public void randomSortForNextSetting()
    {
        if (!useSorts) return;

        var settingAllPictures = getAllPicturesOrg(nextSetting);
        var settinPicturesInSort = getAllNextPicturesInSort();

        var nextSettingSorts = getSorts(nextSetting);

        //A single txt with sort in it
        var randSort = nextSettingSorts[Random.Range(0, nextSettingSorts.Length)];
        char[] sortChars = randSort.text.ToCharArray();

        //Random jumble (with reoccurances)
        //for (int i = 0; i < settingAllPictures.Length; i++)
        //settinPicturesInSort[i] = settingAllPictures[Random.Range(0, settingAllPictures.Length)];


        var index = 0;

        //Look at every second character in txt
        for (int c = 0; c < sortChars.Length; c++)
        {
            var character = sortChars[c];
            if(character.Equals('_'))
            {
                string sortId = sortChars[c + 1].ToString();
                if (int.TryParse(sortChars[c + 2].ToString(), out int n)) sortId += sortChars[c + 2]; //Bad implementation 
                if (int.TryParse(sortChars[c + 3].ToString(), out int m)) sortId += sortChars[c + 3];


                var id = int.Parse(sortId) - 1;
                if(id < settingAllPictures.Length)
                {
                    //print(id);
                    settinPicturesInSort[index] = settingAllPictures[id];
                    index++;
                }
            }


            //if (string.Compare(sortString, "_157", true))
        }
    }





    private TextAsset[] getSorts(Settings s)
    {
        if (s == Settings.City) return citySorts;
        if (s == Settings.Forest) return forestSorts;

        return null;
    }

    private Sprite[] getAllPicturesOrg(Settings s)
    {
        if (s == Settings.City) return allCityPictures;
        if (s == Settings.Forest) return allForestPictures;

        return null;
    }



    public Sprite[] getAllNextPicturesInSort()
    {
        if (nextSetting == Settings.City) return cityPicturesInSort;
        if (nextSetting == Settings.Forest) return forestPicturesInSort;

        return null;
    }


    public GameObject getRandomChunkCurrSetting()
    {
        if (currentSetting == Settings.City) return cityChunks[Random.Range(0, cityChunks.Length)];
        if (currentSetting == Settings.Forest) return forestChunks[Random.Range(0, forestChunks.Length)];

        return null;
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
}
