using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserTower : Tower
{
	public float damage = 10;
	public float laserTime = 0.2f;
	[Range(0f, 1f)]public float arcRange = 0.8f;

	LineRenderer lr;
	RaycastHit[] hits = new RaycastHit[10];

	protected void Awake()
	{
		lr = GetComponent<LineRenderer>();
		lr.enabled = false;
		autoRotate = false;
		upgrades = new List<Upgrade>(new Upgrade[]
		{
			new Upgrade("Damage", ()=>{
				damage *= 1.3f;
				IncreasePowerDrain(powerDrain*0.1f);
			}),
			new Upgrade("Cooldown reduction", ()=>{
				cooldown = (cooldown-laserTime)*0.5f -laserTime;
				IncreasePowerDrain(powerDrain*0.1f);
			}),
			new Upgrade("Range", ()=>{
				range *= 1.3f;
				IncreasePowerDrain(powerDrain*0.1f);
			}),
			new Upgrade("Thighter Arc", ()=>{
				arcRange *= 0.6f;
			}, 0.5f),
			new Upgrade("Wider Arc", ()=>{
				arcRange *= 1.4f;
			}, 0.5f)
		});
	}

	protected override void Shoot(Enemy target)
	{
		Vector3 dir1 = target.GetFuturePosition(-laserTime*arcRange) - transform.position;
		Vector3 dir2 = target.GetFuturePosition(laserTime*(1+arcRange)) - transform.position;
		dir1.y = 0;
		dir2.y = 0;
		StartCoroutine(Shooting(dir1.normalized, dir2.normalized));
	}

	IEnumerator Shooting(Vector3 dir1, Vector3 dir2)
	{
		float time = Time.time;
		lr.SetPosition(0, transform.position);
		lr.enabled = true;
		float elapsed = 0;
		float prevRange = range*0.8f;
		do
		{
			Vector3 dir = Vector3.Lerp(dir1, dir2, elapsed / laserTime).normalized;
			int numHits = Physics.RaycastNonAlloc(transform.position, dir, hits, range);
			prevRange *= 0.9f;
			float d = damage * Time.deltaTime * powerSource.efficiency;
			for (int i = 0; i < numHits; i++)
			{
				Enemy enemy = hits[i].collider.GetComponent<Enemy>();
				if (enemy != null && enemy.Damage(d))
					SetKilled(enemy);
				float dist = hits[i].distance + 0.4f;
				if (dist > prevRange)
					prevRange = dist;
			}
			lr.SetPosition(1, transform.position + dir * prevRange);
			transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
			yield return null;
			elapsed += Time.deltaTime * powerSource.efficiency;
		} while (elapsed < laserTime);
		lr.enabled = false;
	}

	public override string stats
	{
		get
		{
			return string.Format(
				"Laser Tower\nUpgrades: {0}\nDamage: {1:n1}\nDPS: {5:n1}\nRange: {2:n1}\nInterval {3:n1}\nPower Drain: {4}",
				numUpgrades, (int)damage, range, cooldown, (int)powerDrain, damage / cooldown * powerSource.efficiency);
		}
	}
}
