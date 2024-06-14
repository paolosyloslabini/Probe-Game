using UnityEngine;

[CreateAssetMenu(fileName = "RootGenome", menuName = "DNA/Attachments/RootGenome")]
public class RootGenome : Genome //similar to Actions
{
    [SerializeField] float AddToRootSurface;
    public override void Apply(ThingBehaviour thing)
    {
        thing.body.baseRootSurface += AddToRootSurface;
    }
}

