using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card //: MonoBehaviour
{
    private int point;
    private GameObject card;
    private Sprite cardFace;
    
    public int Point { get { return this.point; } }
    public GameObject Face { get { return this.card; } }
    public Sprite cardImage { get {return this.cardFace; } }
    public string Name;
    public bool isFaceUp = true;

    public Card(Sprite CardFace)
    {
        cardFace = CardFace;
        Name = CardFace.name;
        int point;
        switch (Name.Substring(Name.Length - 1))
        {
            // ace
            case "e":
                point = 11;
                break;
            case "k":// jacK
            case "g": // kinG
            case "n": // queeN
            case "0": // 10
                point = 10;
                break;
            default:
                // other remaining possible cards, 2 - 9
                point = Convert.ToInt16(Name.Substring(Name.Length - 1));
                break;
        }
        this.point = point;
    }

    public void makeCard(GameObject prefab)
    {
        card = prefab;
        card.GetComponent<Image>().sprite = cardFace;
    }

}