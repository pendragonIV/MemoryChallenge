using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "ScriptableObjects/LevelsData", order = 1)]
[Serializable]
public class LevelsData :ScriptableObject
{
    [SerializeField]
    private List<Level> singlePlayerLevels;
    [SerializeField]
    private List<Level> multiPlayerLevels;

    public void SetSinglePlayerLevel(int index, bool isPlayable, bool isCompleted, int playerPoint)
    {
        singlePlayerLevels[index] = singlePlayerLevels[index].ChangeLevelIn4(isPlayable, isCompleted, 0, playerPoint);
    }

    public int IndexOfSinglePlayerLevel(Level level)
    {
        return singlePlayerLevels.IndexOf(level);
    }

    public Level GetSinglePlayerLevel(int index)
    {
        return singlePlayerLevels[index];
    }

    public List<Level> GetSinglePlayerLevels()
    {
        return singlePlayerLevels;
    }

    #region MultiPlayer

    public void SetMultiPlayerLevel(int index, bool isPlayable, bool isCompleted, int enemyPoint, int playerPoint)
    {
        multiPlayerLevels[index] = multiPlayerLevels[index].ChangeLevelIn4(isPlayable, isCompleted, enemyPoint, playerPoint);
    }

    public int IndexOfMultiPlayerLevel(Level level)
    {
        return multiPlayerLevels.IndexOf(level);
    }

    public Level GetMultiPlayerLevel(int index)
    {
        return multiPlayerLevels[index];
    }

    public List<Level> GetMultiPlayerLevels()
    {
        return multiPlayerLevels;
    }
    #endregion

    #region Save and Load
    public void SaveDataJSON()
    {
        string singlePlayerLevelsData = JsonHelper.ToJson(singlePlayerLevels.ToArray(), true);
        string multiPlayerLevelsData = JsonHelper.ToJson(multiPlayerLevels.ToArray(), true);

        WriteFile(singlePlayerLevelsData, "/SingleLevels.json");
        WriteFile(multiPlayerLevelsData, "/MultiLevels.json");
    }

    public void LoadDataJSON()
    {
        string singlePlayerLevelsData = ReadFile("/SingleLevels.json");
        string multiPlayerLevelsData = ReadFile("/MultiLevels.json");
        
        this.singlePlayerLevels = new List<Level>(JsonHelper.FromJson<Level>(singlePlayerLevelsData).ToList());
        this.multiPlayerLevels = new List<Level>(JsonHelper.FromJson<Level>(multiPlayerLevelsData).ToList());
    }

    private void WriteFile(string content, string path)
    {
        FileStream file = new FileStream(Application.persistentDataPath + path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(file))
        {
            writer.Write(content);
        }
    }

    private string ReadFile(string path)
    {
        if (File.Exists(Application.persistentDataPath + path))
        {
            FileStream file = new FileStream(Application.persistentDataPath + path, FileMode.Open);

            using (StreamReader reader = new StreamReader(file))
            {
                return reader.ReadToEnd();
            }
        }
        else
        {
            return null;
        }
    }
    #endregion
}

[Serializable]
public struct Level
{
    public bool isSinglePlayer;
    public bool isPlayable;
    public bool isCompleted;

    public int rows;
    public int columns;

    public int enemyPoints;
    public int playerPoints;

    public Level ChangeLevelIn4(bool isPlayable, bool isCompleted, int enemyPoint, int playerPoint)
    {
        return new Level
        {
            isSinglePlayer = this.isSinglePlayer,
            isPlayable = isPlayable,
            isCompleted = isCompleted,

            rows = this.rows,
            columns = this.columns,

            enemyPoints = enemyPoint,
            playerPoints = playerPoint
        };
    }
}


public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

