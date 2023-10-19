using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField]
    private int gold;
    [SerializeField]
    private List<Animal> animals;
    [SerializeField]
    private int currentAnimalIndex;

    public void SetGold(int gold)
    {
        this.gold = gold;
    }

    public int GetGold()
    {
        return gold;
    }

    public void SetCurrentAnimalIndex(int index)
    {
        currentAnimalIndex = index;
    }

    public int GetCurrentAnimalIndex()
    {
        return currentAnimalIndex;
    }

    public List<Animal> GetAnimals()
    {
        return animals;
    }

    public Animal GetAnimalAt(int index)
    {
        return animals[index];
    }

    public void SetAnimalStatus(int index, bool isBought, bool isUsing)
    {
        animals[index] = animals[index].ChangeAnimalStatus(isBought, isUsing);
    }


    #region Save and Load
    public void SaveDataJSON()
    {
        string animalsData = JsonHelper.ToJson(animals.ToArray(), true);

        string content = "{\"gold\":" + gold + ",\"animals\":" + animalsData + ",\"currentAnimalIndex\":" + currentAnimalIndex + "}";
        WriteFile(content);
    }

    public void LoadDataJSON()
    {
        string content = ReadFile();
        if (content != null)
        {
            // = new List<Level>(JsonHelper.FromJson<Level>(content).ToList());
            Debug.Log(JsonHelper.FromJson<Level>(content));
        }
    }

    private void WriteFile(string content)
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/PlayerData.json", FileMode.Create);

        using (StreamWriter writer = new StreamWriter(file))
        {
            writer.Write(content);
        }
    }

    private string ReadFile()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.json"))
        {
            FileStream file = new FileStream(Application.persistentDataPath + "/PlayerData.json", FileMode.Open);

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

public enum AnimalType
{
    Tiger,
    Pig,
    Otter,
    Crocodile,
    Chick,
    Chicken,
}

[Serializable]
public struct Animal
{
    public AnimalType animalType;
    public Sprite animalImage;
    public int price;
    public bool isBought;
    public bool isUsing;

    public Animal ChangeAnimalStatus(bool isBought, bool isUsing)
    {
        return new Animal()
        {
            animalType = this.animalType,
            price = this.price,
            animalImage = this.animalImage,
            isBought = isBought,
            isUsing = isUsing
        };
    }
}