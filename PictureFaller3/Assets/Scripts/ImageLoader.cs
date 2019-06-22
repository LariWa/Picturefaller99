using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class PictureJSON
{
     public string[] nature = new string[255];
     public string[] food = new string[255];
     public string[] city = new string[255];
   
}



public class ImageLoader
{
    

    public string url = "http://localhost:8000/nature_255/nature_9.jpg";
    public Renderer thisRenderer;

    private static PictureJSON loadJson()
    {
        PictureJSON pictureJSON;
        string path = Application.dataPath + "/StreamingAssets/pictures_all.json";
        string contents = File.ReadAllText(path);
        pictureJSON = JsonUtility.FromJson<PictureJSON>(contents);

        Debug.Log(pictureJSON);
        return pictureJSON;
    }


    // automatically called when game started
    public Sprite[] nature;
    public Sprite[] city;
    public Sprite[] food;
    public ImageLoader(Sprite[] nature, Sprite[] city, Sprite[] food) {
        this.nature = nature;
        this.city = city;
        this.food = food;
    }

    // this section will be run independently
    private IEnumerator LoadFromLikeCoroutine(string url, int pos, Sprite[]array)
    {


        Debug.Log("Loading ....");
        WWW wwwLoader = new WWW(url);   // create WWW object pointing to the url
        yield return wwwLoader;         // start loading whatever in that url ( delay happens here )

        Debug.Log("Loaded");
        // set white
        Sprite pic = Sprite.Create(wwwLoader.texture, new Rect(0.0f, 0.0f, wwwLoader.texture.width, wwwLoader.texture.height), new Vector2(0.5f, 0.5f));

        array[pos] = pic;  // set loaded image
    }

  
    public void loadPictures(PictureJSON pictureJSON)
    {
        pictureJSON = loadJson();

        for (int i = 0; i < pictureJSON.nature.Length; i++)
        {

            LoadFromLikeCoroutine(pictureJSON.nature[i], i, nature);
            LoadFromLikeCoroutine(pictureJSON.city[i], i, city);
            LoadFromLikeCoroutine(pictureJSON.food[i], i, food);
        }



    }

   


}
