using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalThingBehaviour", menuName = "GlobalVariables/GlobalThingBehaviour")]
public class GlobalThingBehaviour : GlobalVariable<ThingBehaviour>
{
    public Action<ThingBehaviour> OnDestroyed;

    void OnEnable()
    {
        Subscribe(Value);
        OnValueChangedWithHistory += UpdateSubscriptions;
    }

    void UpdateSubscriptions(ThingBehaviour oldValue, ThingBehaviour newValue)
    {
        Unsubscribe(oldValue);
        Subscribe(newValue);
    }

    void Subscribe(ThingBehaviour thing)
    {
        if (thing == null) return; 
        thing.OnDestroyed += NotifyChange;
        thing.OnDestroyed += DestroyedHandler;
    }

    void Unsubscribe(ThingBehaviour thing)
    {
        if (thing == null) return;
        thing.OnDestroyed -= NotifyChange;
        thing.OnDestroyed -= DestroyedHandler;
    }

    void DestroyedHandler(ThingBehaviour thing)
    {
        OnDestroyed?.Invoke(thing);
    }
}
