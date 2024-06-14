using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionWithCost", menuName = "Action/ActionWithCost")]
public class ActionWithCost : ActionWithCondition {
    public List<CostSO> costs;

    //TO DO = Use serializable dict

    [SerializeField] ResourceDict cost;
    public List<ActionSO> alternativeEffects;


    public override void WhenChecked(ThingBehaviour thing)
    {
        bool costCheck = PayCost(thing);
        //Debug.Log($"{thing}: Cost check for {this} : {costCheck}");
        if (costCheck) base.WhenChecked(thing);
        else DoAlternativeEffects(thing);
    }
    
    public void DoAlternativeEffects(ThingBehaviour thing)
    {
        foreach(ActionSO effect in alternativeEffects)
        {
            effect.Act(thing);
        }
    }

    public bool PayCost(ThingBehaviour thing)
    {
        Dictionary<ResourceType, float> totalCosts = cost.GetDict();

        //Calculate total cost
        foreach(CostSO cost in costs) //for each cost factor
        {
            Dictionary<ResourceType, float> tmpCosts = cost.GetCost(thing); //gets the costs in the cost factor
            foreach(ResourceType res in tmpCosts.Keys) //sums all the costs
            {
                if (totalCosts.ContainsKey(res)) totalCosts[res] += tmpCosts[res];
                else totalCosts[res] = tmpCosts[res];
            }
        }
        //Check that thing has each resource
        foreach(ResourceType res in totalCosts.Keys)
        {
            if (!thing.HasResource(res, totalCosts[res])) return false;
        }

        //Pay cost
        foreach(ResourceType res in totalCosts.Keys) thing.ConsumeResource(res, totalCosts[res]);
        
        return true;

    }
}