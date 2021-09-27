﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
	[SerializeField] private Text playerHand;
	[SerializeField] private Text enemyHand;
	[SerializeField] private GameOverScreen gameOverScreen;
	[SerializeField] private Text _nameLabel;
	[SerializeField] private Text _moneyLabel;
	[SerializeField] private Text resultMessage;

	private Player _player;
	private PlayerInfoLoader playerInfoLoader;
	private UpdateGameLoader updateGameLoader;

	void Start()
	{
		playerInfoLoader = new PlayerInfoLoader();
		playerInfoLoader.OnLoaded += OnPlayerInfoLoaded;
		playerInfoLoader.load();
	}

	void Update()
	{
		UpdateHud();
	}

	void OnDisable()
	{
		playerInfoLoader.OnLoaded -= OnPlayerInfoLoaded;
		updateGameLoader.OnLoaded -= OnGameUpdated;
	}

	public void OnPlayerInfoLoaded(Hashtable playerData)
	{
		_player = new Player(playerData);
	}

	public void UpdateHud()
	{
		var playerCoins = _player.GetCoins();

		if (playerCoins > 0)
		{
			_nameLabel.text = "Name: " + _player.GetName();
			_moneyLabel.text = "Money: $" + playerCoins.ToString();
		}
		else
		{
			gameOverScreen.Display();
		}
	}

	public void HandlePlayerInput(int item)
	{
		UseableItem playerChoice = UseableItem.None;

		switch (item)
		{
			case 1:
				playerChoice = UseableItem.Rock;
				break;
			case 2:
				playerChoice = UseableItem.Paper;
				break;
			case 3:
				playerChoice = UseableItem.Scissors;
				break;
		}

		UpdateGame(playerChoice);
	}

	private void UpdateGame(UseableItem playerChoice)
	{
		updateGameLoader = new UpdateGameLoader(playerChoice);
		updateGameLoader.OnLoaded += OnGameUpdated;
		updateGameLoader.Load();
	}

	public void OnGameUpdated(Hashtable gameUpdateData)
	{
		playerHand.text = DisplayResultAsText((UseableItem)gameUpdateData["resultPlayer"]);
		enemyHand.text = DisplayResultAsText((UseableItem)gameUpdateData["resultOpponent"]);

		var coinAmountChange = (int)gameUpdateData["coinsAmountChange"];
		_player.ChangeCoinAmount(coinAmountChange);
		DisplayResultMessage(coinAmountChange);
	}

	private string DisplayResultAsText (UseableItem result)
	{
		switch (result)
		{
			case UseableItem.Rock:
				return "Rock";
			case UseableItem.Paper:
				return "Paper";
			case UseableItem.Scissors:
				return "Scissors";
		}

		return "Nothing";
	}

	private void DisplayResultMessage(int coinAmountChange)
	{
		if (coinAmountChange > 0)
		{
			resultMessage.text = "Congratulations! You've won $" + coinAmountChange;
			resultMessage.color = Color.green;
		}
		else if (coinAmountChange == 0)
		{
			resultMessage.text = "Tie! Everybody wins! You get your money back.";
			resultMessage.color = Color.yellow;
		}
		else
		{
			resultMessage.text = "Nice try... You've lost $" + coinAmountChange;
			resultMessage.color = Color.red;	
		}
	}
}