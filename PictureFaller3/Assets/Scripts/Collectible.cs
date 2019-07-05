using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collectible : MonoBehaviour
{
    public int scoreGain;
    public int HPgain;
    [Space]
    public float coinRotSpd = 1;
    [Space]
    public float hpScaleVec = 0.5f;
    public float hpScaleDur = 1;
    public float hpScaleDelay = 0.25f;
    public float hpScalePause = 0.5f;
    public int hpScaleVib = 10;
    public float hpScaleElast = 1;


    void Start()
    {
        if (HPgain != 0)
        {
            var size = new Vector3(hpScaleVec, hpScaleVec, hpScaleVec);
            
            Sequence seq = DOTween.Sequence().SetLoops(-1);
            seq.Append(transform.DOPunchScale(size, hpScaleDur, hpScaleVib, hpScaleElast));//.SetEase(Ease.InOutSine));
            seq.AppendInterval(hpScaleDelay);
            seq.Append(transform.DOPunchScale(size, hpScaleDur, hpScaleVib, hpScaleElast));//.SetEase(Ease.InOutSine));
            seq.AppendInterval(hpScalePause);
        }
    }


    void FixedUpdate()
    {
        if (scoreGain != 0)
            transform.Rotate(new Vector3(0, coinRotSpd * Time.deltaTime, 0));
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponentInChildren<PlayerStats>() != null)
        {

            if (HPgain != 0)
                collision.GetComponentInChildren<PlayerStats>().healPlayer(HPgain);

            if (scoreGain != 0)
                FindObjectOfType<ScoreManager>().addScoreCoins(scoreGain);


            //GetComponent<Collider>().enabled = false;
            //GetComponent<MeshRenderer>().enabled = false;

            Destroy(gameObject);
        }

    }
}
