using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cart : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!GameManager.instance.isFirstGuess)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            GameManager.instance.isFirstGuess = true;
            GameManager.instance.firstCardID = GameManager.instance.ingameCards[this.name];
            GameManager.instance.firstCardIndex = int.Parse(this.name);
        }
        else if (!GameManager.instance.isSecondGuess)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            GameManager.instance.isSecondGuess = true;
            GameManager.instance.secondCardID = GameManager.instance.ingameCards[this.name];
            GameManager.instance.secondCardIndex = int.Parse(this.name);

            GameManager.instance.CheckIfCardMatch();
        }
    }
}
