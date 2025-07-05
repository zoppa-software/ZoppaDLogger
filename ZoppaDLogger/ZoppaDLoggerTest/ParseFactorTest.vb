Imports System
Imports Xunit
Imports ZoppaDLogger
Imports ZoppaDLogger.Analysis
Imports ZoppaDLogger.Strings
Imports ZoppaDLogger.Analysis.AnalysisValue

Public Class ParseFactorTest

    <Fact>
    Public Sub TestParseNumverFactor()
        Dim result1 = Analysis.ParserModule.Executes("123.456")
        Assert.Equal(123.456, result1.Expression.GetValue(Nothing).Number)

        Dim result2 = Analysis.ParserModule.Executes("0.001")
        Assert.Equal(0.001, result2.Expression.GetValue(Nothing).Number)

        Dim result3 = Analysis.ParserModule.Executes("1000")
        Assert.Equal(1000.0, result3.Expression.GetValue(Nothing).Number)

        Dim result4 = Analysis.ParserModule.Executes("0")
        Assert.Equal(0.0, result4.Expression.GetValue(Nothing).Number)

        Dim result5 = Analysis.ParserModule.Executes("123_456.789")
        Assert.Equal(123456.789, result5.Expression.GetValue(Nothing).Number)
    End Sub

    <Fact>
    Public Sub TestParseStringFactor()
        Dim result1 = Analysis.ParserModule.Executes("""Hello, World!""")
        Assert.True(result1.Expression.GetValue(Nothing).Str.Equals("Hello, World!"))

        Dim result2 = Analysis.ParserModule.Executes("""12345""")
        Assert.True(result2.Expression.GetValue(Nothing).Str.Equals("12345"))

        Dim result3 = Analysis.ParserModule.Executes("""Test String with spaces""")
        Assert.True(result3.Expression.GetValue(Nothing).Str.Equals("Test String with spaces"))

        Dim result4 = Analysis.ParserModule.Executes("""Special characters !@#$%^&*()""")
        Assert.True(result4.Expression.GetValue(Nothing).Str.Equals("Special characters !@#$%^&*()"))

        Dim result5 = Analysis.ParserModule.Executes("""\""""")
        Assert.True(result5.Expression.GetValue(Nothing).Str.Equals(""""))

        Dim result6 = Analysis.ParserModule.Executes("'\''")
        Assert.True(result6.Expression.GetValue(Nothing).Str.Equals("'"))
    End Sub

    <Fact>
    Public Sub TestParseBoolFactor()
        Dim result1 = Analysis.ParserModule.Executes("true")
        Assert.True(result1.Expression.GetValue(Nothing).Bool)
        Dim result2 = Analysis.ParserModule.Executes("false")
        Assert.False(result2.Expression.GetValue(Nothing).Bool)
    End Sub

    <Fact>
    Public Sub TestParseUnaryFactor()
        Dim result1 = Analysis.ParserModule.Executes("-123.456")
        Assert.Equal(-123.456, result1.Expression.GetValue(Nothing).Number)
        Dim result2 = Analysis.ParserModule.Executes("+123.456")
        Assert.Equal(123.456, result2.Expression.GetValue(Nothing).Number)
        Dim result3 = Analysis.ParserModule.Executes("not true")
        Assert.False(result3.Expression.GetValue(Nothing).Bool)
        Dim result4 = Analysis.ParserModule.Executes("not false")
        Assert.True(result4.Expression.GetValue(Nothing).Bool)
    End Sub

    <Fact>
    Public Sub TestParseParenFactor()
        Dim result1 = Analysis.ParserModule.Executes("(123.456)")
        Assert.Equal(123.456, result1.Expression.GetValue(Nothing).Number)
        Dim result2 = Analysis.ParserModule.Executes("(true)")
        Assert.True(result2.Expression.GetValue(Nothing).Bool)
        Dim result3 = Analysis.ParserModule.Executes("(-123.456)")
        Assert.Equal(-123.456, result3.Expression.GetValue(Nothing).Number)
        Dim result4 = Analysis.ParserModule.Executes("(not false)")
        Assert.True(result4.Expression.GetValue(Nothing).Bool)
    End Sub

    <Fact>
    Public Sub TestParseArrayFactor()
        Dim result1 = Analysis.ParserModule.Executes("[1, 2, 3]")
        Dim arrayValue1 = result1.Expression.GetValue(Nothing).Array
        Assert.Equal(3, arrayValue1.Length)
        Assert.Equal(1.0, arrayValue1(0).Number)
        Assert.Equal(2.0, arrayValue1(1).Number)
        Assert.Equal(3.0, arrayValue1(2).Number)
        Dim result2 = Analysis.ParserModule.Executes("[true, false, true]")
        Dim arrayValue2 = result2.Expression.GetValue(Nothing).Array
        Assert.Equal(3, arrayValue2.Length)
        Assert.True(arrayValue2(0).Bool)
        Assert.False(arrayValue2(1).Bool)
        Assert.True(arrayValue2(2).Bool)
        Dim result3 = Analysis.ParserModule.Executes("['a', 'b', 'c']")
        Dim arrayValue3 = result3.Expression.GetValue(Nothing).Array
        Assert.Equal(3, arrayValue3.Length)
        Assert.True(arrayValue3(0).Str.Equals("a"))
        Assert.True(arrayValue3(1).Str.Equals("b"))
        Assert.True(arrayValue3(2).Str.Equals("c"))

        Assert.Throws(Of Analysis.AnalysisException)(
            Sub()
                Analysis.ParserModule.Executes("[1, , 3,]")
            End Sub
        )
    End Sub

    <Fact>
    Public Sub TestFunctionCall()
        Dim venv As New Analysis.AnalysisEnvironment()
        venv.AddFunction(
            U8String.NewString("test"),
            Function(name)
                Return U8String.NewString("こんにちは! ").Concat(name.Str).ToStringValue()
            End Function
        )
        Dim result1 = Analysis.ParserModule.Executes("test('崇')")
        Assert.True(result1.Expression.GetValue(venv).Str.Equals("こんにちは! 崇"))
    End Sub

    <Fact>
    Public Sub TestVariableFactor()
        Dim venv As New Analysis.AnalysisEnvironment()

        venv.RegistNumber("x", 42)
        Dim result1 = Analysis.ParserModule.Executes("x")
        Assert.Equal(42.0, result1.Expression.GetValue(venv).Number)

        venv.RegistBool("y", True)
        Dim result2 = Analysis.ParserModule.Executes("y")
        Assert.True(result2.Expression.GetValue(venv).Bool)

        venv.RegistStr("z", "Hello")
        Dim result3 = Analysis.ParserModule.Executes("z")
        Assert.True(result3.Expression.GetValue(venv).Str.Equals("Hello"))
    End Sub

    <Fact>
    Public Sub TestArrayAccessFactor()
        Dim venv As New Analysis.AnalysisEnvironment()
        venv.RegistArray("arr", 1, 2, 3)
        Dim result1 = Analysis.ParserModule.Executes("arr[0]")
        Assert.Equal(1.0, result1.Expression.GetValue(venv).Number)
        Dim result2 = Analysis.ParserModule.Executes("arr[1]")
        Assert.Equal(2.0, result2.Expression.GetValue(venv).Number)
        Dim result3 = Analysis.ParserModule.Executes("arr[2]")
        Assert.Equal(3.0, result3.Expression.GetValue(venv).Number)
    End Sub

    Class Test
        Public Property Name As String
        Public Property Age As Integer
        Public Property IsActive As Boolean
        Public Property Scores As Integer()
    End Class

    <Fact>
    Public Sub TestFieldAccessFactor()
        Dim venv As New Analysis.AnalysisEnvironment()
        Dim testObj As New Test With {
            .Name = "崇",
            .Age = 49,
            .IsActive = True,
            .Scores = New Integer() {90, 80, 70}
        }
        venv.RegistObject("testObj", testObj)
        Dim result1 = Analysis.ParserModule.Executes("testObj.Name")
        Assert.True(result1.Expression.GetValue(venv).Str.Equals("崇"))
        Dim result2 = Analysis.ParserModule.Executes("testObj.Age")
        Assert.Equal(49, result2.Expression.GetValue(venv).Number)
        Dim result3 = Analysis.ParserModule.Executes("testObj.IsActive")
        Assert.True(result3.Expression.GetValue(venv).Bool)
        Dim result4 = Analysis.ParserModule.Executes("testObj.Scores[0]")
        Assert.Equal(90.0, result4.Expression.GetValue(venv).Number)
    End Sub

End Class
