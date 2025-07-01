Imports Xunit
Imports ZoppaDLogger.Analysis

' テスト用のダミーIVariable実装
Friend Class DummyVariable
    Implements IVariable

    Public Property Data As String

    Public ReadOnly Property Type As VariableType Implements IVariable.Type
        Get
            Return VariableType.Str
        End Get
    End Property

    Public ReadOnly Property Number As Double Implements IVariable.Number
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Str As String Implements IVariable.Str
        Get
            Return Me.Data
        End Get
    End Property

    Public ReadOnly Property Bool As Boolean Implements IVariable.Bool
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Sub New(data As String)
        Me.Data = data
    End Sub
End Class

Public Class VariablesTest

    <Fact>
    Public Sub Regist_NewVariable_AddsEntry()
        Dim vars As New Variables()
        Dim v As New DummyVariable("foo")

        ' 新しい変数を登録
        vars.Regist("x", New DummyVariable("foo"))
        Assert.Equal("foo", vars.Get("x").Str)

        ' もう一度同じ値で登録してもエラーにならない
        vars.Regist("x", New DummyVariable("bar"))
        Assert.Equal("bar", vars.Get("x").Str)

        ' 変数を登録解除
        vars.Unregist("x")
        Assert.Throws(Of KeyNotFoundException)(Function() vars.Get("x"))
    End Sub


End Class