using UnityEngine;

[CreateAssetMenu(fileName = "EffectMoveRandom", menuName = "Action/Simple/EffectMoveRandom")]
public class EffectMoveRandom : ActionSO
{
    public override void Act(ThingBehaviour thing){
        base.Act(thing);
        Cell cell = thing.CurrentCell.GetRandomNeighbour();
        if (cell == null) return;
        thing.JumpTo(cell);
    }
}