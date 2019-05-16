using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMovement : SkillMaster
{
    public float distance = 2f;
    //ENUM move, teleport, etc
    //private PlayerMovement movement;

    private void Start()
    {
        //movement = GetComponentInParent<PlayerMovement>();
    }

    public override void doSkill(Vector3 mousePos)
    {
        Vector3 diff = mousePos - transform.parent.position;
        diff.Normalize();

        transform.parent.position = transform.parent.position + diff * distance; //SHOULD probably be maximum range, and not always that far if mouse is closer
        //movement. change runspeed
    }


    public override int getInputSlot()
    {
       return 1; 
    }
}