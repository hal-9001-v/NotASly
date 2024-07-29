using System;
using UnityEditorInternal;

public class PlayerTransition
{
    public PlayerTransition(Func<bool> check, IPlayerState state)
    {
        Check = check;
		State = state;
    }

    public Func<bool> Check { get; private set; }
	public IPlayerState State { get; private set; }
}
