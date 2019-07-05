using System.Collections;
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

	public float scoreCount;
	public float highscoreCount;

	public float pointsPerSecond;
	public bool scoreIncreasing;
    
    void Start()
    {
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


        scoreText.text = Mathf.Round(scoreCount) +         " Score";// " Score"; //" Current"
        highscoreText.text = Mathf.Round(highscoreCount) + " Best"; // " Best"; //" Highest"
    }


    public void addScorePictureHit(float time)
    {
        var am = Mathf.RoundToInt((correctPictureMultiplier * 100) / time); //TODO: Better interpolation

        var s = Instantiate(scorePlusPrefab, Vector3.zero, Quaternion.identity);
        s.transform.parent = effectsParent.transform;
        s.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,100);
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
