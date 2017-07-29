using UnityEngine;

public abstract class APowerSource : MonoBehaviour
{
	public abstract float efficiency { get; }

	public abstract void AddDrain(float amount);
	public abstract void RemoveDrain(float amount);
}