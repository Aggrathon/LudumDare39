using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	static GameManager instance;

	public Text moneyText;
	public Text powerText;
	public Generator generator;
	[Space]
	public int startingMoney = 100;

	int money;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		if (generator == null)
			generator = FindObjectOfType<Generator>();
		generator.onEfficiencyChange += OnEfficiency;
		powerText.text = ((int)generator.efficiency * 100) + "%";
		money = startingMoney;
		moneyText.text = money.ToString();
	}

	static public bool TrySpendMoney(int amount)
	{
		if (amount > instance.money)
			return false;
		instance.money -= amount;
		instance.moneyText.text = instance.money.ToString();
		return true;
	}

	static public bool CheckMoney(int amount)
	{
		return amount <= instance.money;
	}

	static public void AddMoney(int amount)
	{
		instance.money += amount;
		instance.moneyText.text = instance.money.ToString();
	}

	static public void DoStructuralDamage(float amount)
	{
		instance.generator.powerGeneration -= amount;
		if (instance.generator.powerGeneration <= 0)
		{
			UIMethods.Loose();
		}
		instance.generator.RecalculateEfficiency();
	}

	static public void WinGame()
	{
		UIMethods.Win();
	}

	void OnEfficiency(float v)
	{
		if (v < 1)
		{
			//TODO error sound
			powerText.text = "<color=red>"+(int)(v * 100) + "%<color>";
		}
		else
			powerText.text = (int)(v * 100) + "%";
	}
}
