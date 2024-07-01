using Controllers;
using Managers;
using Script.Utils;
using UnityEngine;

public class UiManager : Singleton<UiManager>
{
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject mainmenuPanel;


   
    public void PlayBtnPressed()
    {
        mainmenuPanel.SetActive(false);
        gameplayPanel.SetActive(true);
        GameManager.Instance.StartGame();
    }
    
    public void ExitBtnPressed()
    {
        Debug.Log("Exit button pressed");
        mainmenuPanel.SetActive(true);
        gameplayPanel.SetActive(false);
        GameManager.Instance.StartGame();
        GameManager.Instance.ExitGame();
    }
}
