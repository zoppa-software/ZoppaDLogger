Imports Xunit
Imports ZoppaDLogger.Analysis
Imports ZoppaDLogger.Strings

Public Class VariableEnvironmentTests

    <Fact>
    Public Sub RegistNumber_And_Get_ReturnsCorrectValue()
        Dim env As New VariableEnvironment()
        env.RegistNumber("num", 42.0)
        Dim v = env.Get("num")
        Assert.Equal(VariableType.Number, v.Type)
        Assert.Equal(42.0, v.Number)
    End Sub

    <Fact>
    Public Sub RegistStr_And_Get_ReturnsCorrectValue()
        Dim env As New VariableEnvironment()
        env.RegistStr("str", "hello")
        Dim v = env.Get("str")
        Assert.Equal(VariableType.Str, v.Type)
        Assert.Equal(U8String.NewString("hello"), v.Str)
    End Sub

    <Fact>
    Public Sub RegistBool_And_Get_ReturnsCorrectValue()
        Dim env As New VariableEnvironment()
        env.RegistBool("flag", True)
        Dim v = env.Get("flag")
        Assert.Equal(VariableType.Bool, v.Type)
        Assert.True(v.Bool)
    End Sub

    <Fact>
    Public Sub Unregist_RemovesVariable()
        Dim env As New VariableEnvironment()
        env.RegistNumber("x", 1)
        env.Unregist("x")
        Assert.Throws(Of KeyNotFoundException)(Sub() env.Get("x"))
    End Sub

    <Fact>
    Public Sub Hierarchy_ScopeTest()
        Dim env As New VariableEnvironment()
        env.RegistNumber("x", 1)
        env.AddHierarchy()
        env.RegistNumber("x", 2)
        Assert.Equal(2, env.Get("x").Number)
        env.RemoveHierarchy()
        Assert.Equal(1, env.Get("x").Number)
    End Sub

    <Fact>
    Public Sub Get_ThrowsIfNotFound()
        Dim env As New VariableEnvironment()
        Assert.Throws(Of KeyNotFoundException)(Sub() env.Get("notfound"))
    End Sub

End Class