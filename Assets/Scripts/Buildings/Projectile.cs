using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	WaitForSeconds wfs = new WaitForSeconds(0.5f);
	public float damage = 10;

	IEnumerator Deactivate()
	{
		yield return wfs;
		gameObject.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.attachedRigidbody != null) {
			Enemy enemy = other.attachedRigidbody.GetComponent<Enemy>();
			if (enemy != null)
				enemy.Damage(damage);
		}
		gameObject.SetActive(false);
	}
}
