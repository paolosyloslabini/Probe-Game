
//Simple objects that attach to the genomes. They are create by AttachmentDNA
public abstract class GenomeObject
{
    public GenomeObject()
    {
    }

    public abstract void Apply(ThingBehaviour thing);

    public abstract void DeApply(ThingBehaviour thing);
}
