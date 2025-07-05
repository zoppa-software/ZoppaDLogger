Option Strict On
Option Explicit On
Imports ZoppaDLogger.Strings

Namespace Analysis


    Structure VariableExpress
        Implements IExpression

        ' 値
        Private ReadOnly _name As U8String

        ''' <summary>文字列式のコンストラクタ。</summary>
        ''' <param name="value">文字列の値。</param>
        Public Sub New(expr As U8String)
            _name = expr
        End Sub

        Public ReadOnly Property Type As ExpressionType Implements IExpression.Type
            Get
                Return ExpressionType.VariableExpress
            End Get
        End Property

        Public Function GetValue(venv As AnalysisEnvironment) As IValue Implements IExpression.GetValue
            Dim v = venv.Get(_name)
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
