Option Strict On
Option Explicit On

Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' 文字列を表す構造体です。
    ''' この構造体は、文字列の値を保持し、IValueインターフェイスを実装します。
    ''' 文字列の型はValueType.Stringとして定義されます。
    ''' </summary>
    Structure StringValue
        Implements IValue

        ' 文字列の値
        Private ReadOnly _value As U8String

        ''' <summary>文字列のコンストラクタ。</summary>
        ''' <param name="value">文字列。</param>
        Public Sub New(value As U8String)
            _value = value
        End Sub

        ''' <summary>値の型を取得します。</summary>
        ''' <returns>値の型。</returns>
        Public ReadOnly Property Type As ValueType Implements IValue.Type
            Get
                Return ValueType.Str
            End Get
        End Property

        ''' <summary>数値の値を取得します。</summary>
        ''' <returns>数値の値。</returns>
        Public ReadOnly Property Number As Double Implements IValue.Number
            Get
                Return ParserModule.ParseNumber(_value)
            End Get
        End Property

        ''' <summary>文字列の値を取得します。</summary>
        ''' <returns>文字列の値。</returns>
        Public ReadOnly Property Str As U8String Implements IValue.Str
            Get
                Return _value
            End Get
        End Property

        ''' <summary>真偽値の値を取得します。</summary>
        ''' <returns>真偽値の値。</returns>
        Public ReadOnly Property Bool As Boolean Implements IValue.Bool
            Get
                If _value = LexicalModule.TrueKeyword Then
                    Return True
                ElseIf _value = LexicalModule.FalseKeyword Then
                    Return False
                Else
                    ' 文字列が真偽値として解釈できない場合は例外を投げる
                    Throw New AnalysisException("文字列は真偽値として解釈できません。")
                End If
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