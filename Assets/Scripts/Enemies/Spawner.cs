using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

	public event Action<int> onWave;

	public Wave[] waves;
	public float delayBetweenSpawns = 1f;
	public float delayBetweenWaves = 10f;
	public Text text;
	public Enemy[] enemies;

	void Start () {
		StartCoroutine(Spawning());
	}

	public void SetEndlessMode()
	{
		StopAllCoroutines();
		StartCoroutine(EndlessSpawning());
	}


	IEnumerator Spawning()
	{
		for (int i = 0; i < waves.Length; i++)
		{
			yield return WaitForWave();
			text.text = Utils.GetRomanNumeral(i+1);
			if (onWave != null) onWave(i);
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

	IEnumerator EndlessSpawning()
	{
		int counter = 1;
		int bonus = 0;
		yield return WaitForWave();
		while (true)
		{
			if (GameManager.CheckMoney(500))
				bonus++;
			text.text = Utils.GetRomanNumeral(counter);
			if (onWave != null) onWave(counter);
			int value = (int)((counter+bonus) * 0.25f + Mathf.Sqrt(counter+bonus));

			for (int i = 0; i < 3; i++)
			{
				Enemy e = enemies[UnityEngine.Random.Range(0, enemies.Length)];
				int num = value / e.endlessWeight;
				if (num > 0)
				{
					SpawnEnemies(e.gameObject, num);
					yield return new WaitForSeconds(delayBetweenSpawns);
				}
				else
					i--;
			}
			counter++;
			if (counter < 5)
				yield return new WaitForSeconds(delayBetweenWaves*0.5f);
			else
				yield return new WaitForSeconds(delayBetweenWaves);
		}
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
