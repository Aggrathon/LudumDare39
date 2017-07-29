using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class LaserTower : Tower
{
	public float damage = 10;
	public float laserTime = 0.2f;
	[Range(0f, 1f)]public float arcRange = 0.8f;

	LineRenderer lr;
	RaycastHit[] hits = new RaycastHit[10];

	private void Awake()
	{
		lr = GetComponent<LineRenderer>();
		lr.enabled = false;
		autoRotate = false;
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
		do
		{
			Vector3 dir = Vector3.Lerp(dir1, dir2, elapsed / laserTime).normalized;
			int numHits = Physics.RaycastNonAlloc(transform.position, dir, hits, range);
			for (int i = 0; i < numHits; i++)
			{
				Enemy enemy = hits[i].collider.GetComponent<Enemy>();
				if (enemy.Damage(damage * Time.deltaTime))
					SetKilled(enemy);
			}
			lr.SetPosition(1, transform.position + dir * range);
			transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
			yield return null;
			elapsed += Time.deltaTime * powerSource.efficiency;
		} while (elapsed < laserTime);
		lr.enabled = false;
	}
}
