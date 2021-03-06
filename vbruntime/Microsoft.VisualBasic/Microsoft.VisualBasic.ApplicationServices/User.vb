'
' Microsoft.VisualBasic.ApplicationServices.User.cs
'
' Authors:
'   Miguel de Icaza (miguel@novell.com)
'   Mizrahi Rafael (rafim@mainsoft.com)
'   Rolf Bjarne Kvinge  (RKvinge@novell.com)
'
' Copyright (C) 2006-2007 Novell (http://www.novell.com)
'
' Permission is hereby granted, free of charge, to any person obtaining
' a copy of this software and associated documentation files (the
' "Software"), to deal in the Software without restriction, including
' without limitation the rights to use, copy, modify, merge, publish,
' distribute, sublicense, and/or sell copies of the Software, and to
' permit persons to whom the Software is furnished to do so, subject to
' the following conditions:
' 
' The above copyright notice and this permission notice shall be
' included in all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
' EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
' MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
' NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
' LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
' OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
' WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
'

Imports System
Imports System.Globalization
Imports System.Threading
Imports System.Security.Principal
Imports System.ComponentModel

Namespace Microsoft.VisualBasic.ApplicationServices
    Public Class User

        Public Sub New()
            MyBase.New()
        End Sub

        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        Public Sub InitializeWithWindowsUser()
            Throw New NotImplementedException
        End Sub

        Public Function IsInRole(ByVal role As BuiltInRole) As Boolean
            Throw New NotImplementedException
        End Function

        Public Function IsInRole(ByVal role As String) As Boolean
            Throw New NotImplementedException
        End Function

        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        Public Property CurrentPrincipal() As IPrincipal
            Get
                Throw New NotImplementedException
            End Get
            Set(ByVal value As IPrincipal)
                Throw New NotImplementedException
            End Set
        End Property

        Protected Overridable Property InternalPrincipal() As IPrincipal
            Get
                Throw New NotImplementedException
            End Get
            Set(ByVal value As IPrincipal)
                Throw New NotImplementedException
            End Set
        End Property

        Public ReadOnly Property IsAuthenticated() As Boolean
            Get
                Throw New NotImplementedException
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Throw New NotImplementedException
            End Get
        End Property
    End Class

End Namespace

