Option Strict On
Option Explicit On

Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' IVariableインターフェイスは、変数の基本的なプロパティを定義します。
    ''' 変数は数値、文字列、または真偽値のいずれかを表すことができます。
    ''' </summary>
    Public Interface IVariable

        ''' <summary>変数の型を取得します。</summary>
        ''' <returns>変数の型。</returns>
        ReadOnly Property Type As VariableType

        ''' <summary>変数の値を式として取得します。</summary>
        ''' <returns>変数の式。</returns>
        ReadOnly Property Expr As IExpression

        ''' <summary>変数の値を数値として取得します。</summary>
        ''' <returns>変数の数値。</returns>
        ''' <exception cref="InvalidCastException">変数が数値でない場合にスローされます。</exception>
        ReadOnly Property Number As Double

        ''' <summary>
        ''' 変数の値を文字列として取得します。
        ''' </summary>
        ''' <returns>変数の文字列。</returns>
        ReadOnly Property Str As U8String

        ''' <summary>
        ''' 変数の値を真偽値として取得します。
        ''' </summary>
        ''' <returns>変数の真偽値。</returns>
        ''' <exception cref="InvalidCastException">変数が真偽値でない場合にスローされます。</exception>
        ReadOnly Property Bool As Boolean

        ''' <summary>
        ''' 変数の配列を取得します。
        ''' </summary>
        ''' <returns>変数の配列。</returns>
        ''' <remarks>
        ''' このプロパティは、変数が配列である場合に使用されます。
        ''' 配列の要素はIVariable型でなければなりません。
        ''' </remarks>
        ''' <exception cref="InvalidCastException">変数が配列でない場合にスローされます。</exception>
        ReadOnly Property Array As IVariable()

    End Interface

    Structure VariableExpr
        Implements IVariable

        ' 式
        Private ReadOnly _expr As IExpression

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="expr">式。</param>
        Public Sub New(expr As IExpression)
            _expr = expr
        End Sub

        ''' <summary>
        ''' 変数の型を取得します。
        ''' </summary>
        ''' <returns>変数の型。</returns>
        Public ReadOnly Property Type As VariableType Implements IVariable.Type
            Get
                Return VariableType.Expr
            End Get
        End Property

        ''' <summary>
        ''' 変数の値を式として取得します。
        ''' </summary>
        ''' <returns>変数の式。</returns>
        Public ReadOnly Property Expr As IExpression Implements IVariable.Expr
            Get
                Return _expr
            End Get
        End Property

        ''' <summary>
        ''' 変数の値を数値として取得します。
        ''' 式が数値に変換できない場合は例外をスローします。
        ''' </summary>
        ''' <returns>変数の数値。</returns>
        ''' <exception cref="InvalidCastException">式が数値に変換できないためスローされます。</exception>
        Public ReadOnly Property Number As Double Implements IVariable.Number
            Get
                Throw New InvalidCastException("式を数値に変換できません。")
            End Get
        End Property

        ''' <summary>
        ''' 変数の値を文字列として取得します。
        ''' 式が文字列に変換できない場合は例外をスローします。
        ''' </summary>
        ''' <returns>変数の文字列。</returns>
        ''' <exception cref="InvalidCastException">式が文字列に変換できないためスローされます。</exception>
        Public ReadOnly Property Str As U8String Implements IVariable.Str
            Get
                Throw New InvalidCastException("式を文字列に変換できません。")
            End Get
        End Property

        ''' <summary>
        ''' 変数の値を真偽値として取得します。
        ''' 式が真偽値に変換できない場合は例外をスローします。
        ''' </summary>
        ''' <returns>変数の真偽値。</returns>
        ''' <exception cref="InvalidCastException">式が真偽値に変換できないためスローされます。</exception>
        Public ReadOnly Property Bool As Boolean Implements IVariable.Bool
            Get
                Throw New InvalidCastException("式を真偽値に変換できません。")
            End Get
        End Property

        ''' <summary>
        ''' 変数の配列を取得します。
        ''' 式は配列に変換できないため、例外をスローします。
        ''' </summary>
        ''' <returns>変数の配列。</returns>
        ''' <exception cref="InvalidCastException">式が配列に変換できないためスローされます。</exception>
        Public ReadOnly Property Array As IVariable() Implements IVariable.Array
            Get
                Throw New InvalidCastException("式を配列に変換できません。")
            End Get
        End Property

    End Structure

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

        ''' <summary>変数の値を式として取得します。</summary>
        ''' <returns>変数の式。</returns>
        Public ReadOnly Property Expr As IExpression Implements IVariable.Expr
            Get
                Throw New InvalidCastException("数値を式に変換できません。")
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
        Public ReadOnly Property Str As U8String Implements IVariable.Str
            Get
                Return U8String.NewString(_value.ToString())
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

        ''' <summary>
        ''' 変数の配列を取得します。
        ''' この構造体は単一の数値変数を表すため、配列は自身のみを含む配列を返します。
        ''' </summary>
        ''' <returns>変数の配列。</returns>
        ''' <remarks>このプロパティは、IVariableインターフェイスの一部として実装されています。</remarks>
        Public ReadOnly Property Array As IVariable() Implements IVariable.Array
            Get
                Return New IVariable() {Me}
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
        Private ReadOnly _value As U8String

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="value">値。</param>
        Public Sub New(value As U8String)
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

        ''' <summary>変数の値を式として取得します。</summary>
        ''' <returns>変数の式。</returns>
        Public ReadOnly Property Expr As IExpression Implements IVariable.Expr
            Get
                Throw New InvalidCastException("文字列を式に変換できません。")
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
                If Double.TryParse(_value.ToString(), res) Then
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
        Public ReadOnly Property Str As U8String Implements IVariable.Str
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
                If _value = LexicalModule.TrueKeyword Then
                    Return True
                ElseIf _value = LexicalModule.FalseKeyword Then
                    Return False
                Else
                    Throw New InvalidCastException("文字列を真偽値に変換できません。")
                End If
            End Get
        End Property

        ''' <summary>
        ''' 変数の配列を取得します。
        ''' この構造体は単一の数値変数を表すため、配列は自身のみを含む配列を返します。
        ''' </summary>
        ''' <returns>変数の配列。</returns>
        ''' <remarks>このプロパティは、IVariableインターフェイスの一部として実装されています。</remarks>
        Public ReadOnly Property Array As IVariable() Implements IVariable.Array
            Get
                Return New IVariable() {Me}
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

        ''' <summary>変数の値を式として取得します。</summary>
        ''' <returns>変数の式。</returns>
        Public ReadOnly Property Expr As IExpression Implements IVariable.Expr
            Get
                Throw New InvalidCastException("真偽値を式に変換できません。")
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
        Public ReadOnly Property Str As U8String Implements IVariable.Str
            Get
                Return If(_value, LexicalModule.TrueKeyword, LexicalModule.FalseKeyword)
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

        ''' <summary>
        ''' 変数の配列を取得します。
        ''' この構造体は単一の数値変数を表すため、配列は自身のみを含む配列を返します。
        ''' </summary>
        ''' <returns>変数の配列。</returns>
        ''' <remarks>このプロパティは、IVariableインターフェイスの一部として実装されています。</remarks>
        Public ReadOnly Property Array As IVariable() Implements IVariable.Array
            Get
                Return New IVariable() {Me}
            End Get
        End Property

    End Structure

    ''' <summary>
    ''' 変数の配列を表す構造体です。
    ''' この構造体は、IVariableインターフェイスを実装し、複数の変数を格納することができます。
    ''' </summary>
    Structure VariableArray
        Implements IVariable

        ' 変数値
        Private ReadOnly _value As IVariable()

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="value">値の配列。</param>
        Public Sub New(value As IVariable())
            _value = value
        End Sub

        ''' <summary>
        ''' 変数の型を取得します。
        ''' </summary>
        ''' <returns>変数の型。</returns>
        ''' <remarks>この構造体は真偽値型の変数を表します。</remarks>
        Public ReadOnly Property Type As VariableType Implements IVariable.Type
            Get
                Return VariableType.Array
            End Get
        End Property

        ''' <summary>変数の値を式として取得します。</summary>
        ''' <returns>変数の式。</returns>
        Public ReadOnly Property Expr As IExpression Implements IVariable.Expr
            Get
                Throw New InvalidCastException("配列を式に変換できません。")
            End Get
        End Property

        ''' <summary>
        ''' 変数の値を数値として取得します。
        ''' 配列は数値に変換できないため、例外をスローします。
        ''' </summary>
        ''' <returns>変数の数値。</returns>
        ''' <exception cref="InvalidCastException">配列が数値に変換できない場合にスローされます。</exception>
        Public ReadOnly Property Number As Double Implements IVariable.Number
            Get
                Throw New InvalidCastException("配列を数値に変換できません。")
            End Get
        End Property

        ''' <summary>
        ''' 変数の値を文字列として取得します。
        ''' </summary>
        ''' <returns>変数の文字列。</returns>
        Public ReadOnly Property Str As U8String Implements IVariable.Str
            Get
                Dim sb As New List(Of Byte)()
                For i As Integer = 0 To _value.Length - 1
                    If i > 0 Then
                        sb.Add(&H2C) ' カンマ
                    End If
                    sb.AddRange(_value(i).Str.Data)
                Next
                Return U8String.NewString(sb.ToArray())
            End Get
        End Property

        ''' <summary>
        ''' 変数の値を真偽値として取得します。
        ''' 配列は真偽値に変換できないため、例外をスローします。
        ''' </summary>
        ''' <returns>変数の真偽値。</returns>
        ''' <exception cref="InvalidCastException">配列が真偽値に変換できない場合にスローされます。</exception>
        Public ReadOnly Property Bool As Boolean Implements IVariable.Bool
            Get
                Throw New InvalidCastException("配列を真偽値に変換できません。")
            End Get
        End Property

        ''' <summary>
        ''' 変数の配列を取得します。
        ''' </summary>
        ''' <returns>変数の配列。</returns>
        ''' <remarks>この構造体は複数の変数を格納するため、IVariable型の配列を返します。</remarks>
        Public ReadOnly Property Array As IVariable() Implements IVariable.Array
            Get
                Return _value
            End Get
        End Property
    End Structure

End Namespace
