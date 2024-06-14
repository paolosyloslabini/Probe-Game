using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

//BodyGenome contains information to create and inizialize the body of the probe. It's a special DNAAttachment
[CreateAssetMenu(fileName = "BodyGenome", menuName = "DNA/Genomes/Body/BodyGenome")]
public class BodyGenome : GenomeWithStruct<Body>
{
    public override void Apply(ThingBehaviour thing){}
}


//The Body contains basic info on the probe composition. It's the object created by BodyDNA 
//It's the FIRST initialized by the DNA when a new Thing is created
[System.Serializable]
public struct Body : IGenomeStruct
{

    [HideInInspector] public ThingBehaviour owner;
    public float basePanelSurface;
    public float basePanelAbsorbtionRate;
    public float baseRootSurface;
    public float BaseSurfaceAbsorbtionRate;
    public float baseDrillRate;
    public float baseDrillStrength;
    public float BaseScavengeRate;
    public float BaseStealingRate;
    public List<GenomeObject> Attachments{get; private set;}

    public void Initialize(ThingBehaviour owner)
    {
        Attachments = new List<GenomeObject>();
        this.owner = owner;
    }

}