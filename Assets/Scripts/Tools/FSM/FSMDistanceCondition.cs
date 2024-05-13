using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FSMDistanceCondition : FSMCondition
{
    [SerializeField][Range(0, 50)] float distance;
    [SerializeField] bool trueWhenCloser = true;

    [SerializeField] Transform target;
    [SerializeField] Transform self;

    public FSMDistanceCondition(Transform target, Transform self, float distance, bool trueWhenCloser = true) : base((Func<bool>)null)
    {
        this.target = target;
        this.self = self;
        this.distance = distance;
        this.trueWhenCloser = trueWhenCloser;
    }

    public FSMDistanceCondition(FSMCondition toReverse) : base(toReverse)
    {
        throw new Exception("FSMDistanceCondition: Reverse condition not supported");
    }

    public FSMDistanceCondition(FSMCondition a, FSMCondition b, ConditionType conditionType = ConditionType.AND) : base(a, b, conditionType)
    {
        throw new Exception("FSMDistanceCondition: ConditionType not supported");
    }


    public override bool Check()
    {
        if (trueWhenCloser)
        {
            return Vector3.Distance(target.position, self.position) < distance;
        }
        else
        {
            return Vector3.Distance(target.position, self.position) > distance;
        }
    }
}
