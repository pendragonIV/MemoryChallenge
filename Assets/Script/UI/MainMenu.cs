using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Transform currentMode;
    [SerializeField]
    private Text coinText;
    [SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private List<Image> playerImg;

    private void OnEnable()
    {
        SetCoin();
        SetPlayerImg();
    }

    private void SetCoin()
    {
        coinText.text = playerData.GetGold().ToString();
    }

    private void SetPlayerImg()
    {
        playerImg.ForEach(img =>
        {
            img.sprite = playerData.GetAnimalAt(playerData.GetCurrentAnimalIndex()).animalImage;
        });
    }

    public void LoadSinglePlayerLevel()
    {
        Tick();
        LevelManager.instance.LoadSinglePlayerLevel();
    }

    public void LoadMultiPlayerLevel()
    {
        Tick();
        LevelManager.instance.LoadMultiPlayerLevel();
    }

    private void Tick()
    {
        //Disable tick
        if(currentMode != null)
        {
            currentMode.GetChild(2).gameObject.SetActive(false);
        }
        //Tick selected mode
        currentMode = EventSystem.current.currentSelectedGameObject.transform;
        Transform tick = currentMode.GetChild(2);
        tick.gameObject.SetActive(true);
    }
}
