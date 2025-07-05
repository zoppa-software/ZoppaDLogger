Option Strict On
Option Explicit On

Imports System.Runtime.CompilerServices
Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' 埋込式の字句解析に関連する機能を提供します。。
    ''' このモジュールは、埋め込み式の解析や評価に関連する機能を提供します。
    ''' </summary>
    ''' <remarks>
    ''' 埋込式は、他の式と組み合わせて使用されることがあります。
    ''' </remarks>
    Public Module LexicalEmbeddedModule

        ''' <summary>Ifリテラル。</summary>
        Private ReadOnly IfLiteral As U8String = U8String.NewString("{if")

        ''' <summary>ElseIfリテラル。</summary>
        Private ReadOnly ElseIfLiteral As U8String = U8String.NewString("{else if")

        ''' <summary>Elseリテラル。</summary>
        Private ReadOnly ElseLiteral As U8String = U8String.NewString("{else}")

        ''' <summary>EndIfリテラル。</summary>
        Private ReadOnly EndIfLiteral As U8String = U8String.NewString("{/if}")

        ''' <summary>Forリテラル。</summary>
        Private ReadOnly ForLiteral As U8String = U8String.NewString("{for")

        ''' <summary>EndForリテラル。</summary>
        Private ReadOnly EndForLiteral As U8String = U8String.NewString("{/for}")

        ''' <summary>Selectリテラル。</summary>
        Private ReadOnly SelectLiteral As U8String = U8String.NewString("{select")

        ''' <summary>SelectCaseリテラル。</summary>
        Private ReadOnly SelectCaseLiteral As U8String = U8String.NewString("{case")

        ''' <summary>SelectDefaultリテラル。</summary>
        Private ReadOnly SelectDefaultLiteral As U8String = U8String.NewString("{default}")

        ''' <summary>EndSelectリテラル。</summary>
        Private ReadOnly EndSelectLiteral As U8String = U8String.NewString("{/select}")

        ''' <summary>Emptyリテラル。</summary>
        Private ReadOnly EmptyLiteral As U8String = U8String.NewString("{}")

        ''' <summary>
        ''' 埋込ブロックを文字列から分割します。
        ''' このメソッドは、入力文字列を解析して埋込ブロックのリストを生成します。
        ''' 埋込ブロックは、特定の文字（例: {, #, !, $）で始まる部分を表します。
        ''' </summary>
        ''' <param name="input">入力文字列。</param>
        ''' <returns>埋込ブロックリスト。</returns>
        <Extension()>
        Public Function SplitEmbeddedText(input As U8String) As EmbeddedBlock()
            Dim embeddeds As New List(Of EmbeddedBlock)()

            Dim iter = input.GetIterator()
            While iter.HasNext()
                If iter.Current IsNot Nothing Then
                    Dim c = iter.Current.Value
                    Dim embedd As EmbeddedBlock
                    If c.Size = 1 Then
                        ' 1文字の場合はトークン解析します
                        Select Case c.Raw0
                            Case &H7B ' {
                                ' 埋込式ブロックを取得します
                                embedd = GetStatementBlock(input, iter)
                            Case &H21 ' !
                                ' 非エスケープ埋込ブロック
                                embedd = GetNoEscUnfoldBlock(input, iter)
                            Case &H23 ' #
                                ' 展開埋込ブロック
                                embedd = GetUnfoldBlock(input, iter)
                            Case &H24 ' $
                                ' 変数宣言ブロック
                                embedd = GetVariableDefineBlock(input, iter)
                            Case Else
                                ' 非埋込ブロック
                                embedd = New EmbeddedBlock() With {
                                    .kind = EmbeddedType.None,
                                    .str = GetNoneEmbeddedBlock(input, iter)
                                }
                        End Select
                    Else
                        ' 非埋込ブロックを取得します
                        embedd = New EmbeddedBlock() With {
                            .kind = EmbeddedType.None,
                            .str = GetNoneEmbeddedBlock(input, iter)
                        }
                    End If
                    embeddeds.Add(embedd)
                End If
            End While

            ' 埋込ブロックリストを返します
            Return embeddeds.ToArray()
        End Function

        ''' <summary>埋込ブロックを取得します。</summary>
        ''' <param name="input">入力文字列。</param>
        ''' <param name="iter">文字列のイテレーター。</param>
        ''' <param name="isEmbeddedText"></param>
        ''' <returns>埋込ブロック。</returns>
        ''' <remarks>
        ''' このメソッドは、埋込ブロックを取得します。
        ''' </remarks>
        Private Function GetEmbeddedBlock(input As U8String, iter As U8String.U8StringIterator, isEmbeddedText As Boolean) As U8String
            Dim startIndex = iter.CurrentIndex
            Dim closed = False

            ' #, !, $ の場合は一文字飛ばす
            If isEmbeddedText Then
                iter.MoveNext()
            End If
            iter.MoveNext()

            ' 埋め込み式の終わりを探す
            While iter.HasNext()
                Dim pc = iter.MoveNext
                If pc?.Size = 1 Then
                    Select Case pc?.Raw0
                        Case &H7D ' }
                            ' } が見つかった場合、ループを終了
                            closed = True
                            Exit While

                        Case &H5C ' \
                            ' エスケープ文字が見つかった場合の { または } を無視
                            Dim nc = iter.Current
                            If nc?.Size = 1 AndAlso (nc?.Raw0 = &H7B OrElse nc?.Raw0 = &H7D) Then ' { or }
                                iter.MoveNext()
                            End If

                        Case Else
                            ' 他の文字は無視
                    End Select
                End If
            End While

            If Not closed Then
                ' 埋め込み式が閉じられていない場合はエラー
                Throw New AnalysisException("埋め込み式が閉じられていません。")
            End If

            ' 文字列を切り出して返す
            Return U8String.NewSlice(input, startIndex, iter.CurrentIndex - startIndex)
        End Function

        ''' <summary>非埋込ブロックを取得します。</summary>
        ''' <param name="input">入力文字列。</param>
        ''' <param name="iter">文字列のイテレーター。</param>
        ''' <returns>埋込ブロック。</returns>
        ''' <remarks>
        ''' このメソッドは、非埋込ブロックを取得します。
        ''' </remarks>
        Private Function GetNoneEmbeddedBlock(input As U8String, iter As U8String.U8StringIterator) As U8String
            Dim startIndex = iter.CurrentIndex

            While iter.HasNext()
                Dim pc = iter.Current
                If pc?.Size = 1 Then
                    Select Case pc?.Raw0
                        Case &H7B ' {
                            ' { が見つかったらテキスト部の終了
                            Exit While

                        Case &H23, &H21, &H24 ' #, !, $
                            ' #{, !{, ${ が見つかったらテキスト部の終了
                            Dim nc = iter.Peek(1)
                            If nc?.Size = 1 AndAlso nc?.Raw0 = &H7B Then ' {
                                Exit While
                            End If

                        Case &H5C ' \
                            ' エスケープ文字が見つかった場合は次の文字を無視
                            Dim nc1 = iter.Peek(1)
                            If nc1?.Size = 1 AndAlso (nc1?.Raw0 = &H7B OrElse nc1?.Raw0 = &H7D) Then ' { or }
                                iter.MoveNext()
                            Else
                                Dim nc2 = iter.Peek(2)
                                If nc1?.Size = 1 AndAlso (nc1?.Raw0 = &H23 OrElse nc1?.Raw0 = &H21 OrElse nc1?.Raw0 = &H24) AndAlso
                                   nc2?.Size = 1 AndAlso nc2?.Raw0 = &H7B Then ' 1文字目(#, !, $), 2文字目({)
                                    iter.MoveNext()
                                    iter.MoveNext()
                                End If
                            End If
                    End Select
                End If
                iter.MoveNext()
            End While

            ' 文字列を切り出して返す
            Return U8String.NewSlice(input, startIndex, iter.CurrentIndex - startIndex)
        End Function

        ''' <summary>埋込式ブロックを取得します。</summary>
        ''' <param name="input">入力文字列。</param>
        ''' <param name="iter">文字列のイテレーター。</param>
        ''' <returns>埋込式ブロック。</returns>
        ''' <remarks>
        ''' このメソッドは、埋込式ブロックを取得します。
        ''' </remarks>
        Private Function GetStatementBlock(input As U8String, iter As U8String.U8StringIterator) As EmbeddedBlock
            Dim cmd = GetEmbeddedBlock(input, iter, True)
            If cmd.StartWith(IfLiteral) AndAlso If(cmd.At(3)?.IsWhiteSpace, True) Then
                Return New EmbeddedBlock() With {
                    .str = cmd.Mid(4, cmd.Length - 5),
                    .kind = EmbeddedType.IfBlock
                }
            ElseIf cmd.StartWith(ElseIfLiteral) AndAlso If(cmd.At(8)?.IsWhiteSpace, True) Then
                Return New EmbeddedBlock() With {
                    .str = cmd.Mid(9, cmd.Length - 10),
                    .kind = EmbeddedType.ElseIfBlock
                }
            ElseIf cmd = ElseLiteral Then
                Return New EmbeddedBlock() With {
                    .str = cmd,
                    .kind = EmbeddedType.ElseBlock
                }
            ElseIf cmd = EndIfLiteral Then
                Return New EmbeddedBlock() With {
                    .str = cmd,
                    .kind = EmbeddedType.EndIfBlock
                }
            ElseIf cmd.StartWith(ForLiteral) AndAlso If(cmd.At(4)?.IsWhiteSpace, True) Then
                Return New EmbeddedBlock() With {
                    .str = cmd.Mid(5, cmd.Length - 6),
                    .kind = EmbeddedType.ForBlock
                }
            ElseIf cmd = EndForLiteral Then
                Return New EmbeddedBlock() With {
                    .str = cmd,
                    .kind = EmbeddedType.EndForBlock
                }
            ElseIf cmd.StartWith(SelectLiteral) AndAlso If(cmd.At(7)?.IsWhiteSpace, True) Then
                Return New EmbeddedBlock() With {
                    .str = cmd.Mid(8, cmd.Length - 9),
                    .kind = EmbeddedType.SelectBlock
                }
            ElseIf cmd.StartWith(SelectCaseLiteral) AndAlso If(cmd.At(5)?.IsWhiteSpace, True) Then
                Return New EmbeddedBlock() With {
                    .str = cmd.Mid(6, cmd.Length - 7),
                    .kind = EmbeddedType.SelectCaseBlock
                }
            ElseIf cmd = SelectDefaultLiteral Then
                Return New EmbeddedBlock() With {
                    .str = cmd,
                    .kind = EmbeddedType.SelectDefaultBlock
                }
            ElseIf cmd = EndSelectLiteral Then
                Return New EmbeddedBlock() With {
                    .str = cmd,
                    .kind = EmbeddedType.EndSelectBlock
                }
            ElseIf cmd = EmptyLiteral Then
                Return New EmbeddedBlock() With {
                    .str = cmd,
                    .kind = EmbeddedType.EmptyBlock
                }
            Else
                ' 埋込ブロックが認識できない場合はエラーを返す
                Throw New AnalysisException("無効な埋込ブロック: " & cmd.ToString())
            End If
        End Function

        ''' <summary>展開埋込ブロックを取得します。</summary>
        ''' <param name="input">入力文字列。</param>
        ''' <param name="iter">文字列のイテレーター。</param>
        ''' <returns>展開埋込ブロック。</returns>
        ''' <remarks>
        ''' このメソッドは、埋込ブロックを取得します。
        ''' </remarks>
        Private Function GetUnfoldBlock(input As U8String, iter As U8String.U8StringIterator) As EmbeddedBlock
            Dim lc = iter.Peek(1)
            If lc?.Size = 1 AndAlso lc?.Raw0 = &H7B Then ' {
                ' { が見つかった場合は変数宣言ブロックを取得
                Return New EmbeddedBlock() With {
                    .kind = EmbeddedType.Unfold,
                    .str = GetEmbeddedBlock(input, iter, True)
                }
            Else
                Return New EmbeddedBlock() With {
                    .kind = EmbeddedType.None,
                    .str = GetNoneEmbeddedBlock(input, iter)
                }
            End If
        End Function

        ''' <summary>非エスケープ展開埋込ブロックを取得します。</summary>
        ''' <param name="input">入力文字列。</param>
        ''' <param name="iter">文字列のイテレーター。</param>
        ''' <returns>非エスケープ展開埋込ブロック。</returns>
        ''' <remarks>
        ''' このメソッドは、非エスケープ展開埋込ブロックを取得します。
        ''' </remarks>
        Private Function GetNoEscUnfoldBlock(input As U8String, iter As U8String.U8StringIterator) As EmbeddedBlock
            Dim lc = iter.Peek(1)
            If lc?.Size = 1 AndAlso lc?.Raw0 = &H7B Then ' {
                ' { が見つかった場合は変数宣言ブロックを取得
                Return New EmbeddedBlock() With {
                    .kind = EmbeddedType.NoEscapeUnfold,
                    .str = GetEmbeddedBlock(input, iter, True)
                }
            Else
                Return New EmbeddedBlock() With {
                    .kind = EmbeddedType.None,
                    .str = GetNoneEmbeddedBlock(input, iter)
                }
            End If
        End Function

        ''' <summary>変数宣言ブロックを取得します。</summary>
        ''' <param name="input">入力文字列。</param>
        ''' <param name="iter">文字列のイテレーター。</param>
        ''' <returns>埋込ブロック。</returns>
        ''' <remarks>
        ''' このメソッドは、変数宣言ブロックを取得します。
        ''' </remarks>
        Private Function GetVariableDefineBlock(input As U8String, iter As U8String.U8StringIterator) As EmbeddedBlock
            Dim lc = iter.Peek(1)
            If lc?.Size = 1 AndAlso lc?.Raw0 = &H7B Then ' {
                ' { が見つかった場合は変数宣言ブロックを取得
                Return New EmbeddedBlock() With {
                    .kind = EmbeddedType.VariableDefine,
                    .str = GetEmbeddedBlock(input, iter, True)
                }
            Else
                Return New EmbeddedBlock() With {
                    .kind = EmbeddedType.None,
                    .str = GetNoneEmbeddedBlock(input, iter)
                }
            End If
        End Function

        ''' <summary>埋込ブロックを表す構造体です。</summary>
        ''' <remarks>
        ''' 埋込ブロックは、文字列とその種類を持つブロックとして表現されます。
        ''' </remarks>
        Public Structure EmbeddedBlock

            ''' <summary>埋込ブロックの種類。</summary>
            Public kind As EmbeddedType

            ''' <summary>埋込式の文字列。</summary>
            Public str As U8String

        End Structure

    End Module

End Namespace
