using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToBeginning : MonoBehaviour
{
    Vector3 newPlayerPosition;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.transform.position = new Vector3(0, 0, 0);
        }
    }



}
