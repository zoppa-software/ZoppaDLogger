Option Strict On
Option Explicit On

Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' 配列を表す構造体です。
    ''' </summary>
    Structure ArrayValue
        Implements IValue

        ' 真偽値の値
        Private ReadOnly _value As IValue()

        ''' <summary>配列値のコンストラクタ。</summary>
        ''' <param name="value">配列の値リスト。</param>
        Public Sub New(value As IValue())
            _value = value
        End Sub

        ''' <summary>値の型を取得します。</summary>
        ''' <returns>値の型。</returns>
        Public ReadOnly Property Type As ValueType Implements IValue.Type
            Get
                Return ValueType.Array
            End Get
        End Property

        ''' <summary>数値の値を取得します。</summary>
        ''' <returns>数値の値。</returns>
        Public ReadOnly Property Number As Double Implements IValue.Number
            Get
                Throw New InvalidOperationException("配列値は数値になりません。")
            End Get
        End Property

        ''' <summary>文字列の値を取得します。</summary>
        ''' <returns>文字列の値。</returns>
        Public ReadOnly Property Str As U8String Implements IValue.Str
            Get
                Dim res As New List(Of Byte)()
                For i As Integer = 0 To _value.Length - 1
                    If i > 0 Then
                        res.Add(CByte(44)) ' カンマのASCIIコード
                    End If
                    res.AddRange(_value(i).Str.Data)
                Next
                Return U8String.NewString(res.ToArray())
            End Get
        End Property

        ''' <summary>真偽値の値を取得します。</summary>
        ''' <returns>真偽値の値。</returns>
        Public ReadOnly Property Bool As Boolean Implements IValue.Bool
            Get
                Throw New InvalidOperationException("配列値は真偽値になりません。")
            End Get
        End Property

        ''' <summary>配列の値を取得します。</summary>
        ''' <returns>配列の値。</returns>
        Public ReadOnly Property Array As IValue() Implements IValue.Array
            Get
                Return _value
            End Get
        End Property

        ''' <summary>オブジェクトの値を取得します。</summary>
        ''' <returns>オブジェクトの値。</returns>
        Public ReadOnly Property Obj As Object Implements IValue.Obj
            Get
                Return _value
            End Get
        End Property

    End Structure


End Namespace
