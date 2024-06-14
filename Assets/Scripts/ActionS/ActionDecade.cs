using UnityEngine;

[CreateAssetMenu(fileName = "EffectDecade", menuName = "Action/Simple/EffectDecade")]
public class ActionDecade : ActionSO
{
    public float amount;
    public override void Act(ThingBehaviour thing){
        base.Act(thing);
        thing.Decade(amount);
    }
}