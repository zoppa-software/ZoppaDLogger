Option Strict On
Option Explicit On

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
    Public NotInheritable Class VariableEnvironment

        ' 変数階層
        Private ReadOnly _hierarhy As List(Of Variables)

        ''' <summary>コンストラクタ。</summary>
        Public Sub New()
            Me._hierarhy = New List(Of Variables) From {
                New Variables()
            }
        End Sub

        ''' <summary>
        ''' 指定したキーと値の変数を登録します。
        ''' </summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に変数を登録します。
        ''' </remarks>
        Public Sub Regist(key As U8String, value As IVariable)
            Me._hierarhy.Last().Regist(key, value)
        End Sub

        ''' <summary>
        ''' 指定したキーと値の式を登録します。
        ''' </summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する式。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に変数を登録します。
        ''' </remarks>
        Public Sub RegistExpr(key As U8String, value As IExpression)
            Me._hierarhy.Last().Regist(key, New VariableExpr(value))
        End Sub

        ''' <summary>
        ''' 指定したキーと値の式を登録します。
        ''' </summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する式。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に変数を登録します。
        ''' </remarks>
        Public Sub RegistExpr(key As String, value As IExpression)
            Me._hierarhy.Last().Regist(U8String.NewString(key), New VariableExpr(value))
        End Sub

        ''' <summary>
        ''' 指定したキーと値の数値変数を登録します。
        ''' </summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に数値変数を登録します。
        ''' </remarks>
        Public Sub RegistNumber(key As U8String, value As Double)
            Me._hierarhy.Last().Regist(key, New VariableNumber(value))
        End Sub

        ''' <summary>
        ''' 指定したキーと値の数値変数を登録します。
        ''' </summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に数値変数を登録します。
        ''' </remarks>
        Public Sub RegistNumber(key As String, value As Double)
            Me._hierarhy.Last().Regist(U8String.NewString(key), New VariableNumber(value))
        End Sub

        ''' <summary>
        ''' 指定したキーと値の文字列変数を登録します。
        ''' </summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に文字列変数を登録します。
        ''' </remarks>
        Public Sub RegistStr(key As U8String, value As U8String)
            Me._hierarhy.Last().Regist(key, New VariableStr(value))
        End Sub

        ''' <summary>
        ''' 指定したキーと値の文字列変数を登録します。
        ''' </summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に文字列変数を登録します。
        ''' </remarks>
        Public Sub RegistStr(key As U8String, value As String)
            Me._hierarhy.Last().Regist(key, New VariableStr(U8String.NewString(value)))
        End Sub

        ''' <summary>
        ''' 指定したキーと値の文字列変数を登録します。
        ''' </summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に文字列変数を登録します。
        ''' </remarks>
        Public Sub RegistStr(key As String, value As U8String)
            Me._hierarhy.Last().Regist(U8String.NewString(key), New VariableStr(value))
        End Sub

        ''' <summary>
        ''' 指定したキーと値の文字列変数を登録します。
        ''' </summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に文字列変数を登録します。
        ''' </remarks>
        Public Sub RegistStr(key As String, value As String)
            Me._hierarhy.Last().Regist(U8String.NewString(key), New VariableStr(U8String.NewString(value)))
        End Sub

        ''' <summary>
        ''' 指定したキーと値の真偽値変数を登録します。
        ''' </summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に真偽値変数を登録します。
        ''' </remarks>
        Public Sub RegistBool(key As U8String, value As Boolean)
            Me._hierarhy.Last().Regist(key, New VariableBool(value))
        End Sub

        ''' <summary>
        ''' 指定したキーと値の真偽値変数を登録します。
        ''' </summary>
        ''' <param name="key">登録する変数のキー。</param>
        ''' <param name="value">登録する変数の値。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層に真偽値変数を登録します。
        ''' </remarks>
        Public Sub RegistBool(key As String, value As Boolean)
            Me._hierarhy.Last().Regist(U8String.NewString(key), New VariableBool(value))
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
            Dim result As IVariable = Nothing
            For i As Integer = Me._hierarhy.Count - 1 To 0 Step -1
                result = Me._hierarhy(i).Get(key)
                If result IsNot Nothing Then
                    Exit For
                End If
            Next
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
            Me._hierarhy.Last().Unregist(key)
        End Sub

        ''' <summary>
        ''' 指定したキーの変数を削除します。
        ''' </summary>
        ''' <param name="key">削除する変数のキー。</param>
        ''' <remarks>
        ''' このメソッドは、現在の階層から指定されたキーの変数を削除します。
        ''' </remarks>
        Public Sub Unregist(key As String)
            Me._hierarhy.Last().Unregist(U8String.NewString(key))
        End Sub

        ''' <summary>
        ''' 現在の階層に新しい変数階層を追加します。
        ''' </summary>
        ''' <remarks>
        ''' このメソッドは、新しい変数階層を作成し、現在の階層に追加します。
        ''' これにより、変数のスコープを分離できます。
        ''' </remarks>
        Public Sub AddHierarchy()
            Me._hierarhy.Add(New Variables())
        End Sub

        ''' <summary>
        ''' 現在の階層を削除します。
        ''' ただし、最初の階層は削除できません。
        ''' </summary>
        ''' <remarks>
        ''' このメソッドは、現在の変数階層を削除し、前の階層に戻ります。
        ''' </remarks>
        Public Sub RemoveHierarchy()
            If Me._hierarhy.Count > 1 Then
                Me._hierarhy.RemoveAt(Me._hierarhy.Count - 1)
            End If
        End Sub

    End Class

End Namespace
