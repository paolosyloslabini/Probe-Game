using UnityEngine;

[CreateAssetMenu(fileName = "HasFreeSpaceAround", menuName = "Condition/HasFreeSpaceAround")]
public class ConditionFreeSpace : ConditionSO
{
    public int radius;

    public override bool Check(ThingBehaviour thing)
    {
        return thing.CurrentCell.GetRandomNeighbour() != null;
    }
}