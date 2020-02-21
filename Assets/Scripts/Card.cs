using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card //: MonoBehaviour
{   
    private Sprite cardFace;
    private Sprite cardBack;
    private GameObject card;

    public Sprite frontImage { get {return this.cardFace; } }
    public Sprite backImage { get { return this.cardBack; } }
    public GameObject Face { get { return this.card; } set {card = value; } }
    public string Name;
    public bool isFaceUp = true;

    public Card(Sprite CardFace, Sprite CardBack)
    {
        cardFace = CardFace;
        cardBack = CardBack;
        Name = CardFace.name;
    }

    public void makeCard(GameObject prefab)
    {
        card = prefab;
        if (isFaceUp)
        {
            card.GetComponent<Image>().sprite = frontImage;
        }
        else
        {
            card.GetComponent<Image>().sprite = backImage;
        }
    }

    public void setBack(GameObject prefab, Sprite CardFace)
    {
        card = prefab;
        card.GetComponent<Image>().sprite = CardFace;
    }
}