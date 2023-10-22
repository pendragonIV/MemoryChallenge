using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterShop : MonoBehaviour
{
    [SerializeField]
    Button button;

    private void Start()
    { 
        button.onClick.AddListener(Selected);
    }

    public void Selected()
    {
        Animal selectedAnimal =  Shop.instance.playerData.GetAnimalAt(int.Parse(transform.name));
        if(selectedAnimal.isBought)
        {
            Shop.instance.playerData.SetAnimalStatus(Shop.instance.playerData.GetCurrentAnimalIndex(), true, false);
            Shop.instance.playerData.SetCurrentAnimalIndex(int.Parse(transform.name));
            Shop.instance.playerData.SetAnimalStatus(Shop.instance.playerData.GetCurrentAnimalIndex(), true, true);
            Shop.instance.playerData.SaveDataJSON();    
            Shop.instance.ShopInitialize();
        }
        else
        {
            Shop.instance.ShowPopup(int.Parse(transform.name));
            Shop.instance.SetCoin();
            Shop.instance.Buy(int.Parse(transform.name));
            Shop.instance.ShopInitialize();
        }
    }
}
