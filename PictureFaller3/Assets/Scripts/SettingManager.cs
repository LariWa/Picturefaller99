using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    private enum Settings { City, Forest };
    [SerializeField] private Settings startSetting;
    private Settings currentSetting;

    [SerializeField] private GameObject[] cityChunks;
    [SerializeField] private GameObject[] forestChunks;

    void Awake()
    {
        currentSetting = startSetting;
    }


    void Update()
    {
        
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

    public void changeSettingRandomly()
    {
        currentSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);
    }

    public void changeSettingToDifferentOne()
    {
        var nextSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);

        while(currentSetting == nextSetting)
            nextSetting = (Settings)Random.Range(0, System.Enum.GetValues(typeof(Settings)).Length);

        currentSetting = nextSetting;
    }
}
