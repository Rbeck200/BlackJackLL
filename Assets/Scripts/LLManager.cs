using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LLManager : MonoBehaviour
{

	[SerializeField]
	private Sprite[] CardFace;
	[SerializeField]
	private Sprite CardBack;
	[SerializeField]
	private GameObject[] playerCardPosition;
	[SerializeField]
	private GameObject cardBlank;
	[SerializeField]
	private Button drawTopBtn, drawBttmBtn, addTopBtn, addBttmBtn, shuffleBtn, mergeBtn, resetBtn;

	private LinkedList<PokerCard> playerCards;
	private int playerCardPointer = 0;
	private int childPointer = 0;
	private PokerDeck playingDeck;

	private void Start()
	{
		playingDeck = new PokerDeck(CardFace, CardBack, 5);
		playerCards = new LinkedList<PokerCard>();
		drawTopBtn.onClick.AddListener(delegate
		{
			playerDrawTop();
		});

		drawBttmBtn.onClick.AddListener(delegate
		{
			playerDrawBottom();
		});

		addTopBtn.onClick.AddListener(delegate
		{
			addTopDeck();
		});

		addBttmBtn.onClick.AddListener(delegate
		{
			addBttmDeck();
		});

		shuffleBtn.onClick.AddListener(delegate
		{
			playingDeck.ShufflePokerCards();
		});

		mergeBtn.onClick.AddListener(delegate
		{
			PokerDeck playingDeckTemp = new PokerDeck(CardFace, CardBack, 5);
			playingDeck.Merge(playingDeck, playingDeckTemp);
		});

	}

	public void playerDrawTop()
	{
		PokerCard card = playingDeck.DrawPokerCardTop();
		card.makeCard(cardBlank);
		playerCards.AddFirst(card);
		Instantiate(card.Face, playerCardPosition[playerCardPointer++].transform);
		childPointer++;
	}

	public void playerDrawBottom()
	{
		PokerCard card = playingDeck.DrawPokerCardBottom();
		card.makeCard(cardBlank);
		playerCards.AddFirst(card);
		Instantiate(card.Face, playerCardPosition[playerCardPointer++].transform);
		childPointer++;
	}

	public void addBttmDeck()
	{
		PokerCard card = playerCards.First.Value;
		playerCards.RemoveFirst();
		playingDeck.AddBottom(card);
		Destroy(playerCardPosition[--playerCardPointer].transform.GetChild(0).gameObject);
	}

	public void addTopDeck()
	{
		PokerCard card = playerCards.First.Value;
		playerCards.RemoveFirst();
		playingDeck.AddBottom(card);
		Destroy(playerCardPosition[--playerCardPointer].transform.GetChild(0).gameObject);
	}

}
