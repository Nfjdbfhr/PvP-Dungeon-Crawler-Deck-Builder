using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Xml.Schema;

public class Deck : MonoBehaviour
{

    public List<ScriptableCard> Cards = new();
    public Hand PlayerHand;
    public CardBattleRunner Runner;

    public void Shuffle()
    {
        for (int i = Cards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            ScriptableCard temp = Cards[i];
            Cards[i] = Cards[randomIndex];
            Cards[randomIndex] = temp;
        }
    }

    public void DrawCard(int max, int min)
    {
        Runner.CardDragger.CanDrag = false;

        StartCoroutine(DrawXCards(Random.Range(min, max)));        
    }

    IEnumerator DrawXCards(int numOfDraws)
    {
        for (int i = 0; i < numOfDraws; i++)
        {
            if (Cards.Count <= 0)
                break;

            ScriptableCard card = Cards[0];
            Cards.RemoveAt(0);
            PlayerHand.AddCardToHand(card);

            yield return new WaitForSeconds(0.4f);
        }

        Runner.CardDragger.CanDrag = true;
    }
}
