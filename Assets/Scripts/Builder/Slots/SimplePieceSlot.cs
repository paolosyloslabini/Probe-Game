using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceSlot", menuName = "Builder/PieceSlot")]
public class SimplePieceSlot : PieceSlot
{
    public BuildingPieceType acceptedType;
    public override bool CanFit(BuildingPieceSO piece)
    {
        if (piece == null) throw new InvalidOperationException("Null piece provided");
        return piece.buildingPieceType == acceptedType;
    }
}