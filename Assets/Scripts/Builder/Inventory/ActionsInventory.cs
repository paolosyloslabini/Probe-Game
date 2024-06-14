using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ActionsInventory", menuName = "Inventory/ActionsInventory")]
public class ActionsInventory : Inventory<ActionSO>
{
    public void Filter(BuildingPieceType type)
    {
        throw new NotImplementedException();
    }

}
