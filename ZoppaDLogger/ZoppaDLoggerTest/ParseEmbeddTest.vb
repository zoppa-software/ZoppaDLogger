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
    End Sub

    <Fact>
    Public Sub ParseEmbeddTest_Translate_VariableDefineBlock_ReturnsVariableBlock()
        'Dim input = U8String.NewString("${var}")
        'Dim result = ParserModule.Translate(input)
        'Assert.IsType(Of VariableDefineExpress)(result.Expression)
        'Dim varExpr = CType(result.Expression, VariableDefineExpress)
        'Assert.Equal("${var}", varExpr.VariableName.Str.ToString())
    End Sub

End Class
