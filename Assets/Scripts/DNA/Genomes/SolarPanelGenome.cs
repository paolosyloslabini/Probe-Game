using UnityEngine;

[CreateAssetMenu(fileName = "SolarAttachment", menuName = "DNA/Attachments/SimpleSolar")]
public class SolarAttachment : Genome //similar to Actions
{
    [SerializeField] float AddToPanelSurface;
    [SerializeField] float MultiplyPanelAbsorbtionRate;
    public override void Apply(ThingBehaviour thing)
    {
        thing.body.basePanelSurface += AddToPanelSurface;
        thing.body.basePanelAbsorbtionRate *= MultiplyPanelAbsorbtionRate;
    }
}

