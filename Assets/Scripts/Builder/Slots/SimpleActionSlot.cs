using UnityEngine;

[CreateAssetMenu(fileName = "SimpleActionSlot", menuName = "Builder/SimpleActionSlot")]
public class SimpleActionSlot : ActionSlot
{
    public override bool CanFit(ActionSO piece) => true;
}
