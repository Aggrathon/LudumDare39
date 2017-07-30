using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

	public Wave[] waves;
	public float delayBetweenSpawns = 1f;
	public float delayBetweenWaves = 10f;
	public Text text;

	void Start () {
		StartCoroutine(Spawning());
	}


	IEnumerator Spawning()
	{
		for (int i = 0; i < waves.Length; i++)
		{
			yield return WaitForWave();
			text.text = Utils.GetRomanNumeral(i+1);
			for (int j = 0; j < waves[i].enemies.Length; j++)
			{
				yield return new WaitForSeconds(delayBetweenSpawns);
				if (waves[i].enemies[j].prefab == null)
					yield return new WaitForSeconds(waves[i].enemies[j].amount);
				else
					SpawnEnemies(waves[i].enemies[j].prefab, waves[i].enemies[j].amount);
			}
		}
		while (Enemy.activeEnemies.Count > 0) yield return null;
		GameManager.WinGame();
	}

	IEnumerator WaitForWave()
	{
		float time = Time.time + delayBetweenWaves;
		int sec = 0;
		do
		{
			int ns = (int)(time - Time.time);
			if (ns != sec)
			{
				text.text = "<color=grey>" + ns + "s</color>";
				sec = ns;
			}
			yield return null;
		} while (time > Time.time);
	}

	void SpawnEnemies(GameObject prefab, int amount)
	{
		Vector3 pos = transform.position;
		for (int i = 0; i < amount; i++)
		{
			Instantiate(prefab, pos, Quaternion.identity);
		}
	}
}
