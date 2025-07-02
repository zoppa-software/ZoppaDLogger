Option Strict On
Option Explicit On

Imports ZoppaDLogger.Analysis.LexicalModule
Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' パーサー用のイテレーターを表すクラスです。
    ''' このクラスは、解析中の単語、ブロックのイテレーションを管理します。
    ''' </summary>
    ''' <typeparam name="T">イテレーションする要素の型。</typeparam>
    ''' <remarks>
    ''' このクラスは、単語、ブロックリストをイテレートし、次の要素を取得するためのメソッドを提供します。
    ''' </remarks>
    Public NotInheritable Class ParserIterator(Of T)

        ' 要素、ブロックのリストを保持する
        Private ReadOnly _items As List(Of T)

        ' 現在のインデックスを保持する
        Private _currentIndex As Integer

        ''' <summary>現在の要素を取得します。</summary>
        ''' <returns>現在の要素。</returns>
        ''' <remarks>
        ''' 現在のインデックスに対応する要素を返します。
        ''' インデックスが範囲外の場合は、Nothingを返します。
        ''' </remarks>
        Public ReadOnly Property Current As T
            Get
                If _currentIndex < _items.Count Then
                    Return _items(_currentIndex)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="words"></param>
        Public Sub New(items As IEnumerable(Of T))
            Me._items = New List(Of T)(items)
            Me._currentIndex = 0
        End Sub

        ''' <summary>
        ''' 次の要素が存在するかどうかを確認します。
        ''' </summary>
        ''' <returns>次の要素が存在する場合はTrue、それ以外はFalse。</returns>
        ''' <remarks>
        ''' 現在のインデックスがリストの範囲内であるかをチェックします。
        ''' </remarks>
        Public Function HasNext() As Boolean
            Return _currentIndex < _items.Count
        End Function

        ''' <summary>次の要素を取得します。</summary>
        ''' <returns>次の要素。</returns>
        ''' <remarks>
        ''' 現在のインデックスをインクリメントし、次の要素を返します。
        ''' インデックスが範囲外の場合は、Nothingを返します。
        ''' </remarks>
        Public Function [Next]() As T
            If _currentIndex < _items.Count Then
                Dim res = _items(_currentIndex)
                _currentIndex += 1
                Return res
            Else
                Return Nothing
            End If
        End Function

    End Class

End Namespace
