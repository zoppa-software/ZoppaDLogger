Option Strict On
Option Explicit On

Imports System.Runtime.CompilerServices
Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' 分析値を表すモジュールです。
    ''' このモジュールは、分析に関連する値の型や操作を定義します。
    ''' </summary>
    ''' <remarks>
    ''' このモジュールは、分析のための値を定義し、他の分析関連の構造体やクラスで使用されます。
    ''' </remarks>
    Public Module AnalysisValue

        ''' <summary>U8StringをIValueに変換します。</summary>
        ''' <param name="value">変換するU8String値。</param>
        ''' <returns>IValue型のStringValue。</returns>
        <Extension()>
        Public Function ToStringValue(value As U8String) As IValue
            Return New StringValue(value)
        End Function

        ''' <summary>数値をIValueに変換します。</summary>
        ''' <param name="value">変換する数値。</param>
        ''' <returns>IValue型のNumberValue。</returns>
        <Extension()>
        Public Function ToNumberValue(value As Double) As IValue
            Return New NumberValue(value)
        End Function

        ''' <summary>整数をIValueに変換します。</summary>
        ''' <param name="value">変換する整数。</param>
        ''' <returns>IValue型のNumberValue。</returns>
        <Extension()>
        Public Function ToNumberValue(value As Integer) As IValue
            Return New NumberValue(value)
        End Function

        ''' <summary>真偽値をIValueに変換します。</summary>
        ''' <param name="value">変換する真偽値。</param>
        ''' <returns>IValue型のBooleanValue。</returns>
        <Extension()>
        Public Function ToBooleanValue(value As Boolean) As IValue
            Return New BooleanValue(value)
        End Function

        ''' <summary>IValueの配列をIValueに変換します。</summary>
        ''' <param name="values">変換するIValueの配列。</param>
        ''' <returns>IValue型のArrayValue。</returns>
        <Extension()>
        Public Function ToArrayValue(values As IValue()) As IValue
            Return New ArrayValue(values)
        End Function

        ''' <summary>オブジェクトをIValueに変換します。</summary>
        ''' <param name="value">変換するオブジェクト。</param>
        ''' <returns>IValue型のObjectValue。</returns>
        <Extension()>
        Public Function ToObjectValue(value As Object) As IValue
            Return New ObjectValue(value)
        End Function

        ''' <summary>
        ''' オブジェクトをIValueに変換します。
        ''' オブジェクトの型に応じて、適切なIValueを返します。
        ''' 
        ''' 例:
        ''' - ToValue(True) => BooleanValue
        ''' - ToValue(1.0) => NumberValue
        ''' - ToValue("example") => StringValue
        ''' - ToValue([1, 2, 3]) => ArrayValue
        ''' </summary>
        ''' <param name="obj">オブジェクト。</param>
        ''' <returns>IValue。</returns>
        Public Function ConvertToValue(obj As Object) As IValue
            Select Case obj.GetType()
                Case GetType(Boolean)
                    Return New BooleanValue(CBool(obj))
                Case GetType(Double)
                    Return New NumberValue(CDbl(obj))
                Case GetType(String)
                    Return New StringValue(U8String.NewString(CStr(obj)))
                Case GetType(U8String)
                    Return New StringValue(DirectCast(obj, U8String))
                Case Else
                    If obj.GetType().IsArray Then
                        Dim arr = CType(obj, Array)
                        Dim items = New IValue(arr.Length - 1) {}
                        For i As Integer = 0 To arr.Length - 1
                            Dim v = arr.GetValue(i)
                            Select Case v.GetType()
                                Case GetType(Double)
                                    items(i) = New NumberValue(CDbl(v))
                                Case GetType(Integer)
                                    items(i) = New NumberValue(CInt(v))
                                Case GetType(String)
                                    items(i) = New StringValue(U8String.NewString(CStr(v)))
                                Case GetType(U8String)
                                    items(i) = New StringValue(DirectCast(v, U8String))
                                Case GetType(Boolean)
                                    items(i) = New BooleanValue(CBool(v))
                                Case Else
                                    items(i) = New ObjectValue(v)
                            End Select
                        Next
                        Return New ArrayValue(items)
                    Else
                        Return New ObjectValue(obj)
                    End If
            End Select
        End Function

    End Module

End Namespace
