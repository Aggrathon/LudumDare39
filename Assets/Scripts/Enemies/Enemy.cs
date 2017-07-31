using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour {

	static Transform goal;
	static public LinkedList<Enemy> activeEnemies = new LinkedList<Enemy>();

	public float maxHealth = 100;
	public int killReward = 10;
	public float penalty = 1;
	public int endlessWeight = 1;

	NavMeshAgent nav;
	LinkedListNode<Enemy> node;
	float health;
	
	void Awake () {
		nav = GetComponent<NavMeshAgent>();
		if (goal == null)
			goal = GameObject.FindGameObjectWithTag("Finish").transform;
	}

	void OnEnable()
	{
		node = activeEnemies.AddLast(this);
		health = maxHealth;
		if (!nav.isOnNavMesh)
		{
			nav.enabled = false;
			NavMeshHit closestHit;
			if (NavMesh.SamplePosition(transform.position, out closestHit, 10, nav.areaMask))
			{
				transform.position = closestHit.position;
			}
			nav.enabled = true;
		}
		nav.Warp(transform.position);
		nav.SetDestination(goal.position);
	}

	private void OnDisable()
	{
		activeEnemies.Remove(node);
	}

	private void Update()
	{
		if ((transform.position-goal.position).sqrMagnitude < 1)
		{
			GameManager.DoStructuralDamage(penalty);
			Die();
		}
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
			Die();
			return true;
		}
		return false;
	}

	virtual protected void Die()
	{
		GameManager.AddMoney(killReward);
		Destroy(gameObject);
	}
}
