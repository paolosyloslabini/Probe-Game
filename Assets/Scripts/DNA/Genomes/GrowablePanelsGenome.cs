using System;
using UnityEngine;


[CreateAssetMenu(fileName = "GrowablePanelsGenome", menuName = "DNA/Genomes/GrowablePanelsGenome")]
public class GrowablePanelsGenome : GenomeWithAttachment<GrowablePanelObject>
{
    [SerializeField] float growingRate;
    [SerializeField] ResourceType growingCostResource;
    [SerializeField] float maxSize;
    [SerializeField] float AddToBasePanelSurface;
    [SerializeField] float MultiplyPanelAbsorbtionRate;

    public override GrowablePanelObject MakePart(ThingBehaviour owner)
    {
        return new GrowablePanelObject(growingRate, growingCostResource, maxSize);
    }

    public override void Effect(ThingBehaviour thing)
    {
        thing.body.basePanelSurface += AddToBasePanelSurface;
        thing.body.basePanelAbsorbtionRate *= MultiplyPanelAbsorbtionRate;
    }
}


public class GrowablePanelObject : GenomeObject
{
    readonly float growingRate; //per unit of resource
    readonly ResourceType growingCostResource;

    float totalGrown = 0;
    readonly float maxSize;


    public GrowablePanelObject(float growingRate, ResourceType growingCostResource, float maxSize)
    {
        this.growingRate = growingRate;
        this.growingCostResource = growingCostResource;
        this.maxSize = maxSize;
    }

    public override void Apply(ThingBehaviour thing)
    {
        thing.OnGrow += Grow;
    }

    public override void DeApply(ThingBehaviour thing)
    {            
        thing.OnGrow -= Grow;
    }

    public void Grow(ResourceType resource, float amount, ThingBehaviour thing)
    {
        if (resource != growingCostResource) return;
        float toGrow = Math.Min(growingRate*amount, maxSize - totalGrown); //only grows up to maxSize cap
        thing.body.basePanelSurface += toGrow;
        totalGrown += toGrow;
        //Debug.Log($"Growing by {toGrow}. Current size: {thing.body.basePanelSurface}. TotalGrown: {totalGrown}");
        if (totalGrown >= maxSize) thing.OnGrow -= Grow;
    }
}
