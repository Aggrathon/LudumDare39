﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : APowerSource
{

	public float powerGeneration = 100f;

	public event Action<float> onEfficiencyChange;

	float powerDrain = 0;
	float _efficiency = 2;

	override public void AddDrain(float amount)
	{
		powerDrain += amount;
		RecalculateEfficiency();
	}

	override public void RemoveDrain(float amount)
	{
		powerDrain -= amount;
		RecalculateEfficiency();
	}

	public void RecalculateEfficiency()
	{
		if (powerDrain < powerGeneration)
			_efficiency = 2 - powerDrain / powerGeneration;
		else
			_efficiency = Mathf.Max(1 - (powerDrain - powerGeneration) * 2 / powerGeneration, 0);
		if (onEfficiencyChange != null) onEfficiencyChange(efficiency);
	}

	override public float efficiency { get { return _efficiency; } }

	public override bool powerState
	{
		get
		{
			return true;
		}
	}
}
