Option Strict On
Option Explicit On
Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' 変数式を表す構造体です。
    ''' この構造体は、変数名を保持し、式の型を提供します。
    ''' </summary>
    ''' <remarks>
    ''' 変数式は、変数の値を取得するために使用されます。
    ''' 例: x, y, z
    ''' </remarks>
    Structure VariableExpression
        Implements IExpression

        ' 値
        Private ReadOnly _name As U8String

        ''' <summary>文字列式のコンストラクタ。</summary>
        ''' <param name="value">文字列の値。</param>
        Public Sub New(expr As U8String)
            _name = expr
        End Sub

        ''' <summary>式の型を取得します。</summary>
        ''' <returns>式の型。</returns>
        Public ReadOnly Property Type As ExpressionType Implements IExpression.Type
            Get
                Return ExpressionType.VariableExpression
            End Get
        End Property

        ''' <summary>
        ''' 式の値を取得します。
        ''' 変数式は、変数の値を取得するために使用されます。
        ''' </summary>
        ''' <param name="venv">変数環境。</param>
        ''' <returns>変数の値。</returns>
        Public Function GetValue(venv As AnalysisEnvironment) As IValue Implements IExpression.GetValue
            Dim v = venv.Get(_name)
            If v Is Nothing Then
                Throw New InvalidOperationException($"変数 '{_name}' は定義されていません。")
            End If

            Select Case v.Type
                Case VariableType.Expr
                    Return DirectCast(v, ExprVariable).Value.GetValue(venv)
                Case VariableType.Bool
                    Return New BooleanValue(DirectCast(v, BooleanVariable).Value)
                Case VariableType.Number
                    Return New NumberValue(DirectCast(v, NumberVariable).Value)
                Case VariableType.Str
                    Return New StringValue(DirectCast(v, StringVariable).Value)
                Case VariableType.Array
                    Dim arr = DirectCast(v, ArrayVariable).Value.Select(Function(i) i.GetValue(venv)).ToArray()
                    Return New ArrayValue(arr)
                Case VariableType.Obj
                    Return New ObjectValue(DirectCast(v, ObjectVariable).Value)
                Case Else
                    Throw New InvalidOperationException("サポートされていない変数の型です。")
            End Select
        End Function

    End Structure

End Namespace
