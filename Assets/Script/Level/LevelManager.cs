using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public LevelsData levelsData;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public int currentLevelRows;
    public int currentLevelColumns;
    public Level currentLevel;
    public int currentLevelIndex;

    public void SetCurrentLevelPlayerPoint(int currentPoint)
    {
        int maxPoint = currentLevelRows * currentLevelColumns / 2;
        if(currentPoint < maxPoint && currentPoint > levelsData.GetSinglePlayerLevel(currentLevelIndex).playerPoints)
        {
            levelsData.SetSinglePlayerLevel(currentLevelIndex, true, false, currentPoint);
        }
        else if(currentPoint == maxPoint)
        {
            CompleteLevel(currentLevelIndex, maxPoint);
        }
    }

    public void CompleteLevel(int index, int maxPoint)
    {
        levelsData.SetSinglePlayerLevel(index, true, true, maxPoint);
    }

    public void LoadMultiPlayerLevel()
    {
        LevelUI.instance.Init(levelsData.GetMultiPlayerLevels());
    }

    public void LoadSinglePlayerLevel()
    {
        LevelUI.instance.Init(levelsData.GetSinglePlayerLevels());
    }
}
