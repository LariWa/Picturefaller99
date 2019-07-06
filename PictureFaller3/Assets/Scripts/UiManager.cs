using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI countdown;
    public Image[] portals;
    public float portalRotateSpeed = -20;
    //public float portalRotateSpeedRandMore;
    [Space]
    //public float portalScaleDur = 1;
    //public float portalScaleStr = 1;
    //public int portalScaleVib = 10;
    //public float portalScaleRand = 90;

    public float portalScaleShrinkDur = 1;
    public float portalScaleExpandDur = 1;
    public float scaleDivider = 0.5f;
    public float scaleOffset = 0.25f;
    public int portalScaleVib = 10;
    public float portalScaleElast = 1;

    [Space]

    public float cdScale = 1.5f;
    public float cdScaleDur = 0.5f;
    public float visibleSpd = 0.1f;
    public float invisibleSpd = 0.25f;

    private SoundEffects soundEffects;


    void Start()
    {
        soundEffects = FindObjectOfType<SoundEffects>();
        StartCoroutine(startPortalAnim());
        countdown.text = "";
    }

    private IEnumerator startPortalAnim()
    {
        //portal.transform.DORotate(new Vector3(0,0,transform.rotation.z + 1), portalRotateSpeed).SetLoops(-1);
        for (int i = 0; i < portals.Length; i++)
        {
            //portal.transform.DOShakeScale(portalScaleDur, portalScaleStr, portalScaleVib, portalScaleRand); //SetLoops(-1)
            //portal.transform.DOPunchScale(punchVec, portalScaleDur, portalScaleVib, portalScaleElast).SetLoops(-1);
            Sequence seq = DOTween.Sequence().SetLoops(-1);
            seq.Append(portals[i].transform.DOScale(portals[i].transform.localScale * scaleDivider, portalScaleShrinkDur).SetEase(Ease.InOutSine));
            seq.Append(portals[i].transform.DOScale(portals[i].transform.localScale, portalScaleExpandDur).SetEase(Ease.Linear));

            yield return new WaitForSeconds(scaleOffset);
        }
    }

    void LateUpdate()
    {
        // animate portal
        //portal.transform.DORotate();

        for (int i = 0; i < portals.Length; i++)
            portals[i].transform.Rotate(new Vector3(0, 0, (portalRotateSpeed /* + Random.Range(0, portalRotateSpeedRandMore)*/) * Time.deltaTime));
        
    }


    public void setCountdown(float secondsLeft)
    {
        if(secondsLeft < 0)
            countdown.text = "";
        else
        {
            var lastText = countdown.text;
            secondsLeft = Mathf.FloorToInt(secondsLeft) + 1; //Mathf.CeilToInt(secondsLeft) + 1;
            countdown.text = secondsLeft + "";


            // New number to display
            if(lastText != countdown.text)
            {
                countdown.alpha = 0;
                Sequence seq = DOTween.Sequence();
                seq.Append(countdown.transform.DOPunchScale(Vector3.one * cdScale, cdScaleDur));
                seq.Insert(0, countdown.DOFade(1, visibleSpd));
                seq.Append(countdown.DOFade(0, invisibleSpd));

                soundEffects.countdownTick(secondsLeft);
                soundEffects.countdownTickOffbeat();
            }
        }
    }

}
