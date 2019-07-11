using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class ScreenShakeTest : MonoBehaviour
{

    //INSTEAD ONE PIXEL UNIT IN PICO8 ITS NOW ONE WORLD UNIT SO A TILE

    private Vector3 origPos;
    private Vector3 offset;

    private Vector2 trauma;
    private Vector2 shake;

    public float objectShake = 1; //shake of 2 would be very brutal... (transform.x + 2)
    public float maxoffset = 2; //shake of 2 would be very brutal... (transform.x + 2)
    public float traumaFallOff = 0.03333f;
    public float powerOfAllShakes = 2;

    private bool shakingSel;
    private bool shakingObj;
    private Vector3 shakePos;

    private GameObject player;
    public CinemachineVirtualCamera followCam;
    public CinemachineVirtualCamera followCamShake;
    public Transform shakeFollowObj;
    public Transform shakeFixedObj;
    public float wrongShakeDur = 1;
    public float wrongShakeStr = 1;
    public int wrongShakeVibrate = 10;
    public float objShakeDur = 1;
    public float objShakeStr = 1;
    public int objShakeVibrate = 10;
    //public CinemachineVirtualCamera v2;
    //public CinemachineConfiner conf;

    //TODO: add duration, fade in and out, frequency, maybe rotate cam OR JUST GET THE *FREE* ASSET  https://www.youtube.com/watch?v=9A9yj8KnM8c&ab_channel=Brackeys
    // https://www.youtube.com/watch?v=tu-Qe66AvtY&t=636s&ab_channel=GDC
    // https://www.reddit.com/r/gamedesign/comments/6o090l/tried_to_focus_on_game_feel_in_this_game_i_didnt/
    // DIFFERENCE BETWEEN GAME FEEL AND JUICE https://www.youtube.com/watch?v=S-EmAitPYg8&ab_channel=GustavDahl

    protected Cinemachine.CinemachineBasicMultiChannelPerlin _perlin;

    void Start()
    {
        //origPos = shakeFollowObj.position;
        player = GameObject.FindGameObjectWithTag("Player");
        //cinemachine = GetComponent< CinemachineBrain>();
        //addShake(Vector2.up, 1f);

        _perlin = followCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

    }

    public void addShake(Vector2 dir, float strength) //dir only 1,0  0,-1  1,1  etc
    {
        //origPos = shakeFollowObj.position;

        //followCam.enabled = false;
        //followCamShake.enabled = true;

        //makes sure always 1 or 0 (?)
        if (dir.x < 0) dir.x = -dir.x;
        if (dir.x > 0) dir.x = 1;
        if (dir.y < 0) dir.y = -dir.y;
        if (dir.y > 0) dir.y = 1;
        trauma = new Vector2(trauma.x + (dir.x * strength), trauma.y + (dir.y * strength));

        //v1.enabled = false;
        //v2.enabled = true;
    }

    public float getShake()
    {
        return shake.magnitude;
    }

    void Update() //update shake
    {
        trauma.x -= traumaFallOff * Time.deltaTime; // is this the falloff? one every sec?  maybe use Time.deltaTime and * 
        if (trauma.x <= 0) trauma.x = 0;
        if (trauma.x >= 1) trauma.x = 1;
        shake.x = Mathf.Pow(trauma.x, powerOfAllShakes);

        trauma.y -= traumaFallOff * Time.deltaTime;
        if (trauma.y <= 0) trauma.y = 0;
        if (trauma.y >= 1) trauma.y = 1;
        shake.y = Mathf.Pow(trauma.y, powerOfAllShakes);


        offset = new Vector2(shake.x * (maxoffset * Random.Range(-1f, 1f)), shake.y * (maxoffset * Random.Range(-1f, 1f)));

    }


    void LateUpdate() //show shake
    {


        /*if (shakingObj)
        {
            shakeFixedObj.position = shakePos + offset;
        }

        if (offset.magnitude <= 0.01) //so that it follows again quickly
        {
            // shakeFollowObj.transform.parent = followThing;
            shakingObj = false;

            followCam.enabled = true;
            followCamShake.enabled = false;
        }

        print(trauma);*/


        // problem is ship moves away during shake
        // -> maybe faster shake and big just on death

        // or maybe try shake Vcam? well is smoothed...
        // or try using dotween shake?
        // OR RATHER MAKE A SHAKE VIRTUAL CAMERA, MAKE IT FOLLOW PLAYER VERY ACCURATE AND SHAKE THAT
    }


    public void wrongSelection()
    {
        //maxoffset = 3;
        //shakePos = shakeFollowObj.transform.position;
        //shakingSel = true;

        shakeFixedObj.transform.DOShakePosition(wrongShakeDur, wrongShakeStr, wrongShakeVibrate);
    }

    public void hitObj()
    {
        //followCam.n
        StartCoroutine(ShakeCamera());
    }

    protected virtual IEnumerator ShakeCamera() // is this smooth bcz of cinemachine? maybe use the calculation methon from above again here
    {
        _perlin.m_AmplitudeGain = objShakeStr;
        _perlin.m_FrequencyGain = objShakeVibrate;
        yield return new WaitForSeconds(objShakeDur);
        CameraReset();
    }

    public virtual void CameraReset()
    {
        _perlin.m_AmplitudeGain = 0;
        _perlin.m_FrequencyGain = 0;
    }


    /*
    public void hitObj()
    {
        if (!shakingObj)
        {

            shakeFixedObj.transform.position = shakeFollowObj.transform.position;
            followCamShake.transform.position = followCam.transform.position;
            //shakePos = shakeFollowObj.transform.position;
            shakingObj = true;

            //followCam.enabled = false;
            followCamShake.enabled = true;

            //addShake(Random.insideUnitSphere, objectShake);

            //shakeFixedObj.position = new Vector3(0, 0, player.transform.position.z + 9);
            //followCamShake.transform.position = followCam.transform.position;
            //GetComponent<CameraManager>().setFixedPos(shakePos);


            shakeFixedObj.transform.DOShakePosition(objShakeDur, objShakeStr, objShakeVibrate);
            Invoke("stopShake", objShakeDur);
        }
    }

    private void stopShake()
    {
        shakingObj = false;

        //followCam.enabled = true;
        followCamShake.enabled = false;
    }*/
}
