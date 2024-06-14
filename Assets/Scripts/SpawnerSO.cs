using System;
using UnityEngine;
using UnityEngine.WSA;

[CreateAssetMenu(fileName = "Spawner", menuName = "Spawner")]
public class SpawnerSO : ScriptableObject
{
    public ThingBehaviour defaultPrefab;
    public DNA basicDna;
    public GlobalBlueprint currentGlobalBlueprint;

    internal ThingBehaviour Spawn(Cell cell, DNA dna, bool activate = false)
    {
        ThingBehaviour thing = Instantiate(defaultPrefab);
        thing.Initialize(dna);
        thing.AssignToCell(cell);
        if (activate) thing.Active = true;
        thing.Container.AddResource(2f, ResourceType.energy);
        return thing;
    }

    internal ThingBehaviour SpawnCurrentSelection(Cell cell, bool activate = false) => Spawn(cell, currentGlobalBlueprint.Value.MakeDNA(), activate);

    internal ThingBehaviour SpawnBasic(Cell cell, bool activate = false) => Spawn(cell, basicDna);

}

