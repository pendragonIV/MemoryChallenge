using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform panelWindow;
    [SerializeField]
    private Transform pausePanel;
    [SerializeField]
    private Transform gameOverPanel;

    [SerializeField] 
    private Transform winPanel;
    [SerializeField]
    private Text winScore;
    [SerializeField]
    private Text winCoin;
    [SerializeField]
    private Image playerWinImg;

    [SerializeField]
    private Transform losePanel;
    [SerializeField]
    private Text loseScore;
    [SerializeField]
    private Text loseCoin;
    [SerializeField]
    private Image playerLoseImg;

    [SerializeField]
    private Transform gameOverlay;
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Transform gameArea;

    [SerializeField]
    private Transform wolfScore;
    [SerializeField]
    private Text wolfScoreText;
    [SerializeField]
    private Transform playerScore;
    [SerializeField]
    private Text playerScoreText;

    [SerializeField]
    private Transform healthContainer;
    [SerializeField]
    private Transform healthPrefab;

    [SerializeField]
    private Text coin;

    private void Start()
    {
        coin.text = GameManager.instance.playerData.GetGold().ToString();
        gameOverlay.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);  

        if(!LevelManager.instance.currentLevel.isSinglePlayer)
        {
            wolfScore.gameObject.SetActive(true);
            playerScore.gameObject.SetActive(true);
        }
        else
        {
            wolfScore.gameObject.SetActive(false);
            playerScore.gameObject.SetActive(true);
            SetSinglePlayerScore(0);
        }
    }

    public void SetWolfScore(int score)
    {
        wolfScoreText.text = score.ToString();
    }

    public void SetSinglePlayerScore(int score)
    {
        playerScoreText.text = score.ToString() + "/" + (LevelManager.instance.currentLevelColumns * LevelManager.instance.currentLevelRows)/2;
    }

    public void PlayGame()
    {
        gameOverlay.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        FaceDownAllCards();
    }

    public void GameOver()
    {
        gameOverlay.gameObject.SetActive(true);
        panelWindow.gameObject.SetActive(true);
        gameOverPanel.gameObject.SetActive(true);
    }

    public void SetGameOverSingle(Sprite playerImg, int coin, int score, int maxScore)
    {
        loseScore.text = score + "/" + maxScore;
        loseCoin.text = coin.ToString();
        playerLoseImg.sprite = playerImg;
    }

    public void WinGame()
    {
        gameOverlay.gameObject.SetActive(true);
        panelWindow.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
    }

    public void SetWinSinglePanel(Sprite playerImg, int coin, int score, int maxScore)
    {
        winScore.text = score + "/" + maxScore;
        winCoin.text = coin.ToString();
        playerWinImg.sprite = playerImg;
    }

    public void FaceDownAllCards()
    {
        foreach(Transform child in gameArea)
        {
            child.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void FaceDownCard(int index)
    {
        gameArea.GetChild(index).GetChild(0).gameObject.SetActive(true);
    }

    public void DestroyCard(int index)
    {
        gameArea.GetChild(index).GetComponent<Image>().color = new Color(0,0,0,0);
        gameArea.GetChild(index).GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void UpdateHealth(int currentHealth)
    {
        foreach(Transform child in healthContainer)
        {
            Destroy(child.gameObject);
        }
        for(int i = 0; i< 3; i++)
        {
            Transform health = Instantiate(healthPrefab, healthContainer);
            if(i > currentHealth - 1)
            {
                health.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void PauseGame()
    {
        gameOverlay.gameObject.SetActive(true);
        panelWindow.gameObject.SetActive(true);
        pausePanel.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        gameOverlay.gameObject.SetActive(false);
        panelWindow.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
    }
}
