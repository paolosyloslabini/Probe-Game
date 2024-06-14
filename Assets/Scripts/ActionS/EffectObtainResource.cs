using UnityEngine;

[CreateAssetMenu(fileName = "EffectObtain", menuName = "Action/Simple/EffectObtain")]
public class EffectObtainResource : ActionSO
{
    public ResourceType resourceType;
    public float resourceAmount;
    public override void Act(ThingBehaviour thing){
        base.Act(thing);
        thing.Container.AddResource(resourceAmount, resourceType);
        Debug.Log($"Adding {resourceAmount} {resourceType}");
    }
}