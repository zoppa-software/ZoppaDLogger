Option Strict On
Option Explicit On

Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' 真偽値を表す構造体です。
    ''' この構造体は、真偽値の値を保持し、IValueインターフェイスを実装します。
    ''' 真偽値の型はValueType.Boolとして定義されます。
    ''' </summary>
    Structure BooleanValue
        Implements IValue

        ' 真偽値の値
        Private ReadOnly _value As Boolean

        ''' <summary>真偽値のコンストラクタ。</summary>
        ''' <param name="value">数値の値。</param>
        Public Sub New(value As Boolean)
            _value = value
        End Sub

        ''' <summary>値の型を取得します。</summary>
        ''' <returns>値の型。</returns>
        Public ReadOnly Property Type As ValueType Implements IValue.Type
            Get
                Return ValueType.Bool
            End Get
        End Property

        ''' <summary>数値の値を取得します。</summary>
        ''' <returns>数値の値。</returns>
        Public ReadOnly Property Number As Double Implements IValue.Number
            Get
                Return If(_value, 1, .0)
            End Get
        End Property

        ''' <summary>文字列の値を取得します。</summary>
        ''' <returns>文字列の値。</returns>
        Public ReadOnly Property Str As U8String Implements IValue.Str
            Get
                Return If(_value, LexicalModule.TrueKeyword, LexicalModule.FalseKeyword)
            End Get
        End Property

        ''' <summary>真偽値の値を取得します。</summary>
        ''' <returns>真偽値の値。</returns>
        Public ReadOnly Property Bool As Boolean Implements IValue.Bool
            Get
                Return _value
            End Get
        End Property

        ''' <summary>配列の値を取得します。</summary>
        ''' <returns>配列の値。</returns>
        Public ReadOnly Property Array As IValue() Implements IValue.Array
            Get
                Return New IValue() {Me}
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