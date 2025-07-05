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

    <Fact>
    Public Sub TestCalc()
        ' 数値の乗算解析をテスト
        Dim result1 = Analysis.ParserModule.Executes("123.456 * 2")
        Assert.Equal(246.912, result1.Expression.GetValue(Nothing).Number)
        Dim result2 = Analysis.ParserModule.Executes("0.001 * 1000")
        Assert.Equal(1.0, result2.Expression.GetValue(Nothing).Number)
        Dim result3 = Analysis.ParserModule.Executes("1000 * 0")
        Assert.Equal(0.0, result3.Expression.GetValue(Nothing).Number)
        Dim result4 = Analysis.ParserModule.Executes("123_456.789 * 2")
        Assert.Equal(246913.578, result4.Expression.GetValue(Nothing).Number)

        ' 数値の除算解析をテスト
        Dim result5 = Analysis.ParserModule.Executes("123.456 / 2")
        Assert.Equal(61.728, result5.Expression.GetValue(Nothing).Number)
        Dim result6 = Analysis.ParserModule.Executes("0.001 / 1000")
        Assert.Equal(0.000001, result6.Expression.GetValue(Nothing).Number)
        Dim result7 = Analysis.ParserModule.Executes("1000 / 2")
        Assert.Equal(500.0, result7.Expression.GetValue(Nothing).Number)
        Dim result8 = Analysis.ParserModule.Executes("123_456.789 / 3")
        Assert.Equal(41152.263, result8.Expression.GetValue(Nothing).Number)
        Dim result9 = Analysis.ParserModule.Executes("1000 / 0")
        Assert.Throws(Of DivideByZeroException)(Function() result9.Expression.GetValue(Nothing))

        ' 数値の加算解析をテスト
        Dim result10 = Analysis.ParserModule.Executes("123.456 + 2")
        Assert.Equal(125.456, result10.Expression.GetValue(Nothing).Number)
        Dim result11 = Analysis.ParserModule.Executes("0.001 + 1000")
        Assert.Equal(1000.001, result11.Expression.GetValue(Nothing).Number)
        Dim result12 = Analysis.ParserModule.Executes("1000 + 0")
        Assert.Equal(1000.0, result12.Expression.GetValue(Nothing).Number)
        Dim result13 = Analysis.ParserModule.Executes("123_456.789 + 2")
        Assert.Equal(123458.789, result13.Expression.GetValue(Nothing).Number)

        ' 文字列の加算解析をテスト
        Dim resultString1 = Analysis.ParserModule.Executes("""Hello"" + "" World!""")
        Assert.True(resultString1.Expression.GetValue(Nothing).Str.Equals("Hello World!"))

        ' 数値の減算解析をテスト
        Dim result14 = Analysis.ParserModule.Executes("123.456 - 2")
        Assert.Equal(121.456, result14.Expression.GetValue(Nothing).Number)
        Dim result15 = Analysis.ParserModule.Executes("0.001 - 1000")
        Assert.Equal(-999.999, result15.Expression.GetValue(Nothing).Number)
        Dim result16 = Analysis.ParserModule.Executes("1000 - 0")
        Assert.Equal(1000.0, result16.Expression.GetValue(Nothing).Number)
        Dim result17 = Analysis.ParserModule.Executes("123_456.789 - -2")
        Assert.Equal(123458.789, result17.Expression.GetValue(Nothing).Number)
    End Sub

    <Fact>
    Public Sub TestEquals()
        ' 等価演算子の解析をテスト
        Dim result1 = Analysis.ParserModule.Executes("123.456 == 123.456")
        Assert.True(result1.Expression.GetValue(Nothing).Bool)
        Dim result2 = Analysis.ParserModule.Executes("123.456 == 123.457")
        Assert.False(result2.Expression.GetValue(Nothing).Bool)
        Dim result3 = Analysis.ParserModule.Executes("0.001 == 0.001")
        Assert.True(result3.Expression.GetValue(Nothing).Bool)
        Dim result4 = Analysis.ParserModule.Executes("1000 == 1000")
        Assert.True(result4.Expression.GetValue(Nothing).Bool)

        ' 文字列の等価演算子の解析をテスト
        Dim resultString1 = Analysis.ParserModule.Executes("""Hello"" == ""Hello""")
        Assert.True(resultString1.Expression.GetValue(Nothing).Bool)
        Dim resultString2 = Analysis.ParserModule.Executes("""Hello"" == ""World!""")
        Assert.False(resultString2.Expression.GetValue(Nothing).Bool)
        Dim resultString3 = Analysis.ParserModule.Executes("""Hello"" == ""Hello World!""")
        Assert.False(resultString3.Expression.GetValue(Nothing).Bool)

        ' 真偽値の等価演算子の解析をテスト
        Dim resultBool1 = Analysis.ParserModule.Executes("true == true")
        Assert.True(resultBool1.Expression.GetValue(Nothing).Bool)
        Dim resultBool2 = Analysis.ParserModule.Executes("true == false")
        Assert.False(resultBool2.Expression.GetValue(Nothing).Bool)
        Dim resultBool3 = Analysis.ParserModule.Executes("false == false")
        Assert.True(resultBool3.Expression.GetValue(Nothing).Bool)

        ' 配列の等価演算子の解析をテスト
        Dim resultArray1 = Analysis.ParserModule.Executes("[1, 2, 3] == [1, 2, 3]")
        Assert.True(resultArray1.Expression.GetValue(Nothing).Bool)
        Dim resultArray2 = Analysis.ParserModule.Executes("[1, 2, 3] == [4, 5, 6]")
        Assert.False(resultArray2.Expression.GetValue(Nothing).Bool)
        Dim resultArray3 = Analysis.ParserModule.Executes("[1, 2, 3] == [1, 2, 3, 4]")
        Assert.False(resultArray3.Expression.GetValue(Nothing).Bool)
        Dim resultArray4 = Analysis.ParserModule.Executes("[1, 2, 3] == [1, 2]")
        Assert.False(resultArray4.Expression.GetValue(Nothing).Bool)
    End Sub

    <Fact>
    Public Sub TestNotEquals()
        ' 非等価演算子の解析をテスト
        Dim result1 = Analysis.ParserModule.Executes("123.456 <> 123.457")
        Assert.True(result1.Expression.GetValue(Nothing).Bool)
        Dim result2 = Analysis.ParserModule.Executes("123.456 <> 123.456")
        Assert.False(result2.Expression.GetValue(Nothing).Bool)
        Dim result3 = Analysis.ParserModule.Executes("0.001 <> 0.002")
        Assert.True(result3.Expression.GetValue(Nothing).Bool)
        Dim result4 = Analysis.ParserModule.Executes("1000 <> 999")
        Assert.True(result4.Expression.GetValue(Nothing).Bool)
        ' 文字列の非等価演算子の解析をテスト
        Dim resultString1 = Analysis.ParserModule.Executes("""Hello"" <> ""World!""")
        Assert.True(resultString1.Expression.GetValue(Nothing).Bool)
        Dim resultString2 = Analysis.ParserModule.Executes("""Hello"" <> ""Hello""")
        Assert.False(resultString2.Expression.GetValue(Nothing).Bool)
        ' 真偽値の非等価演算子の解析をテスト
        Dim resultBool1 = Analysis.ParserModule.Executes("true <> false")
        Assert.True(resultBool1.Expression.GetValue(Nothing).Bool)
        Dim resultBool2 = Analysis.ParserModule.Executes("true <> true")
        Assert.False(resultBool2.Expression.GetValue(Nothing).Bool)
        ' 配列の非等価演算子の解析をテスト
        Dim resultArray1 = Analysis.ParserModule.Executes("[1, 2, 3] <> [4, 5, 6]")
        Assert.True(resultArray1.Expression.GetValue(Nothing).Bool)
        Dim resultArray2 = Analysis.ParserModule.Executes("[1, 2, 3] <> [1, 2, 3]")
        Assert.False(resultArray2.Expression.GetValue(Nothing).Bool)
    End Sub

    <Fact>
    Public Sub TestGreaterThan()
        ' 大なり演算子の解析をテスト
        Dim result1 = Analysis.ParserModule.Executes("123.456 > 123.455")
        Assert.True(result1.Expression.GetValue(Nothing).Bool)
        Dim result2 = Analysis.ParserModule.Executes("123.456 > 123.456")
        Assert.False(result2.Expression.GetValue(Nothing).Bool)
        Dim result3 = Analysis.ParserModule.Executes("0.001 > 0.000999")
        Assert.True(result3.Expression.GetValue(Nothing).Bool)
        Dim result4 = Analysis.ParserModule.Executes("1000 > 999")
        Assert.True(result4.Expression.GetValue(Nothing).Bool)
        ' 文字列の大なり演算子の解析をテスト
        Dim resultString1 = Analysis.ParserModule.Executes("""Hello"" > ""Hello""")
        Assert.False(resultString1.Expression.GetValue(Nothing).Bool)
        Dim resultString2 = Analysis.ParserModule.Executes("""Hello"" > ""World!""")
        Assert.False(resultString2.Expression.GetValue(Nothing).Bool)
    End Sub

    <Fact>
    Public Sub TestLessThan()
        ' 小なり演算子の解析をテスト
        Dim result1 = Analysis.ParserModule.Executes("123.456 < 123.457")
        Assert.True(result1.Expression.GetValue(Nothing).Bool)
        Dim result2 = Analysis.ParserModule.Executes("123.456 < 123.456")
        Assert.False(result2.Expression.GetValue(Nothing).Bool)
        Dim result3 = Analysis.ParserModule.Executes("0.001 < 0.001001")
        Assert.True(result3.Expression.GetValue(Nothing).Bool)
        Dim result4 = Analysis.ParserModule.Executes("999 < 1000")
        Assert.True(result4.Expression.GetValue(Nothing).Bool)
        ' 文字列の小なり演算子の解析をテスト
        Dim resultString1 = Analysis.ParserModule.Executes("""Hello"" < ""World!""")
        Assert.True(resultString1.Expression.GetValue(Nothing).Bool)
        Dim resultString2 = Analysis.ParserModule.Executes("""World!"" < ""Hello""")
        Assert.False(resultString2.Expression.GetValue(Nothing).Bool)
    End Sub

    <Fact>
    Public Sub TestGreaterThanOrEqual()
        ' 大なりイコール演算子の解析をテスト
        Dim result1 = Analysis.ParserModule.Executes("123.456 >= 123.456")
        Assert.True(result1.Expression.GetValue(Nothing).Bool)
        Dim result2 = Analysis.ParserModule.Executes("123.456 >= 123.455")
        Assert.True(result2.Expression.GetValue(Nothing).Bool)
        Dim result3 = Analysis.ParserModule.Executes("0.001 >= 0.001")
        Assert.True(result3.Expression.GetValue(Nothing).Bool)
        Dim result4 = Analysis.ParserModule.Executes("1000 >= 999")
        Assert.True(result4.Expression.GetValue(Nothing).Bool)
    End Sub

    <Fact>
    Public Sub TestLessThanOrEqual()
        ' 小なりイコール演算子の解析をテスト
        Dim result1 = Analysis.ParserModule.Executes("123.456 <= 123.456")
        Assert.True(result1.Expression.GetValue(Nothing).Bool)
        Dim result2 = Analysis.ParserModule.Executes("123.456 <= 123.457")
        Assert.True(result2.Expression.GetValue(Nothing).Bool)
        Dim result3 = Analysis.ParserModule.Executes("0.001 <= 0.001")
        Assert.True(result3.Expression.GetValue(Nothing).Bool)
        Dim result4 = Analysis.ParserModule.Executes("999 <= 1000")
        Assert.True(result4.Expression.GetValue(Nothing).Bool)
    End Sub

    <Fact>
    Public Sub TestLogicalOperators()
        ' 論理演算子の解析をテスト
        Dim result1 = Analysis.ParserModule.Executes("true and false")
        Assert.False(result1.Expression.GetValue(Nothing).Bool)
        Dim result2 = Analysis.ParserModule.Executes("true or false")
        Assert.True(result2.Expression.GetValue(Nothing).Bool)
        Dim result3 = Analysis.ParserModule.Executes("not true")
        Assert.False(result3.Expression.GetValue(Nothing).Bool)
        Dim result4 = Analysis.ParserModule.Executes("true xor false")
        Assert.True(result4.Expression.GetValue(Nothing).Bool)
    End Sub

    <Fact>
    Public Sub TestTemaryOperators()
        ' 三項演算子の解析をテスト
        Dim result1 = Analysis.ParserModule.Executes("true ? 1 : 0")
        Assert.Equal(1, result1.Expression.GetValue(Nothing).Number)
        Dim result2 = Analysis.ParserModule.Executes("false ? 1 : 0")
        Assert.Equal(0, result2.Expression.GetValue(Nothing).Number)
        Dim result3 = Analysis.ParserModule.Executes("1 > 0 ? ""Yes"" : ""No""")
        Assert.True(result3.Expression.GetValue(Nothing).Str.Equals("Yes"))
        Dim result4 = Analysis.ParserModule.Executes("0 > 1 ? ""Yes"" : ""No""")
        Assert.True(result4.Expression.GetValue(Nothing).Str.Equals("No"))
    End Sub

End Class
