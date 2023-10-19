using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardsData", menuName = "ScriptableObjects/CardsData", order = 1)]
public class CardsData : ScriptableObject
{
    [SerializeField]
    private List<Card> cards;
    [SerializeField]
    private Sprite defaultCardImg;

    public List<Card> GetCards()
    {
        return cards;
    }

    public Card GetCardAt(int index)
    {
        return cards[index];
    }   

    public int NumberOfCards()
    {
        return cards.Count;
    }
}

[Serializable]
public struct Card
{
    public int id;
    public Sprite sprite;
}