Option Strict On
Option Explicit On

Namespace Analysis

    ''' <summary>
    ''' 変数の型を定義する列挙型です。
    ''' 変数は数値、文字列、または真偽値のいずれかを表すことができます。
    ''' </summary>
    Public Enum VariableType
        ''' <summary>数値。</summary>
        Number
        ''' <summary>文字列。</summary>
        Str
        ''' <summary>真偽値。</summary>
        Bool
    End Enum

    ''' <summary>
    ''' IVariableインターフェイスは、変数の基本的なプロパティを定義します。
    ''' 変数は数値、文字列、または真偽値のいずれかを表すことができます。
    ''' </summary>
    Public Interface IVariable

        ''' <summary>変数の型を取得します。</summary>
        ''' <returns>変数の型。</returns>
        ReadOnly Property Type As VariableType

        ''' <summary>変数の値を数値として取得します。</summary>
        ''' <returns>変数の数値。</returns>
        ''' <exception cref="InvalidCastException">変数が数値でない場合にスローされます。</exception>
        ReadOnly Property Number As Double

        ''' <summary>
        ''' 変数の値を文字列として取得します。
        ''' </summary>
        ''' <returns>変数の文字列。</returns>
        ReadOnly Property Str As String

        ''' <summary>
        ''' 変数の値を真偽値として取得します。
        ''' </summary>
        ''' <returns>変数の真偽値。</returns>
        ''' <exception cref="InvalidCastException">変数が真偽値でない場合にスローされます。</exception>
        ReadOnly Property Bool As Boolean

    End Interface

    ''' <summary>
    ''' 変数の数値を表す構造体です。
    ''' この構造体は、IVariableインターフェイスを実装し、数値型の変数を表現します。
    ''' </summary>
    Structure VariableNumber
        Implements IVariable

        ' 変数値
        Private ReadOnly _value As Double

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="value">値。</param>
        Public Sub New(value As Double)
            _value = value
        End Sub

        ''' <summary>
        ''' 変数の型を取得します。
        ''' </summary>
        ''' <returns>変数の型。</returns>
        ''' <remarks>この構造体は数値型の変数を表します。</remarks>
        Public ReadOnly Property Type As VariableType Implements IVariable.Type
            Get
                Return VariableType.Number
            End Get
        End Property

        ''' <summary>
        ''' 変数の値を数値として取得します。
        ''' </summary>
        ''' <returns>変数の数値。</returns>
        ''' <remarks>この構造体は数値型の変数を表します。</remarks>
        ''' <exception cref="InvalidCastException">変数が数値でない場合にスローされます。</exception>
        Public ReadOnly Property Number As Double Implements IVariable.Number
            Get
                Return _value
            End Get
        End Property

        ''' <summary>
        ''' 変数の値を文字列として取得します。
        ''' </summary>
        ''' <returns>変数の文字列。</returns>
        ''' <remarks>この構造体は数値型の変数を表します。</remarks>
        Public ReadOnly Property Str As String Implements IVariable.Str
            Get
                Return _value.ToString()
            End Get
        End Property

        ''' <summary>
        ''' 数値を真偽値に変換します。
        ''' 0以外の数値はTrue、0はFalseを返します。
        ''' </summary>
        ''' <returns>変換された真偽値。</returns>
        ''' <exception cref="InvalidCastException">数値が真偽値に変換できない場合にスローされます。</exception>
        Public ReadOnly Property Bool As Boolean Implements IVariable.Bool
            Get
                Return _value <> 0
            End Get
        End Property

    End Structure

    ''' <summary>
    ''' 変数の文字列を表す構造体です。
    ''' この構造体は、IVariableインターフェイスを実装し、文字列型の変数を表現します。
    ''' </summary>
    Structure VariableStr
        Implements IVariable

        ' 変数値
        Private ReadOnly _value As String

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="value">値。</param>
        Public Sub New(value As String)
            _value = value
        End Sub

        ''' <summary>
        ''' 変数の型を取得します。
        ''' </summary>
        ''' <returns>変数の型。</returns>
        ''' <remarks>この構造体は文字列型の変数を表します。</remarks>
        Public ReadOnly Property Type As VariableType Implements IVariable.Type
            Get
                Return VariableType.Str
            End Get
        End Property

        ''' <summary>
        ''' 変数の値を数値として取得します。
        ''' 文字列が数値に変換できない場合は例外をスローします。
        ''' </summary>
        ''' <returns>変数の数値。</returns>
        ''' <exception cref="InvalidCastException">文字列が数値に変換できない場合にスローされます。</exception>
        Public ReadOnly Property Number As Double Implements IVariable.Number
            Get
                Dim res As Double
                If Double.TryParse(_value, res) Then
                    Return res
                Else
                    Throw New InvalidCastException("文字列を数値に変換できません。")
                End If
            End Get
        End Property

        ''' <summary>
        ''' 変数の値を文字列として取得します。
        ''' </summary>
        ''' <returns>変数の文字列。</returns>
        ''' <remarks>この構造体は文字列型の変数を表します。</remarks>
        Public ReadOnly Property Str As String Implements IVariable.Str
            Get
                Return _value
            End Get
        End Property

        ''' <summary>
        ''' 文字列を真偽値に変換します。
        ''' "true" または "false" の場合はそれに応じた真偽値を返し、それ以外の場合は例外をスローします。
        ''' </summary>
        ''' <returns>変換された真偽値。</returns>
        ''' <exception cref="InvalidCastException">文字列が真偽値に変換できない場合にスローされます。</exception>
        Public ReadOnly Property Bool As Boolean Implements IVariable.Bool
            Get
                Select Case _value.ToLowerInvariant()
                    Case "true"
                        Return True
                    Case "false"
                        Return False
                    Case Else
                        Throw New InvalidCastException("文字列を真偽値に変換できません。")
                End Select
            End Get
        End Property

    End Structure

    ''' <summary>
    ''' 変数の真偽値を表す構造体です。
    ''' この構造体は、IVariableインターフェイスを実装し、真偽値型の変数を表現します。
    ''' </summary>
    Structure VariableBool
        Implements IVariable

        ' 変数値
        Private ReadOnly _value As Boolean

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="value">値。</param>
        Public Sub New(value As Boolean)
            _value = value
        End Sub

        ''' <summary>
        ''' 変数の型を取得します。
        ''' </summary>
        ''' <returns>変数の型。</returns>
        ''' <remarks>この構造体は真偽値型の変数を表します。</remarks>
        Public ReadOnly Property Type As VariableType Implements IVariable.Type
            Get
                Return VariableType.Bool
            End Get
        End Property

        ''' <summary>
        ''' 変数の値を数値として取得します。
        ''' 真偽値はTrueの場合は1.0、Falseの場合は0.0を返します。
        ''' </summary>
        ''' <returns>変数の数値。</returns>
        ''' <remarks>この構造体は真偽値型の変数を表します。</remarks>
        Public ReadOnly Property Number As Double Implements IVariable.Number
            Get
                Return If(_value, 1.0, 0.0)
            End Get
        End Property

        ''' <summary>
        ''' 変数の値を文字列として取得します。
        ''' 真偽値はTrueの場合は"True"、Falseの場合は"False"を返します。
        ''' </summary>
        ''' <returns>変数の文字列。</returns>
        ''' <remarks>この構造体は真偽値型の変数を表します。</remarks>
        Public ReadOnly Property Str As String Implements IVariable.Str
            Get
                Return _value.ToString()
            End Get
        End Property

        ''' <summary>
        ''' 真偽値をそのまま返します。
        ''' </summary>
        ''' <returns>変数の真偽値。</returns>
        ''' <remarks>この構造体は真偽値型の変数を表します。</remarks>
        Public ReadOnly Property Bool As Boolean Implements IVariable.Bool
            Get
                Return _value
            End Get
        End Property

    End Structure

End Namespace
