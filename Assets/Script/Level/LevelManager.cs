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

    public LevelUI levelUI;

    public int currentLevelRows;
    public int currentLevelColumns;

    public void LoadMultiPlayerLevel()
    {
        levelUI.Init(levelsData.GetMultiPlayerLevels());
    }

    public void LoadSinglePlayerLevel()
    {
        levelUI.Init(levelsData.GetSinglePlayerLevels());
    }
}
