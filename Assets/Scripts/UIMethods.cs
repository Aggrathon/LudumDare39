using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIMethods : MonoBehaviour
{
	public GameObject menu;
	float oldTimeScale = 1;

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
		oldTimeScale = Time.timeScale;
		Time.timeScale = 0;
		menu.SetActive(true);
	}

	public void CloseMenu()
	{
		Time.timeScale = Mathf.Max(1, oldTimeScale);
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
}
