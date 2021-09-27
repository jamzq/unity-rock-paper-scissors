using UnityEngine;
using System.Collections;
using System;

public class PlayerInfoLoader
{
	public delegate void OnLoadedAction(Hashtable playerData);
	public event OnLoadedAction OnLoaded;

	public void Load()
	{
		Hashtable mockPlayerData = new Hashtable();
		mockPlayerData["userId"] = 1;
		mockPlayerData["name"] = PlayerPrefs.GetString("name", "");
		mockPlayerData["coins"] = 50;

		OnLoaded(mockPlayerData);
	}
}