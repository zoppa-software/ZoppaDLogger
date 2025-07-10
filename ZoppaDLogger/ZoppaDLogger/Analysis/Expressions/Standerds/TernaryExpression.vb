Option Strict On
Option Explicit On

Namespace Analysis

    ''' <summary>
    ''' 三項演算子式を表す構造体です。
    ''' この構造体は、真偽値の値を保持し、式の型を提供します。
    ''' 三項演算子は、条件に基づいて異なる値を返す式です。
    ''' 例: 条件 ? 真の値 : 偽の値
    ''' </summary>
    ''' <remarks>
    ''' 三項演算子は、条件が真の場合に真の値を、偽の場合に偽の値を返します。
    ''' </remarks>
    Structure TernaryExpression
        Implements IExpression

        ' 条件式
        Private ReadOnly _condition As IExpression

        ' 真の値
        Private ReadOnly _trueExpr As IExpression

        ' 偽の値
        Private ReadOnly _falseExpr As IExpression

        ''' <summary>三項演算子式のコンストラクタ。</summary>
        ''' <param name="condition">条件式。</param>
        ''' <param name="trueExpr">条件が真の場合の値。</param>
        ''' <param name="falseExpr">条件が偽の場合の値。</param>
        ''' <remarks>
        ''' このコンストラクタは、三項演算子の条件、真の値、偽の値を初期化します。
        ''' </remarks>
        Public Sub New(condition As IExpression, trueExpr As IExpression, falseExpr As IExpression)
            _condition = condition
            _trueExpr = trueExpr
            _falseExpr = falseExpr
        End Sub

        ''' <summary>式の型を取得します。</summary>
        ''' <returns>式の型。</returns>
        Public ReadOnly Property Type As ExpressionType Implements IExpression.Type
            Get
                Return ExpressionType.TernaryExpression
            End Get
        End Property

        ''' <summary>式の値を取得します。</summary>
        ''' <param name="venv">変数環境。</param>
        Public Function GetValue(venv As AnalysisEnvironment) As IValue Implements IExpression.GetValue
            Dim con As IValue = _condition.GetValue(venv)
            If con.Type <> ValueType.Bool Then
                Throw New InvalidOperationException($"三項演算子の条件（{_condition}）は真偽値でなければなりません。型: {con.Type}")
            End If
            If con.Bool Then
                Return _trueExpr.GetValue(venv)
            Else
                Return _falseExpr.GetValue(venv)
            End If
        End Function

    End Structure

End Namespace