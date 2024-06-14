using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public enum BuildingPieceType {head, module, body, top, side, bottom}

[CreateAssetMenu(fileName = "BuildingPiece", menuName = "Building/BuildingPiece")]
public class BuildingPieceSO : ScriptableObject, IInventorable
{
    //IInventorable_______________________________________________
    [field: SerializeField] public virtual Sprite Sprite { get; set; }
    [field: SerializeField] public virtual string Name { get; set; }
    [field: SerializeField] public virtual string Description { get; set; }
    //_______________________________________________________________

    public List<ActionTag> AllowedActionTags;
    public BuildingPieceType buildingPieceType;
    public List<Genome> genomesList;
    public List<ActionSO> passiveActions;
    [Serialize] public ResourceDict cost;

    public virtual void Apply(DNA dna)
    {
        Debug.Log($"Applying {this} to {dna}");
        foreach (var genome in genomesList) dna.genericGenomes.Add(genome);
        foreach (var action in passiveActions) dna.passiveActionsSOs.Add(action);
    }
}

public class BuildingPieceWithModules : BuildingPieceSO
{
    public List<BuildingPieceSO> modules;
    public override void Apply(DNA dna)
    {
        foreach(var module in modules) module.Apply(dna);
        base.Apply(dna);
    }
}
