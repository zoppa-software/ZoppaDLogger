Option Strict On
Option Explicit On

Imports ZoppaDLogger.Collections

Namespace Analysis

    ''' <summary>
    ''' 変数のコレクションを表すクラスです。
    ''' このクラスは、変数の名前と値を格納し、名前で検索することができます。
    ''' </summary>
    Public NotInheritable Class Variables

        ' 変数リスト
        Private ReadOnly _variables As Btree(Of Entry)

        ''' <summary>
        ''' コンストラクタ。
        ''' 変数のコレクションを空のB木として初期化します。
        ''' </summary>
        Public Sub New()
            Me._variables = New Btree(Of Entry)()
        End Sub

        ''' <summary>
        ''' 変数を登録します。
        ''' 既に同じ名前の変数が存在する場合は、値を更新します。
        ''' </summary>
        ''' <param name="name">登録する変数名。</param>
        Public Sub Regist(name As String, value As IVariable)
            Dim entry As New Entry(name, value)
            Dim serd As Entry = Me._variables.Search(entry)
            If serd Is Nothing Then
                Me._variables.Insert(entry)
            Else
                serd.Value = value
            End If
        End Sub

        ''' <summary>
        ''' 変数を取得します。
        ''' </summary>
        ''' <param name="name">変数名。</param>
        ''' <exception cref="KeyNotFoundException">指定した名前の変数がない。</exception>
        Public Function [Get](name As String) As IVariable
            Dim entry As Entry = Me._variables.Search(New Entry(name, Nothing))
            If entry IsNot Nothing Then
                Return entry.Value
            Else
                Throw New KeyNotFoundException($"変数 '{name}' は登録されていません。")
            End If
        End Function

        ''' <summary>
        ''' 変数を登録解除します。
        ''' 指定した名前の変数が存在する場合は削除します。
        ''' </summary>
        ''' <param name="name">登録解除する変数名。</param>
        Public Sub Unregist(name As String)
            Dim entry As Entry = Me._variables.Search(New Entry(name, Nothing))
            If entry IsNot Nothing Then
                Me._variables.Remove(entry)
            End If
        End Sub

        ''' <summary>変数エントリ。</summary>
        Private NotInheritable Class Entry
            Implements IComparable(Of Entry)

            ' 変数の名前
            Public ReadOnly Name As String

            ' 変数の値
            Public Value As IVariable

            '''' <summary>
            ''' コンストラクタ。
            ''' 変数の名前と値を指定してエントリを初期化します。
            ''' </summary>
            ''' <param name="name">変数の名前。</param>
            ''' <param name="value">変数の値。</param>
            Public Sub New(name As String, value As IVariable)
                Me.Name = name
                Me.Value = value
            End Sub

            ''' <summary>
            ''' 変数の名前でエントリを比較します。
            ''' </summary>
            ''' <param name="other">比較対象のエントリ。</param>
            ''' <returns>比較結果。名前が同じ場合は0、異なる場合は名前の辞書順で比較した結果。</returns>
            Public Function CompareTo(other As Entry) As Integer Implements IComparable(Of Entry).CompareTo
                Return Me.Name.CompareTo(other.Name)
            End Function

        End Class

    End Class

End Namespace
