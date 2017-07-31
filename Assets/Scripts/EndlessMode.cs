using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndlessMode : MonoBehaviour
{
	public string scene = "Swirl";
	AsyncOperation async;

	private void Start()
	{
		async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
		async.allowSceneActivation = true;
	}

	private void Update()
	{
		if(async.isDone)
		{
			FindObjectOfType<Spawner>().SetEndlessMode();
			gameObject.SetActive(false);
		}
	}
}
