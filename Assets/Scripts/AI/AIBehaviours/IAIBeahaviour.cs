using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIBeahaviour
{
    public FSMState GetFSMState(Guard body);

    public FSMCondition DoneCondition { get; }
}
