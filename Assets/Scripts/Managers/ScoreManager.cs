using System.Collections;
using System.Collections.Generic;
using Script.Utils;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    private const string BestScoreKey = "BESTSCORE";

    private int currentScore = 0;
    private int bestScore = 0;
    void Start()
    {
        if (PlayerPrefs.HasKey(BestScoreKey))
        {
            bestScore = PlayerPrefs.GetInt(BestScoreKey);
            bestScoreText.SetText(bestScore.ToString());
        }
    }

    public void UpdateScore(int value)
    {
        currentScore += value;
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            bestScoreText.SetText(bestScore.ToString());
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
        }
        scoreText.SetText(currentScore.ToString());
    }
}
