using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectPanelEnergy", menuName = "Action/Simple/PanelEnergy")]
public class EffectPanelEnergy : ActionSO
{
    public override void Act(ThingBehaviour thing){
        base.Act(thing);
        float resourceAmount = thing.body.basePanelAbsorbtionRate*thing.body.basePanelSurface;

        //Debug.Log($"{thing} : getting {resourceAmount} {ResourceType.energy} with {thing.body.basePanelSurface} panel surface");
        thing.Container.AddResource(resourceAmount, ResourceType.energy);
    }
}