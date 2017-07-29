using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitch : APowerSource {

	bool on;
	int animOnBool = Animator.StringToHash("on");
	float powerDrain = 0;

	public Animator anim;
	public Renderer[] wires;
	public Material wireOnMaterial;
	public Material wireOffMaterial;
	public APowerSource powerSource;

	private void Start()
	{
		on = false;
		Switch();
	}

	public void Switch()
	{
		on = !on;
		anim.SetBool(animOnBool, on);
		for (int i = 0; i < wires.Length; i++)
		{
			wires[i].material = on ? wireOnMaterial : wireOffMaterial;
		}
		if (on)
			powerSource.AddDrain(powerDrain);
		else
			powerSource.RemoveDrain(powerDrain);
	}

	override public float efficiency
	{
		get
		{
			if (on)
				return powerSource.efficiency;
			else
				return 0;
		}
	}

	override public void AddDrain(float amount)
	{
		powerDrain += amount;
		if (on)
			powerSource.AddDrain(powerDrain);
	}

	override public void RemoveDrain(float amount)
	{
		powerDrain -= amount;
		if(on)
			powerSource.RemoveDrain(amount);
	}
}
