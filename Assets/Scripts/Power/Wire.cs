using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class Wire : MonoBehaviour {

	public Transform[] points;
	public float offset = 0.05f;

	[ContextMenu("Update Points")]
	private void Start()
	{
		LineRenderer lr = GetComponent<LineRenderer>();
		lr.positionCount = points.Length*2;
		Vector3 off = new Vector3();
		for (int i = 0; i < points.Length; i++)
		{
			if (i != points.Length - 1)
			{
				if (i != 0)
					off = points[i - 1].position - points[i].position;
				else if (points.Length > 2)
					off = points[2].position - points[i].position;
				else
					off = Quaternion.Euler(0, 90, 0) * points[i + 1].position - points[i].position;
				off += points[i + 1].position - points[i].position;
				off = new Vector3(Mathf.Sign(off.x) * offset, 0, Mathf.Sign(off.z) * offset);
			}
			lr.SetPosition(i, points[i].position + off);
			lr.SetPosition(lr.positionCount-i-1, points[i].position - off);
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
