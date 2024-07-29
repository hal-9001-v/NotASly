
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
	public IPlayerState Next { get; }

	/// <summary>
	/// Checks wether it makes sense to switch to this state. It will return the next state if it makes sense to switch(or null if it doesnt).
	/// It might be an intermediary state or this state itself.
	/// </summary>
	public IPlayerState Check();

	public void Enter();

	public void Exit();

}
