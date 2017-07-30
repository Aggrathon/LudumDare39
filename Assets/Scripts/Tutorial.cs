using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	public Spawner spawner;
	public Generator generator;
	public GameObject introText;
	public GameObject buttonText;
	public GameObject efficiencyText;
	public GameObject moreInfoText;
	public GameObject upgradeText;

	private void Awake()
	{
		spawner.gameObject.SetActive(false);
		spawner.onWave += Wave;
	}

	void Start () {
		StartCoroutine(StartTutorial());
	}

	IEnumerator StartTutorial()
	{
		introText.SetActive(true);
		while (generator.efficiency > 1.95f)
			yield return null;
		introText.SetActive(false);
		spawner.gameObject.SetActive(true);
	}

	void Wave(int num)
	{
		switch(num)
		{
			case 1:
				buttonText.SetActive(true);
				SoundManager.TutorialTip();
				break;
			case 2:
				efficiencyText.SetActive(true);
				SoundManager.TutorialTip();
				break;
			case 3:
				upgradeText.SetActive(true);
				SoundManager.TutorialTip();
				break;
			case 4:
				moreInfoText.SetActive(true);
				SoundManager.TutorialTip();
				break;
		}
	}
}
