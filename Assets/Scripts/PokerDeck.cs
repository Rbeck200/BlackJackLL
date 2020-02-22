using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerDeck : Deck
{
    private LinkedList<PokerCard> pokerDeck;

    private void ShufflePokerCards()
    {
        bool random = true; //constant
        int randNum = UnityEngine.Random.Range(5, 11); //constant
        int counter = 0; //constant

        for (int i = 0; i < 100; i++)
        {
            LinkedList<PokerCard> temp = new LinkedList<PokerCard>();
            while (pokerDeck.First != null || pokerDeck.Last != null)
            {
                LinkedListNode<PokerCard> tempNode = pokerDeck.First;
                pokerDeck.RemoveFirst(); //constant
                temp.AddLast(tempNode); //constant
                counter++; //constant
                if (pokerDeck.Last != null)
                {
                    tempNode = pokerDeck.Last;
                    pokerDeck.RemoveLast(); //constant
                    temp.AddLast(tempNode); //constant
                    counter++; //constant
                    if ((pokerDeck.First != null || pokerDeck.Last != null) && counter % randNum == 0)
                    {
                        if (random)
                        {
                            random = false; //constant
                            tempNode = pokerDeck.Last;
                            pokerDeck.RemoveLast(); //constant
                            temp.AddLast(tempNode); //constant
                            counter++;  //constant
                        }
                        else
                        {
                            random = true; //constant
                            tempNode = pokerDeck.First;
                            pokerDeck.RemoveFirst(); //constant
                            temp.AddLast(tempNode); //constant
                            counter++; //constant
                        }
                        randNum = UnityEngine.Random.Range(5, 11); //constant
                    }
                    else
                    {
                        randNum = UnityEngine.Random.Range(5, 11); //constant
                    }
                }
            }
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
