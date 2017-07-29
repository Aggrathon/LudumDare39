using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class Wire : MonoBehaviour {

	public Transform[] points;

	[ContextMenu("Update Points")]
	private void Start()
	{
		LineRenderer lr = GetComponent<LineRenderer>();
		lr.positionCount = points.Length;
		for (int i = 0; i < points.Length; i++)
		{
			lr.SetPosition(i, points[i].position);
		}
	}

	[System.Serializable]
	public struct Offset
	{
		public Transform transform;
		public Vector3 offset;

		public Vector3 position { get { return transform.position + offset; } }
	}
}
