using System;
using UnityEngine;


public abstract class Slot<T> : ScriptableObject
{
    public string Name = "defaultSlot";
    public T defaultValue;
    public abstract bool CanFit(T piece);
    public bool Validate() => CanFit(defaultValue);
    void OnValidate()
    {
        if (!Validate()) throw new InvalidOperationException($"Slot {this}: default {defaultValue} does not fit the criteria");
        Debug.Log($"Slot {this} validated");
    }
}

public abstract class PieceSlot : Slot<BuildingPieceSO>
{
}

public abstract class ActionSlot : Slot<ActionSO>{}
