using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if(instance != null && instance != this) 
        {
            Destroy(this.gameObject);
        }else
        {
            instance = this;
        }
    }

    public CartInit cartInit;

    private void Start()
    {
        cartInit = gameObject.GetComponent<CartInit>();

        cartInit.InitializeCard(LevelManager.instance.currentLevelColumns, LevelManager.instance.currentLevelRows);
    }
}
