using UnityEngine;

public abstract class Tower : MonoBehaviour {

	public float range = 4;
	public float cooldown = 0.1f;

	Enemy target;
	float timer;
	protected bool autoRotate = true;
	
	void Update () {
		if(timer < 0)
		{
			if (GetTarget() != null)
			{
				Shoot(target);
				timer += cooldown;
			}
		}
		else
			timer -= Time.deltaTime;
		if (autoRotate && target != null)
		{
			Vector3 lt = target.transform.position;
			lt.y = transform.position.y;
			transform.LookAt(lt, Vector3.up);
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
}
