using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{

	public float damage = 10;
	public float projectileSpeed = 10;
	public GameObject projectilePrefab;

	protected override void Shoot(Enemy target)
	{
	}
}
