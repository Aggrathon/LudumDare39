using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerSwitch : APowerSource {

	[SerializeField] bool on;
	float powerDrain = 0;

	public APowerSource powerSource;
	[Space]
	public Image img;
	public Sprite onImage;
	public Sprite offImage;
	public Renderer[] wires;
	public Material wireOnMaterial;
	public Material wireOffMaterial;

	private void Awake()
	{
		powerSource.onPowerStateChanged += UpdateWires;
	}

	private void Start()
	{
		if (!on)
		{
			UpdateWires(this);
			img.sprite = offImage;
		}
	}

	public void Switch()
	{
		on = !on;
		img.sprite = on ? onImage : offImage;
		if (on)
			powerSource.AddDrain(powerDrain);
		else
			powerSource.RemoveDrain(powerDrain);
		UpdateWires(this);
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

	public override bool powerState
	{
		get
		{
			return powerSource.powerState && on;
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

	void UpdateWires(APowerSource source)
	{
		if (powerState)
			for (int i = 0; i < wires.Length; i++)
				wires[i].material = wireOnMaterial;
		else
			for (int i = 0; i < wires.Length; i++)
				wires[i].material = wireOffMaterial;
		ChangePowerState();
	}
}
