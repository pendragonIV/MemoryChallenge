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
        WriteFile(animalsData, "/AnimalsData.json");

        string content = "{\"gold\":" + gold + ",\"currentAnimalIndex\":" + currentAnimalIndex + "}";
        WriteFile(content, "/PlayerData.json");
    }

    public void LoadDataJSON()
    {
        string content = ReadFile("/PlayerData.json");
        string animalsData = ReadFile("/AnimalsData.json");
        if (content != null)
        {
            JsonUtility.FromJsonOverwrite(content, this);
        }
        if(animalsData != null)
        {
            this.animals = new List<Animal>(JsonHelper.FromJson<Animal>(animalsData).ToList());
        }
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
    public Sprite animalLoseImg;
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
            animalLoseImg = this.animalLoseImg,
            isBought = isBought,
            isUsing = isUsing
        };
    }
}