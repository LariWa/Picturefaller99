using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int scoreGain;
    public int HPgain;

    public float coinRotSpd = 1;

    void Start()
    {
        
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
                FindObjectOfType<ScoreManager>().addScorePictureHit(scoreGain);


            //GetComponent<Collider>().enabled = false;
            //GetComponent<MeshRenderer>().enabled = false;

            Destroy(gameObject);
        }

    }
}
