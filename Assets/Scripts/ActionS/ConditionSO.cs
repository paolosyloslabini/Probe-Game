using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionSO : ScriptableObject{
    //e.g Cell_Has_Iron
    public abstract bool Check(ThingBehaviour thing);
}
