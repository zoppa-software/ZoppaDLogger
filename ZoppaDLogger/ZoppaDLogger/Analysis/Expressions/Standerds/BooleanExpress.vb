Option Strict On
Option Explicit On

Namespace Analysis

    ''' <summary>
    ''' 真偽値式を表す構造体です。
    ''' この構造体は、真偽値の値を保持し、式の型を提供します。
    ''' </summary>
    Structure BooleanExpress
        Implements IExpression

        ' 値
        Private ReadOnly _value As Boolean

        ''' <summary>真偽値式のコンストラクタ。</summary>
        ''' <param name="value">真偽値の値。</param>
        Public Sub New(value As Boolean)
            _value = value
        End Sub

        ''' <summary>式の型を取得します。</summary>
        ''' <returns>式の型。</returns>
        Public ReadOnly Property Type As ExpressionType Implements IExpression.Type
            Get
                Return ExpressionType.BooleanExpress
            End Get
        End Property

        ''' <summary>式の値を取得します。</summary>
        ''' <param name="venv">変数環境。</param>
        Public Function GetValue(venv As AnalysisEnvironment) As IValue Implements IExpression.GetValue
            Return New BooleanValue(_value)
        End Function

    End Structure

End Namespace
