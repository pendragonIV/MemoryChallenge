using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform panelWindow;
    [SerializeField]
    private Transform pausePanel;
    [SerializeField]
    private Transform gameOverlay;

    public void PauseGame()
    {
        gameOverlay.gameObject.SetActive(true);
        panelWindow.gameObject.SetActive(true);
        pausePanel.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        gameOverlay.gameObject.SetActive(false);
        panelWindow.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
    }
}
