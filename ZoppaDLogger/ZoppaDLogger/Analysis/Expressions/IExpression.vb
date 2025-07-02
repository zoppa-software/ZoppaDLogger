Option Strict On
Option Explicit On

Namespace Analysis

    Public Interface IExpression

        ''' <summary>式の型を取得します。</summary>
        ''' <returns>式の型。</returns>
        ReadOnly Property Type As ExpressionType

    End Interface

End Namespace
