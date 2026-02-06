// See copright.md for copyright information.

using System;

namespace WinTabDN.WinForms;

/// <summary>
/// Support for registering a Native Windows message with MessageEvents class.
/// </summary>
public class MessageReceivedEventArgs : EventArgs
{
    private readonly System.Windows.Forms.Message _message;

    /// <summary>
    /// MessageReceivedEventArgs constructor.
    /// </summary>
    /// <param name="message">Native windows message to be registered.</param>
    public MessageReceivedEventArgs(System.Windows.Forms.Message message) { _message = message; }

    /// <summary>
    /// Return native Windows message handled by this object.
    /// </summary>
    public System.Windows.Forms.Message Message { get { return _message; } }
}
