using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public Transform currentMode;
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
