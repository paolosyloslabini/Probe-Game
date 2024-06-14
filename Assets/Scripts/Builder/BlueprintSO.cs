using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Blueprint", menuName = "Building/Blueprints/Blueprint")]
public class BlueprintSO : ScriptableObject, IInventorable
//The BlueprintSO is a frame for the Blueprint object. It sets the allowed pieces (through the Slot SOs), manages the object contruction and update.
{

    //IInventorable
    [field: SerializeField] public Sprite Sprite { get; set; }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public string Description { get; set; }
    //______________

    public BodyGenome bodyGenome;
    public ContainerGenome containerGenome;
    public List<PieceSlot> pieceSlots;
    public List<ActionSlot> actionSlots;
    public List<ActionTag> allowedActionTags;
    internal string defaultName = "Basic Blueprint";

    //TODO Visualizer

    internal void Apply(DNA dna, Blueprint blueprint)
    {
        dna.bodyGenome = bodyGenome;
        dna.containerGenome = containerGenome;
        foreach (var piece in blueprint.pieces)
        {
            piece.Apply(dna);
        }

        //TODO APPLY ACTIONS
        dna.activeActionsSOs.AddRange(blueprint.activeActions);
    }

    public bool SetPiece(Blueprint blueprint, int idx, BuildingPieceSO piece = null)
    {
        if (piece == null) return false;
        
        idx %= pieceSlots.Count;
        PieceSlot slot = pieceSlots[idx];
        
        bool canFit = slot.CanFit(piece);
        
        if (!canFit) return false;
        blueprint.pieces[idx] = piece;
        return true;
    }

    internal BuildingPieceSO UnsetPiece(Blueprint blueprint, int slotIdx)
    {
        slotIdx %= blueprint.pieces.Count;
        BuildingPieceSO oldPiece = blueprint.pieces[slotIdx];
        Debug.Log($"Unsetting {oldPiece} from idx {slotIdx} for the default {pieceSlots[slotIdx].defaultValue}");
        blueprint.pieces[slotIdx] = pieceSlots[slotIdx].defaultValue;
        return oldPiece;
    }

    internal List<BuildingPieceSO> GetDefaultPieces()
    {
        var list = new List<BuildingPieceSO>();
        foreach (var slot in pieceSlots) list.Add(slot.defaultValue);
        return list;
    }

    internal List<ActionSO> GetDefaultActions()
    {
        var list = new List<ActionSO>();
        foreach (var slot in actionSlots) list.Add(slot.defaultValue);
        return list;
    }

    
    public bool SetAction(Blueprint blueprint, int idx, ActionSO action = null)
    {
        if (action == null) return false;
        
        idx %= actionSlots.Count;
        ActionSlot slot = actionSlots[idx];
        
        bool canFit = slot.CanFit(action);
        bool isAllowed = canFit && action.IsAllowed(blueprint);
        Debug.Log($"Attaching {action}. SLOT FIT: {canFit}. BLUEPRINT FIT: {isAllowed}");
        if (!(canFit&&isAllowed)) return false;
        blueprint.activeActions[idx] = action;
        return true;
    }

    internal ActionSO UnsetAction(Blueprint blueprint, int slotIdx)
    {
        slotIdx %= blueprint.activeActions.Count;
        ActionSO oldAction = blueprint.activeActions[slotIdx];
        Debug.Log($"Unsetting {oldAction} from idx {slotIdx} for the default {actionSlots[slotIdx].defaultValue}");
        blueprint.activeActions[slotIdx] = actionSlots[slotIdx].defaultValue;
        return oldAction;
    }

    internal List<ActionTag> GetAllowedActionTags(Blueprint blueprint)
    {
        var tags = new List<ActionTag>();
        tags.AddRange(allowedActionTags);
        foreach(var piece in blueprint.pieces)
        {
            tags.AddRange(piece.AllowedActionTags);
        }
        return tags;
    }
}
