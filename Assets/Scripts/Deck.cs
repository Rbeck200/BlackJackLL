using System;
using System.Collections.Generic;
using UnityEngine;
public class Deck //: MonoBehaviour
{
    private List<PokerCard> deck;
    private void ShuffleCards()
    {
        for (int i = 0; i < (deck.Count * 1000); i++)
        {
            int index1 = UnityEngine.Random.Range(0, deck.Count - 1);
            int index2 = UnityEngine.Random.Range(0, deck.Count - 1);
            if (index1 == index2)
            {
                index2 = UnityEngine.Random.Range(0, deck.Count - 1);
            }
            PokerCard temp = deck[index1];
            deck[index1] = deck[index2];
            deck[index2] = temp;
        }
    }

    public Deck(Sprite[] cardFaces ,int numDecks)
    {
        deck = new List<PokerCard>();
        while (numDecks-- > 0)
        {
            foreach (Sprite card in cardFaces)
            {
                deck.Add(new PokerCard(card));
            }
        }
        ShuffleCards();
    }

    public Card DrawCard() {
        Card chosen = deck[deck.Count - 1];
        deck.RemoveAt(deck.Count - 1);
        return chosen;
    }
}