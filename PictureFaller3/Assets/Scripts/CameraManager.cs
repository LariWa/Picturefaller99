using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private float defaultBlendTime = 2f;
    //[SerializeField] private Vector3 fixedPosOffset;
    [SerializeField] private GameObject camNormal;
    [SerializeField] private GameObject camPics;
    [SerializeField] private Camera ignorePostCamera;
    private CinemachineFramingTransposer framing;

    private Camera mainCam;

    //private float slowMoProgress = 1;
    private PlayerMovement player;
    private Transform fixedPos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        mainCam = GetComponent<Camera>();

        fixedPos = new GameObject("Fixed Pos").transform;
    }

   
    void LateUpdate()
    {

        //Just linear...
        //cam.m_Lens.FieldOfView = Mathf.Lerp(maxFoV, normalFoV, slowMoProgress); 
        //framing.m_CameraDistance = Mathf.Lerp(naxDistance, normalDistance, slowMoProgress);


        //if (player.floating)
        //{
        //     camPics.SetActive(true);
        //     camNormal.SetActive(false);
        // }
        //else
        //{
        //     camPics.SetActive(false);
        //    camNormal.SetActive(true);
        // }

        
        
        if (player.floating || player.divingDown)
        {
            ignorePostCamera.gameObject.SetActive(true);
            ignorePostCamera.fieldOfView = mainCam.fieldOfView;
        }
        else
        {
            ignorePostCamera.gameObject.SetActive(false);
        }

    }


    public void setNormalCam(bool hardCut)
    {
        var brain = FindObjectOfType<CinemachineBrain>();
        if(hardCut)brain.m_DefaultBlend.m_Time = 0; // 0 Time equals a cut
        else       brain.m_DefaultBlend.m_Time = defaultBlendTime;

        camPics.SetActive(false);
        camNormal.SetActive(true);
    }

    public void setPictureCam()
    {
        var brain = FindObjectOfType<CinemachineBrain>();
        brain.m_DefaultBlend.m_Time = defaultBlendTime;


        //Dont follow player but stay at his last position
        fixedPos.position = new Vector3(0, 0, player.transform.position.z);// + fixedPosOffset;
        camPics.GetComponent<CinemachineVirtualCamera>().Follow = fixedPos;

        camPics.SetActive(true);
        camNormal.SetActive(false);
    }



}
