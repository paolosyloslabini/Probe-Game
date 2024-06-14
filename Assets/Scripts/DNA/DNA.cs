using System;
using System.Collections.Generic;
using UnityEngine;

//DNA is an immutable part of probes. It contains information to initialize a new probe, and information to run the probe
//DNA contains genomes, immutable parts that may create genomeObjects
[CreateAssetMenu(fileName = "dna", menuName = "DNA/dna")]
public class DNA : ScriptableObject
{

    //todo Point to parent/child mutation for cool tree
    public BodyGenome bodyGenome;
    public ContainerGenome containerGenome;
    public List<Genome> genericGenomes = new(); //order matters
    public List<ActionSO> passiveActionsSOs = new();
    public List<ActionSO> activeActionsSOs = new(); 
    public Blueprint blueprint;

}

