Option Strict On
Option Explicit On

Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' リスト式を表す構造体です。
    ''' この構造体は、複数の式をリストとして保持し、式の型を提供します。
    ''' </summary>
    ''' <remarks>
    ''' リスト式は、複数の式をまとめて扱うために使用されます。
    ''' 各式は個別に評価され、その結果が式として返されます。
    ''' </remarks>
    Structure ListExpress
        Implements IExpression

        ' 各式のリスト
        Private ReadOnly _expressions As IExpression()

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="expressions">リスト内の式のリスト。</param>
        Public Sub New(expressions As IExpression())
            _expressions = expressions
        End Sub

        ''' <summary>式の型を取得します。</summary>
        ''' <returns>式の型。</returns>
        Public ReadOnly Property Type As ExpressionType Implements IExpression.Type
            Get
                Return ExpressionType.ListExpress
            End Get
        End Property

        ''' <summary>式の値を取得します。</summary>
        ''' <param name="venv">変数環境。</param>
        ''' <returns>リストの値。</returns>
        ''' <remarks>
        ''' このメソッドは、リスト内の各式を評価し、その結果を返します。
        ''' </remarks>
        Public Function GetValue(venv As AnalysisEnvironment) As IValue Implements IExpression.GetValue
            Throw New NotImplementedException()
        End Function

    End Structure

End Namespace
