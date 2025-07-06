Option Strict On
Option Explicit On

Namespace Analysis

    ''' <summary>
    ''' 空の式を表す構造体です。
    ''' この構造体は、何も値を持たない式を表現します。
    ''' </summary>
    ''' <remarks>
    ''' 空の式は、特定の状況で使用されることがありますが、
    ''' 通常は何も意味を持たないため、実際の値を返すことはありません。
    ''' </remarks>
    Structure EmptyExpress
        Implements IExpression

        ''' <summary>式の型を取得します。</summary>
        ''' <returns>式の型。</returns>
        Public ReadOnly Property Type As ExpressionType Implements IExpression.Type
            Get
                Return ExpressionType.EmptyExpress
            End Get
        End Property

        ''' <summary>
        ''' 式の値を取得します。
        ''' 空の式は何も値を持たないため、空の文字列値を返します。
        ''' </summary>
        ''' <param name="venv">変数環境。</param>
        ''' <returns>空の文字列値。</returns>
        Public Function GetValue(venv As AnalysisEnvironment) As IValue Implements IExpression.GetValue
            Return StringValue.Empty
        End Function

    End Structure

End Namespace