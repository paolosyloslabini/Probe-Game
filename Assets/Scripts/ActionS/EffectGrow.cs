using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectGrow", menuName = "Action/Simple/EffectGrow")]
public class EffectGrow : ActionSO
{
    public ResourceType resourceType;
    public float amount; 
    public override void Act(ThingBehaviour thing){
        base.Act(thing);
        thing.Grow(resourceType, amount);
    }
}