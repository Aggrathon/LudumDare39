﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour {

	static Transform goal;
	static public LinkedList<Enemy> activeEnemies;

	public float maxHealth = 100;

	NavMeshAgent nav;
	LinkedListNode<Enemy> node;
	float health;
	
	void Awake () {
		nav = GetComponent<NavMeshAgent>();
		if (goal == null)
			goal = GameObject.FindGameObjectWithTag("Finish").transform;
		if (activeEnemies == null)
			activeEnemies = new LinkedList<Enemy>();
	}

	void OnEnable()
	{
		nav.SetDestination(goal.position);
		node = activeEnemies.AddLast(this);
		health = maxHealth;
	}

	private void OnDisable()
	{
		activeEnemies.Remove(node);
	}

	public Vector3 GetFuturePosition(float time)
	{
		return transform.position + nav.velocity * time;
	}

	public bool Damage(float amount)
	{
		health -= amount;
		if (health < 0)
		{
			gameObject.SetActive(false);
			return true;
		}
		return false;
	}
}
