using System;
using System.Collections.Generic;

public interface IUpgradable
{
	int GetUpgradeCost(UpgradeData upgrade);
	bool Upgrade(UpgradeData upgrade);
	List<UpgradeData> upgrades { get; }
}

public struct UpgradeData
{
	public string name;
	public float costMultiplier;
	public bool unique;
	public Action func;

	public UpgradeData(string name, Action func, float cost = 1, bool once = false)
	{
		this.name = name;
		this.func = func;
		this.costMultiplier = cost;
		this.unique = once;
	}
}