Option Strict On
Option Explicit On

Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' 値を表すインターフェイスです。
    ''' このインターフェイスは、値の型と値自体を取得するためのプロパティを定義します。
    ''' </summary>
    ''' <remarks>
    ''' このインターフェイスは、数値、文字列、真偽値などの異なる型の値を表現するために使用されます。
    ''' </remarks>
    Public Interface IValue

        ''' <summary>
        ''' 値の型を取得します。
        ''' </summary>
        ''' <returns>値の型。</returns>
        ''' <remarks>
        ''' このプロパティは、値の型を示すValueType列挙体を返します。
        ''' </remarks>
        ReadOnly Property Type As ValueType

    End Interface

    ''' <summary>
    ''' 数値を表す構造体です。
    ''' この構造体は、数値の値を保持し、IValueインターフェイスを実装します。
    ''' 数値の型はValueType.Numberとして定義されます。
    ''' </summary>
    Structure NumberValue
        Implements IValue

        ' 数値の値
        Public ReadOnly Value As Double

        ''' <summary>数値のコンストラクタ。</summary>
        ''' <param name="value">数値の値。</param>
        Public Sub New(value As Double)
            Me.Value = value
        End Sub

        ''' <summary>値の型を取得します。</summary>
        ''' <returns>値の型。</returns>
        Public ReadOnly Property Type As ValueType Implements IValue.Type
            Get
                Return ValueType.Number
            End Get
        End Property

    End Structure

    ''' <summary>
    ''' 文字列を表す構造体です。
    ''' この構造体は、文字列の値を保持し、IValueインターフェイスを実装します。
    ''' 文字列の型はValueType.Stringとして定義されます。
    ''' </summary>
    Structure StringValue
        Implements IValue

        ' 文字列の値
        Public ReadOnly Value As U8String

        ''' <summary>文字列のコンストラクタ。</summary>
        ''' <param name="value">文字列。</param>
        Public Sub New(value As U8String)
            Me.Value = value
        End Sub

        ''' <summary>値の型を取得します。</summary>
        ''' <returns>値の型。</returns>
        Public ReadOnly Property Type As ValueType Implements IValue.Type
            Get
                Return ValueType.Str
            End Get
        End Property

    End Structure

    ''' <summary>
    ''' 真偽値を表す構造体です。
    ''' この構造体は、真偽値の値を保持し、IValueインターフェイスを実装します。
    ''' 真偽値の型はValueType.Boolとして定義されます。
    ''' </summary>
    Structure BooleanValue
        Implements IValue

        ' 真偽値の値
        Public ReadOnly Value As Boolean

        ''' <summary>真偽値のコンストラクタ。</summary>
        ''' <param name="value">数値の値。</param>
        Public Sub New(value As Boolean)
            Me.Value = value
        End Sub

        ''' <summary>値の型を取得します。</summary>
        ''' <returns>値の型。</returns>
        Public ReadOnly Property Type As ValueType Implements IValue.Type
            Get
                Return ValueType.Bool
            End Get
        End Property

    End Structure

    ''' <summary>
    ''' 配列を表す構造体です。
    ''' </summary>
    Structure ArrayValue
        Implements IValue

        ' 真偽値の値
        Public ReadOnly Value As IValue()

        ''' <summary>配列値のコンストラクタ。</summary>
        ''' <param name="value">配列の値リスト。</param>
        Public Sub New(value As IValue())
            Me.Value = value
        End Sub

        ''' <summary>値の型を取得します。</summary>
        ''' <returns>値の型。</returns>
        Public ReadOnly Property Type As ValueType Implements IValue.Type
            Get
                Return ValueType.Array
            End Get
        End Property

    End Structure

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
        Public ReadOnly Value As Object

        ''' <summary>オブジェクト値のコンストラクタ。</summary>
        ''' <param name="obj">対象となるオブジェクト。</param>
        ''' <remarks>
        ''' このコンストラクタは、オブジェクト値を初期化します。
        ''' </remarks>
        Public Sub New(obj As Object)
            Me.Value = obj
        End Sub

        ''' <summary>値の型を取得します。</summary>
        ''' <returns>値の型。</returns>
        Private ReadOnly Property Type As ValueType Implements IValue.Type
            Get
                Return ValueType.Obj
            End Get
        End Property

    End Structure

End Namespace
