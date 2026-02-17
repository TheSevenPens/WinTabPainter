// See copright.md for copyright information.

using System;

namespace SevenLib.WinTab;

/// <summary>
/// Class to create and configure Wintab contexts.
/// </summary>
public static class CWintabContextFactory
{
    /// <summary>
    /// Returns the default system context or digitizer, with useful context overrides.
    /// </summary>
    /// <param name="cat">EWTICategoryIndex.WTI_DEFCONTEXT for digitizer context. EWTICategoryIndex.WTI_DEFSYSCTX for system context</param>
    /// <param name="options_I">caller's options; OR'd into context options</param>
    /// <returns>A configured WintabContext</returns>
    public static CWintabContext GetDefaultContext(Enums.EWTICategoryIndex cat, Enums.ECTXOptionValues options_I = 0)
    {
        // In the original code this is made up of two separate methods that
        // do almost the exact same thing. Merged them with the cat parameter
        // to indicate which kind of context to build

        // EWTICategoryIndex.WTI_DEFSYSCTX = System context
        // EWTICategoryIndex.WTI_DEFCONTEXT = Digitizer context

        if (cat != Enums.EWTICategoryIndex.WTI_DEFSYSCTX && cat != Enums.EWTICategoryIndex.WTI_DEFCONTEXT)
        {
            throw new System.ArgumentOutOfRangeException(nameof(cat));
        }

        var context = GetDefaultContextCore(cat);

        if (context == null)
        {
            return context;
        }

        // Add caller's options.
        context.Options |= (uint)options_I;

        if (cat == Enums.EWTICategoryIndex.WTI_DEFSYSCTX)
        {
            // Make sure we get data packet messages.
            context.Options |= (uint)Enums.ECTXOptionValues.CXO_MESSAGES;
        }

        // Send all possible data bits (not including extended data).
        // This is redundant with CWintabContext initialization, which
        // also inits with PK_PKTBITS_ALL.
        uint PACKETDATA = (uint)Enums.EWintabPacketBit.PK_PKTBITS_ALL;  // The Full Monty 
        uint PACKETMODE = (uint)Enums.EWintabPacketBit.PK_BUTTONS;

        // Set the context data bits.
        context.PktData = PACKETDATA;
        context.PktMode = PACKETMODE;
        context.MoveMask = PACKETDATA;
        context.BtnUpMask = context.BtnDnMask;

        // Name the context
        context.Name = (cat == Enums.EWTICategoryIndex.WTI_DEFSYSCTX) ? "SYSTEM CONTEXT" : "DIGITIZER CONTEXT";

        return context;
    }

    /// <summary>
    /// Helper function to get digitizing or system default context.
    /// </summary>
    /// <param name="contextIndex_I">Use WTI_DEFCONTEXT for digital context or WTI_DEFSYSCTX for system context</param>
    /// <returns>Returns the default context or null on error.</returns>
    private static CWintabContext GetDefaultContextCore(Enums.EWTICategoryIndex contextIndex_I)
    {
        var context = new CWintabContext();
        context.LogicalContext = Interop.WinTabFunctions.WTInfoAObject<Structs.WintabLogContext>((uint)contextIndex_I, 0);
        return context;
    }
}
