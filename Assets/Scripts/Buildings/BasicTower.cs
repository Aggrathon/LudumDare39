using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{
	public float projectileSpeed = 10;
	public GameObject projectilePrefab;
	public bool waitForRotation = true;

	Rigidbody[] projectiles;
	int index = 0;

	private void Awake()
	{
		projectiles = new Rigidbody[5];
		for (int i = 0; i < projectiles.Length; i++)
		{
			GameObject go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
			projectiles[i] = go.GetComponent<Rigidbody>();
			go.SetActive(false);
		}
		upgrades = new List<Upgrade>(new Upgrade[]
		{
			new Upgrade("Damage", ()=>{
				for (int i = 0; i < projectiles.Length; i++)
				{
					projectiles[i].GetComponent<Projectile>().damage *= 1.3f;
				}
				IncreasePowerDrain(powerDrain*0.1f);
			}),
			new Upgrade("Firing Rate", ()=>{
				cooldown *= 0.75f;
				IncreasePowerDrain(powerDrain*0.1f);
			}),
			new Upgrade("Range", ()=>{
				range *= 1.3f;
				IncreasePowerDrain(powerDrain*0.1f);
			}),
			new Upgrade("Projectile Speed", ()=>{
				projectileSpeed *= 1.4f;
				IncreasePowerDrain(powerDrain*0.1f);
			}),
		});
	}

	protected override void Shoot(Enemy target)
	{
		if(Vector3.Angle(transform.forward, target.transform.position-transform.position) > 20)
		{
			firingTimer = 0.1f;
			return;
		}
		Vector3 pos = transform.position + transform.forward * 0.7f;
		projectiles[index].MovePosition(pos);
		projectiles[index].transform.position = pos;
		projectiles[index].gameObject.SetActive(true);
		projectiles[index].velocity = transform.forward * projectileSpeed;
		index = (index + 1) % projectiles.Length;
	}

	public override string stats
	{
		get
		{
			float damage = projectiles[0].GetComponent<Projectile>().damage;
			return string.Format(
				"Magnetic Tower\nUpgrades: {0}\nDamage: {1:n1}\nDPS: {5:n1}\nRange: {2:n1}\nCooldown {3:n1}\nPower Drain: {4}",
				numUpgrades, damage, range, cooldown, (int)powerDrain, damage/cooldown*powerSource.efficiency);
		}
	}
}
