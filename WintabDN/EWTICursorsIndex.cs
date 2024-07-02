///////////////////////////////////////////////////////////////////////////////
// CWintabFuncs.cs - Wintab32 function wrappers for WintabDN
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

namespace WintabDN
{
    /// <summary>
    /// Index values for WTI_CURSORS.
    /// </summary>
    public enum EWTICursorsIndex
    {
         CSR_NAME            = 1,
         CSR_ACTIVE			 = 2,
         CSR_PKTDATA		 = 3,
         CSR_BUTTONS		 = 4,
         CSR_BUTTONBITS		 = 5,
         CSR_BTNNAMES		 = 6,
         CSR_BUTTONMAP		 = 7,
         CSR_SYSBTNMAP		 = 8,
         CSR_NPBUTTON		 = 9,
         CSR_NPBTNMARKS		 = 10,
         CSR_NPRESPONSE		 = 11,
         CSR_TPBUTTON        = 12,
         CSR_TPBTNMARKS		 = 13,
         CSR_TPRESPONSE		 = 14,
         CSR_PHYSID			 = 15,
         CSR_MODE			 = 16,
         CSR_MINPKTDATA		 = 17,
         CSR_MINBUTTONS		 = 18,
         CSR_CAPABILITIES	 = 19,
         CSR_TYPE			 = 20
    }

}
