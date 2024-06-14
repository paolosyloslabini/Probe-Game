using UnityEngine;

[CreateAssetMenu(fileName = "DrillAttachment", menuName = "DNA/Attachments/SimpleDrill")]
public class DrillAttachment : Genome //similar to Actions
{
    [SerializeField] float AddToDrillRate;
    [SerializeField] float AddToDrillStrength;
    public override void Apply(ThingBehaviour thing)
    {
        thing.body.baseDrillRate += AddToDrillRate;
        thing.body.baseDrillStrength += AddToDrillStrength;
    }
}

