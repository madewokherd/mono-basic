Module ExplicitConversionUShortToBoolean1
    Function Main() As Integer
        Dim result As Boolean
        Dim value1 As UShort
        Dim value2 As Boolean
        Dim const2 As Boolean

        value1 = 40US
        value2 = CBool(value1)
        const2 = CBool(40US)

        result = value2 = const2

        If result = False Then
            System.Console.WriteLine("FAIL ExplicitConversionUShortToBoolean1")
            Return 1
        End If
    End Function
End Module
