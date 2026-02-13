using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace SevenLib.WinTab.WinForms;

/// <summary>
/// Windows native message handler, to provide support for detecting and
/// responding to Wintab messages. 
/// </summary>
public static partial class MessageEvents
{
    private static readonly object _lock = new object();
    private static MessageWindow _window;
    private static IntPtr _windowHandle;
    private static SynchronizationContext _context;

    /// <summary>
    /// MessageEvents delegate.
    /// </summary>
    public static event EventHandler<MessageReceivedEventArgs> MessageReceived;

    /// <summary>
    /// Registers to receive the specified native Windows message.
    /// </summary>
    /// <param name="message">Native Windows message to watch for.</param>
    public static void WatchMessage(int message)
    {
        EnsureInitialized();
        _window.RegisterEventForMessage(message);
    }

    /// <summary>
    /// Returns the MessageEvents native Windows handle.
    /// </summary>
    public static IntPtr WindowHandle
    {
        get
        {
            EnsureInitialized();
            return _windowHandle;
        }
    }


    public static void ClearMessageEvents()
    {
        MessageReceived = null;
    }

    private static void EnsureInitialized()
    {
        lock (_lock)
        {
            if (_window == null)
            {
                _context = AsyncOperationManager.SynchronizationContext;
                
                var tcs = new TaskCompletionSource<IntPtr>();
                var t = new Thread(() =>
                {
                    // Create the native window on this thread
                    var win = new MessageWindow();
                    _window = win;
                    
                    // Signal the handle is ready
                    tcs.SetResult(win.Handle);

                    // Start the standard Windows message loop
                    System.Windows.Forms.Application.Run();
                });
                
                t.Name = "MessageEvents message loop";
                t.IsBackground = true;
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                // Wait for the window creation to complete before returning
                _windowHandle = tcs.Task.GetAwaiter().GetResult();
            }
        }
    }
}
