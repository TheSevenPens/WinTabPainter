///////////////////////////////////////////////////////////////////////////////
// MessageEvents.cs - native Windows message handling for WintabDN
//
// This code in this file is based on the example given at:
//  http://msdn.microsoft.com/en-us/magazine/cc163417.aspx
//  by Steven Toub.
//
// Copyright (c) 2010, Wacom Technology Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace WintabDN.WinForms;

public static partial class MessageEvents
{
    private class MessageWindow : Form
    {
        private ReaderWriterLock _lock = new ReaderWriterLock();
        private Dictionary<int, bool> _messageSet = new Dictionary<int, bool>();

        public void RegisterEventForMessage(int messageID)
        {
            _lock.AcquireWriterLock(Timeout.Infinite);
            _messageSet[messageID] = true;
            _lock.ReleaseWriterLock();
        }

        protected override void WndProc(ref Message m)
        {
            _lock.AcquireReaderLock(Timeout.Infinite);
            bool handleMessage = _messageSet.ContainsKey(m.Msg);
            _lock.ReleaseReaderLock();

            if (handleMessage)
            {
                MessageEvents._context.Post(delegate(object state)
                {
                    EventHandler<MessageReceivedEventArgs> handler = MessageEvents.MessageReceived;
                    if (handler != null) handler(null, new MessageReceivedEventArgs((Message)state));
                }, m);
            }

            base.WndProc(ref m);
        }
    }
}
