using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform panelWindow;
    [SerializeField]
    private Transform pausePanel;
    [SerializeField]
    private Transform gameOverPanel;

    #region Win and Lose Panel
    [SerializeField] 
    private Transform winPanel;
    [SerializeField]
    private Text winScore;
    [SerializeField]
    private Text winCoin;
    [SerializeField]
    private Image playerWinImg;
    [SerializeField]
    private Text winPanelText;

    [SerializeField]
    private Transform losePanel;
    [SerializeField]
    private Text loseScore;
    [SerializeField]
    private Text loseCoin;
    [SerializeField]
    private Image playerLoseImg;

    #endregion

    [SerializeField]
    private Transform gameOverlay;
    [SerializeField]
    private Button playButton;

    [SerializeField]
    public Transform gameArea;

    #region Score
    [SerializeField]
    private Transform wolfScore;
    [SerializeField]
    private Text wolfScoreText;
    [SerializeField]
    private Image wolfImg;
    [SerializeField]
    private Image playerImg;
    [SerializeField]
    private Transform playerScore;
    [SerializeField]
    private Text playerScoreText;
    #endregion

    [SerializeField]
    private Text coin;
    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color disabledColor;
   

    private void Start()
    {
        coin.text = GameManager.instance.playerData.GetGold().ToString();
        playerImg.sprite = GameManager.instance.playerData.GetAnimalAt(GameManager.instance.playerData.GetCurrentAnimalIndex()).animalImage;
        gameOverlay.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);  

        if(!LevelManager.instance.currentLevel.isSinglePlayer)
        {
            wolfScore.gameObject.SetActive(true);
            playerScore.gameObject.SetActive(true);
            SetWolfScore(0);
            SetPlayerScore(0);
        }
        else
        {
            wolfScore.gameObject.SetActive(false);
            playerScore.gameObject.SetActive(true);
            SetPlayerScore(0);
        }
    }

    public void SetWolfScore(int score)
    {
        wolfScoreText.text = score.ToString();
    }

    public void SetPlayerScore(int score)
    {
        if (LevelManager.instance.currentLevel.isSinglePlayer)
        {
            playerScoreText.text = score.ToString() + "/" + (LevelManager.instance.currentLevelColumns * LevelManager.instance.currentLevelRows) / 2;
        }
        else
        {
            playerScoreText.text = score.ToString();
        }
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

        FadeIn(panelWindow.GetComponent<CanvasGroup>(), panelWindow.GetComponent<RectTransform>());

        gameOverPanel.gameObject.SetActive(true);
    }

    public void WinGame()
    {
        gameOverlay.gameObject.SetActive(true);
        panelWindow.gameObject.SetActive(true);

        FadeIn(panelWindow.GetComponent<CanvasGroup>(), panelWindow.GetComponent<RectTransform>());

        winPanel.gameObject.SetActive(true);
        winPanelText.text = "You Win";
    }

    public void DrawGame()
    {
        gameOverlay.gameObject.SetActive(true);
        panelWindow.gameObject.SetActive(true);

        FadeIn(panelWindow.GetComponent<CanvasGroup>(), panelWindow.GetComponent<RectTransform>());

        winPanel.gameObject.SetActive(true);
        winPanelText.text = "Draw";
    }

    public void SetLosePanel(Sprite playerImg, int coin, int playerScore, int enemyScore)
    {
        loseScore.text = playerScore + " : " + enemyScore;
        loseCoin.text = coin.ToString();
        playerLoseImg.sprite = playerImg;
    }

    public void SetWinSinglePanel(Sprite playerImg, int coin, int score, int maxScore)
    {
        winScore.text = score + "/" + maxScore;
        winCoin.text = coin.ToString();
        playerWinImg.sprite = playerImg;
    }

    public void SetWinMultiPlayerPanel(Sprite playerImg, int coin, int playerScore, int enemyScore)
    {
        winScore.text = playerScore + " : " + enemyScore;
        winCoin.text = coin.ToString();
        playerWinImg.sprite = playerImg;
    }

    public void FaceDownAllCards()
    {
        foreach(Transform child in gameArea)
        {
            child.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(FlipDownCardAnimation(child, child.GetComponent<Animator>()));
        }
    }

    public void FaceDownCard(int index)
    {
        gameArea.GetChild(index).GetChild(0).gameObject.SetActive(true);
        StartCoroutine(FlipDownCardAnimation(gameArea.GetChild(index), gameArea.GetChild(index).GetComponent<Animator>()));
    }

    private IEnumerator FlipDownCardAnimation(Transform obj, Animator animator)
    {
        obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
        animator.enabled = true;
        animator.Play("Card");
        yield return new WaitForSecondsRealtime(1f);
        animator.enabled = false;
        obj.GetComponent<CanvasGroup>().blocksRaycasts = true;
        obj.GetChild(0).gameObject.SetActive(true);
    }

    public Transform FlipCard(int index)
    {
        gameArea.GetChild(index).GetComponent<CanvasGroup>().blocksRaycasts = false;
        return gameArea.GetChild(index);
    }

    public IEnumerator FlipCardAnimation(int index, Transform transform)
    {
        Animator animator = transform.GetComponent<Animator>();
        animator.enabled = true;
        animator.Play("CardReverse");
        yield return new WaitForSecondsRealtime(1f);
        gameArea.GetChild(index).transform.GetChild(0).gameObject.SetActive(false);
        animator.enabled = false;
    }

    public void DestroyCard(int index)
    {
        gameArea.GetChild(index).GetComponent<Image>().color = new Color(0,0,0,0);
        gameArea.GetChild(index).GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void PauseGame()
    {
        gameOverlay.gameObject.SetActive(true);
        panelWindow.gameObject.SetActive(true);

        FadeIn(panelWindow.GetComponent<CanvasGroup>(), panelWindow.GetComponent<RectTransform>());

        pausePanel.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {        
        gameOverlay.gameObject.SetActive(false);
        panelWindow.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
    }

    public void ChangeTurn(bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            wolfImg.color = disabledColor;
            playerImg.color = defaultColor;
        }
        else
        {
            wolfImg.color = defaultColor;
            playerImg.color = disabledColor;
        }
    }

    private void FadeIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition = new Vector3(0, -1000, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), 0.5f, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1, .5f);
    }

    private void FadeOut(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 1f;
        rectTransform.transform.localPosition = new Vector3(0, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(0, -1000), 0.5f, false).SetEase(Ease.InOutQuint);
        canvasGroup.DOFade(0, .5f);
    }
}
