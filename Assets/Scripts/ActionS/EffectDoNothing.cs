using UnityEngine;

[CreateAssetMenu(fileName = "EffectDoNothing", menuName = "Action/Simple/EffectDoNothing")]
public class EffectDoNothing : ActionSO
{
    public string DEBUG_STRING;
    public override void Act(ThingBehaviour thing){
        //Debug.Log($"{thing} is doing NOTHING. DEBUG_STRING: {DEBUG_STRING}");
    }
}