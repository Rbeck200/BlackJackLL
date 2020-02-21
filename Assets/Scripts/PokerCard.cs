using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerCard : Card
{
    private string cardsuit;

    public string Suit { get {return cardsuit; } }

    public PokerCard(Sprite CardFace) : base(CardFace)
    {

    }
}

