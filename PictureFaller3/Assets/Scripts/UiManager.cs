using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UiManager : MonoBehaviour
{
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



    void Start()
    {

        StartCoroutine(startPortalAnim());

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
}
