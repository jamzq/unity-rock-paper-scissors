using UnityEngine;
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
	[SerializeField] private WelcomeScreen welcomeScreen;
	[SerializeField] private Text winStreakCounter;


	private Player _player;
	private PlayerInfoLoader playerInfoLoader;
	private UpdateGameLoader updateGameLoader;
	private int winStreak = 0;

	void Start()
	{
		playerInfoLoader = new PlayerInfoLoader();
		playerInfoLoader.OnLoaded += OnPlayerInfoLoaded;
		playerInfoLoader.Load();

		updateGameLoader = new UpdateGameLoader();
		updateGameLoader.OnLoaded += OnGameUpdated;
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
		if (string.IsNullOrEmpty(_player.GetName()))
		{
			welcomeScreen.Display();
		}
		else
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

	public void ConfirmPlayerName()
	{
		welcomeScreen.ConfirmPlayerName();
		playerInfoLoader.Load();
	}

	private void UpdateGame(UseableItem playerChoice)
	{
		updateGameLoader.SetPlayerHand(playerChoice);
		updateGameLoader.Load();
	}

	public void OnGameUpdated(Hashtable gameUpdateData)
	{
		playerHand.text = DisplayResultAsText((UseableItem)gameUpdateData["resultPlayer"]);
		enemyHand.text = DisplayResultAsText((UseableItem)gameUpdateData["resultOpponent"]);

		var coinAmountChange = (int)gameUpdateData["coinsAmountChange"];
		_player.ChangeCoinAmount(coinAmountChange);
		UpdateResults(coinAmountChange);
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

	private void UpdateResults(int coinAmountChange)
	{
		if (coinAmountChange > 0)
		{
			resultMessage.text = "Congratulations! You've won $" + coinAmountChange;
			resultMessage.color = Color.green;
			winStreak++;
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
			winStreak = 0;
		}

		winStreakCounter.text = "Win streak: " + winStreak;
	}
}