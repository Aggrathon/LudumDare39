using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{
	public float projectileSpeed = 10;
	public GameObject projectilePrefab;
	public bool waitForRotation = true;
	public AudioClip[] sounds;

	Rigidbody[] projectiles;
	AudioSource audioSource;
	int index = 0;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		projectiles = new Rigidbody[5];
		for (int i = 0; i < projectiles.Length; i++)
		{
			GameObject go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
			projectiles[i] = go.GetComponent<Rigidbody>();
			go.SetActive(false);
		}
		upgrades = new List<UpgradeData>(new UpgradeData[]
		{
			new UpgradeData("Damage", ()=>{
				for (int i = 0; i < projectiles.Length; i++)
				{
					projectiles[i].GetComponent<Projectile>().damage *= 1.3f;
				}
				IncreasePowerDrain(powerDrain*0.1f);
			}),
			new UpgradeData("Firing Rate", ()=>{
				cooldown *= 0.75f;
				IncreasePowerDrain(powerDrain*0.1f);
			}),
			new UpgradeData("Range", ()=>{
				range *= 1.3f;
				IncreasePowerDrain(powerDrain*0.1f);
			}),
			new UpgradeData("Projectile Speed", ()=>{
				projectileSpeed *= 1.5f;
				IncreasePowerDrain(powerDrain*0.1f);
			}, 0.5f),
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
		audioSource.pitch = UnityEngine.Random.Range(0.6f, 1.3f);
		audioSource.PlayOneShot(sounds[UnityEngine.Random.Range(0, sounds.Length)]);

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
