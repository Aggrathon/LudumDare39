using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButton : MonoBehaviour {

	[SerializeField]
	public UIMethods.BuildType buildType;
	public APowerSource powerSource;

	private void Awake()
	{
		powerSource.onPowerStateChanged += PowerState;
	}

	private void OnDestroy()
	{
		powerSource.onPowerStateChanged -= PowerState;
	}

	public void ShowBuildMenu()
	{
		UIMethods.Build(buildType, this);
	}

	void PowerState(APowerSource ps)
	{
		gameObject.SetActive(ps.powerState);
	}
}
