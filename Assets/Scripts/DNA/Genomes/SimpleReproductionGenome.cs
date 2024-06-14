using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//BodyDNA contains information to create and inizialize the body of the probe. It's a special DNAAttachment
[CreateAssetMenu(fileName = "SimpleReproductionGenome", menuName = "DNA/Genomes/SimpleReproductionGenome")]
public class SimpleReproductionGenome : ReproductionGenome
{    
    //Reproduction requirements
    [SerializeField] float resourceFraction = .2f;
    [SerializeField] int maxRadius = 1;
    [SerializeField] bool noDiagonal = true;
    [SerializeField] public ResourceDict maxResourcesDict;


    public override Reproducer MakePart(ThingBehaviour owner)
    {
        return new Reproducer(this, maxResourcesDict);
    }

    public override void NurtureChild(ThingBehaviour parent, ThingBehaviour child)
    {
        child.parent = parent;
        child.Container.GetResourceFrom(parent.Container, 0.5f, ResourceType.energy, fractional:true);
    }

    public bool CheckResources(ThingBehaviour owner, Reproducer reproducer)
    {
        var costDict = reproducer._buildingCost;
        Debug.Log($"Reproducer contains {reproducer.container.GetContentString()}");
        Debug.Log($"Building cost is energy: {costDict[ResourceType.energy]}, iron: {costDict[ResourceType.iron]}");

        foreach (var res in costDict.Keys) 
        {
            float fraction = reproducer.container.GetAmount(res)/costDict[res];
            if (fraction < 1) return false;
        }
        return true;
    }

    public void MoveResourcesToContainer(ThingBehaviour owner, Reproducer reproducer, float fraction)
    {
        foreach (KeyValuePair<ResourceType, float > kvp in reproducer._buildingCost)
        {
            reproducer.container.GetResourceFrom(owner.Container, kvp.Value*fraction, kvp.Key);
        }    
    }

    public override bool CanMakeChild(ThingBehaviour owner, Reproducer reproducer)
    {
        int availableCells = owner.CurrentCell.GetNeighbours(onlyAvailable: true, radius: maxRadius, noDiagonal: noDiagonal).Count;
        bool check = (availableCells > 0) && CheckResources(owner, reproducer);
        return check;
    }

    public override Dictionary<ResourceType, float> CalculateReproductiveCost(ThingBehaviour thing)
    {
        return thing.dna.blueprint.CalculateBuildingCost();
    }

    public override void Effect(ThingBehaviour thing){}

    public override ThingBehaviour MakeChild(ThingBehaviour parent, Reproducer reproducer, bool activate = false)
    {
        Cell neighbourCell = parent.CurrentCell.GetRandomNeighbour(onlyAvailable: true, radius: maxRadius, noDiagonal: noDiagonal);
        if (neighbourCell == null) return null;
        Debug.Log($"Making a child in {neighbourCell}");
        ThingBehaviour child = spawner.Spawn(neighbourCell, GetChildDNA(parent));
        NurtureChild(parent, child);
        if (activate) child.Active = true;
        return child;
    }

    internal override bool Reproduce(ThingBehaviour thing, Reproducer reproducer)
    {
        bool canMakeChild = CanMakeChild(thing, reproducer);
        if (!canMakeChild) 
        {
            MoveResourcesToContainer(thing, reproducer, resourceFraction);
            return false; 
        }
        reproducer.container.RemoveResources(reproducer._buildingCost);
        var child = MakeChild(thing, reproducer, activate: true);
        if (child == null) return false;
        else return true;        
    }
}

