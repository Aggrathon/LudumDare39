
public class SplittingEnemy : Enemy
{
	bool hasSplitted;

	protected override void Die()
	{
		if (!hasSplitted)
		{
			Instantiate(gameObject, transform.position, transform.rotation).GetComponent<SplittingEnemy>().hasSplitted = true;
			Instantiate(gameObject, transform.position, transform.rotation).GetComponent<SplittingEnemy>().hasSplitted = true;
		}
		base.Die();
	}
}
