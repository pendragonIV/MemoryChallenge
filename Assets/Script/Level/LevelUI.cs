using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField]
    private Transform levelContainer;
    [SerializeField]
    private Transform levelPrefab;

    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color playerColor;
    [SerializeField]
    private Color enemyColor;

    public static LevelUI instance;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void Init(List<Level> levels)
    {
        // Clear all level buttons in level container (bc we have 2 modes: single player and multiplayer)
        foreach (Transform child in levelContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < levels.Count; i++)
        {
            Transform level = Instantiate(levelPrefab, levelContainer);
            SetLevelIn4(level,levels[i]);
        }
    }

    private void SetLevelIn4(Transform level, Level levelData)
    {
        //Level name
        level.GetChild(0).GetComponent<Text>().text = "Level " + levelData.columns + "x" + levelData.rows;

        SetPoint(level, levelData);

        SetPlayableLevel(level, levelData);

        //Set data for holder
        level.GetComponent<LevelHolder>().levelData = levelData;
    }

    private void SetPlayableLevel(Transform level, Level levelData)
    {
        // Child 2 is Filter
        // Set UI for Playable level
        if (levelData.isPlayable)
        {
            level.GetComponent<CanvasGroup>().blocksRaycasts = true;
            level.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            level.GetComponent<CanvasGroup>().blocksRaycasts = false;
            level.GetChild(2).gameObject.SetActive(true);
        }
    }

    private void SetPoint(Transform level, Level levelData)
    {
        if (levelData.isSinglePlayer)
        {
            var scoreContainer = level.GetChild(1);

            var playerScoreText = scoreContainer.GetChild(0).GetComponent<Text>();
            playerScoreText.text = levelData.playerPoints.ToString();
            playerScoreText.color = defaultColor;

            var maxPoint = scoreContainer.GetChild(1).GetComponent<Text>();
            maxPoint.text = ((levelData.rows * levelData.columns) / 2).ToString(); // 2 car will be 1 point
            maxPoint.color = defaultColor;
            
            var breaker = scoreContainer.GetChild(2).GetComponent<Text>();
            breaker.text = "/";
        }
        else
        {
            var scoreContainer = level.GetChild(1);

            var playerScoreText = scoreContainer.GetChild(0).GetComponent<Text>();
            playerScoreText.text = levelData.playerPoints.ToString();

            var enemyScoreText = scoreContainer.GetChild(1).GetComponent<Text>();
            enemyScoreText.text = levelData.enemyPoints.ToString();
        }
    }
}
