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

        ''' <summary>数値の値を取得します。</summary>
        ''' <returns>数値の値。</returns>
        ReadOnly Property Number As Double

        ''' <summary>文字列の値を取得します。</summary>
        ''' <returns>文字列の値。</returns>
        ReadOnly Property Str As U8String

        ''' <summary>真偽値の値を取得します。</summary>
        ''' <returns>真偽値の値。</returns>
        ReadOnly Property Bool As Boolean

        ''' <summary>
        ''' 値を配列として取得します。
        ''' </summary>
        ''' <returns>値の配列。</returns>
        ''' <remarks>
        ''' このプロパティは、値をIValue型の配列として返します。
        ''' 単一の値の場合は、その値を含む配列を返します。
        ''' </remarks>
        ReadOnly Property Array As IValue()

        ''' <summary>
        ''' オブジェクトとしての値を取得します。
        ''' </summary>
        ''' <returns>オブジェクトとしての値。</returns>
        ''' <remarks>
        ''' このプロパティは、値をObject型として返します。
        ''' 具体的な値の型に応じて、適切なオブジェクトを返します。
        ''' </remarks>
        ReadOnly Property Obj As Object

    End Interface

End Namespace
