using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1 : MonoBehaviour {

	public Spawner spawner;
	public Generator generator;
	public GameObject introText;
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
		while (generator.efficiency > 1.6f)
			yield return null;
		efficiencyText.SetActive(true);
		while (generator.efficiency > 1.3f)
			yield return null;
		moreInfoText.SetActive(true);
	}
}
