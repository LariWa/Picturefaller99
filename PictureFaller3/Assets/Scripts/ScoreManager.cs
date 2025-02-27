﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
	public TextMeshProUGUI highscoreText;
	public GameObject effectsParent;
	public GameObject scorePlusPrefab;
	public float correctPictureMultiplier = 5f;
	public float dimMultiplier = 1.5f;

	public float scoreCount;
	public float highscoreCount;

	public float pointsPerSecond;
	public bool scoreIncreasing;

    private DifficultyManager difficultyManager;
    
    void Start()
    {
        difficultyManager = FindObjectOfType<DifficultyManager>();

        if (PlayerPrefs.HasKey("Highscore"))
		{
			highscoreCount = PlayerPrefs.GetFloat("Highscore");
		}
	}

    void Update()
    {
        if ((scoreIncreasing) && (!MenuController.GameIsPaused))
        {
            scoreCount += pointsPerSecond * Time.deltaTime;
        } else {
            scoreCount += 0;
        }
	
		if (scoreCount > highscoreCount)
		{
			highscoreCount = scoreCount;
			PlayerPrefs.SetFloat("Highscore", highscoreCount);
		}

        if(scoreText != null)
        {
            scoreText.text = Mathf.Round(scoreCount) + " Score";// " Score"; //" Current"
            highscoreText.text = Mathf.Round(highscoreCount) + " Best"; // " Best"; //" Highest"
        }

    }


    public void addScorePictureHit(float time)
    {
        if(this.enabled)
        {

            if (time <= 0) time = 0.1f;
            var am = Mathf.RoundToInt((correctPictureMultiplier / time) * (difficultyManager.getDim() * dimMultiplier));


            var s = Instantiate(scorePlusPrefab, Vector3.zero, Quaternion.identity);
            s.transform.parent = effectsParent.transform;
            s.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
            s.transform.GetComponent<TextMeshProUGUI>().text = "+" + am;
            scoreCount += am;

            //Animation....     better
            Destroy(s, 1f);
            Sequence seq = DOTween.Sequence();
            //seq.Append(s.transform.DOPunchScale(Vector3.one, 0.5f));
            //seq.Append(s.transform.DOScale(0f, 0.5f));
            seq.Append(s.transform.DOPunchScale(Vector3.one, 1, 1));
            seq.Insert(0, s.transform.DOShakeScale(0.75f, 1));
            seq.Insert(0.5f, s.transform.GetComponent<TextMeshProUGUI>().DOFade(0, 0.25f));
        }

    }


    public void addScoreCoins(float points)
    {
        if (this.enabled)
        {
            var am = points;


            //todo: other pos

            var s = Instantiate(scorePlusPrefab, Vector3.zero, Quaternion.identity);
            s.transform.parent = effectsParent.transform;
            s.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
            s.transform.GetComponent<TextMeshProUGUI>().text = "+" + am;
            scoreCount += am;

            //Animation....     better
            Destroy(s, 1f);
            Sequence seq = DOTween.Sequence();
            //seq.Append(s.transform.DOPunchScale(Vector3.one, 0.5f));
            //seq.Append(s.transform.DOScale(0f, 0.5f));
            seq.Append(s.transform.DOPunchScale(Vector3.one, 1, 1));
            seq.Insert(0, s.transform.DOShakeScale(0.75f, 1));
            seq.Insert(0.5f, s.transform.GetComponent<TextMeshProUGUI>().DOFade(0, 0.25f));
        }
    }
}
