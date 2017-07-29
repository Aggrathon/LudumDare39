using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public Wave[] waves;
	public float delayBetweenSpawns = 1f;
	public float delayBetweenWaves = 10f;


	void Start () {
		StartCoroutine(Spawning());
	}


	IEnumerator Spawning()
	{
		yield return new WaitForSeconds(delayBetweenWaves);
		for (int i = 0; i < waves.Length; i++)
		{
			for (int j = 0; j < waves[i].enemies.Length; j++)
			{
				if (waves[i].enemies[j].prefab == null)
					yield return new WaitForSeconds(waves[i].enemies[j].amount);
				else
					SpawnEnemies(waves[i].enemies[j].prefab, waves[i].enemies[j].amount);
				yield return new WaitForSeconds(delayBetweenSpawns);
			}
			yield return new WaitForSeconds(delayBetweenWaves);
		}
	}

	void SpawnEnemies(GameObject prefab, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			Instantiate(prefab, transform.position, Quaternion.identity).transform.parent = transform;
		}
	}
}
