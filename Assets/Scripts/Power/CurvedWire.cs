using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CurvedWire : MonoBehaviour {

	public Transform source;
	public Transform target;
	public int segments = 20;
	public float directionStrength = 1;

	[ContextMenu("Update Points")]
	void Start () {
		LineRenderer lr = GetComponent<LineRenderer>();
		lr.positionCount = segments;
		Vector3 pos = source.position;
		lr.SetPosition(0, pos);
		for (int i = 1; i < segments; i++)
		{
			float influence = ((float)(i - 1) / (float)(segments - 2));
			pos += (target.position - pos) * (influence*influence+influence)*0.5f;
			influence = (1-influence)*(1-influence);
			pos += source.forward * (directionStrength * influence);
			lr.SetPosition(i, pos);
		}
	}

}
