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
    public Dictionary<string,int> ingameCards;

    private void Start()
    {
        ingameCards = new Dictionary<string, int>();

        cartInit = gameObject.GetComponent<CartInit>();
        cartInit.InitializeCard(LevelManager.instance.currentLevelColumns, LevelManager.instance.currentLevelRows);

        cartInit.SetCard();
    }
}
