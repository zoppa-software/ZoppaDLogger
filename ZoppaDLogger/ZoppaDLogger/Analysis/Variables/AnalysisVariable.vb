Option Strict On
Option Explicit On

Imports System.Runtime.CompilerServices
Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' 変数の値を解析するための拡張メソッドを提供するモジュールです。
    ''' このモジュールは、変数の型に応じて値を真偽値、数値、文字列に変換する機能を提供します。
    ''' </summary>
    Public Module AnalysisVariable

        ''' <summary>
        ''' 変数の値を真偽値に変換します。
        ''' 変数の型に応じて、適切な変換を行います。
        ''' 文字列の場合は、"true" または "false" のキーワードを使用して変換します。
        ''' 数値の場合は、0以外を真とし、0を偽とします。
        ''' 
        ''' 例:
        ''' - Bool(VariableBool(True)) => True
        ''' - Bool(VariableNumber(1)) => True
        ''' - Bool(VariableStr("true")) => True
        ''' - Bool(VariableStr("false")) => False
        ''' </summary>
        ''' <param name="value">変数。</param>
        ''' <returns>真偽値。</returns>
        <Extension()>
        Public Function Bool(value As IVariable) As Boolean
            Select Case value.Type
                Case VariableType.Bool
                    Return DirectCast(value, BooleanVariable).Value
                Case VariableType.Number
                    Dim nv = DirectCast(value, NumberVariable).Value
                    Return nv <> 0
                Case VariableType.Str
                    Dim sv = DirectCast(value, StringVariable).Value
                    Select Case sv
                        Case LexicalModule.TrueKeyword
                            Return True
                        Case LexicalModule.FalseKeyword
                            Return False
                        Case Else
                            Throw New InvalidCastException("文字列を真偽値に変換できません。")
                    End Select
                Case Else
                    Throw New InvalidCastException("真偽値に変換できませんでした。")
            End Select
        End Function

        ''' <summary>
        ''' 変数の値を文字列に変換します。
        ''' 変数の型に応じて、適切な変換を行います。
        ''' 
        ''' 例:
        ''' - Str(VariableBool(True)) => "true"
        ''' - Str(VariableNumber(1)) => "1"
        ''' - Str(VariableStr("example")) => "example"
        ''' </summary>
        ''' <param name="value">変数。</param>
        ''' <returns>文字列。</returns>
        <Extension()>
        Public Function Number(value As IVariable) As Double
            Select Case value.Type
                Case VariableType.Bool
                    Dim bv = DirectCast(value, BooleanVariable).Value
                    Return If(bv, 1.0, 0.0)
                Case VariableType.Number
                    Return DirectCast(value, NumberVariable).Value
                Case VariableType.Str
                    Return ParserModule.ParseNumber(DirectCast(value, StringVariable).Value)
                Case Else
                    Throw New InvalidCastException("数値に変換できませんでした。")
            End Select
        End Function

        ''' <summary>
        ''' 変数の値を文字列に変換します。
        ''' 変数の型に応じて、適切な変換を行います。
        ''' 
        ''' 例:
        ''' - Str(VariableBool(True)) => "true"
        ''' - Str(VariableNumber(1)) => "1"
        ''' - Str(VariableStr("example")) => "example"
        ''' - Str(VariableArray([1, 2, 3])) => "1,2,3"
        ''' </summary>
        ''' <param name="value">変数。</param>
        ''' <param name="venv"> 解析環境。</param>
        ''' <returns>文字列。</returns>
        <Extension()>
        Public Function Str(value As IVariable, venv As AnalysisEnvironment) As U8String
            Select Case value.Type
                Case VariableType.Bool
                    Dim bv = DirectCast(value, BooleanVariable).Value
                    Return If(bv, LexicalModule.TrueKeyword, LexicalModule.FalseKeyword)

                Case VariableType.Number
                    Return U8String.NewString(DirectCast(value, NumberVariable).Value.ToString())

                Case VariableType.Str
                    Return DirectCast(value, StringVariable).Value

                Case VariableType.Array
                    Dim av = DirectCast(value, ArrayVariable).Value
                    Dim res As New List(Of Byte)()
                    For i As Integer = 0 To av.Length - 1
                        If i > 0 Then
                            res.Add(CByte(44)) ' カンマのASCIIコード
                        End If
                        res.AddRange(av(i).GetValue(venv).Str.Data)
                    Next
                    Return U8String.NewString(res.ToArray())

                Case VariableType.Obj
                    Dim obj = DirectCast(value, ObjectVariable).Value
                    If TypeOf obj Is U8String Then
                        Return DirectCast(obj, U8String)
                    ElseIf TypeOf obj Is String Then
                        Return U8String.NewString(obj.ToString())
                    ElseIf TypeOf obj Is IValue Then
                        Return U8String.NewString(obj.ToString())
                    Else
                        Throw New InvalidCastException("オブジェクトを文字列に変換できません。")
                    End If

                Case Else
                    Throw New InvalidCastException("文字列に変換できませんでした。")
            End Select
        End Function

    End Module

End Namespace
