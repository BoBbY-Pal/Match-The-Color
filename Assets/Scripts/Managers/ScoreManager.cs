using System;
using System.Collections;
using System.Collections.Generic;
using Script.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreManager : Singleton<ScoreManager>
{
    [FormerlySerializedAs("scoreText")] [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    public const string BestScoreKey = "BESTSCORE";

    private int currentScore = 0;
    private int bestScore = 0;

    public void UpdateScoreText()
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
        currentScoreText.SetText(currentScore.ToString());
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
    
    public int GetBestScore()
    {
        return bestScore;
    }
    
    public void ResetCurrentScore()
    {
        currentScore = 0;
        currentScoreText.SetText("0");
    }
}
