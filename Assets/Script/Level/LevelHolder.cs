using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelHolder : MonoBehaviour, IPointerClickHandler
{
    public Level levelData;
    public void OnPointerClick(PointerEventData eventData)
    {
        LevelManager.instance.currentLevelRows = levelData.rows;
        LevelManager.instance.currentLevelColumns = levelData.columns;
        LevelManager.instance.currentLevel = levelData;

        ChangeToGameScene();
    }

    public void ChangeToGameScene()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScene("GameScene"));
    }

    private IEnumerator ChangeScene(string sceneName)
    {
        //Optional: Add animation here

        yield return new WaitForSecondsRealtime(.4f);
        SceneManager.LoadScene(sceneName);
    }

}
