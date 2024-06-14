using UnityEngine;

[CreateAssetMenu(fileName = "StubPiece", menuName = "Building/StubPiece")]
public class StubPieceSO : BuildingPieceSO
{
    void OnValidate()
    {
        Name = buildingPieceType + " stub";
        Description = $"An attachment point for pieces of type {buildingPieceType}";
    }
}
