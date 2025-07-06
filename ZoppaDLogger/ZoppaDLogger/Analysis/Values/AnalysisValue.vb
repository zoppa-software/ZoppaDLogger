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

        ''' <summary>オブジェクトの配列をIValueに変換します。</summary>
        ''' <param name="values">変換するオブジェクトの配列。</param>
        ''' <returns>IValue型のArrayValue。</returns>
        Public Function ToArrayValue(Of T)(values As T()) As IValue
            Dim items = New IValue(values.Length - 1) {}
            For i As Integer = 0 To values.Length - 1
                items(i) = ObjToValue(values(i))
            Next
            Return New ArrayValue(items)
        End Function

        ''' <summary>オブジェクトをIValueに変換します。</summary>
        ''' <param name="v">変換するオブジェクト。</param>
        ''' <returns>IValue型のObjectValue。</returns>
        Private Function ObjToValue(v As Object) As IValue
            Select Case v.GetType()
                Case GetType(Double)
                    Return New NumberValue(CDbl(v))
                Case GetType(Integer)
                    Return New NumberValue(CInt(v))
                Case GetType(String)
                    Return New StringValue(U8String.NewString(CStr(v)))
                Case GetType(U8String)
                    Return New StringValue(DirectCast(v, U8String))
                Case GetType(Boolean)
                    Return New BooleanValue(CBool(v))
                Case GetType(IValue)
                    Return DirectCast(v, IValue)
                Case Else
                    Return New ObjectValue(v)
            End Select
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
                Case GetType(IValue())
                    Return New ArrayValue(DirectCast(obj, IValue()))
                Case Else
                    If obj.GetType().IsArray Then
                        Dim arr = CType(obj, Array)
                        Dim items = New IValue(arr.Length - 1) {}
                        For i As Integer = 0 To arr.Length - 1
                            items(i) = ObjToValue(arr.GetValue(i))
                        Next
                        Return New ArrayValue(items)
                    Else
                        Return New ObjectValue(obj)
                    End If
            End Select
        End Function

        ''' <summary>
        ''' IValueを数値に変換します。
        ''' IValueの型に応じて、適切な数値を返します。
        ''' </summary>
        ''' <param name="val">変換するIValue。</param>
        ''' <returns>数値。</returns>
        ''' <exception cref="InvalidOperationException">数値に変換できない場合にスローされます。</exception>
        <Extension()>
        Public Function Number(val As IValue) As Double
            Select Case val.Type
                Case ValueType.Number
                    Return DirectCast(val, NumberValue).Value
                Case ValueType.Bool
                    Return If(DirectCast(val, BooleanValue).Value, 1, 0)
                Case ValueType.Str
                    Dim o = DirectCast(val, StringValue).Value
                    Return ParserModule.ParseNumber(o)
                Case ValueType.Obj
                    Dim o = DirectCast(val, ObjectValue).Value
                    If TypeOf o Is Double Then
                        Return CDbl(o)
                    ElseIf TypeOf o Is Integer Then
                        Return CDbl(CInt(o))
                    Else
                        Throw New InvalidOperationException("オブジェクト値は数値になりません。")
                    End If
                Case Else
                    Throw New InvalidOperationException("数値に変換することができません")
            End Select
        End Function

        ''' <summary>
        ''' IValueを文字列に変換します。
        ''' IValueの型に応じて、適切な文字列を返します。
        ''' </summary>
        ''' <param name="val">変換するIValue。</param>
        ''' <returns>文字列。</returns>
        ''' <exception cref="InvalidOperationException">文字列に変換できない場合にスローされます。</exception>
        <Extension()>
        Public Function Str(val As IValue) As U8String
            Select Case val.Type
                Case ValueType.Str
                    Return DirectCast(val, StringValue).Value
                Case ValueType.Number
                    Return U8String.NewString(DirectCast(val, NumberValue).Value.ToString())
                Case ValueType.Bool
                    Return If(DirectCast(val, BooleanValue).Value, LexicalModule.TrueKeyword, LexicalModule.FalseKeyword)
                Case ValueType.Obj
                    Dim o = DirectCast(val, ObjectValue).Value
                    If TypeOf o Is U8String Then
                        Return CType(o, U8String)
                    ElseIf TypeOf o Is String Then
                        Return U8String.NewString(CType(o, String))
                    Else
                        Return U8String.NewString(o.ToString())
                    End If
                Case ValueType.Array
                    Dim o = DirectCast(val, ArrayValue).Value
                    Dim res As New List(Of Byte)()
                    For i As Integer = 0 To o.Length - 1
                        If i > 0 Then
                            res.Add(CByte(44)) ' カンマのASCIIコード
                        End If
                        res.AddRange(o(i).Str.GetByteEnumerable())
                    Next
                    Return U8String.NewStringChangeOwner(res.ToArray())
                Case Else
                    Throw New InvalidOperationException("文字列に変換することができません")
            End Select
        End Function

        ''' <summary>
        ''' IValueを真偽値に変換します。
        ''' IValueの型に応じて、適切な真偽値を返します。
        ''' </summary>
        ''' <param name="val">変換するIValue。</param>
        ''' <returns>真偽値。</returns>
        ''' <exception cref="InvalidOperationException">真偽値に変換できない場合にスローされます。</exception>
        <Extension()>
        Public Function Bool(val As IValue) As Boolean
            Select Case val.Type
                Case ValueType.Bool
                    Return DirectCast(val, BooleanValue).Value
                Case ValueType.Str
                    Dim o = DirectCast(val, StringValue).Value
                    If o = TrueKeyword Then
                        Return True
                    ElseIf o = FalseKeyword Then
                        Return False
                    Else
                        ' 文字列が真偽値として解釈できない場合は例外を投げる
                        Throw New InvalidOperationException("文字列を真偽値として解釈できません。")
                    End If
                Case ValueType.Number
                    Return DirectCast(val, NumberValue).Value <> 0
                Case ValueType.Obj
                    Dim o = DirectCast(val, ObjectValue).Value
                    If TypeOf o Is Boolean Then
                        Return CType(o, Boolean)
                    Else
                        Throw New InvalidOperationException("オブジェクト値を真偽値として解釈できません。")
                    End If
                Case Else
                    Throw New InvalidOperationException("真偽値に変換することができません")
            End Select
        End Function

        ''' <summary>
        ''' IValueを配列値に変換します。
        ''' IValueの型に応じて、適切な配列値を返します。
        ''' </summary>
        ''' <param name="val">変換するIValue。</param>
        ''' <returns>配列値。</returns>
        <Extension()>
        Public Function Array(val As IValue) As IValue()
            Select Case val.Type
                Case ValueType.Array
                    Return DirectCast(val, ArrayValue).Value
                Case Else
                    Return New IValue() {val}
            End Select
        End Function

        ''' <summary>
        ''' IValueをオブジェクト値に変換します。
        ''' IValueの型に応じて、適切なオブジェクト値を返します。
        ''' </summary>
        ''' <param name="val">変換するIValue。</param>
        ''' <returns>配列オブジェクト値。</returns>
        <Extension()>
        Public Function Obj(val As IValue) As Object
            Select Case val.Type
                Case ValueType.Array
                    Return DirectCast(val, ArrayValue).Value
                Case ValueType.Bool
                    Return DirectCast(val, BooleanValue).Value
                Case ValueType.Number
                    Return DirectCast(val, NumberValue).Value
                Case ValueType.Str
                    Return DirectCast(val, StringValue).Value
                Case ValueType.Obj
                    Return DirectCast(val, ObjectValue).Value
                Case Else
                    Throw New InvalidOperationException("オブジェクト値に変換することができません")
            End Select
        End Function

    End Module

End Namespace
