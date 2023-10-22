using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartInit : MonoBehaviour
{
    [SerializeField]
    private Transform cartContainer;
    [SerializeField]
    private GameObject cartPrefab;
    [SerializeField]
    private CardsData cardsData;

    private float cartSpacing = 10f;

    [SerializeField]
    private List<Card> tmpCards;
    [SerializeField]
    private List<Card> usedCards;

    [SerializeField]
    private List<GameObject> initializedCard;

    private void Start()
    {   
        tmpCards = new List<Card>();
        usedCards = new List<Card>();
        initializedCard = new List<GameObject>();

        cardsData.GetCards().ForEach(x => tmpCards.Add(x));
    }

    public void InitializeCard(int cols, int rows)
    {
        SetCartSize();

        for (int i = 0; i < (cols * rows); i++)
        {
            GameObject cart = Instantiate(cartPrefab, cartContainer);
            cart.name = i.ToString();
            initializedCard.Add(cart);
        }

        for (int i = 0; i < (cols * rows) / 2; i++)
        {
            usedCards.Add(GetCard());
        }
    }

    public void SetCard()
    {
        foreach (var item in usedCards)
        {
            SetCardData(item);
            SetCardData(item);
        }
    }

    private void SetCardData(Card data)
    {
        int randomIndex = Random.Range(0, initializedCard.Count);
        GameObject tmpCard = initializedCard[randomIndex];

        tmpCard.GetComponent<Image>().sprite = data.sprite;
        GameManager.instance.ingameCards.Add(tmpCard.name, data.id);
        initializedCard.RemoveAt(randomIndex);
    }

    private Card GetCard()
    {
        Card tempData;

        int cartIndex = Random.Range(0, tmpCards.Count);
        tempData = tmpCards[cartIndex];
        tmpCards.RemoveAt(cartIndex);

        return tempData;
    }

    private void SetCartSize()
    {
        GridLayoutGroup gridLayoutGroup = cartContainer.GetComponent<GridLayoutGroup>();
        RectTransform rectTransform = cartContainer.GetComponent<RectTransform>();
        float width = rectTransform.rect.width;
        float cellWidth = (width - cartSpacing * (LevelManager.instance.currentLevelColumns + 1)) / LevelManager.instance.currentLevelColumns; //3->4 space, 4->5 space,... -> +1

        gridLayoutGroup.spacing = new Vector2(cartSpacing, cartSpacing);
        gridLayoutGroup.padding = new RectOffset(10, 10, 10, 10);

        gridLayoutGroup.cellSize = new Vector2(cellWidth,cellWidth);
    }
}
