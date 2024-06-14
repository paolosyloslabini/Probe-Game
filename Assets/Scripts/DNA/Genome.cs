using System.Net.Mail;
using UnityEngine;

//Part of the immutable DNA. They are used to initialize the probe
public abstract class Genome : ScriptableObject 
{
    public abstract void Apply(ThingBehaviour thing);
}

public abstract class GenomeWithGeneralObj<T> : Genome
{    
    public abstract T MakePart(ThingBehaviour owner);
}


public interface IGenomeStruct
{
    public void Initialize(ThingBehaviour thing);
}

public abstract class GenomeWithStruct<T> : GenomeWithGeneralObj<T> where T : IGenomeStruct
{    
    [SerializeField] T variables;

    public override T MakePart(ThingBehaviour owner)
    {
        T newSTruct = variables;
        newSTruct.Initialize(owner);
        return newSTruct;
    }
}


public abstract class GenomeWithAttachment<T> : GenomeWithGeneralObj<T> where T : GenomeObject
{    

    public override void Apply(ThingBehaviour thing)
    {
        GenomeObject newAttachment = MakePart(thing);
        thing.body.Attachments.Add(newAttachment);
        newAttachment.Apply(thing);
        Effect(thing);
    }

    public abstract void Effect(ThingBehaviour thing); //effects are called at the end of Apply
}