using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteHolder;
    [SerializeField] SpriteRenderer shadowCover;
    [SerializeField] CardSO cardData;
    [SerializeField] Transform[] fourCorners;
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


    public void AskBelowToCheck()
    {
        //Debug.Log(GetName()+" is asking below to check");
        foreach (Transform corner in fourCorners)
        {
            RaycastHit2D hit = Physics2D.Raycast(corner.position, corner.forward, Mathf.Infinity);//0.3 a layer
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<Card>(out Card card))
                {
                    card.CheckForRemovingCover();
                }
            }
        }
    }


    public void CheckForRemovingCover()
    {
        if (!IsCovered())
        {
            PealOffCover();
        }
    }
    public bool IsCovered()
    {
        foreach (Transform corner in fourCorners)
        {
            RaycastHit2D hit = Physics2D.Raycast(corner.position, -corner.forward, 3);//0.3 a layer
            if (hit.collider!=null)
            {
                if (hit.collider.TryGetComponent<Card>(out Card card))
                {
                    if (card!=this)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public void PealOffCover()
    {
        shadowCover.enabled = false;
    }
}
