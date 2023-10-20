using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cart : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Animator animator;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.instance.isPlayerTurn)
        {
            if (!GameManager.instance.isFirstGuess)
            {
                this.transform.GetChild(0).gameObject.SetActive(false);
                GameManager.instance.isFirstGuess = true;
                GameManager.instance.firstCardID = GameManager.instance.ingameCards[this.name];
                GameManager.instance.firstCardIndex = int.Parse(this.name);
                this.GetComponent<CanvasGroup>().blocksRaycasts = false;
                StartCoroutine(FlipCardAnimation(this.transform));
                
            }
            else if (!GameManager.instance.isSecondGuess)
            {
                this.transform.GetChild(0).gameObject.SetActive(false);
                GameManager.instance.isSecondGuess = true;
                GameManager.instance.secondCardID = GameManager.instance.ingameCards[this.name];
                GameManager.instance.secondCardIndex = int.Parse(this.name);
                this.GetComponent<CanvasGroup>().blocksRaycasts = false;
                StartCoroutine (FlipCardAnimation(this.transform));   

                GameManager.instance.CheckIfCardMatch();
            }
        }
    }

    private IEnumerator FlipCardAnimation(Transform obj)
    {
        animator.enabled = true;
        animator.Play("CardReverse");   
        yield return new WaitForSecondsRealtime(1f);
        animator.enabled = false;
        obj.transform.GetChild(0).gameObject.SetActive(false);
    }
}
