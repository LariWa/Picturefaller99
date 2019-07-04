using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UiManager : MonoBehaviour
{
    public Image portal;
    public float portalRotateSpeed = -20;
    //public float portalRotateSpeedRandMore;
    [Space]
    //public float portalScaleDur = 1;
    //public float portalScaleStr = 1;
    //public int portalScaleVib = 10;
    //public float portalScaleRand = 90;

    public float portalScaleShrinkDur = 1;
    public float portalScaleExpandDur = 1;
    public float portalScalePunch = 0.5f;
    private Vector3 smallScale;
    public int portalScaleVib = 10;
    public float portalScaleElast = 1;



    void Start()
    {
        smallScale = new Vector3(portalScalePunch, portalScalePunch, portalScalePunch);

        //portal.transform.DORotate(new Vector3(0,0,transform.rotation.z + 1), portalRotateSpeed).SetLoops(-1);


        //portal.transform.DOShakeScale(portalScaleDur, portalScaleStr, portalScaleVib, portalScaleRand); //SetLoops(-1)
        //portal.transform.DOPunchScale(punchVec, portalScaleDur, portalScaleVib, portalScaleElast).SetLoops(-1);
        Sequence seq = DOTween.Sequence().SetLoops(-1);
        seq.Append(portal.transform.DOScale(smallScale, portalScaleShrinkDur).SetEase(Ease.InOutSine));
        seq.Append(portal.transform.DOScale(portal.transform.localScale, portalScaleExpandDur).SetEase(Ease.Linear));
    }


    void LateUpdate()
    {
        // animate portal
        //portal.transform.DORotate();
        portal.transform.Rotate(new Vector3(0,0, (portalRotateSpeed /* + Random.Range(0, portalRotateSpeedRandMore)*/) * Time.deltaTime));

    }
}
