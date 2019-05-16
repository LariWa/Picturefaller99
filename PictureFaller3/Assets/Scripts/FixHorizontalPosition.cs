using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixHorizontalPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //var pos = transform.parent.position;
        //transform.position = new Vector3(-pos.x, pos.y, -pos.z);
        var par = transform.parent;
        transform.parent = null;
        transform.position = new Vector3(0, transform.position.y, 0);
        transform.parent = par;
    }
}
