namespace SevenLib.WinTab.Stylus;

public class StylusUtils
{

    public static SevenLib.Stylus.StylusButtonState  Update(SevenLib.Stylus.StylusButtonState state, StylusButtonChange change)
    {
        uint _state = state.Value;

        if (change.Change == StylusButtonChangeType.Pressed)
        {
            if (change.ButtonId == SevenLib.Stylus.StylusButtonId.Tip)
            {
                _state |= SevenLib.Stylus.StylusButtonState.TipMask;
            }
            else if (change.ButtonId == SevenLib.Stylus.StylusButtonId.LowerButton)
            {
                _state |= SevenLib.Stylus.StylusButtonState.LowerMask;
            }
            else if (change.ButtonId == SevenLib.Stylus.StylusButtonId.UpperButton)
            {
                _state |= SevenLib.Stylus.StylusButtonState.UpperMask;
            }
            else if (change.ButtonId == SevenLib.Stylus.StylusButtonId.BarrelButton)
            {
                _state |= SevenLib.Stylus.StylusButtonState.BarrelMask;
            }
        }
        else if (change.Change == StylusButtonChangeType.Released)
        {
            if (change.ButtonId == SevenLib.Stylus.StylusButtonId.Tip)
            {
                _state &= ~SevenLib.Stylus.StylusButtonState.TipMask;
            }
            else if (change.ButtonId == SevenLib.Stylus.StylusButtonId.LowerButton)
            {
                _state &= ~SevenLib.Stylus.StylusButtonState.LowerMask;
            }
            else if (change.ButtonId == SevenLib.Stylus.StylusButtonId.UpperButton)
            {
                _state &= ~SevenLib.Stylus.StylusButtonState.UpperMask;
            }
            else if (change.ButtonId == SevenLib.Stylus.StylusButtonId.BarrelButton)
            {
                _state &= ~SevenLib.Stylus.StylusButtonState.BarrelMask;
            }
        }

        var new_state = new SevenLib.Stylus.StylusButtonState { Value = _state };
        return new_state;
    }
    
}
