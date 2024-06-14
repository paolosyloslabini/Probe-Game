using UnityEngine;

[CreateAssetMenu(fileName = "EffectCollect", menuName = "Action/Simple/EffectCollect")]
public class ActionCollectResource : ActionSO
{
    public ResourceType resourceType;
    public float resourceAmount;
    public override void Act(ThingBehaviour thing){
        base.Act(thing);
        thing.GetResourceFromCell(resourceAmount, resourceType);
    }
}