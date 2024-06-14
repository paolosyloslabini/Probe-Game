using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Blueprint
//The blueprint struct stores the pieces that compose the probe. 
//The blueprintSO regulates its behaviours and fixes the allowed pieces.
{
    public string name;
    [SerializeField] internal List<BuildingPieceSO> pieces;
    [SerializeField] internal List<ActionSO> activeActions; 
    [SerializeField] internal BlueprintSO blueprintSO;

    public Blueprint(BlueprintSO blueprintSO)
    {
        this.name = blueprintSO.defaultName;
        this.blueprintSO = blueprintSO;
        pieces = new List<BuildingPieceSO>(blueprintSO.GetDefaultPieces());
        activeActions = new List<ActionSO>(blueprintSO.GetDefaultActions());

    }

    public Blueprint(Blueprint original)
    {
        name = original.name;
        blueprintSO = original.blueprintSO;
        pieces = new List<BuildingPieceSO>(original.pieces.Count);
        foreach (BuildingPieceSO piece in original.pieces)
            pieces.Add(piece);

        activeActions = new List<ActionSO>();
        foreach (var act in original.activeActions)
            activeActions.Add(act);
    }

    public void Apply(DNA dna) => blueprintSO.Apply(dna, this);
    public bool SetPiece(BuildingPieceSO piece, int idx) => blueprintSO.SetPiece(this, idx, piece);

    public BuildingPieceSO GetPiece(int idx)
    {
        idx %= pieces.Count;
        return pieces[idx];
    }

    internal DNA MakeDNA()
    {
        DNA newDNA = ScriptableObject.CreateInstance<DNA>();
        newDNA.blueprint = new Blueprint(this);
        newDNA.blueprint.Apply(newDNA);
        return newDNA;
    }

    internal BuildingPieceSO UnsetPiece(int slotIdx) => blueprintSO.UnsetPiece(this, slotIdx);

    internal bool SetAction(int slotIdx, ActionSO action) => blueprintSO.SetAction(this, slotIdx, action);

    internal ActionSO UnsetAction(int slotIdx) => blueprintSO.UnsetAction(this, slotIdx);

    internal List<ActionTag> GetAllowedActionTags() => blueprintSO.GetAllowedActionTags(this);

    public Dictionary<ResourceType, float> CalculateBuildingCost()
    {
        Dictionary<ResourceType, float> costs = new();
        foreach (var piece in pieces)
        {
            var resDict = piece.cost.GetDict();
            foreach(var kvp in resDict)
            {
                var resource = kvp.Key;
                var amount = kvp.Value;
                if (costs.ContainsKey(resource)) costs[resource] += amount;
                else costs[resource] = amount;
            }
        }
        return costs;
    }
}