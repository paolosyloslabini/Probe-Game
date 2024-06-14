using UnityEngine;

[CreateAssetMenu(fileName = "EffectDamage", menuName = "Action/Simple/EffectDamage")]
public class ActionTakeDamage : ActionSO
{
    public float damageAmount;
    public override void Act(ThingBehaviour thing){
        base.Act(thing);
        thing.TakeDamage(10);
    }
}
