using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteHolder;
    [SerializeField] CardSO cardData;
    // Start is called before the first frame update
    public void SetUP(CardSO _cardData)
    {
        cardData = _cardData;
        spriteHolder.sprite = cardData.GetSprite();
    }
    public CardSO.Name GetName()
    {
        return cardData.GetName();
    }
}
