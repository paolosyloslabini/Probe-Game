using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectDrill", menuName = "Action/Simple/EffectDrill")]
public class ActionDrillResource : ActionSO
{
    public ResourceType resourceType;
    public float minDrillStrength;
    public override void Act(ThingBehaviour thing){
        base.Act(thing);
        Body body = thing.body;
        float excessStrength = Math.Max(body.baseDrillStrength - minDrillStrength, 0);
        float resourceAmount = body.baseDrillRate*excessStrength;

        //Debug.Log($"{thing} : extracting {resourceAmount} {resourceType} with {excessStrength} strength");
        thing.GetResourceFromCell(resourceAmount, resourceType);
    }
}