using System;
using UnityEngine;


[CreateAssetMenu(fileName = "PieceInventory", menuName = "Inventory/PieceInventory")]
public class PiecesInventory : Inventory<BuildingPieceSO>
{
    public void Filter(BuildingPieceType type)
    {
        throw new NotImplementedException();
    }

}
