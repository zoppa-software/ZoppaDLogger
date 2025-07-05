Imports System
Imports Xunit
Imports ZoppaDLogger
Imports ZoppaDLogger.Analysis
Imports ZoppaDLogger.Strings
Imports ZoppaDLogger.Analysis.AnalysisValue

Public Class ParseCalcTest

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
