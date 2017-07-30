using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{
	public float projectileSpeed = 10;
	public GameObject projectilePrefab;

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
		projectiles[index].gameObject.SetActive(true);
		projectiles[index].MovePosition(transform.position + transform.forward * 0.7f);
		projectiles[index].velocity = transform.forward * projectileSpeed;
		index = (index + 1) % projectiles.Length;
	}
}
