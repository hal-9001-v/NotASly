using System;
using UnityEngine;

[Serializable]
public class Timer
{
	[SerializeField] float time;
	float elapsedTime;
	[SerializeField] public bool onlyOnce;
	public float NormalizedTime => Mathf.Clamp01(elapsedTime / time);

	public bool IsFinished { get; private set; }

	public Timer(float time, bool onlyOnce = false)
	{
		this.time = time;
		this.onlyOnce = onlyOnce;
	}

	public bool UpdateTimer()
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= time)
		{
			if (onlyOnce && IsFinished)
			{
				return false;
			}
			else
			{
				IsFinished = true;
				return true;
			}
		}
		return false;
	}

	public void ResetTimer()
	{
		elapsedTime = 0;
	}

	public void ResetTimer(float time)
	{
		this.time = time;
		elapsedTime = 0;
	}


}
