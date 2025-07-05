Option Strict On
Option Explicit On
Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' オブジェクト値を表す構造体です。
    ''' この構造体は、オブジェクトの値を表現し、式として評価するためのメソッドを提供します。
    ''' </summary>
    ''' <remarks>
    ''' この構造体は、オブジェクトのプロパティやメソッドを含む値を表現するために使用されます。
    ''' </remarks>
    Structure ObjectValue
        Implements IValue

        ''' <summary>対象となるオブジェクト。</summary>
        Private ReadOnly _obj As Object

        ''' <summary>オブジェクト値のコンストラクタ。</summary>
        ''' <param name="obj">対象となるオブジェクト。</param>
        ''' <remarks>
        ''' このコンストラクタは、オブジェクト値を初期化します。
        ''' </remarks>
        Public Sub New(obj As Object)
            _obj = obj
        End Sub

        ''' <summary>値の型を取得します。</summary>
        ''' <returns>値の型。</returns>
        Private ReadOnly Property Type As ValueType Implements IValue.Type
            Get
                Return ValueType.Obj
            End Get
        End Property

        ''' <summary>数値の値を取得します。</summary>
        ''' <returns>数値の値。</returns>
        Public ReadOnly Property Number As Double Implements IValue.Number
            Get
                If TypeOf _obj Is Double Then
                    Return CDbl(_obj)
                ElseIf TypeOf _obj Is Integer Then
                    Return CDbl(CInt(_obj))
                Else
                    Throw New InvalidOperationException("オブジェクト値は数値になりません。")
                End If
            End Get
        End Property

        ''' <summary>文字列の値を取得します。</summary>
        ''' <returns>文字列の値。</returns>
        Public ReadOnly Property Str As U8String Implements IValue.Str
            Get
                If TypeOf _obj Is U8String Then
                    Return CType(_obj, U8String)
                ElseIf TypeOf _obj Is String Then
                    Return U8String.NewString(CType(_obj, String))
                Else
                    Throw New InvalidOperationException("オブジェクト値は文字列になりません。")
                End If
            End Get
        End Property

        ''' <summary>真偽値の値を取得します。</summary>
        ''' <returns>真偽値の値。</returns>
        Public ReadOnly Property Bool As Boolean Implements IValue.Bool
            Get
                If TypeOf _obj Is Boolean Then
                    Return CType(_obj, Boolean)
                Else
                    Throw New InvalidOperationException("オブジェクト値は真偽値になりません。")
                End If
            End Get
        End Property

        ''' <summary>配列の値を取得します。</summary>
        ''' <returns>配列の値。</returns>
        Public ReadOnly Property Array As IValue() Implements IValue.Array
            Get
                If TypeOf _obj Is IValue() Then
                    Return CType(_obj, IValue())
                Else
                    Throw New InvalidOperationException("オブジェクト値は配列になりません。")
                End If
            End Get
        End Property

        ''' <summary>オブジェクトの値を取得します。</summary>
        ''' <returns>オブジェクトの値。</returns>
        Public ReadOnly Property Obj As Object Implements IValue.Obj
            Get
                Return _obj
            End Get
        End Property

    End Structure

End Namespace
