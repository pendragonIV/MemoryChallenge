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

    public void LoadMultiPlayerLevel()
    {
        LevelUI.instance.Init(levelsData.GetMultiPlayerLevels());
    }

    public void LoadSinglePlayerLevel()
    {
        LevelUI.instance.Init(levelsData.GetSinglePlayerLevels());
    }
}
