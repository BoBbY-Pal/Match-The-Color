
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
    
    [SerializeField] private RectTransform gameInfoPanel;
    [SerializeField] private GameoverScreen gameOverPanel;
    [SerializeField] private TextMeshProUGUI mainMenuBestScore;

    public Image soundImage;
    public Sprite muteSprite;
    public Sprite unMuteSprite;
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

    public void ActivateInfoPanel()
    {
        gameInfoPanel.gameObject.SetActive(true);
        SoundManager.Instance.Play(SoundTypes.ButtonClick);
        gameInfoPanel.DOLocalMoveY(0, 0.6f).SetEase(Ease.InFlash);
    }
    public void DeActivateInfoPanel()
    {
        SoundManager.Instance.Play(SoundTypes.ButtonClick);

        gameInfoPanel.DOLocalMoveY(800, 0.6f).SetEase(Ease.OutBounce).OnComplete(() => gameInfoPanel.gameObject.SetActive(false));
    }
    
    public void OpenProfile()
    {
        Debug.Log("Open profile");
        SoundManager.Instance.Play(SoundTypes.ButtonClick);

        // Replace the URL with the LinkedIn profile URL you want to open
        string url = "https://www.linkedin.com/in/bobby-pal/";
        Application.OpenURL(url);
    }
    
    public void MuteBtnPressed()
    {
        Debug.Log("MUTE");
        bool muteStatus = SoundManager.Instance.isMute;
        soundImage.sprite = muteStatus ? unMuteSprite : muteSprite;
        SoundManager.Instance.Mute(!muteStatus);
        SoundManager.Instance.Play(SoundTypes.ButtonClick);
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
