' 
' Visual Basic.Net Compiler
' Copyright (C) 2004 - 2010 Rolf Bjarne Kvinge, RKvinge@novell.com
' 
' This library is free software; you can redistribute it and/or
' modify it under the terms of the GNU Lesser General Public
' License as published by the Free Software Foundation; either
' version 2.1 of the License, or (at your option) any later version.
' 
' This library is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
' Lesser General Public License for more details.
' 
' You should have received a copy of the GNU Lesser General Public
' License along with this library; if not, write to the Free Software
' Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
' 

Public Class CDecExpression
    Inherits ConversionExpression

    Sub New(ByVal Parent As ParsedObject, ByVal Expression As Expression)
        MyBase.New(Parent, Expression)
    End Sub

    Sub New(ByVal Parent As ParsedObject, ByVal IsExplicit As Boolean)
        MyBase.New(Parent, IsExplicit)
    End Sub

    Protected Overrides Function GenerateCodeInternal(ByVal Info As EmitInfo) As Boolean
        Return GenerateCode(Me, Info)
    End Function

    Protected Overrides Function ResolveExpressionInternal(ByVal Info As ResolveInfo) As Boolean
        Dim result As Boolean = True

        result = MyBase.ResolveExpressionInternal(Info) AndAlso result

        result = Validate(Info, Me) AndAlso result

        Return result
    End Function

    Public Overrides Function GetConstant(ByRef result As Object, ByVal ShowError As Boolean) As Boolean
        If Not Expression.GetConstant(result, ShowError) Then Return False
        Return ConvertToDecimal(result, ShowError)
    End Function

    Shared Function Validate(ByVal Info As ResolveInfo, ByVal Conversion As ConversionExpression) As Boolean
        Dim result As Boolean = True

        Dim expType As Mono.Cecil.TypeReference = Nothing
        Dim expTypeCode As TypeCode
        Dim Expression As Expression = Conversion.Expression
        Dim ExpressionType As TypeReference = Conversion.ExpressionType

        result = ValidateForNullable(Info, Conversion, expTypeCode, expType) AndAlso result

        If Conversion.GetConstant(Nothing, False) Then Return result

        Select Case expTypeCode
            Case TypeCode.DateTime, TypeCode.Char
                Info.Compiler.Report.ShowMessage(Messages.VBNC30311, Expression.Location, Helper.ToString(Expression, expType), Helper.ToString(Expression, ExpressionType))
                result = False
            Case TypeCode.Object
                If Helper.CompareType(expType, Info.Compiler.TypeCache.System_Object) Then
                    'OK
                ElseIf Helper.CompareType(expType, Info.Compiler.TypeCache.Nothing) Then
                    'OK
                Else
                    result = Conversion.FindUserDefinedConversionOperator() AndAlso result
                End If
            Case TypeCode.Single, TypeCode.Decimal, TypeCode.Double, TypeCode.UInt64, TypeCode.Int64, TypeCode.UInt32, TypeCode.Int32, TypeCode.Int16, TypeCode.Byte, TypeCode.SByte, TypeCode.UInt16
                'Implicitly convertible
            Case Else
                If Conversion.IsExplicit = False AndAlso Conversion.Location.File(Conversion.Compiler).IsOptionStrictOn Then
                    result = Conversion.Compiler.Report.ShowMessage(Messages.VBNC30512, Conversion.Location, Helper.ToString(Conversion, expType), Helper.ToString(Conversion, ExpressionType)) AndAlso result
                End If
        End Select

        Return result
    End Function

    Overloads Shared Function GenerateCode(ByVal Conversion As ConversionExpression, ByVal Info As EmitInfo) As Boolean
        Dim result As Boolean = True
        Dim expType As Mono.Cecil.TypeReference = Nothing
        Dim expTypeCode As TypeCode
        Dim Expression As Expression = Conversion.Expression

        result = GenerateCodeForExpression(Conversion, Info, expTypeCode, expType) AndAlso result

        Select Case expTypeCode
            Case TypeCode.Boolean
                Emitter.EmitCall(Info, Info.Compiler.TypeCache.MS_VB_CS_Conversions__ToDecimal_Boolean)
            Case TypeCode.Decimal
                'Nothing to do
            Case TypeCode.DateTime, TypeCode.Char
                Info.Compiler.Report.ShowMessage(Messages.VBNC30311, Expression.Location, Helper.ToString(Expression, expType), Helper.ToString(Expression, expType))
                result = False
            Case TypeCode.SByte, TypeCode.Int16
                Emitter.EmitConv_I4_Overflow(Info, expType)
                Emitter.EmitNew(Info, Info.Compiler.TypeCache.System_Decimal__ctor_Int32)
            Case TypeCode.Int32
                Emitter.EmitNew(Info, Info.Compiler.TypeCache.System_Decimal__ctor_Int32)
            Case TypeCode.Int64
                Emitter.EmitNew(Info, Info.Compiler.TypeCache.System_Decimal__ctor_Int64)
            Case TypeCode.Byte, TypeCode.UInt16, TypeCode.UInt32, TypeCode.UInt64
                Emitter.EmitConv_U8_Overflow_Underflow(Info, expType)
                Emitter.EmitNew(Info, Info.Compiler.TypeCache.System_Decimal__ctor_UInt64)
            Case TypeCode.Double
                Emitter.EmitNew(Info, Info.Compiler.TypeCache.System_Decimal__ctor_Double)
            Case TypeCode.Single
                Dim constant As Object = Nothing
                If Expression.GetConstant(constant, False) Then
                    'VBC BUG? This seems to be a bug in vbc.exe.
                    Emitter.EmitLoadDecimalValue(Info, New Decimal(CDbl(constant)))
                Else
                    'CORRECT CODE.
                    Emitter.EmitNew(Info, Info.Compiler.TypeCache.System_Decimal__ctor_Single)
                End If
            Case TypeCode.Object
                If Helper.CompareType(expType, Info.Compiler.TypeCache.System_Object) Then
                    Emitter.EmitCall(Info, Info.Compiler.TypeCache.MS_VB_CS_Conversions__ToDecimal_Object)
                ElseIf Helper.CompareType(expType, Info.Compiler.TypeCache.Nothing) Then
                    Emitter.EmitCall(Info, Info.Compiler.TypeCache.MS_VB_CS_Conversions__ToDecimal_Object)
                Else
                    Return Info.Compiler.Report.ShowMessage(Messages.VBNC99997, Expression.Location)
                End If
            Case TypeCode.String
                Emitter.EmitCall(Info, Info.Compiler.TypeCache.MS_VB_CS_Conversions__ToDecimal_String)
            Case Else
                Return Info.Compiler.Report.ShowMessage(Messages.VBNC99997, Expression.Location)
        End Select

        Return result
    End Function

    Overrides ReadOnly Property ExpressionType() As Mono.Cecil.TypeReference
        Get
            Return Compiler.TypeCache.System_Decimal
        End Get
    End Property
End Class

