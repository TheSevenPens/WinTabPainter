using System;
using System.Collections.Generic;
using System.Threading;

namespace WintabDN.WinForms;

public static partial class MessageEvents
{
    private class MessageWindow : System.Windows.Forms.NativeWindow
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly Dictionary<int, bool> _messageSet = new Dictionary<int, bool>();

        public MessageWindow()
        {
            var cp = new System.Windows.Forms.CreateParams { Caption = "WintabDN Message Window" };
            CreateHandle(cp);
        }

        public void RegisterEventForMessage(int messageID)
        {
            _lock.EnterWriteLock();
            try
            {
                _messageSet[messageID] = true;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            bool handleMessage = false;
            _lock.EnterReadLock();
            try
            {
                handleMessage = _messageSet.ContainsKey(m.Msg);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            if (handleMessage)
            {
                MessageEvents._context?.Post(delegate (object state)
                {
                    EventHandler<MessageReceivedEventArgs> handler = MessageEvents.MessageReceived;
                    handler?.Invoke(null, new MessageReceivedEventArgs((System.Windows.Forms.Message)state));
                }, m);
            }

            base.WndProc(ref m);
        }
    }
}
