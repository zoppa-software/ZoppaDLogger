Imports System
Imports Xunit
Imports ZoppaDLogger
Imports ZoppaDLogger.Analysis
Imports ZoppaDLogger.Strings
Imports ZoppaDLogger.Analysis.AnalysisValue

Public Class ParseEmbeddTest

    <Fact>
    Public Sub ParseEmbeddTest_Translate_OnlyPlainText_ReturnsNoneBlock()
        Dim input = U8String.NewString("plain text only")
        Dim result = ParserModule.Translate(input)
        Assert.True(result.Expression.GetValue(Nothing).Str.Equals("plain text only"))

        Dim input2 = U8String.NewString("a\{b\}c")
        Dim result2 = ParserModule.Translate(input2)
        Assert.True(result2.Expression.GetValue(Nothing).Str.Equals("a{b}c"))

        Dim input3 = U8String.NewString("a\#{b")
        Dim result3 = ParserModule.Translate(input3)
        Assert.True(result3.Expression.GetValue(Nothing).Str.Equals("a#{b"))
    End Sub

    <Fact>
    Public Sub ParseEmbeddTest_Translate_Enpty()
        Dim input = U8String.NewString("こんにちわ！ {}世界")
        Dim result = ParserModule.Translate(input)
        Assert.True(result.Expression.GetValue(Nothing).Str.Equals("こんにちわ！ 世界"))
    End Sub

    <Fact>
    Public Sub ParseEmbeddTest_Unfold()
        Dim input = U8String.NewString("1 + 1 = #{1 + 1}")
        Dim result = ParserModule.Translate(input)
        Assert.True(result.Expression.GetValue(Nothing).Str.Equals("1 + 1 = 2"))

        Dim input2 = U8String.NewString("1 + 1 = #{1 + 1} esc = !{2 + 2}")
        Dim result2 = ParserModule.Translate(input2)
        Assert.True(result2.Expression.GetValue(Nothing).Str.Equals("1 + 1 = 2 esc = 4"))
    End Sub

End Class
