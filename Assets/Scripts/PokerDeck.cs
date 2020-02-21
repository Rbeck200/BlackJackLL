using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerDeck : Deck
{
    private LinkedList<PokerCard> pokerDeck;

    private void ShufflePokerCards()
    {
        bool random = true;
        int randNum = UnityEngine.Random.Range(5, 11);
        int counter = 0;
        
        for (int i = 0; i < 100; i++)
        {
            LinkedList<PokerCard> temp = new LinkedList<PokerCard>();
            while (pokerDeck.First != null || pokerDeck.Last != null)
            {
                LinkedListNode<PokerCard> tempNode = pokerDeck.First;
                pokerDeck.RemoveFirst();
                temp.AddLast(tempNode);
                counter++;
                if (pokerDeck.Last != null)
                {
                    tempNode = pokerDeck.Last;
                    pokerDeck.RemoveLast();
                    temp.AddLast(tempNode);
                    counter++;
                    if ((pokerDeck.First != null || pokerDeck.Last != null) && counter % randNum == 0)
                    {
                        if (random)
                        {
                            random = false;
                            tempNode = pokerDeck.Last;
                            pokerDeck.RemoveLast();
                            temp.AddLast(tempNode);
                            
                            counter++; ;
                        }
                        else
                        {
                            random = true;
                            tempNode = pokerDeck.First;
                            pokerDeck.RemoveFirst();
                            temp.AddLast(tempNode);
                            
                            counter++;
                        }
                        randNum = UnityEngine.Random.Range(5, 11);
                    }
                    else
                    {
                        randNum = UnityEngine.Random.Range(5, 11);
                    }
                }
            }
            pokerDeck.Clear();
            pokerDeck = temp;
        }
    }
    public PokerDeck(Sprite[] cardFaces, Sprite cardBack, int numDecks) : base(cardFaces, cardBack, numDecks)
    {
        pokerDeck = new LinkedList<PokerCard>();
        while (numDecks-- > 0)
        {
            foreach (Sprite card in cardFaces)
            {
                pokerDeck.AddLast(new PokerCard(card, cardBack));
            }
        }
        ShufflePokerCards();
    }

    public PokerCard DrawPokerCardFirst()
    {
        PokerCard chosen = pokerDeck.First.Value;
        pokerDeck.RemoveFirst();
        return chosen;
    }

    public PokerCard DrawPokerCardLast()
    {
        PokerCard chosen = pokerDeck.Last.Value;
        pokerDeck.RemoveLast();
        return chosen;
    }


}
