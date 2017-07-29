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
		generator.onEfficiencyChange += v => powerText.text = (int)(v*100)+"%";
		powerText.text = ((int)generator.efficiency).ToString();
		money = startingMoney;
		moneyText.text = money.ToString();
	}

	public void SetGameSpeed(float speed)
	{
		Time.timeScale = speed;
	}

	private void OnDestroy()
	{
		instance = null;
		Time.timeScale = 1;
	}

	static public bool TrySpendMoney(int amount)
	{
		if (amount > instance.money)
			return false;
		instance.money -= amount;
		instance.moneyText.text = instance.money.ToString();
		return true;
	}

	static public void AddMoney(int amount)
	{
		instance.money += amount;
		instance.moneyText.text = instance.money.ToString();
	}

	static public void DoStructuralDamage(float amount)
	{
		instance.generator.powerGeneration -= amount;
		instance.generator.RecalculateEfficiency();
		//TODO Do structural damage
	}
}
