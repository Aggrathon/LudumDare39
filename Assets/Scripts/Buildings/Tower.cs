using UnityEngine;

public abstract class Tower : MonoBehaviour {

	public float range = 4;
	public float cooldown = 0.1f;
	public int cost = 100;
	public float powerDrain = 10;

	[System.NonSerialized] public APowerSource powerSource;

	Enemy target;
	protected float firingTimer;
	protected bool autoRotate = true;

	virtual protected void Start()
	{
		powerSource.AddDrain(powerDrain);
	}
	
	void Update () {
		if(firingTimer < 0)
		{
			if (GetTarget() != null)
			{
				Shoot(target);
				firingTimer += cooldown;
			}
		}
		else
			firingTimer -= Time.deltaTime * powerSource.efficiency;
		if (autoRotate && target != null)
		{
			Vector3 lt = target.GetFuturePosition(0.1f)-transform.position;
			lt.y = 0;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lt, Vector3.up), 180 * Time.deltaTime);
		}
		else
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 60 * Time.deltaTime);
		}
	}

	protected abstract void Shoot(Enemy target);

	public Enemy GetTarget()
	{
		if (target == null || !target.isActiveAndEnabled || (transform.position-target.transform.position).sqrMagnitude > range*range)
		{
			float len = float.PositiveInfinity;
			target = null;
			var iter = Enemy.activeEnemies.First;
			while(iter != null)
			{
				float l = (transform.position - iter.Value.transform.position).sqrMagnitude;
				if (l < range * range && l < len)
				{
					len = l;
					target = iter.Value;
				}
				iter = iter.Next;
			}
		}
		return target;
	}

	public void SetKilled(Enemy enemy)
	{
		if (enemy == target)
			target = null;
	}
}
