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
        if (scoreIncreasing)
		{
			scoreCount += pointsPerSecond * Time.deltaTime;
		}
	
		if (scoreCount > highscoreCount)
		{
			highscoreCount = scoreCount;
			PlayerPrefs.SetFloat("Highscore", highscoreCount);
		}
		scoreText.text = Mathf.Round(scoreCount) +         " Score"; //" Current"
        highscoreText.text = Mathf.Round(highscoreCount) + " Best"; //" Highest"
    }


    public void addScorePictureHit(float time)
    {
        var am = Mathf.RoundToInt((correctPictureMultiplier * 100) / time); //TODO: Better interpolation

        var s = Instantiate(scorePlusPrefab, Vector3.zero, Quaternion.identity);
        s.transform.parent = effectsParent.transform;
        s.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        s.transform.GetComponent<TextMeshProUGUI>().text = "+" + am;
        scoreCount += am;

        //Animation....     FIX IT (position, fade, scale)
        Destroy(s, 1f);
        Sequence seq = DOTween.Sequence();
        seq.Append(s.transform.DOPunchScale(Vector3.one, 0.5f));
        seq.Append(s.transform.DOScale(0f, 0.5f));
    }
}
