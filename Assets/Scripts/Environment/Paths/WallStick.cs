using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStick : MonoBehaviour, IPathInteractable
{
	public Path Path => GetComponent<Path>();

	public Vector3 GetClosestPoint(Vector3 point)
	{
		return Path.GetClosestPoint(point);
	}

	public float GetClosestT(Vector3 point)
	{
		return Path.GetClosestT(point);
	}

	private void OnDrawGizmosSelected()
	{
		int sections = 5;
		for (int i = 0; i < sections; i++)
		{
			var t = i / (sections - 1);
			var point = Path.GetPosition(t);
			var tangent = Path.GetTangent(t);
			var normal = Vector3.Cross(tangent, Vector3.up);

			Gizmos.color = Color.red;
			Gizmos.DrawSphere(point, 0.1f);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(point, point + normal.normalized);


		}
	}
}
