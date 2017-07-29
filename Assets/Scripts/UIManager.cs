﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	static UIManager instance;

	public RectTransform buildPanel;
	public GameObject[] towerPrefabs;
	public GameObject[] trapPrefabs;
	public GameObject[] supportPrefabs;

	[System.Serializable]
	public enum BuildType
	{
		Tower,
		Trap,
		Support
	}

	private void Awake()
	{
		instance = this;
	}

	static public void Build(BuildType type, BuildButton btn)
	{
		RectTransform buildPanel = instance.buildPanel;
		GameObject[] prefabs;
		switch (type)
		{
			case BuildType.Tower:
				prefabs = instance.towerPrefabs;
				break;
			case BuildType.Trap:
				prefabs = instance.trapPrefabs;
				break;
			case BuildType.Support:
				prefabs = instance.supportPrefabs;
				break;
			default:
				prefabs = instance.towerPrefabs;
				break;
		}
		while (buildPanel.childCount < prefabs.Length + 2)
			Instantiate(buildPanel.GetChild(2).gameObject, buildPanel);
		for (int i = 0; i < prefabs.Length; i++)
		{
			GameObject prefab = prefabs[i];
			Transform tr = buildPanel.GetChild(i+2);
			Tower t = prefabs[i].GetComponent<Tower>();
			Button b = tr.GetComponent<Button>();
			b.interactable = GameManager.CheckMoney(t.cost);
			b.onClick.RemoveAllListeners();
			b.onClick.AddListener(() => {
				GameObject go = Instantiate(prefab, btn.transform.position, btn.transform.rotation);
				go.GetComponent<Tower>().powerSource = btn.powerSource;
				Destroy(btn.gameObject);
				buildPanel.gameObject.SetActive(false);
			});
			tr.GetChild(0).GetComponent<Text>().text = t.name;
			tr.GetChild(1).GetComponent<Text>().text = t.cost.ToString();
			tr.gameObject.SetActive(true);
		}
		for (int i = prefabs.Length+2; i < buildPanel.childCount; i++)
		{
			buildPanel.GetChild(i).gameObject.SetActive(false);
		}
		buildPanel.gameObject.SetActive(true);
	}
}
