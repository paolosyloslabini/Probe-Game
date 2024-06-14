using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum LogicCondition {AND_CONDITION, OR_CONDITION};

public enum ActionTag{DRILL,SCAVENGE,BREATHE,REPRODUCE,PANEL,WALK}

public abstract class ActionSO : ScriptableObject, IInventorable{
    
    //IInventorable
    [field: SerializeField] public Sprite Sprite { get; set; }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public string Description { get; set; }
    //______________


    [SerializeField] public List<ActionTag> actionTags;

    public bool IsAllowed(Blueprint blueprint)
    {
        //check that each actionTag is allowed by the blueprint
        foreach (var tag in actionTags)
        {
            if (!blueprint.GetAllowedActionTags().Contains(tag)) return false;
        }
        return true;
    }


    private void OnEnable()
    {
        // Ensure default sprite is set in the editor
        if (Sprite == null) Sprite = Resources.Load<Sprite>("Sprites/actionImg");

        if (Name == "") Name = name;

        if (Description == "") Description = "Action lacks a description.";
    
    }

    public virtual void Act(ThingBehaviour thing)
    {
        //Debug.Log($"{thing}: Action {this} called");
    }
}

public abstract class ActionWithCondition : ActionSO {
    public List<ConditionSO> conditions;
    public List<ActionSO> effects;

    public LogicCondition logic; //either AND or OR, decides how to evaluate conditions

    public override void Act(ThingBehaviour thing){
        base.Act(thing);
        bool conditionCheck = CheckConditions(thing);
        //Debug.Log($"{thing}: Action {this} Check state: {conditionCheck}");
        if (conditionCheck) WhenChecked(thing);
    }

    public bool CheckConditions(ThingBehaviour thing)
    {
        if (logic == LogicCondition.AND_CONDITION)
        {
            foreach (ConditionSO condition in conditions)
            {
                if (!condition.Check(thing)) return false;
            }
        }

        if (logic == LogicCondition.OR_CONDITION)
        {
            bool checkValue = false;
            foreach (ConditionSO condition in conditions)
            {
                if (condition.Check(thing)) checkValue = true;
            }
            if (!checkValue) return false;
        }
        return true;
    }

    public virtual void WhenChecked(ThingBehaviour thing)
    {
        DoEffects(thing);
    }

    public void DoEffects(ThingBehaviour thing)
    {
        foreach(ActionSO effect in effects)
        {
            effect.Act(thing);
        }
    }
}