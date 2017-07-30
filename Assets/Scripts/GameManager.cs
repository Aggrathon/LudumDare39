using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	static GameManager instance;
	
	public Generator generator;
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
		OnEfficiency(generator.efficiency);
		AddMoney(startingMoney);
		UIMethods.SetHealth(generator.health);
	}

	static public bool TrySpendMoney(int amount)
	{
		if (amount > instance.money)
			return false;
		instance.money -= amount;
		UIMethods.SetMoney(instance.money);
		return true;
	}

	static public bool CheckMoney(int amount)
	{
		return amount <= instance.money;
	}

	static public void AddMoney(int amount)
	{
		instance.money += amount;
		UIMethods.SetMoney(instance.money);
	}

	static public void DoStructuralDamage(float amount)
	{
		instance.generator.powerGeneration -= amount/2;
		instance.generator.health -= amount;
		if (instance.generator.health <= 0)
		{
			UIMethods.Loose();
		}
		UIMethods.SetHealth(instance.generator.health);
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
		}
		UIMethods.SetPower(v);
	}
}
