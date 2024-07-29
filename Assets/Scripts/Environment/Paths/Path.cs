using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class Path : MonoBehaviour
{
	SplineContainer SplineContainer => GetComponent<SplineContainer>();

	public Vector3 GetPosition(float t)
	{
		return SplineContainer.EvaluatePosition(t);
	}

	public Vector3 GetPosition(float t, out float newT, Vector3 direction, float displacement)
	{
		var sign = GetSign(t, direction);

		var delta = displacement / SplineContainer.Spline.GetLength();
		newT = t + delta * sign;
		newT = Mathf.Clamp(newT, 0, 1);
		return SplineContainer.EvaluatePosition(newT);
	}

	public float GetTWithDisplacement(float t, float displacement)
	{
		var delta = displacement / SplineContainer.Spline.GetLength();
		return Mathf.Clamp01(t + delta);
	}

	public float GetSign(float t, Vector3 direction)
	{
		var tangent = SplineContainer.EvaluateTangent(t);
		return Mathf.Sign(Vector3.Dot(tangent, direction));
	}

	public Vector3 GetTangent(float t)
	{
		var tangent = SplineContainer.EvaluateTangent(t);
		return tangent;
	}

	public float GetClosestT(Vector3 position)
	{
		float closest = -1;
		float closestDistance = float.MaxValue;
		for (float i = 0; i < 1; i += 0.05f)
		{
			var pos = SplineContainer.EvaluatePosition(i);
			var newDistance = Vector3.Distance(pos, position);
			if (newDistance < closestDistance)
			{
				closest = i;
				closestDistance = newDistance;
			}
		}

		return closest;
	}

	public Vector3 GetClosestPoint(Vector3 position)
	{
		return SplineContainer.EvaluatePosition(GetClosestT(position));
	}

	[ContextMenu("Snap To Ground")]
	void SnapToGround()
	{
		var points = SplineContainer.Spline.Knots.ToArray();
		for (int i = 0; i < points.Count(); i++)
		{
			if (Physics.Raycast(transform.TransformPoint(points[i].Position), Vector3.down, out RaycastHit hit, 1))
			{
				points[i] = new BezierKnot(transform.InverseTransformPoint(hit.point), points[i].TangentIn, points[i].TangentOut);
			}
		}

		SplineContainer.Spline.Knots = points;

	}

	[ContextMenu("Reverse Order")]
	void ReverseOrder()
	{
		var points = SplineContainer.Spline.Knots.ToArray();
		System.Array.Reverse(points);
		SplineContainer.Spline.Knots = points;
	}
}
