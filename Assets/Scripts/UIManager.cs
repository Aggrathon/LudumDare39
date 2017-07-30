using System.Collections;
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
		buildPanel.GetChild(0).GetComponent<Text>().text = "Build";
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
				if (GameManager.TrySpendMoney(t.cost))
				{
					GameObject go = Instantiate(prefab, btn.transform.position, btn.transform.rotation);
					go.GetComponent<Tower>().powerSource = btn.powerSource;
					Destroy(btn.gameObject);
					buildPanel.gameObject.SetActive(false);
				}
				else
					b.interactable = false;
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

	static public void Upgrade(Tower tower)
	{
		RectTransform buildPanel = instance.buildPanel;
		buildPanel.GetChild(0).GetComponent<Text>().text = "Upgrade";
		while (buildPanel.childCount < tower.upgrades.Count + 2)
			Instantiate(buildPanel.GetChild(2).gameObject, buildPanel);
		for (int i = 0; i < tower.upgrades.Count; i++)
		{
			Tower.Upgrade u = tower.upgrades[i];
			Transform tr = buildPanel.GetChild(i + 2);
			Button b = tr.GetComponent<Button>();
			int cost = tower.GetUpgradeCost(u);
			b.interactable = GameManager.CheckMoney(cost);
			b.onClick.RemoveAllListeners();
			b.onClick.AddListener(() => {
				if (tower.UpgradeTower(u))
				{
					buildPanel.gameObject.SetActive(false);
				}
				else
					b.interactable = false;
			});
			tr.GetChild(0).GetComponent<Text>().text = u.name;
			tr.GetChild(1).GetComponent<Text>().text = cost.ToString();
			tr.gameObject.SetActive(true);
		}
		for (int i = tower.upgrades.Count + 2; i < buildPanel.childCount; i++)
		{
			buildPanel.GetChild(i).gameObject.SetActive(false);
		}
		buildPanel.gameObject.SetActive(true);
	}
}
