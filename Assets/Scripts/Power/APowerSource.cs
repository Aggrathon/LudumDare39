using UnityEngine;
using System;

public abstract class APowerSource : MonoBehaviour
{
	public event Action<APowerSource> onPowerStateChanged;

	protected void ChangePowerState()
	{
		onPowerStateChanged.Invoke(this);
	}

	public abstract float efficiency { get; }
	public abstract bool powerState { get; }

	public abstract void AddDrain(float amount);
	public abstract void RemoveDrain(float amount);
}