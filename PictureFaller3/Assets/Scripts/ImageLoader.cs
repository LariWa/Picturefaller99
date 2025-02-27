﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

[System.Serializable]
public class PictureJSON
{
    public string[] nature = new string[225];
    public string[] food = new string[225];
    public string[] city = new string[225];
}


public class ImageLoader : MonoBehaviour
{

    public string url = "http://localhost:8000/nature_255/nature_9.jpg";
    public Renderer thisRenderer;
    private SettingManager settingM;

    static ImageLoader instance = null;

    // automatically called when game started
    public Sprite[] nature = new Sprite[225];
    public Sprite[] city = new Sprite[225];
    public Sprite[] food = new Sprite[225];
    public Texture[] tex = new Texture[225];


    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        loadJSON();
    }
    public IEnumerator asyncLoadImage(string url, int pos, Sprite[] array)
    {
        Debug.Log("asyncLoadImage method");
     
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)

            {
                Debug.Log("ERROR: " + uwr.error);
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(uwr);
               
                Sprite pic = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                tex[pos] = texture;
                array[pos] = pic;  // set loaded image

            }
        }
    }

    public void loadImage(string url, int pos, Sprite[] array)
    {
        StartCoroutine(asyncLoadImage(url, pos, array));
    }


    public Sprite[] getCityPics()
    {
        return city;
    }

    public Sprite[] getNaturePics()
    {
        return nature;
    }

    public Sprite[] getFoodPics()
    {
        return food;
    }

    public void loadPictures(PictureJSON pictureJSON)
    {

        for (int i = 0; i < pictureJSON.nature.Length; i++)
        {
            //Debug.Log(pictureJSON.nature[i]);
            Debug.Log(pictureJSON.city[i]);
            Debug.Log(pictureJSON.food[i]);

            loadImage(pictureJSON.nature[i], i, nature);
            loadImage(pictureJSON.city[i], i, city);
            loadImage(pictureJSON.food[i], i, food);
        }
        // settingM.setPictureArrays(nature, city, food);


    }


    private IEnumerator asyncLoad()
    {
        Debug.Log("asyncLoad method");
        string path = "http://localhost:8000/pictures_all.json";
        Debug.Log(path);
        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)

        {
            Debug.Log("ERROR: " + www.error);
        }
        else
        {
            PictureJSON pictureJSON;
            string contents = www.downloadHandler.text;
            Debug.Log(contents);
            pictureJSON = JsonUtility.FromJson<PictureJSON>(contents);
            loadPictures(pictureJSON);
        }


    }
    public void loadJSON()
    {
        //Start();
        Debug.Log("LOADJSON");
        StartCoroutine("asyncLoad");


    }


    public void he()
    {
        Debug.Log("Test");
    }


}