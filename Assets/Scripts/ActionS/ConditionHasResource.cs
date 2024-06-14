using UnityEngine;

[CreateAssetMenu(fileName = "CellHasResource", menuName = "Condition/CellHasResource")]
public class ConditionHasResource : ConditionSO
{
    public ResourceType resourceType;

    public override bool Check(ThingBehaviour thing)
    {
        return thing.CurrentCell.container.GetAmount(resourceType) > 0;
    }
}