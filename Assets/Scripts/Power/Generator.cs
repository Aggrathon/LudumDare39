using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : APowerSource, IUpgradable
{

	public float powerGeneration = 100f;
	public float health = 100;

	public event Action<float> onEfficiencyChange;

	float powerDrain = 0;
	float _efficiency = 2;
	int numUpgrades;

	private void Awake()
	{
		numUpgrades = 0;
		upgrades = new List<UpgradeData>(new UpgradeData[]
		{
			new UpgradeData("Boost Health", () => { health += 20; UIMethods.SetHealth(health); }),
			new UpgradeData("Boost Power", () => { powerGeneration += 20; RecalculateEfficiency(); })
		});
	}

	override public void AddDrain(float amount)
	{
		powerDrain += amount;
		RecalculateEfficiency();
	}

	override public void RemoveDrain(float amount)
	{
		powerDrain -= amount;
		RecalculateEfficiency();
	}

	public void RecalculateEfficiency()
	{
		if (powerDrain < powerGeneration)
			_efficiency = 2 - powerDrain / powerGeneration;
		else if (powerDrain < powerGeneration * 1.5f)
			_efficiency = ((powerDrain - powerGeneration) * 2 / powerGeneration)*0.8f+0.2f;
		else
			_efficiency = 0.2f;
		if (onEfficiencyChange != null) onEfficiencyChange(efficiency);
	}

	override public float efficiency { get { return _efficiency; } }

	public override bool powerState
	{
		get
		{
			return true;
		}
	}

	public List<UpgradeData> upgrades { get; protected set; }

	public void ShowStats()
	{
		if(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
			UIMethods.ShowStats(
				string.Format("Power Supply\nPower output: {0}\nPower demand: {1}\nHealth: {2}\nUpgrades: {3}",
				powerGeneration, powerDrain, health, numUpgrades), transform.position, 1);
	}

	public void HideStats()
	{
		UIMethods.HideStats();
	}

	public int GetUpgradeCost(UpgradeData upgrade)
	{
		return (int)(upgrade.costMultiplier * (float)(100 + 20 * numUpgrades));
	}

	public bool Upgrade(UpgradeData upgrade)
	{
		if (!GameManager.TrySpendMoney(GetUpgradeCost(upgrade)))
			return false;
		if (upgrade.unique)
			upgrades.Remove(upgrade);
		numUpgrades++;
		upgrade.func();
		return true;
	}

	public void ShowUpgrades()
	{
		UIMethods.Upgrade(this);
	}
}
