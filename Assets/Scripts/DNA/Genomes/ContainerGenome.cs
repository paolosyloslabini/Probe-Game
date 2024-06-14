using UnityEngine;


//ContainerDNA contains information to create and inizialize the container of the probe. It's a special DNAAttachment
[CreateAssetMenu(fileName = "ContainerGenome", menuName = "DNA/Genomes/Container/ContainerGenome")]
//It's the SECOND initialized by the DNA when a new Thing is created
public class ContainerGenome : GenomeWithGeneralObj<Container>
{
    [SerializeField] ResourceDict containerCapacityDict;
    public override void Apply(ThingBehaviour thing){}

    public override Container MakePart(ThingBehaviour owner)
    {
        return new Container(containerCapacityDict);
    }
}