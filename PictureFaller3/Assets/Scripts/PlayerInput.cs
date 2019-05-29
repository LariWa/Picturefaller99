using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Skillset skills;
    private PlayerMovement movement;

    void Start()
    {
        skills = GetComponent<Skillset>();
        movement = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        if(!movement.floating && !movement.divingDown && Input.GetKey(KeyCode.Space))
            skills.getPrimary().tryToDoSkill(transform.position + Vector3.down);


        //Primary fire, left  lick
        /*if (Input.GetMouseButton(0))
            skills.getPrimary().tryToDoSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetKeyDown(KeyCode.Space))
            skills.getSecondary().tryToDoSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition));*/

        /*
        //Primary fire, hold right click 
        if (Input.GetMouseButton(1))
            skills.getSecondary().showIndication(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonUp(1))
        {
            skills.getSecondary().removeIndication();
            skills.getSecondary().tryToDoSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        //Movement Skill, space
        if (Input.GetKeyDown(KeyCode.Space))
            skills.getMovement().tryToDoSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition));
*/
    }
}
