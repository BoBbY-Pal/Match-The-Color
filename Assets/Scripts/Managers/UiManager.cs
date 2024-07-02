
using System;
using DG.Tweening;
using Managers;
using Script.Utils;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject mainmenuPanel;
    [SerializeField] private GameoverScreen gameOverPanel;
    [SerializeField] private TextMeshProUGUI mainMenuBestScore;
    private void Start()
    {
        gameplayPanel.SetActive(false);
        mainmenuPanel.SetActive(true);
        mainMenuBestScore.text = ScoreManager.Instance.GetBestScore().ToString();
    }

    public void PlayBtnPressed()
    {
        SoundManager.Instance.Play(SoundTypes.ButtonClick);
        mainmenuPanel.SetActive(false);
        gameplayPanel.SetActive(true);
        GameManager.Instance.StartGame();
        ScoreManager.Instance.UpdateScoreText();
    }
    
    public void ExitBtnPressed()
    {
        Debug.Log("Exit button pressed");
        SoundManager.Instance.Play(SoundTypes.ButtonClick); 
        mainMenuBestScore.text = ScoreManager.Instance.GetBestScore().ToString();

        mainmenuPanel.SetActive(true);
        gameplayPanel.SetActive(false);
        GameManager.Instance.ExitGame();
    }

    public void RestartBtnPressed()
    {
        DisableGameoverPanel();
        gameplayPanel.SetActive(false);
        GameManager.Instance.ExitGame();
        
        PlayBtnPressed();
    }
    
    public void Gameover()
    {
        SoundManager.Instance.Play(SoundTypes.GameOver);
        EnableGameoverPanel();
        GameManager.Instance.isGameOver = true;
    }

    void DisableGameoverPanel()
    {
        gameOverPanel.rectTransform.DOLocalMoveY(1100, 0.6f).OnComplete(() => gameOverPanel.gameObject.SetActive(false));
    }
    
    void EnableGameoverPanel()
    {
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.currentScore.text = "Current Score: " + ScoreManager.Instance.GetCurrentScore();
        gameOverPanel.bestScore.text = "Best Score: " + ScoreManager.Instance.GetBestScore();
        gameOverPanel.rectTransform.DOLocalMoveY(0, 0.6f);
    }
}
