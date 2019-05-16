using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject camNormal;
    [SerializeField] private GameObject camPics;
    private CinemachineFramingTransposer framing;

    //private float slowMoProgress = 1;
    private PlayerMovement player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

   
    void Update() //Late?
    {

        //Just linear...
        //cam.m_Lens.FieldOfView = Mathf.Lerp(maxFoV, normalFoV, slowMoProgress); 
        //framing.m_CameraDistance = Mathf.Lerp(naxDistance, normalDistance, slowMoProgress);


        if (player.floating)
        {
            camPics.SetActive(true);
            camNormal.SetActive(false);
        }
        else
        {
            camPics.SetActive(false);
            camNormal.SetActive(true);
        }
    }


}
