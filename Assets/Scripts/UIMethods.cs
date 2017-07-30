using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIMethods : MonoBehaviour
{
	public static UIMethods instance { get; protected set; }

	public GameObject menu;
	[Space]
	public GameObject infoPanel;
	public Text infoText;
	public LineRenderer infoLine;
	[Space]
	public RectTransform buildPanel;
	public GameObject[] towerPrefabs;
	public GameObject[] trapPrefabs;
	public GameObject[] supportPrefabs;
	[Space]
	public GameObject victoryPanel;
	public GameObject defeatPanel;
	[Space]
	public Text money;
	public Text power;
	public Text health;

	float timeScale = 1;

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
		timeScale = 1;
		Time.timeScale = 1;
		if (menu == null)
			enabled = false;
	}

	public void SetGameSpeed(float speed)
	{
		timeScale = speed;
		Time.timeScale = speed;
	}

	private void OnDestroy()
	{
		Time.timeScale = 1;
	}

	public void ChangeScene(string scene)
	{
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void OpenMenu()
	{
		Time.timeScale = 0;
		menu.SetActive(true);
	}

	public void CloseMenu()
	{
		Time.timeScale = timeScale;
		menu.SetActive(false);
	}

	public void ToggleMenu()
	{
		if (menu.activeSelf)
			OpenMenu();
		else
			CloseMenu();
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
			ToggleMenu();
	}

	public static void SetHealth(float amount)
	{
		instance.health.text = Mathf.Round(amount) + " %";
	}

	public static void SetMoney(int amount)
	{
		instance.money.text = amount.ToString();
	}

	public static void SetPower(float amount)
	{
		if (amount < 1)
		{
			instance.power.text = "<color=red>" + (int)(amount * 100) + "%</color>";
		}
		else
			instance.power.text = (int)(amount * 100) + "%";
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
			Transform tr = buildPanel.GetChild(i + 2);
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
					SoundManager.Build();
				}
				else
					b.interactable = false;
			});
			tr.GetChild(0).GetComponent<Text>().text = t.name;
			tr.GetChild(1).GetComponent<Text>().text = t.cost.ToString();
			tr.gameObject.SetActive(true);
		}
		for (int i = prefabs.Length + 2; i < buildPanel.childCount; i++)
		{
			buildPanel.GetChild(i).gameObject.SetActive(false);
		}
		buildPanel.gameObject.SetActive(true);
	}
	static public void Upgrade(IUpgradable structure)
	{
		RectTransform buildPanel = instance.buildPanel;
		buildPanel.GetChild(0).GetComponent<Text>().text = "Upgrade";
		while (buildPanel.childCount < structure.upgrades.Count + 2)
			Instantiate(buildPanel.GetChild(2).gameObject, buildPanel);
		for (int i = 0; i < structure.upgrades.Count; i++)
		{
			UpgradeData u = structure.upgrades[i];
			Transform tr = buildPanel.GetChild(i + 2);
			Button b = tr.GetComponent<Button>();
			int cost = structure.GetUpgradeCost(u);
			b.interactable = GameManager.CheckMoney(cost);
			b.onClick.RemoveAllListeners();
			b.onClick.AddListener((UnityEngine.Events.UnityAction)(() => {
				if (structure.Upgrade(u))
				{
					buildPanel.gameObject.SetActive(false);
				}
				else
					b.interactable = false;
			}));
			tr.GetChild(0).GetComponent<Text>().text = u.name;
			tr.GetChild(1).GetComponent<Text>().text = cost.ToString();
			tr.gameObject.SetActive(true);
		}
		for (int i = structure.upgrades.Count + 2; i < buildPanel.childCount; i++)
		{
			buildPanel.GetChild(i).gameObject.SetActive(false);
		}
		buildPanel.gameObject.SetActive(true);
	}

	static public void ShowStats(string stats, Vector3 position, float range)
	{
		int num = instance.infoLine.positionCount;
		float div = 1f / (float)(num - 1);
		for (int i = 0; i < num; i++)
		{
			instance.infoLine.SetPosition(i, position + Quaternion.Euler(0, 360 * i * div, 0) * new Vector3(0, 0, range));
		}
		instance.infoText.text = stats;
		instance.infoPanel.SetActive(true);
	}

	static public void HideStats()
	{
		instance.infoPanel.SetActive(false);
	}

	static public void Win()
	{
		instance.victoryPanel.SetActive(true);
	}

	static public void Loose()
	{
		instance.defeatPanel.SetActive(true);
		Time.timeScale = 0;
	}
}
