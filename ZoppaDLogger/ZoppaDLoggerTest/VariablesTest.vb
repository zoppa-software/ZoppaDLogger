Imports Xunit
Imports ZoppaDLogger.Analysis
Imports ZoppaDLogger.Strings

' テスト用のダミーIVariable実装
Friend Class DummyVariable
    Implements IVariable

    Public Property Data As U8String

    Public ReadOnly Property Type As VariableType Implements IVariable.Type
        Get
            Return VariableType.Str
        End Get
    End Property

    Public Sub New(data As U8String)
        Me.Data = data
    End Sub
End Class

Public Class VariablesTest

    <Fact>
    Public Sub Regist_NewVariable_AddsEntry()
        Dim env As New AnalysisEnvironment()

        Dim vars As New Variables()
        Dim v = U8String.NewString("foo")
        vars.Regist("x", New DummyVariable(v))
        Assert.Equal(v, CType(vars.Get("x"), DummyVariable).Data)

        ' 新しい変数を登録
        Dim v1 = U8String.NewString("bar")
        vars.Regist("x", New DummyVariable(v1))
        Assert.Equal(v1, CType(vars.Get("x"), DummyVariable).Data)

        ' もう一度同じ値で登録してもエラーにならない
        vars.Regist("x", New DummyVariable(v1))
        Assert.Equal(v1, CType(vars.Get("x"), DummyVariable).Data)

        ' 変数を登録解除
        vars.Unregist("x")
        Assert.Throws(Of KeyNotFoundException)(Function() vars.Get("x"))
    End Sub


End Class