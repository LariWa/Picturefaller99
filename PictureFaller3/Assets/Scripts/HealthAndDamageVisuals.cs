using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HealthAndDamageVisuals : MonoBehaviour
{
    private EntityStatsMaster entity;
    private Vector3 offset;

    //public Canvas canvasWorldSpace; 
    public List<Slider> HPsliders;
    public GameObject damageTextPrefab;

    void Awake()
    {
        entity = transform.parent.GetComponentInChildren<EntityStatsMaster>();

        if (entity is PlayerStats) HPsliders.Add(GameObject.FindGameObjectWithTag("PlayerHPSliderInterface").GetComponent<Slider>());

        //if (entity is EnemyLogic) HPslider.transform.parent = canvasWorldSpace.transform;

        //if(HPslider) offset = HPslider.transform.position - entity.transform.parent.position;
    }


    void LateUpdate()
    {
        //if (entity is EnemyLogic) HPslider.transform.position = entity.transform.parent.position + offset;
    }

    public void showDamageText(int damage, Vector3 entitiyPos)
    {
        //Should add pooling, bad to instantiate to heavily!!!
        var dmgTxt = Instantiate(damageTextPrefab, entitiyPos, Quaternion.identity);
        dmgTxt.GetComponent<TextMeshPro>().text = damage.ToString();

        float dmgTxtDuration = 0.5f;
        Vector2 startPos = transform.position;  //Place the start of damage text at the head of thing if entity or slightly above objects !!!!!!!!!!!!!!!
        Destroy(dmgTxt, dmgTxtDuration);

        //Animate text -> start medium, get bigger shortly but then fade and get smaller, also go into random dir? and SLIGHTLY rotate? well go like an arc, so jump
        Sequence recallSequence = DOTween.Sequence();
        var moveVec = Vector2.up + (Vector2.right * Random.Range(-0.5f,0.5f));
        recallSequence.Append(dmgTxt.transform.DOMove(startPos + moveVec/*Random.insideUnitCircle*/, dmgTxtDuration/2));
        recallSequence.Append(dmgTxt.GetComponent<TextMeshPro>().DOFade(0, dmgTxtDuration / 2)); //.color.DOColor(Color.clear, dmgTxtDuration)); // = Color.clear;
        recallSequence.Insert(dmgTxtDuration / 2, dmgTxt.transform.DOScale(0.5f, dmgTxtDuration / 2)); //.color.DOColor(Color.clear, dmgTxtDuration)); // = Color.clear;

        //DOPunchScale
        //SetEase

        //recallSequence.Append(rune.transform.DOJump(transform.position, recallArc, 1, recallDelay));
        //recallSequence.Join(rune.transform.DORotate(new Vector3(0, 0, 300), recallDelay, RotateMode.FastBeyond360));
        //recallSequence.Append(rune.transform.DOMoveY(recallArc, recallDelay / 2)).SetEase(Ease.InCubic);
        //recallSequence.Append(rune.transform.DOMoveY(-recallArc, recallDelay / 2)).SetEase(Ease.OutCubic);

        //recallSequence.AppendCallback(() => backpack.collectMonster(monNr));

    }

    public void setHealth(int health)
    {
        foreach(Slider s in HPsliders)
            s.value = health;
    }

    public void setMaxHP(int healthMax)
    {
        foreach (Slider s in HPsliders)
            s.maxValue = healthMax;
    }
}
