using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private Sprite[] CardFace;
	[SerializeField]
	private Sprite CardBack;
	[SerializeField]
	private GameObject[] playerCardPosition, dealerCardPosition;
	[SerializeField]
	private GameObject cardBlank;
	[SerializeField]
	private Button mainBtn, standBtn, resetBalanceBtn, resetGameBtn, addBtn, subBtn, playBtn;
	[SerializeField]
	private Slider betSlider;
	[SerializeField]
	private Text moneyTxt, betTxt, playerPointsTxt, dealerPointsTxt, placeBetTxt, selectBetTxt, winTxt, numDecksTxt;
	[SerializeField]
	private Image resetImgBtn;

	private LinkedList<PokerCard> playerCards;
	private LinkedList<PokerCard> dealerCards;
	private bool playing;
	private int playerPoints;
	private int dealerPoints, displayDealerPoints;
	private int playerMoney;
	private int currentBet;
	private int playerCardPointer, dealerCardPointer;
	private int numDecks;
	private string cardImage;
	private string BackOfCard;
	private PokerDeck playingDeck;

	private void Setup()
	{
		numDecks++;
		numDecksTxt.text = "You are playing with " + numDecks + " decks.";

		addBtn.onClick.AddListener(delegate
		{
			numDecks++;
			numDecksTxt.text = "You are playing with " + numDecks + " decks.";
		});
		subBtn.onClick.AddListener(delegate
		{
			if (numDecks > 1){
				numDecks--;
				numDecksTxt.text = "You are playing with " + numDecks + " decks.";
			}else{
				numDecksTxt.text = "You are playing with " + numDecks + " decks.";
			}
		});
		playBtn.GetComponent<Button>().onClick.AddListener(delegate
		{
			addBtn.gameObject.SetActive(false);
			subBtn.gameObject.SetActive(false);
			numDecksTxt.gameObject.SetActive(false);
			playBtn.gameObject.SetActive(false);
			gameReset();
		});
	}
	
	private void Start() {
		Setup();
		playerMoney = 1000;
		currentBet = 50;
		resetBalanceBtn.gameObject.SetActive(false);

		mainBtn.onClick.AddListener(delegate {
			if (playing) {
				
				playerDraw();
			} else {
				
				gameStart();
			}
		});

		standBtn.onClick.AddListener(delegate {
			playerEndTurn();
		});

		betSlider.onValueChanged.AddListener(delegate {
			updateCurrentBet();
		});
		
		resetBalanceBtn.onClick.AddListener(delegate {
			playerMoney = 1000;
			betSlider.maxValue = playerMoney;
		});

		resetGameBtn.onClick.AddListener(delegate
		{
			currentBet = 0;
			selectBetTxt.text = "$" + currentBet.ToString();
		});
	}
	
	private void Update() {
		moneyTxt.text = "Money: $" + playerMoney.ToString();
	}

	public void gameStart() {
		if (playerMoney > 0)
		{
			playerMoney -= currentBet;
			if (playerMoney < 0) {
				playerMoney += currentBet;
				betSlider.maxValue = playerMoney;
				return;
				
			}

			playing = true;
			;
			// Update UI accordingly
			betSlider.gameObject.SetActive(false);
			selectBetTxt.gameObject.SetActive(false);
			placeBetTxt.gameObject.SetActive(false);
			mainBtn.GetComponentInChildren<Text>().text = "Hit";
			standBtn.gameObject.SetActive(true);
			betTxt.text = "Bet: $" + currentBet.ToString();
			resetBalanceBtn.gameObject.SetActive(false);
			resetGameBtn.gameObject.SetActive(false);

			// assign the playing deck with 2 deck of cards
			//playingDeck = new Deck(cardFaces, numDecks);
			playingDeck = new PokerDeck(CardFace, CardBack, numDecks);


			// draw 2 cards for player and dealer
			dealerDraw();
			
			playerDraw();
			
			dealerDraw();
			
			playerDraw();
			
			updatePlayerPoints();
			
			updateDealerPoints(true);
			
			checkIfPlayerBlackjack();
			
		}
	}

	private void checkIfPlayerBlackjack()
	{
		if (playerPoints == 21)
		{
			playerBlackjack();
		}
	}

	public void gameEnd() {
		mainBtn.gameObject.SetActive(false);
		standBtn.gameObject.SetActive(false);
		betSlider.gameObject.SetActive(false);
		placeBetTxt.text = "";
		selectBetTxt.text = "";

		resetImgBtn.gameObject.SetActive(true);
		resetImgBtn.GetComponent<Button>().onClick.AddListener(delegate {
			gameReset();
		});
	}

	public void dealerDraw() {
		Debug.Log("Starts Dealer Draw");
		PokerCard card = playingDeck.DrawPokerCardFirst();
		
		GameObject cardFace;
		
		dealerCards.AddFirst(card);
	
		if (dealerCardPointer <= 0) {
			Debug.Log("Dealer Draws First Card");
			card.isFaceUp = false;
			card.makeCard(cardBlank);
		} else {
			Debug.Log("Dealer Draws Not First Card");
			card.makeCard(cardBlank);
		}
		cardFace = card.Face;
		Instantiate(cardFace, dealerCardPosition[dealerCardPointer++].transform);
		updateDealerPoints(false);
	}

	public void playerDraw() {
		PokerCard card = playingDeck.DrawPokerCardFirst();
		card.makeCard(cardBlank);
		playerCards.AddFirst(card);
		Instantiate(card.Face, playerCardPosition[playerCardPointer++].transform);
		updatePlayerPoints();
		if (playerPoints > 21)
			playerBusted();
	}

	private void playerEndTurn() {
		Debug.Log("Player End Turn");
		revealDealersCards();
		// dealer start drawing
		while (dealerPoints < 17 && dealerPoints < playerPoints) {
			Debug.Log("Dealer Draw Card");
			dealerDraw();
			
		}
		updateDealerPoints(false);
		if (dealerPoints > 21)
		{
			Debug.Log("Player Wins Dealer Busted");
			dealerBusted();
		}
		else if (dealerPoints > playerPoints)
		{
			Debug.Log("Dealer Wins Player Loses");
			dealerWin(false);
		}
		else if (dealerPoints == playerPoints)
		{
			Debug.Log("Draw");
			gameDraw();
		}
		else
		{
			Debug.Log("Player wins Dealer Loses");
			playerWin(false);
		}
	}

	private void revealDealersCards() {
		// reveal the dealer's down-facing card
		Destroy(dealerCardPosition[0].transform.GetChild(0).gameObject);
		dealerCards.Last.Value.isFaceUp = true;
		dealerCards.Last.Value.makeCard(cardBlank);
		Instantiate(dealerCards.Last.Value.Face, dealerCardPosition[0].transform);	//use last because the first card put in is at the end
	}

	private void updatePlayerPoints() {
		playerPoints = 0;
		foreach(PokerCard card in playerCards) {
			playerPoints += card.Point;
		}

		// transform ace to 1 if there is any
		if (playerPoints > 21)
		{
			playerPoints = 0;
			foreach(PokerCard card in playerCards) {
				if (card.Point == 11)
					playerPoints += 1;
				else
					playerPoints += card.Point;
			}
		}

		playerPointsTxt.text = playerPoints.ToString();
	}

	private void updateDealerPoints(bool hideFirstCard) {
		Debug.Log("Start Dealer Points Update");
		dealerPoints = 0;
		foreach(PokerCard card in dealerCards) {
			dealerPoints += card.Point;
		}

		// transform ace to 1 if there is any
		if (dealerPoints > 21)
		{
			dealerPoints = 0;
			foreach(PokerCard card in dealerCards) {
				if (card.Point == 11)
					dealerPoints += 1;
				else
					dealerPoints += card.Point;
			}
		}

		if (hideFirstCard)
			displayDealerPoints = dealerCards.Last.Previous.Value.Point;
		else
			displayDealerPoints = dealerPoints;
		dealerPointsTxt.text = displayDealerPoints.ToString();
	}

	private void updateCurrentBet() {
		currentBet = (int) betSlider.value;
		selectBetTxt.text = "$" + currentBet.ToString();
	}

	private void playerBusted() {
		dealerWin(true);
	}

	private void dealerBusted() {
		playerWin(true);
	}

	private void playerBlackjack() {
		winTxt.text = "Blackjack!";
		playerMoney += currentBet * 2;
		gameEnd();
	}

	private void playerWin(bool winByBust) {
		if (winByBust)
			winTxt.text = "Dealer Busted\nYou Win!";
		else
			winTxt.text = "Player Win!";
		playerMoney += currentBet * 2;
		gameEnd();
	}

	private void dealerWin(bool winByBust) {
		if (winByBust)
			winTxt.text = "You Busted\nDealer Wins!";
		else
			winTxt.text = "Dealer Wins!";
		gameEnd();
	}

	private void gameDraw() {
		winTxt.text = "Draw";
		playerMoney += currentBet;
		gameEnd();
	}

	private void gameReset() {
		playing = false;
		
		// reset points
		playerPoints = 0;
		dealerPoints = 0;
		playerCardPointer = 0;
		dealerCardPointer = 0;
		currentBet = 0;

		// reset cards
		playingDeck = new PokerDeck(CardFace, CardBack, numDecks);
		playerCards = new LinkedList<PokerCard>();
		dealerCards = new LinkedList<PokerCard>();

		// reset UI

		mainBtn.gameObject.SetActive(true);
		mainBtn.GetComponentInChildren<Text>().text = "Deal";
		standBtn.gameObject.SetActive(false);
		betSlider.gameObject.SetActive(true);
		betSlider.maxValue = playerMoney;
		selectBetTxt.gameObject.SetActive(true);
		selectBetTxt.text = "$" + currentBet.ToString();
		placeBetTxt.gameObject.SetActive(true);
		playerPointsTxt.text = "";
		dealerPointsTxt.text = "";
		betTxt.text = "";
		winTxt.text = "";
		resetImgBtn.gameObject.SetActive(false);
		resetBalanceBtn.gameObject.SetActive(true);
		resetGameBtn.gameObject.SetActive(true);

		// clear cards on table
		clearCards();
	}

	private void clearCards() {
		foreach(GameObject playerCard in playerCardPosition){
			if (playerCard.transform.childCount > 0)
				for (int i = 0; i < playerCard.transform.childCount; i++)
				{
					Destroy(playerCard.transform.GetChild(i).gameObject);
				}
		}
		foreach(GameObject dealerCard in dealerCardPosition)
		{
			if (dealerCard.transform.childCount > 0)
				for (int i = 0; i < dealerCard.transform.childCount; i++)
				{
					Destroy(dealerCard.transform.GetChild(i).gameObject);
				}
		}
	}

	
}
