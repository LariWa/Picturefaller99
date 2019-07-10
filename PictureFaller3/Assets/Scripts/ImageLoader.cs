    using System.Collections;
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



    class ImageLoader: MonoBehaviour
    {

        public string url = "http://localhost:8000/nature_255/nature_9.jpg";
        public Renderer thisRenderer;
        private SettingManager settingM ;


        // automatically called when game started
        public Sprite[] nature ;
        public Sprite[] city;
        public Sprite[] food;
        public Texture[] tex = new Texture[225];
    /*public ImageLoader(Sprite[] nature, Sprite[] city, Sprite[] food)
    {
        this.nature = nature;
        this.city = city;
        this.food = food;
    }*/

    public void Start()
    {
        settingM = GetComponent<SettingManager>();
        nature = settingM.getPictureArrays().Item1;
        city = settingM.getPictureArrays().Item2;
        food = settingM.getPictureArrays().Item3;
    }
    public IEnumerator asyncLoadImage(string url, int pos, Sprite[] array)
    {
        Debug.Log("LOAD2");
        //string path = "http://localhost:8000/nature_225/nature_1.jpg";
        //Debug.Log(path);
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            //UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)

            {
                Debug.Log("ERROR: " + uwr.error);
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(uwr);
                Debug.Log("texture");
                Debug.Log(texture);
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



    public void loadPictures(PictureJSON pictureJSON)
        {

        for (int i = 0; i < pictureJSON.nature.Length; i++)
        {
            Debug.Log(pictureJSON.nature[i]);
                loadImage(pictureJSON.nature[i], i, nature);
                loadImage(pictureJSON.city[i], i, city);
                loadImage(pictureJSON.food[i], i, food);
            }
        settingM.setPictureArrays(nature, city, food);


        }

        private IEnumerator asyncLoad()
        {
            Debug.Log("LOAD2");
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
            Start();
            Debug.Log("LOAD");
             StartCoroutine("asyncLoad");


            }


        public void he()
        {
            Debug.Log("Test");
        }


    }
