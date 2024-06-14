using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum ExtractMethods {surface, drill, scavenge, steal}


public class ThingBehaviour : MonoBehaviour
{


    //ACTIONS
    public Action<ResourceType, float, ThingBehaviour> OnGrow { get; internal set; }
    public Action<ThingBehaviour> OnDestroyed;
    public Action<ThingBehaviour> OnReproducing;
    public Action<int> OnActionTaken;

    //LOCATION
    [HideInInspector] public Cell CurrentCell {get; private set;}

    //MUTABLE PARTS
    [HideInInspector] public Container Container {get; private set;}
    [SerializeField] public Body body;
    public ThingBehaviour parent;

    //DNA
    public DNA dna;

    public void Initialize(DNA dna)
    {
        this.dna = dna;
        body = dna.bodyGenome.MakePart(this);
        Container = dna.containerGenome.MakePart(this);
        Container.AddResource(3, ResourceType.energy);

        foreach (var attached in dna.genericGenomes)
        {
            attached.Apply(this);
        }
    }

    //UTILS

    private bool _active = false;
    public bool Active
    {
        get {return _active;}
        set {_active = value;
        if (value) StartCoroutine(ClockCaller());
        else StopAllCoroutines(); //todofix
        }
    }
    private int currentActionIdx = 0;



    //FUNCTIONS

    //Assign this entity to a terrain cell    
    public void AssignToCell(Cell cell)
    {
        //does not check availability!;
        //Debug.Log($"{this}: Assigning to cell {cell}");
        CurrentCell = cell;
        CurrentCell.Attach(this);
        gameObject.transform.position = cell.WorldPosition + new Vector3(0, cell.height*1.5f, 0);
    }

    public void DetachFromCell()
    {
        //Debug.Log($"{this}: Detaching from cell {myCell}");
        if (CurrentCell != null) CurrentCell.Detach(this);
        CurrentCell = null;
        gameObject.transform.position = new Vector3(10,10,10);
    }

    public bool JumpTo(Cell cell)
    {
        if (cell == null || !cell.available) return false; //could be redundant
        DetachFromCell();
        AssignToCell(cell);
        return true;
    }


    //Call the next clock iteration on this entity
    public void Clock()
    {
        DoAction();
    }

    void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
        DetachFromCell();
    }

    public void DoAction()
    {
        //Do all passive action. 
        foreach (ActionSO action in dna.passiveActionsSOs)
        {
            action.Act(this);
        }
        
        //Do current active action.
        if (dna.activeActionsSOs.Count > 0)
        {
            dna.activeActionsSOs[currentActionIdx].Act(this);

            //Advance the state to next non-passive action
            currentActionIdx++;
            currentActionIdx %= dna.activeActionsSOs.Count;
            OnActionTaken?.Invoke(currentActionIdx);
        }
    }

    IEnumerator ClockCaller()
    {
        while (true)
        {
            Clock();
        // Wait for one second (Unity measures time in seconds)
            yield return new WaitForSeconds(1);
        }
    }


    void Start()
    {
    }

    public float GetResourceFromCell(float amount, ResourceType resourceType, Cell cell = null)
    {
        if (cell == null) cell = CurrentCell;
        if (cell == null) throw new InvalidOperationException("Trying to extract without a cell");
        float extracted = Container.GetResourceFrom(cell.container, amount, resourceType);
        //Debug.Log($"Extracted {extracted} {resourceType} from current cell");
        return extracted;
    }
    
    public string GetInfoString()
    {
        string info = $"{this}\n";
        info += "CONTAINS: \n";
        info += Container.GetContentString();
        info += CurrentCell.GetInfoString();
        return info;
    }

    internal bool HasResource(ResourceType res, float amount)
    {
        if (amount == 0) return true;
        return Container.GetAmount(res) >= amount;
    }

    internal void ConsumeResource(ResourceType res, float amount)
    {
        Container.RemoveResource(amount, res);
    }

    public void Grow(ResourceType resource, float amount)
    {
        OnGrow?.Invoke(resource, amount, this);
    }

    public void Reproduce()
    {
        OnReproducing?.Invoke(this);
    }

    internal void Decade(float amount)
    {
        Debug.Log($"Decading by {amount}");
    }

    internal void TakeDamage(float amount)
     {
        Debug.Log($"Took {amount} damage!");
     }
}

public enum ContainerType{Reproduction, Main}