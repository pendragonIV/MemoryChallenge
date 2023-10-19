using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if(instance != null && instance != this) 
        {
            Destroy(this.gameObject);
        }else
        {
            instance = this;
        }
    }

    public PlayerData playerData;
    public CartInit cartInit;
    public GameScene gameScene;   
    public Dictionary<string,int> ingameCards;

    public bool isFirstGuess;
    public bool isSecondGuess;

    public int firstCardIndex;
    public int secondCardIndex;

    public int firstCardID;
    public int secondCardID;

    public int currentScore = 0;

    public int currentHealth = 3;

    private void Start()
    {
        ingameCards = new Dictionary<string, int>();
        cartInit = gameObject.GetComponent<CartInit>();
        gameScene = gameObject.GetComponent<GameScene>();

        cartInit.InitializeCard(LevelManager.instance.currentLevelColumns, LevelManager.instance.currentLevelRows);

        LevelManager.instance.currentLevelIndex = LevelManager.instance.levelsData.IndexOfSinglePlayerLevel(LevelManager.instance.currentLevel);

        gameScene.UpdateHealth(currentHealth);

        cartInit.SetCard();
    }


    public void CheckIfCardMatch()
    {
        StartCoroutine(Checking());
    }

    private IEnumerator Checking()
    {
        yield return new WaitForSeconds(1.5f);

        if (firstCardID == secondCardID)
        {
            gameScene.DestroyCard(firstCardIndex);
            gameScene.DestroyCard(secondCardIndex);

            currentScore++;
            gameScene.SetSinglePlayerScore(currentScore);
            LevelManager.instance.SetCurrentLevelPlayerPoint(currentScore);

            ingameCards.Remove(firstCardIndex.ToString());
            ingameCards.Remove(secondCardIndex.ToString());

            if(currentScore == (LevelManager.instance.currentLevelColumns * LevelManager.instance.currentLevelRows) / 2)
            {
                gameScene.WinGame();
                playerData.SetGold(playerData.GetGold() + 10 * currentScore);
                gameScene.SetWinSinglePanel(playerData.GetAnimalAt(playerData.GetCurrentAnimalIndex()).animalImage, playerData.GetGold(), currentScore, currentScore);
            }
        }
        else
        {
            gameScene.FaceDownCard(firstCardIndex);
            gameScene.FaceDownCard(secondCardIndex);
            currentHealth--;
            gameScene.UpdateHealth(currentHealth);
            if(currentHealth == 0)
            {
                gameScene.GameOver();
                gameScene.SetGameOverSingle(playerData.GetAnimalAt(playerData.GetCurrentAnimalIndex()).animalLoseImg, playerData.GetGold(), currentScore, (LevelManager.instance.currentLevelColumns * LevelManager.instance.currentLevelRows) / 2);
            }
        }

        isFirstGuess = false;
        isSecondGuess = false;
    }
}
