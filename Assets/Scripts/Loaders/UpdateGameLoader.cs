using System.Collections;
using System;

public class UpdateGameLoader
{
	public delegate void OnLoadedAction(Hashtable gameUpdateData);
	public event OnLoadedAction OnLoaded;

	private UseableItem _playerHand;

	public void SetPlayerHand(UseableItem playerHand)
	{
		_playerHand = playerHand;
	}

	public void Load()
	{
		var mockGameUpdate = new Hashtable();
		var useableItemValues = Enum.GetValues(typeof(UseableItem));
		var opponentHand = (UseableItem) useableItemValues.GetValue(UnityEngine.Random.Range(1, useableItemValues.Length));

		mockGameUpdate["resultPlayer"] = _playerHand;
		mockGameUpdate["resultOpponent"] = opponentHand;
		mockGameUpdate["coinsAmountChange"] = GetCoinsAmount(_playerHand, opponentHand);
		
		OnLoaded(mockGameUpdate);
	}

	private int GetCoinsAmount(UseableItem playerHand, UseableItem opponentHand)
	{
		Result drawResult = ResultAnalyzer.GetResultState(playerHand, opponentHand);

		if (drawResult.Equals (Result.Won))
		{
			return 10;
		}
		else if (drawResult.Equals (Result.Lost))
		{
			return -10;
		}
		else
		{
			return 0;
		}
	}
}