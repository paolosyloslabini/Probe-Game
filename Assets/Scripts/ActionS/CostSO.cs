using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CostSO : ScriptableObject {

    public abstract Dictionary<ResourceType,float> GetCost(ThingBehaviour thing);

    public abstract bool PayCost(ThingBehaviour thing);
    
}