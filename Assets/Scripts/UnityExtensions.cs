using UnityEngine;

public static class UnityExtensions
{
	public static T[] FindObjectsByType<T>(this GameObject go) where T : Component
	{
		return go.GetComponentsInChildren<T>();
	}


	public static Quaternion HorizontalRotation(this Transform transform)
	{
		return Quaternion.Euler(0, transform.eulerAngles.y, 0);
	}

	public static float HorizontalDitance(this Vector3 a, Vector3 b)
	{
		a.y = 0;
		b.y = 0;
		return Vector3.Distance(a, b);
	}

	public static Vector3 HorizontalForward(this Transform transform)
	{
		var forward = transform.forward;
		forward.y = 0;
		return forward.normalized;
	}
}