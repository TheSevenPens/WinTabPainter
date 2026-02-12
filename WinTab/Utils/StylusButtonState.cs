
namespace WinTab.Utils;

public struct StylusButtonState
{
    private uint _state;

    private const uint TipMask = 0x0001;
    private const uint LowerMask = 0x0002;
    private const uint UpperMask = 0x0004;
    private const uint BarrelMask = 0x0008; // Included for completeness, though not explicitly used in original TabletSession logic yet

    public StylusButtonState(uint initialState)
    {
        _state = initialState;
    }

    public bool IsTipDown => (_state & TipMask) != 0;
    public bool IsLowerButtonDown => (_state & LowerMask) != 0;
    public bool IsUpperButtonDown => (_state & UpperMask) != 0;
    public bool IsBarrelButtonDown => (_state & BarrelMask) != 0;


    public void Update(StylusButtonChange change)
    {
        if (change.Change == StylusButtonChangeType.Pressed)
        {
            if (change.ButtonId == StylusButtonId.Tip)
            {
                _state |= TipMask;
            }
            else if (change.ButtonId == StylusButtonId.LowerButton)
            {
                _state |= LowerMask;
            }
            else if (change.ButtonId == StylusButtonId.UpperButton)
            {
                _state |= UpperMask;
            }
            else if (change.ButtonId == StylusButtonId.BarrelButton)
            {
                _state |= BarrelMask;
            }
        }
        else if (change.Change == StylusButtonChangeType.Released)
        {
            if (change.ButtonId == StylusButtonId.Tip)
            {
                _state &= ~TipMask;
            }
            else if (change.ButtonId == StylusButtonId.LowerButton)
            {
                _state &= ~LowerMask;
            }
            else if (change.ButtonId == StylusButtonId.UpperButton)
            {
                _state &= ~UpperMask;
            }
            else if (change.ButtonId == StylusButtonId.BarrelButton)
            {
                _state &= ~BarrelMask;
            }
        }
    }
    
    public override string ToString() => _state.ToString();
}
