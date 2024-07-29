using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerInteractable))]
public class SafeGround : MonoBehaviour, ISafe
{
	[SerializeField] Transform safeGroundPosition;
	public Vector3 Position { get { return safeGroundPosition.position; } }


	void Start()
	{
		var triggerInteractable = GetComponent<TriggerInteractable>();
		triggerInteractable.OnEnterAction += (interactor) =>
		{
			var player = interactor.GetComponent<Player>();
			if(player != null)
			{
				PrepareSafe(player);
			}
		};
	}

	public void PrepareSafe(Player player)
	{
		player.safe = this;
	}

	private void OnDrawGizmos()
	{
		if (safeGroundPosition != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(safeGroundPosition.position, 0.5f);
		}
	}

	public IPlayerState BackToSafe(Player player)
	{
		var aligner = player.GetComponent<Aligner>();
		var mover = player.GetComponent<Mover>();

		return aligner.Align(safeGroundPosition.position, mover);
	}
}
