using UnityEngine;

[CreateAssetMenu(fileName = "GlobalBlueprint", menuName = "GlobalVariables/Blueprint")]
public class GlobalBlueprint : GlobalVariable<Blueprint>
{
    public bool SetPiece(BuildingPieceSO piece, int slotIdx)
    {
        bool success = this.Value.SetPiece(piece, slotIdx);
        if (success) NotifyChange();
        return success;
    }

    public bool UnsetPiece(int slotIdx)
    {
        bool success = this.Value.UnsetPiece(slotIdx);
        if (success) NotifyChange();
        return success;
    }

    public bool SetAction(int slotIdx, ActionSO action)
    {
        bool success = this.Value.SetAction(slotIdx, action);
        if (success) NotifyChange();
        return success;
    }

    public bool UnsetAction(int slotIdx)
    {
        bool success = this.Value.UnsetAction(slotIdx);
        if (success) NotifyChange();
        return success;   
    }
}
