using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine.UIElements;

//ReproductionGenome contains information to create and inizialize the reproductive set of the probe. 
//It's a special Genome that attaches a struct to the probe (the reproducer, defined below). this is used for non-immutable aspects of reproduction (e.g materials)
//It's the THIRD initialized by the DNA when a new Thing is created

public abstract class ReproductionGenome : GenomeWithAttachment<Reproducer>
{    
    public SpawnerSO spawner;
    public abstract Dictionary<ResourceType,float> CalculateReproductiveCost(ThingBehaviour owner);
    public abstract bool CanMakeChild(ThingBehaviour owner, Reproducer reproducer); //counts amounts of possible offsprings

    public abstract void NurtureChild(ThingBehaviour owner, ThingBehaviour child);

    public DNA GetChildDNA(ThingBehaviour owner) => owner.dna;

    public abstract ThingBehaviour MakeChild(ThingBehaviour parent, Reproducer reproducer, bool activate = false);

    internal abstract bool Reproduce(ThingBehaviour thing, Reproducer reproducer);
}


//The Reproducer contains basic info on the probe reproductive system. It's the object created by ReproductionGenome
//it has a container to store genetic stuff
[Serializable]
public class Reproducer : GenomeObject
{
    public readonly Container container;
    readonly ReproductionGenome _reproductionGenome;
    public Dictionary<ResourceType, float> _buildingCost;

    public Reproducer(ReproductionGenome reproductionGenome, ResourceDict maxResourcesDict)
    {
        _reproductionGenome = reproductionGenome;
        container = new(maxResourcesDict);
        _buildingCost = new();
    }

    public override void Apply(ThingBehaviour thing)
    {
        thing.OnReproducing += Reproduce;
        _buildingCost = _reproductionGenome.CalculateReproductiveCost(thing);
    }

    public override void DeApply(ThingBehaviour thing)
    {
        thing.OnReproducing -= Reproduce;
    }

    public void Reproduce(ThingBehaviour thing)
    {
        _reproductionGenome.Reproduce(thing, this);
    }

}

