using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPositionTo : MonoBehaviour
{
    [SerializeField] private Transform reference;
    //[SerializeField] private Vector3 axis;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - reference.transform.position;
    }


    void LateUpdate()
    {
        transform.position = new Vector3(0, 0, reference.transform.position.z) + offset;
    }
}
