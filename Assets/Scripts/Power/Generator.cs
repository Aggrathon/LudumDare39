using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : APowerSource
{

	public float powerGeneration = 100f;

	float powerDrain = 0;

	override public void AddDrain(float amount)
	{
		powerDrain += amount;
	}

	override public void RemoveDrain(float amount)
	{
		powerDrain -= amount;
	}

	override public float efficiency {
		get {
			if (powerDrain < powerGeneration)
				return 2 - powerDrain / powerGeneration;
			else
				return Mathf.Max(3 - powerDrain / powerGeneration * 2, 0);
		}
	}

	public override bool powerState
	{
		get
		{
			return true;
		}
	}
}
