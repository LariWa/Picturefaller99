using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private float defaultBlendTime = 2f;
    //[SerializeField] private Vector3 fixedPosOffset;
    [SerializeField] private GameObject camNormal;
    [SerializeField] private GameObject camPics;
    [SerializeField] private Camera ignorePostCamera;
    [SerializeField] private CinemachineVirtualCamera normalVCam;
    [SerializeField] private GameObject introVCam;
    [SerializeField] private float fovNormal = 60f;
    [SerializeField] private float fovSlow = 40f;
    [SerializeField] private float maxLensDist = 30f;
    [SerializeField] private float slowEffectTimeIn = 0.25f;
    [SerializeField] private float slowEffectTimeBack = 0.1f;
    [SerializeField] private float normalSaturation = 5f;
    [SerializeField] private float slowSaturation = -100f;
    [SerializeField] private float normalVigInt = 0.4f;
    [SerializeField] private float slowVigInt = 0.45f;
    [SerializeField] private float normalVigSmo = 0.2f;
    [SerializeField] private float slowVigSmo = 0.6f;
    private CinemachineFramingTransposer framing;

    private Camera mainCam;
    private bool slowmoSet;

    //private float slowMoProgress = 1;
    private PlayerMovement player;
    private Transform fixedPos;

    private PostProcessVolume ppVol;

    private bool isGameplay;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        if (player != null)
            isGameplay = true;
        else
            return;

        Invoke("turnOffIntroCam", 0.1f);

        mainCam = GetComponent<Camera>();
        ppVol = mainCam.GetComponent<PostProcessVolume>();

        fixedPos = new GameObject("Fixed Pos").transform;
    }


    private void turnOffIntroCam()
    {
        introVCam.SetActive(false);
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

        if (!isGameplay) return;
        
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



    public void setSlowMoCam(bool mode) // TODO: refactor this mess
    {
        if (mode)
        {
            if(!slowmoSet)
            {
                slowmoSet = true;

                LensDistortion lens;
                ppVol.profile.TryGetSettings(out lens);
                lens.intensity.value = 0f; 
                DOTween.To(() => lens.intensity.value, x => lens.intensity.value = x, maxLensDist, slowEffectTimeIn);

                ColorGrading col;
                ppVol.profile.TryGetSettings(out col);
                col.saturation.value = normalSaturation;
                DOTween.To(() => col.saturation.value, x => col.saturation.value = x, slowSaturation, slowEffectTimeIn);

                Vignette vig;
                ppVol.profile.TryGetSettings(out vig);
                vig.intensity.value = normalVigInt;
                DOTween.To(() => vig.intensity.value, x => vig.intensity.value = x, slowVigInt, slowEffectTimeIn);
                vig.smoothness.value = normalVigSmo;
                DOTween.To(() => vig.smoothness.value, x => vig.smoothness.value = x, slowVigSmo, slowEffectTimeIn);


                DOTween.To(() => normalVCam.m_Lens.FieldOfView, x => normalVCam.m_Lens.FieldOfView = x, fovSlow, slowEffectTimeIn);
            }
        }
        else
        {
            //if (slowmoSet)
            //{
                slowmoSet = false;

                LensDistortion lens;
                ppVol.profile.TryGetSettings(out lens);
                DOTween.To(() => lens.intensity.value, x => lens.intensity.value = x, 0, slowEffectTimeBack);

                ColorGrading col;
                ppVol.profile.TryGetSettings(out col);
                DOTween.To(() => col.saturation.value, x => col.saturation.value = x, normalSaturation, slowEffectTimeBack);

                Vignette vig;
                ppVol.profile.TryGetSettings(out vig);
                DOTween.To(() => vig.intensity.value, x => vig.intensity.value = x, normalVigInt, slowEffectTimeBack);
                DOTween.To(() => vig.smoothness.value, x => vig.smoothness.value = x, normalVigSmo, slowEffectTimeBack);

                DOTween.To(() => normalVCam.m_Lens.FieldOfView, x => normalVCam.m_Lens.FieldOfView = x, fovNormal, slowEffectTimeBack);
            //}
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


    public Transform getFixedPos()
    {
        return fixedPos;
    }

}
