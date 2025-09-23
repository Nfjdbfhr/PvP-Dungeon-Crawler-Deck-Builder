using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Hand : MonoBehaviour
{
    public List<GameObject> CardsInHand = new();
    public Transform CardParent;
    public GameObject CardPrefab;

    // layout calculations
    public float Radius;
    public float AngularSpread;
    private float SpreadMod = 16.66f;
    public Vector3 Center = Vector3.zero;

    void Start()
    {
        ArrangeHand();
    }

    void Update()
    {
        for (int i = 0; i < CardsInHand.Count; i++)
        {
            if (CardsInHand[i] == null)
            {
                RemoveCardFromHand(i);
            }
        }
    }

    public void RemoveCardFromHand(Card card)
    {
        CardsInHand.Remove(card.gameObject);
        ArrangeHand();
    }
    public void RemoveCardFromHand(int index)
    {
        CardsInHand.RemoveAt(index);
        ArrangeHand();
    }
    public void RemoveCardFromHand(GameObject cardObj)
    {
        CardsInHand.Remove(cardObj);
        ArrangeHand();
    }

    // move the cards to make a fanned out hand
    public void ArrangeHand()
    {
        CardsInHand = CardsInHand.Where(card => card != null).ToList();

        if (CardsInHand.Count == 0)
            return;

        if (CardsInHand.Count == 1)
        {
            CardsInHand[0].transform.position = new Vector3(Center.x, Center.y - Radius, 0f);
            CardsInHand[0].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            return;
        }

        AngularSpread = (CardsInHand.Count / 2) * SpreadMod;

        float angleStep = AngularSpread / (CardsInHand.Count - 1);
        float startAngle = -AngularSpread / 2f;

        for (int i = 0; i < CardsInHand.Count; i++)
        {

            float angle = startAngle + (angleStep * i);
            float rad = angle * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(
                Center.x + Mathf.Sin(rad) * Radius,
                Center.y - Mathf.Cos(rad) * Radius,
                0f
            );

            // move and rotate card
            CardsInHand[i].transform.position = pos;
            CardsInHand[i].transform.rotation = Quaternion.Euler(0, 0, angle);

            // set rendering order so leftmost is behind, rightmost on top
            SpriteRenderer sr = CardsInHand[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = CardsInHand.Count - i;
            }
        }
    }

    public void AddCardToHand(ScriptableCard Card)
    {
        GameObject newCard = Instantiate(CardPrefab, new Vector3(0, Radius, 0), Quaternion.identity);
        newCard.transform.parent = CardParent;

        Card cardData = newCard.GetComponent<Card>();
        cardData.CardObj = Card;
        cardData.Start();

        CardsInHand.Add(newCard);

        ArrangeHand();
    }
}
