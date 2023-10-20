using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField]
    private List<string> flipableCards;
    public bool isFirstGuess;
    public bool isSecondGuess;

    public int firstCardIndex;
    public int secondCardIndex;

    public int firstCardID;
    public int secondCardID;

    public int currentScore = 0;

    public int playerScore = 0;
    public int enemyScore = 0;

    public bool isPlayerTurn = true;
    public bool isGameEnded = false;

    private void Start()
    {
        ingameCards = new Dictionary<string, int>();
        cartInit = gameObject.GetComponent<CartInit>();
        gameScene = gameObject.GetComponent<GameScene>();

        cartInit.InitializeCard(LevelManager.instance.currentLevelColumns, LevelManager.instance.currentLevelRows);

        if (!LevelManager.instance.currentLevel.isSinglePlayer)
        {
            LevelManager.instance.currentLevelIndex = LevelManager.instance.levelsData.IndexOfMultiPlayerLevel(LevelManager.instance.currentLevel);
        }
        else
        {
            LevelManager.instance.currentLevelIndex = LevelManager.instance.levelsData.IndexOfSinglePlayerLevel(LevelManager.instance.currentLevel);
        }

        cartInit.SetCard();

        StartCoroutine(EnemyFlipCard());
    }

    private void Update()
    {
        gameScene.ChangeTurn(isPlayerTurn);
    }

    public void CheckIfCardMatch()
    {
        StartCoroutine(Checking());
    }

    private IEnumerator Checking()
    {
        yield return new WaitForSeconds(1f);

        if (firstCardID == secondCardID)
        {
            gameScene.DestroyCard(firstCardIndex);
            gameScene.DestroyCard(secondCardIndex);

            currentScore++;

            if (LevelManager.instance.currentLevel.isSinglePlayer)
            {
                gameScene.SetPlayerScore(currentScore);
                LevelManager.instance.SetCurrentLevelPlayerPoint(currentScore, 0, 0);
            }
            else
            {
                UpScoreMultiMode();
            }

            ingameCards.Remove(firstCardIndex.ToString());
            ingameCards.Remove(secondCardIndex.ToString());

            CheckWin();
        }
        else
        {
            gameScene.FaceDownCard(firstCardIndex);
            gameScene.FaceDownCard(secondCardIndex);

            if (!LevelManager.instance.currentLevel.isSinglePlayer)
            {
                isPlayerTurn = !isPlayerTurn;
            }
        }

        isFirstGuess = false;
        isSecondGuess = false;
    }

    private void CheckWin()
    {
        if (currentScore == (LevelManager.instance.currentLevelColumns * LevelManager.instance.currentLevelRows) / 2)
        {
            isGameEnded = true;
            if (LevelManager.instance.currentLevel.isSinglePlayer)
            {
                playerData.SetGold(playerData.GetGold() + 10 * currentScore);
                gameScene.SetWinSinglePanel(playerData.GetAnimalAt(playerData.GetCurrentAnimalIndex()).animalImage, playerData.GetGold(), currentScore, currentScore);
                gameScene.WinGame();
                if(LevelManager.instance.currentLevelIndex < LevelManager.instance.levelsData.GetSinglePlayerLevels().Count - 1)
                {
                    if(!LevelManager.instance.levelsData.GetSinglePlayerLevel(LevelManager.instance.currentLevelIndex + 1).isPlayable)
                    {
                        LevelManager.instance.levelsData.SetSinglePlayerLevel(LevelManager.instance.currentLevelIndex + 1, true, false, 0);
                    }
                }
            }
            else
            {
                if(playerScore > enemyScore)
                {
                    playerData.SetGold(playerData.GetGold() + 10 * currentScore);
                    gameScene.SetWinMultiPlayerPanel(playerData.GetAnimalAt(playerData.GetCurrentAnimalIndex()).animalImage, playerData.GetGold(), playerScore, enemyScore);
                    gameScene.WinGame();
                    if (LevelManager.instance.currentLevelIndex < LevelManager.instance.levelsData.GetMultiPlayerLevels().Count - 1)
                    {
                        if (!LevelManager.instance.levelsData.GetMultiPlayerLevel(LevelManager.instance.currentLevelIndex + 1).isPlayable)
                        {
                            LevelManager.instance.levelsData.SetMultiPlayerLevel(LevelManager.instance.currentLevelIndex + 1, true, false, 0, 0);
                        }
                    }
                }
                else if(playerScore < enemyScore)
                {
                    gameScene.GameOver();
                    gameScene.SetLosePanel(playerData.GetAnimalAt(playerData.GetCurrentAnimalIndex()).animalLoseImg, playerData.GetGold(), playerScore, enemyScore);
                }
                else
                {
                    gameScene.DrawGame();
                    gameScene.SetWinMultiPlayerPanel(playerData.GetAnimalAt(playerData.GetCurrentAnimalIndex()).animalImage, playerData.GetGold(), playerScore, enemyScore);
                }
            }
        }
    }

    public void UpScoreMultiMode()
    {
        if (isPlayerTurn)
        {
            playerScore++;
            gameScene.SetPlayerScore(playerScore);
            LevelManager.instance.SetCurrentLevelPlayerPoint(currentScore, enemyScore, playerScore);
        }
        else
        {
            enemyScore++;
            gameScene.SetWolfScore(enemyScore);
            LevelManager.instance.SetCurrentLevelPlayerPoint(currentScore, enemyScore, playerScore);
        }
    }


    private IEnumerator EnemyFlipCard()
    {
        while (true)
        {
            yield return new WaitForSeconds(.1f);
            if (!LevelManager.instance.currentLevel.isSinglePlayer && !isGameEnded)
            {
                if (!isPlayerTurn)
                {
                    flipableCards = new List<string>();
                    flipableCards.AddRange(ingameCards.Keys);
                    int randomIndex;

                    if (!isFirstGuess)
                    {
                        yield return new WaitForSeconds(1f);
                        randomIndex = Random.Range(0, flipableCards.Count);
                        randomIndex = int.Parse(flipableCards[randomIndex]);

                        Transform firstCard = gameScene.FlipCard(randomIndex);
                        firstCardIndex = int.Parse(firstCard.name);
                        firstCardID = ingameCards[firstCardIndex.ToString()];
                        isFirstGuess = true;
                        yield return StartCoroutine(gameScene.FlipCardAnimation(firstCardIndex,firstCard));
                    }
                    else if (!isSecondGuess && isFirstGuess)
                    {
                        do
                        {
                            randomIndex = Random.Range(0, flipableCards.Count);
                            randomIndex = int.Parse(flipableCards[randomIndex]);
                        } while (randomIndex == firstCardIndex);

                        Transform secondCard = gameScene.FlipCard(randomIndex);
                        secondCardIndex = int.Parse(secondCard.name);
                        secondCardID = ingameCards[secondCardIndex.ToString()];
                        isSecondGuess = true;
                        yield return StartCoroutine(gameScene.FlipCardAnimation(secondCardIndex,secondCard));
                        yield return new WaitForSeconds(1f);
                        EnemyChecking();
                    }
                }
            }
            
        }
    }

    private void EnemyChecking()
    {
        if (firstCardID == secondCardID)
        {
            gameScene.DestroyCard(firstCardIndex);
            gameScene.DestroyCard(secondCardIndex);

            currentScore++;

            UpScoreMultiMode();

            ingameCards.Remove(firstCardIndex.ToString());
            ingameCards.Remove(secondCardIndex.ToString());

            CheckWin();
        }
        else
        {
            gameScene.FaceDownCard(firstCardIndex);
            gameScene.FaceDownCard(secondCardIndex);

            isPlayerTurn = !isPlayerTurn;
        }

        isFirstGuess = false;
        isSecondGuess = false;
    }

}
