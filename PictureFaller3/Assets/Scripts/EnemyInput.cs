using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInput : MonoBehaviour
{
    private Skillset skills;


    void Start()
    {
        skills = GetComponent<Skillset>();
    }

    // Update is called once per frame
    void Update()
    {
        if (true)
            skills.getPrimary().tryToDoSkill(transform.position + Vector3.up);

    }
}
