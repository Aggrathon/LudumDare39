using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1 : MonoBehaviour {

	public Spawner spawner;
	public Generator generator;
	public GameObject introText;
	public GameObject buttonText;
	public GameObject efficiencyText;
	public GameObject moreInfoText;

	private void Awake()
	{
		spawner.gameObject.SetActive(false);
	}

	void Start () {
		StartCoroutine(Tutorial());
	}

	IEnumerator Tutorial()
	{
		introText.SetActive(true);
		while (generator.efficiency > 1.95f)
			yield return null;
		introText.SetActive(false);
		spawner.gameObject.SetActive(true);
		while (generator.efficiency > 1.7f)
			yield return null;
		buttonText.SetActive(true);
		while (generator.efficiency > 1.4f)
			yield return null;
		efficiencyText.SetActive(true);
		while (generator.efficiency > 1.1f)
			yield return null;
		moreInfoText.SetActive(true);
	}
}
