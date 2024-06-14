public delegate void FloatModifier(VariableFloat var);

public class VariableFloat
{
    float value;
    FloatModifier modifiers;

    public void AddModifier(FloatModifier mod) { modifiers += mod; }
    public float Value(float baseValue)
    {
        value = baseValue;
        modifiers?.Invoke(this);
        return value;
    }
}
