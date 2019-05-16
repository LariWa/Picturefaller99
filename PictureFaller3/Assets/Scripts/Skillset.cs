using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillset : MonoBehaviour
{
    public List<SkillMaster> allSkills;

    void Start()
    {

    }


    public SkillMaster getPrimary()
    {
        return allSkills[0];
    }

    public SkillMaster getSecondary()
    {
        return allSkills[1];
    }

    public SkillMaster getMovement()
    {
        return allSkills[2];
    }



    /*
    public void doPrimary(Vector3 mousePos)
    {
        primaryFire.doSkill(mousePos);
    }

    public void doMovement(Vector3 mousePos)
    {
        movementSkill.doSkill(mousePos);
    }*/
}


