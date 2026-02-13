
namespace SevenLib.Stylus;

public struct StylusButtonState
{
    public uint Value;

    public const uint TipMask = 0x0001;
    public const uint LowerMask = 0x0002;
    public const uint UpperMask = 0x0004;
    public const uint BarrelMask = 0x0008; // Included for completeness, though not explicitly used in original TabletSession logic yet

    public StylusButtonState(uint initialState)
    {
        Value = initialState;
    }

    public bool IsTipDown => (Value & TipMask) != 0;
    public bool IsLowerButtonDown => (Value & LowerMask) != 0;
    public bool IsUpperButtonDown => (Value & UpperMask) != 0;
    public bool IsBarrelButtonDown => (Value & BarrelMask) != 0;

    
    public override string ToString() => Value.ToString();
}
