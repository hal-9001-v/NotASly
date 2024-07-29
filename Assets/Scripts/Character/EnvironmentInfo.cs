using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentInfo : MonoBehaviour
{
	[SerializeField] UnityEvent onFallEvent;
	public Action OnFallAction;

	[SerializeField] float maxMenaceRadius;
	[SerializeField] float minMenaceRadius;

	PlayerMenace[] Menaces => FindObjectsByType<PlayerMenace>(FindObjectsSortMode.None);

	public void Fall()
	{
		onFallEvent.Invoke();
		OnFallAction?.Invoke();
	}

	public Transform GetClosestMenace()
	{
		//Get the closest menace
		var closestMenace = Menaces.OrderBy(m => Vector3.Distance(m.transform.position, transform.position)).FirstOrDefault();

		if (closestMenace && Vector3.Distance(closestMenace.transform.position, transform.position) < maxMenaceRadius)
		{
			return closestMenace.transform;
		}

		return null;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, maxMenaceRadius);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, minMenaceRadius);
	}
}
