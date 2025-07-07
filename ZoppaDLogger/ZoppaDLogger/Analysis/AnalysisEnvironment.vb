Option Strict On
Option Explicit On

Imports System.Reflection
Imports ZoppaDLogger.Analysis.Variables
Imports ZoppaDLogger.Collections
Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' 変数の環境を表すクラスです。
    ''' 変数の定義や管理を行います。
    ''' </summary>
    ''' <remarks>
    ''' このクラスは、変数のスコープや値の管理を行うために使用されます。
    ''' </remarks>
    Public NotInheritable Class AnalysisEnvironment

        ' 変数階層
        Private ReadOnly _hierarhy As Variables

        ' 関数リスト
        Private ReadOnly _functuons As Btree(Of FuncEntry)

        ''' <summary>コンストラクタ。</summary>
        Public Sub New()
            Me._hierarhy = New Variables()
            Me._functuons = New Btree(Of FuncEntry)()
            Me._functuons.Insert(
                New FuncEntry(U8String.NewString("now"), Nothing, GetType(AnalysisEnvironment).GetMethod("Now"))
            )
        End Sub

        ''' <summary>現在の日時を文字列として取得します。</summary>
        ''' <returns>現在の日時を表す文字列。</returns>
        ''' <remarks>
        ''' このメソッドは、"yyyy/MM/dd HH:mm:ss"形式で現在の日時を返します。
        ''' </remarks>
        Private Shared Function Now() As String
            Return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        End Function

        ''' <summary>指定したキーと値の変数を登録します。</summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に変数を登録します。
        ''' </remarks>
        Public Sub Regist(key As U8String, value As IVariable)
            Me._hierarhy.Regist(key, value)
        End Sub

        ''' <summary>指定したキーと値の式を登録します。</summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する式。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に変数を登録します。
        ''' </remarks>
        Public Sub RegistExpr(key As U8String, value As IExpression)
            Me._hierarhy.Regist(key, New ExprVariable(value))
        End Sub

        ''' <summary>指定したキーと値の式を登録します。</summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する式。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に変数を登録します。
        ''' </remarks>
        Public Sub RegistExpr(key As String, value As IExpression)
            Me._hierarhy.Regist(U8String.NewString(key), New ExprVariable(value))
        End Sub

        ''' <summary>指定したキーと値の数値変数を登録します。</summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に数値変数を登録します。
        ''' </remarks>
        Public Sub RegistNumber(key As U8String, value As Double)
            Me._hierarhy.Regist(key, New NumberVariable(value))
        End Sub

        ''' <summary>指定したキーと値の数値変数を登録します。</summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に数値変数を登録します。
        ''' </remarks>
        Public Sub RegistNumber(key As String, value As Double)
            Me._hierarhy.Regist(U8String.NewString(key), New NumberVariable(value))
        End Sub

        ''' <summary>指定したキーと値の文字列変数を登録します。</summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に文字列変数を登録します。
        ''' </remarks>
        Public Sub RegistStr(key As U8String, value As U8String)
            Me._hierarhy.Regist(key, New StringVariable(value))
        End Sub

        ''' <summary>指定したキーと値の文字列変数を登録します。</summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に文字列変数を登録します。
        ''' </remarks>
        Public Sub RegistStr(key As U8String, value As String)
            Me._hierarhy.Regist(key, New StringVariable(U8String.NewString(value)))
        End Sub

        ''' <summary>指定したキーと値の文字列変数を登録します。</summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に文字列変数を登録します。
        ''' </remarks>
        Public Sub RegistStr(key As String, value As U8String)
            Me._hierarhy.Regist(U8String.NewString(key), New StringVariable(value))
        End Sub

        ''' <summary>指定したキーと値の文字列変数を登録します。</summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に文字列変数を登録します。
        ''' </remarks>
        Public Sub RegistStr(key As String, value As String)
            Me._hierarhy.Regist(U8String.NewString(key), New StringVariable(U8String.NewString(value)))
        End Sub

        ''' <summary>指定したキーと値の真偽値変数を登録します。</summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に真偽値変数を登録します。
        ''' </remarks>
        Public Sub RegistBool(key As U8String, value As Boolean)
            Me._hierarhy.Regist(key, New BooleanVariable(value))
        End Sub

        ''' <summary>指定したキーと値の真偽値変数を登録します。</summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に真偽値変数を登録します。
        ''' </remarks>
        Public Sub RegistBool(key As String, value As Boolean)
            Me._hierarhy.Regist(U8String.NewString(key), New BooleanVariable(value))
        End Sub

        ''' <summary>指定したキーと値の配列を登録します。</summary>
        ''' <typeparam name="T">配列の値。</typeparam>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する配列。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層にオブジェクト変数を登録します。
        ''' </remarks>
        Public Sub RegistArray(Of T)(key As String, ParamArray value As T())
            Me.RegistArray(U8String.NewString(key), value)
        End Sub

        ''' <summary>指定したキーと値の配列を登録します。</summary>
        ''' <typeparam name="T">配列の値。</typeparam>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する配列。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層にオブジェクト変数を登録します。
        ''' </remarks>
        Public Sub RegistArray(Of T)(key As U8String, ParamArray value As T())
            Dim arr = value.Select(
                Function(v) As IExpression
                    Select Case GetType(T)
                        Case GetType(Double)
                            Return New NumberExpress(CDbl(CObj(v)))
                        Case GetType(Integer)
                            Return New NumberExpress(CDbl(CInt(CObj(v))))
                        Case GetType(String)
                            Return New StringExpress(U8String.NewString(CStr(CObj(v))))
                        Case GetType(U8String)
                            Return New StringExpress(DirectCast(CObj(v), U8String))
                        Case GetType(Boolean)
                            Return New BooleanExpress(CBool(CObj(v)))
                        Case Else
                            Return New ObjectExpress(CObj(v))
                    End Select
                End Function).ToArray()
            Me._hierarhy.Regist(key, New ArrayVariable(arr))
        End Sub

        ''' <summary>
        ''' 指定したキーと値のオブジェクト変数を登録します。
        ''' </summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="obj">登録するオブジェクト。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層にオブジェクト変数を登録します。
        ''' </remarks>
        Public Sub RegistObject(key As String, obj As Object)
            Me._hierarhy.Regist(U8String.NewString(key), New ObjectVariable(obj))
        End Sub

        ''' <summary>
        ''' 指定したキーと値のオブジェクト変数を登録します。
        ''' </summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="obj">登録するオブジェクト。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層にオブジェクト変数を登録します。
        ''' </remarks>
        Public Sub RegistObject(key As U8String, obj As Object)
            Me._hierarhy.Regist(key, New ObjectVariable(obj))
        End Sub

        ''' <summary>
        ''' 指定したキーの変数を取得します。
        ''' </summary>
        ''' <param name="key">取得する変数のキー。</param>
        ''' <returns>指定されたキーの変数。</returns>
        ''' <exception cref="KeyNotFoundException">指定されたキーの変数が存在しない場合にスローされます。</exception>
        ''' <remarks>
        ''' このメソッドは、現在の階層から指定されたキーの変数を検索します。
        ''' もし見つからない場合は、例外をスローします。
        ''' </remarks>
        Public Function [Get](key As U8String) As IVariable
            Dim result = Me._hierarhy.Get(key)
            If result Is Nothing Then
                Throw New KeyNotFoundException($"変数 '{key}' は存在しません")
            End If
            Return result
        End Function

        ''' <summary>
        ''' 指定したキーの変数を取得します。
        ''' </summary>
        ''' <param name="key">取得する変数のキー。</param>
        ''' <returns>指定されたキーの変数。</returns>
        ''' <exception cref="KeyNotFoundException">指定されたキーの変数が存在しない場合にスローされます。</exception>
        ''' <remarks>
        ''' このメソッドは、現在の階層から指定されたキーの変数を検索します。
        ''' もし見つからない場合は、例外をスローします。
        ''' </remarks>
        Public Function [Get](key As String) As IVariable
            Return Me.Get(U8String.NewString(key))
        End Function

        ''' <summary>
        ''' 指定したキーの変数を削除します。
        ''' </summary>
        ''' <param name="key">削除する変数のキー。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層から指定されたキーの変数を削除します。
        ''' </remarks>
        Public Sub Unregist(key As U8String)
            Me._hierarhy.Unregist(key)
        End Sub

        ''' <summary>
        ''' 指定したキーの変数を削除します。
        ''' </summary>
        ''' <param name="key">削除する変数のキー。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層から指定されたキーの変数を削除します。
        ''' </remarks>
        Public Sub Unregist(key As String)
            Me._hierarhy.Unregist(U8String.NewString(key))
        End Sub

        ''' <summary>
        ''' 新しいスコープを開始します。
        ''' 現在のスコープをスタックにプッシュし、新しいスコープを作成します。
        ''' </summary>
        ''' <returns></returns>
        Public Function GetScope() As Variables.Scope
            Return _hierarhy.GetScope()
        End Function

        '''' <summary>
        '''' 現在の階層に新しい変数階層を追加します。
        '''' </summary>
        '''' <remarks>
        '''' このメソッドは、新しい変数階層を作成し、現在の階層に追加します。
        '''' これにより、変数のスコープを分離できます。
        '''' </remarks>
        'Public Sub AddHierarchy()
        '    Me._hierarhy.Add(New Variables())
        'End Sub

        '''' <summary>
        '''' 現在の階層を削除します。
        '''' ただし、最初の階層は削除できません。
        '''' </summary>
        '''' <remarks>
        '''' このメソッドは、現在の変数階層を削除し、前の階層に戻ります。
        '''' </remarks>
        'Public Sub RemoveHierarchy()
        '    If Me._hierarhy.Count > 1 Then
        '        Me._hierarhy.RemoveAt(Me._hierarhy.Count - 1)
        '    End If
        'End Sub

        ''' <summary>
        ''' 指定した名前の変数の値を取得します。
        ''' </summary>
        ''' <param name="name">取得する変数の名前。</param>
        ''' <returns>指定された名前の変数の値。</returns>
        ''' <exception cref="KeyNotFoundException">指定された名前の変数が存在しない場合にスローされます。</exception>
        ''' <remarks>
        ''' このメソッドは、変数の値をIValueとして返します。
        ''' </remarks>
        Public Function GetValue(name As U8String) As IValue
            Dim val = Me.Get(name)
            Select Case val.Type
                Case VariableType.Expr
                    Return DirectCast(val, ExprVariable).Value.GetValue(Me)
                Case VariableType.Number
                    Return New NumberValue(DirectCast(val, NumberVariable).Value)
                Case VariableType.Str
                    Return New StringValue(DirectCast(val, StringVariable).Value)
                Case VariableType.Bool
                    Return New BooleanValue(DirectCast(val, BooleanVariable).Value)
                Case VariableType.Array
                    Dim arr = DirectCast(val, ArrayVariable).Value.Select(Function(i) i.GetValue(Me)).ToArray()
                    Return New ArrayValue(arr)
                Case VariableType.Obj
                    Return New ObjectValue(DirectCast(val, ObjectVariable).Value)
                Case Else
                    Throw New InvalidOperationException($"変数 '{name}' の値は取得できません。")
            End Select
        End Function

        ''' <summary>
        ''' 関数の登録を行います。
        ''' このメソッドは、関数名と関数の実行方法を登録します。
        ''' 登録された関数は、後で呼び出すことができます。
        ''' </summary>
        ''' <param name="name">関数名。</param>
        ''' <param name="func">関数の実行方法。</param>
        Public Sub AddFunction(name As U8String, func As Func(Of IValue))
            Dim entry As New FuncEntry(name, func.Target, func.Method)
            Me._functuons.Insert(entry)
        End Sub

        ''' <summary>
        ''' 関数の登録を行います。
        ''' このメソッドは、関数名と関数の実行方法を登録します。
        ''' 登録された関数は、後で呼び出すことができます。
        ''' </summary>
        ''' <param name="name">関数名。</param>
        ''' <param name="func">関数の実行方法。</param>
        Public Sub AddFunction(name As U8String, func As Func(Of IValue, IValue))
            Dim entry As New FuncEntry(name, func.Target, func.Method)
            Me._functuons.Insert(entry)
        End Sub

        ''' <summary>
        ''' 関数の登録を行います。
        ''' このメソッドは、関数名と関数の実行方法を登録します。
        ''' 登録された関数は、後で呼び出すことができます。
        ''' </summary>
        ''' <param name="name">関数名。</param>
        ''' <param name="func">関数の実行方法。</param>
        Public Sub AddFunction(name As U8String, func As Func(Of IValue, IValue, IValue))
            Dim entry As New FuncEntry(name, func.Target, func.Method)
            Me._functuons.Insert(entry)
        End Sub

        ''' <summary>
        ''' 関数の登録を行います。
        ''' このメソッドは、関数名と関数の実行方法を登録します。
        ''' 登録された関数は、後で呼び出すことができます。
        ''' </summary>
        ''' <param name="name">関数名。</param>
        ''' <param name="func">関数の実行方法。</param>
        Public Sub AddFunction(name As U8String, func As Func(Of IValue(), IValue))
            Dim entry As New FuncEntry(name, func.Target, func.Method)
            Me._functuons.Insert(entry)
        End Sub

        ''' <summary>
        ''' 関数を呼び出します。
        ''' 指定された関数名と引数で関数を実行し、結果を返します。
        ''' </summary>
        ''' <param name="name">呼び出す関数名。</param>
        ''' <param name="parameter">関数に渡す引数の配列。</param>
        ''' <returns>関数の実行結果。</returns>
        ''' <remarks>
        ''' このメソッドは、登録された関数を呼び出し、結果を返します。
        ''' </remarks>
        Friend Function CallFunction(name As U8String, parameter() As IValue) As IValue
            Dim entry = Me._functuons.Search(New FuncEntry(name, Nothing, Nothing))
            Return CType(entry.callfunc?.Invoke(entry.callins, parameter), IValue)
        End Function

        ''' <summary>関数エントリ。</summary>
        Private Structure FuncEntry
            Implements IComparable(Of FuncEntry)

            ''' <summary>関数名。</summary>
            Public ReadOnly name As U8String

            ''' <summary>関数のインスタンス。</summary>
            Public ReadOnly callins As Object

            ''' <summary>実行する関数。</summary>
            Public ReadOnly callfunc As MethodInfo

            ''' <summary>関数エントリのコンストラクタ。</summary>
            ''' <param name="name">関数名。</param>
            ''' <param name="callins">関数のインスタンス。</param>
            ''' <param name="callfunc">実行する関数。</param>
            Public Sub New(name As U8String, callins As Object, callfunc As MethodInfo)
                Me.name = name
                Me.callins = callins
                Me.callfunc = callfunc
            End Sub

            ''' <summary>
            ''' 関数エントリを比較します。
            ''' このメソッドは、関数名を基準にしてエントリを比較します。
            ''' </summary>
            ''' <param name="other">比較対象。</param>
            ''' <returns>比較結果。</returns>
            Public Function CompareTo(other As FuncEntry) As Integer Implements IComparable(Of FuncEntry).CompareTo
                Return name.CompareTo(other.name)
            End Function

        End Structure

    End Class

End Namespace
