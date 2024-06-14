using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EffectReproduce", menuName = "Action/Simple/EffectReproduce")]
public class EffectReproduce : ActionSO
{
    public override void Act(ThingBehaviour thing){
        base.Act(thing);
        thing.Reproduce();
    }
}