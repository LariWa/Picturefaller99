using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float fadeSpd = 0.5f;
    private Rigidbody rb;
    

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();   
    }

    void FixedUpdate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        var playerStats = other.transform.GetComponentInChildren<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.damagePlayer(damage, true);
            other.transform.GetComponentInChildren<PlayerMovement>().knockBack(transform.position);

            FindObjectOfType<SoundEffects>().hitDmgObj();
            FindObjectOfType<ScreenShakeTest>().hitObj();

            //Destroy(transform.parent.gameObject);
            GetComponent<Collider>().enabled = false;

            //Not working because material not set to fade    !!!!!!!!!!!!!!!!!!!!!!!!!
            var mat = GetComponent<MeshRenderer>().material;
            //StartCoroutine(fadeObjOver(0, fadeSpd, mat)); //https://answers.unity.com/questions/1004666/change-material-rendering-mode-in-runtime.html
        }

    }


    private IEnumerator fadeObjOver(float aValue, float aTime, Material material)
    {
        float alpha = material.color.a;

        for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
        {
            setFadeAlpha(Mathf.Lerp(alpha, aValue, t), material);
            yield return null;
        }
        setFadeAlpha(aValue, material);
    }
    private void setFadeAlpha(float am, Material material)
    {
        var col = material.color;
        col.a = am;
        material.color = col;
    }
}
