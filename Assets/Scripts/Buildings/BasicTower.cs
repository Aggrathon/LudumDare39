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
	}

	protected override void Shoot(Enemy target)
	{
		if(Vector3.Angle(transform.forward, target.transform.position-transform.position) > 30)
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
}
