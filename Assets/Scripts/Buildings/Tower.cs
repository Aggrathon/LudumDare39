using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class Tower : MonoBehaviour {

	public float range = 4;
	public float cooldown = 0.1f;
	public int cost = 100;
	public float powerDrain = 10;

	[System.NonSerialized] public APowerSource powerSource;
	[System.NonSerialized] public List<Upgrade> upgrades;

	Enemy target;
	Button towerImg;
	Text towerLevel;
	int numUpgrades = 0;
	protected float firingTimer;
	protected bool autoRotate = true;

	virtual protected void Start()
	{
		powerSource.AddDrain(powerDrain);
		towerImg = GetComponentInChildren<Button>();
		towerLevel = GetComponentInChildren<Text>();
		powerSource.onPowerStateChanged += UpdatePowerState;
		UpdatePowerState(powerSource);
	}
	
	void Update () {
		if(firingTimer < 0)
		{
			if (GetTarget() != null)
			{
				firingTimer += cooldown;
				Shoot(target);
			}
		}
		else
			firingTimer -= Time.deltaTime * powerSource.efficiency;
		if (autoRotate && target != null && powerSource.powerState)
		{
			Vector3 lt = target.GetFuturePosition(0.1f)-transform.position;
			lt.y = 0;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lt, Vector3.up), 180 * Time.deltaTime);
		}
		else
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 30 * Time.deltaTime);
		}
	}

	protected abstract void Shoot(Enemy target);

	public Enemy GetTarget()
	{
		if (target == null || !target.isActiveAndEnabled || (transform.position-target.transform.position).sqrMagnitude > range*range)
		{
			float len = float.PositiveInfinity;
			target = null;
			var iter = Enemy.activeEnemies.First;
			while(iter != null)
			{
				float l = (transform.position - iter.Value.transform.position).sqrMagnitude;
				if (l < range * range && l < len)
				{
					len = l;
					target = iter.Value;
				}
				iter = iter.Next;
			}
		}
		return target;
	}

	public void SetKilled(Enemy enemy)
	{
		if (enemy == target)
			target = null;
	}

	void UpdatePowerState(APowerSource ps)
	{
		towerImg.interactable = ps.powerState;
	}

	protected void IncreasePowerDrain(float amount)
	{
		powerDrain += amount;
		powerSource.AddDrain(amount);
	}

	public int GetUpgradeCost(Upgrade upgrade)
	{
		return (int)((float)cost * upgrade.costMultiplier);
	}

	public void ShowUpgrades()
	{
		UIManager.Upgrade(this);
	}

	public bool UpgradeTower(Upgrade upgrade)
	{
		if (!GameManager.TrySpendMoney(GetUpgradeCost(upgrade)))
			return false;
		if (upgrade.unique)
			upgrades.Remove(upgrade);
		else
		{
			upgrade.costMultiplier *= 1.2f;
			cost += (int)((float)cost * 0.1f);
		}
		upgrade.func();
		numUpgrades++;
		towerLevel.text = Utils.GetRomanNumeral(numUpgrades);
		return true;
	}

	public struct Upgrade
	{
		public string name;
		public float costMultiplier;
		public bool unique;
		public Action func;

		public Upgrade(string name, Action func, float cost = 1, bool once = false)
		{
			this.name = name;
			this.func = func;
			this.costMultiplier = cost;
			this.unique = once;
		} 
	}
}
