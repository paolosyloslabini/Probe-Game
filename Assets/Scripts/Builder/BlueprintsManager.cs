using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "BlueprintsManager", menuName = "Builder/BlueprintsManager")]
public class BlueprintsManager : ScriptableObject
{


    //Stored Blueprints Manager
    public List<Blueprint> savedBlueprints;

    //action manager
    public List<ActionSO> activeActionsSOs; 


    //blueprint manager and creator
    [SerializeField] private List<BlueprintSO> buildableBlueprintSOs;
    [SerializeField] private GlobalBlueprint _currentBlueprintGlobal;


    //*******************************
    //***********METHODS*************
    //*******************************

    //Blueprint manager and creator  
    public void StartNewBlueprint(BlueprintSO blueprintSO)
    {
        _currentBlueprintGlobal.Value = new Blueprint(blueprintSO);
        Debug.Log($"Starting a new Blueprint from {blueprintSO}");
    }
    
    //___________________________________________________________________________________

    //Stored Blueprints Manager
    public Blueprint LoadBlueprint(int saveIdx, bool saveCurrent = false)
    {
        if (savedBlueprints == null || savedBlueprints.Count == 0) throw new InvalidOperationException("No saved blueprint");
        if (saveIdx != saveIdx%savedBlueprints.Count) throw new InvalidOperationException("Index out of bound");
        return new Blueprint(savedBlueprints[saveIdx]);
    }
    public void LoadBlueprintToCurrent(int saveIdx) => _currentBlueprintGlobal.Value = LoadBlueprint(saveIdx);

    public void SaveBlueprint(Blueprint blueprint, int saveIdx)
    {
        if (saveIdx < 0) return;
        Blueprint newBlueprint = new Blueprint(blueprint);
        if (savedBlueprints.Count == 0 || saveIdx >= savedBlueprints.Count)
        {
            savedBlueprints.Add(newBlueprint);
            return;
        }
        savedBlueprints[saveIdx] = newBlueprint;
    }
    public void SaveBlueprint(int Idx) => SaveBlueprint(_currentBlueprintGlobal.Value, Idx);
    //___________________________________________________________________________________


}