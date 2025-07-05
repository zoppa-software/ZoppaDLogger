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

    End Interface

    ''' <summary>
    ''' 変数の式を表す構造体です。
    ''' この構造体は、IVariableインターフェイスを実装し、変数の式を表現します。
    ''' </summary>
    Structure ExprVariable
        Implements IVariable

        ''' <summary>変数の式を取得します。</summary>
        Public ReadOnly Value As IExpression

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="expr">式。</param>
        Public Sub New(value As IExpression)
            Me.Value = value
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

    End Structure

    ''' <summary>
    ''' 変数の数値を表す構造体です。
    ''' この構造体は、IVariableインターフェイスを実装し、数値型の変数を表現します。
    ''' </summary>
    Structure NumberVariable
        Implements IVariable

        ''' <summary>変数の数値を取得します。</summary>
        Public ReadOnly Value As Double

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="value">値。</param>
        Public Sub New(value As Double)
            Me.Value = value
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

    End Structure

    ''' <summary>
    ''' 変数の文字列を表す構造体です。
    ''' この構造体は、IVariableインターフェイスを実装し、文字列型の変数を表現します。
    ''' </summary>
    Structure StringVariable
        Implements IVariable

        ''' <summary>変数の文字列を取得します。</summary>
        Public ReadOnly Value As U8String

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="value">値。</param>
        Public Sub New(value As U8String)
            Me.Value = value
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

    End Structure

    ''' <summary>
    ''' 変数の真偽値を表す構造体です。
    ''' この構造体は、IVariableインターフェイスを実装し、真偽値型の変数を表現します。
    ''' </summary>
    Structure BooleanVariable
        Implements IVariable

        ''' <summary>変数の真偽値を取得します。</summary>
        Public ReadOnly Value As Boolean

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="value">値。</param>
        Public Sub New(value As Boolean)
            Me.Value = value
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

    End Structure

    ''' <summary>
    ''' 変数の配列を表す構造体です。
    ''' この構造体は、IVariableインターフェイスを実装し、複数の変数を格納することができます。
    ''' </summary>
    Structure ArrayVariable
        Implements IVariable

        ''' <summary>変数の配列値を取得します。</summary>
        Public ReadOnly Value As IExpression()

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="value">値の配列。</param>
        Public Sub New(value As IExpression())
            Me.Value = value
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

    End Structure

    ''' <summary>
    ''' 変数のオブジェクトを表す構造体です。
    ''' この構造体は、IVariableインターフェイスを実装し、オブジェクト型の変数を表現します。
    ''' </summary>
    Structure ObjectVariable
        Implements IVariable

        ''' <summary>変数のオブジェクト値を取得します。</summary>
        Public ReadOnly Value As Object

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="value">オブジェクト。</param>
        Public Sub New(value As Object)
            Me.Value = value
        End Sub

        ''' <summary>
        ''' 変数の型を取得します。
        ''' </summary>
        ''' <returns>変数の型。</returns>
        ''' <remarks>この構造体はオブジェクト型の変数を表します。</remarks>
        Public ReadOnly Property Type As VariableType Implements IVariable.Type
            Get
                Return VariableType.Obj
            End Get
        End Property

    End Structure

End Namespace
