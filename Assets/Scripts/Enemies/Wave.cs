using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Object/Wave")]
public class Wave : ScriptableObject {

	public Spawn[] enemies;

	[System.Serializable]
	public struct Spawn
	{
		public GameObject prefab;
		public int amount;
	}
}
