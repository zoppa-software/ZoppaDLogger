Option Strict On
Option Explicit On

Imports System.Linq.Expressions
Imports ZoppaDLogger.Analysis.Variables
Imports ZoppaDLogger.Strings

Namespace Analysis

    Partial Module ParserModule

        ''' <summary>
        ''' 埋め込みテキストを解析します。
        ''' </summary>
        ''' <param name="iter">パーサーイテレーター。</param>
        ''' <returns>解析された式。</returns>
        ''' <remarks>
        ''' このメソッドは、埋め込みテキストを解析し、式を生成します。
        ''' </remarks>
        Private Function ParseEmbeddedText(iter As ParserIterator(Of EmbeddedBlock)) As IExpression
            ' 埋め込み式のリストを作成します
            Dim exprs As New List(Of IExpression)()

            ' 埋め込み式の解析を行います
            While iter.HasNext()
                Dim embedded = iter.Current
                Select Case embedded.kind
                    Case EmbeddedType.None
                        ' 埋込式以外
                        exprs.Add(New PlainTextExpression(embedded.str))
                        iter.Next()

                    Case EmbeddedType.Unfold
                        ' 展開埋込式
                        Dim inExpr = ParserModule.DirectExecutes(embedded.str.Mid(2, embedded.str.Length - 3))
                        exprs.Add(New UnfoldExpression(inExpr))
                        iter.Next()

                    Case EmbeddedType.NoEscapeUnfold
                        ' 非エスケープ展開埋込式
                        Dim inExpr = ParserModule.DirectExecutes(embedded.str.Mid(2, embedded.str.Length - 3))
                        exprs.Add(New NoEscapeUnfoldExpression(inExpr))
                        iter.Next()

                    Case EmbeddedType.VariableDefine
                        ' 変数定義埋込式
                        exprs.Add(ParseVariableDefineBlock(embedded.str.Mid(2, embedded.str.Length - 3)))
                        iter.Next()

                    Case EmbeddedType.IfBlock
                        ' Ifブロック
                        Dim expr = ParseIfStatement(iter)
                        If iter.HasNext() AndAlso iter.Current.kind = EmbeddedType.EndIfBlock Then
                            exprs.Add(expr)
                            iter.Next()
                        Else
                            Throw New AnalysisException("Ifブロックが閉じられていません。")
                        End If

                    Case EmbeddedType.ElseIfBlock, EmbeddedType.ElseBlock, EmbeddedType.EndIfBlock
                        ' ElseIfブロック、Elseブロック、EndIfブロックはエラー
                        Throw New AnalysisException("Ifブロックが開始されていません。ElseIf、Else、EndIfはIfブロック内でのみ使用できます。")

                    Case EmbeddedType.ForBlock
                        ' Forブロック
                        Dim expr = ParseForStatement(iter)
                        If iter.HasNext() AndAlso iter.Current.kind = EmbeddedType.EndForBlock Then
                            exprs.Add(expr)
                            iter.Next()
                        Else
                            Throw New AnalysisException("Forブロックが閉じられていません。")
                        End If

                        'Case LexicalEmbeddedModule.EmbeddedKind.SelectBlock
                        '    ' Selectブロックを解析します
                        '    Dim expr = ParseSelectBlock(iter)
                        '    If iter.HasNext() AndAlso iter.Current.kind = LexicalEmbeddedModule.EmbeddedKind.EndSelectBlock Then
                        '        exprs.Add(expr)
                        '        iter.Next()
                        '    Else
                        '        Throw New AnalysisException("Selectブロックが閉じられていません。")
                        '    End If

                    Case EmbeddedType.EmptyBlock
                        ' 空のブロック
                        exprs.Add(New EmptyExpression())
                        iter.Next()
                End Select
            End While

            Return New ListExpression(exprs.ToArray())
        End Function

        '        /// 埋め込み式の解析を行う関数
        '/// この関数は、埋め込み式を含む文字列を解析し、`Expression` を返します。
        'pub fn embeddedTextParser(
        '    allocator: Allocator,
        '    store: *ExpressionStore,
        '    iter: *Iterator(Lexical.EmbeddedText),
        ') ParserError!*Expression {
        '    // バッファを作成します
        '    var exprs = ArrayList(* Expression).init(allocator);
        '    defer exprs.deinit();

        '    // 埋め込み式の解析を行います
        '    While (iter.hasNext()) {
        '        Const embedded = iter.peek().?;

        '        switch (embedded.kind) {
        '            .ForBlock => {
        '                // Forブロックを解析、最後の埋め込み式が EndFor であることを確認します
        '                Const expr = try parseForBlock(allocator, store, iter);
        '                If (iter.hasNext() And iter.peek().?.kind == .EndForBlock) {
        '                    exprs.append(expr) catch return ParserError.OutOfMemoryExpression;
        '                    _ = iter.next();
        '                } else {
        '                    Return ParserError.ForBlockNotClosed;
        '                }
        '            },
        '            .EndForBlock => {
        '                // EndForブロックを解析
        '                Return ParserError.IfBlockNotStarted;
        '            },
        '            .EmptyBlock => {
        '                // 空のブロックは無視します
        '                _ = iter.next();
        '            },
        '            .SelectBlock => {
        '                // selectブロックを解析、最後の埋め込み式が EndSelect であることを確認します
        '                Const expr = try parseSelectBlock(allocator, store, iter);
        '                If (iter.hasNext() And iter.peek().?.kind == .EndSelectBlock) {
        '                    exprs.append(expr) catch return ParserError.OutOfMemoryExpression;
        '                    _ = iter.next();
        '                } else {
        '                    Return ParserError.SelectBlockNotClosed;
        '                }
        '            },
        '            .SelectCaseBlock, .SelectDefaultBlock, .EndSelectBlock >= {
        '                // Selectブロックが閉じられていない場合はエラーを返します
        '                Return ParserError.SelectBlockNotStarted;
        '            },
        '            //else => {
        '            //    // サポートされていない埋め込み式の場合はエラーを返す
        '            //    return ParserError.UnsupportedEmbeddedExpression;
        '            //},
        '        }
        '    }

        '    // 埋め込み式、非埋め込み式のリストを作成し、返します
        '    Const list_expr = store.get({}) Catch Return ParserError.OutOfMemoryExpression;
        '    Const list_exprs = exprs.toOwnedSlice() Catch Return ParserError.OutOfMemoryExpression;
        '    list_expr.*= .{ .ListExpress = .{ .exprs = list_exprs } };
        '    Return list_expr;
        '}

        ''' <summary>
        ''' 変数定義ブロックを解析します。
        ''' 変数定義ブロックは、変数名とその値を定義するために使用されます。
        ''' 変数名はU8String型で指定され、値はIExpression型で表されます。
        ''' </summary>
        ''' <param name="embeddedText">変数定義ブロック文字列。</param>
        ''' <returns>変数定義式。</returns>
        Private Function ParseVariableDefineBlock(embeddedText As U8String) As IExpression
            ' 式バッファを生成
            Dim exprs As New List(Of VariableDefineExpression)()

            ' 入力文字列を単語に分割
            Dim words = LexicalModule.SplitWords(embeddedText)

            ' 変数式を解析します
            Dim iter As New ParserIterator(Of LexicalModule.Word)(words)
            While iter.HasNext()
                exprs.Add(ParseVvariable(iter))
                If iter.HasNext() Then
                    If iter.Current.kind <> WordType.Semicolon Then
                        Throw New AnalysisException("変数定義はセミコロンで区切られている必要があります")
                    End If
                    iter.Next() ' セミコロンをスキップ
                End If
            End While

            ' 解析していない文が残っている場合はエラーを返します
            If iter.HasNext() Then
                Throw New AnalysisException("変数定義ブロックが正しく宣言されていません")
            End If

            ' 変数式をリストとして返します
            Return New VariableDefineListExpression(exprs.ToArray())
        End Function

        ''' <summary>
        ''' Ifステートメントを解析します。
        ''' Ifステートメントは、条件に基づいて異なる処理を実行するために使用されます。
        ''' このメソッドは、Ifブロック、ElseIfブロック、Elseブロック、およびEndIfブロックを解析します。
        ''' </summary>
        ''' <param name="iter">イテレータ。</param>
        ''' <returns>Ifステートメント。</returns>
        Private Function ParseIfStatement(iter As ParserIterator(Of EmbeddedBlock)) As IExpression
            ' 式バッファを生成します
            Dim exprs As New List(Of IExpression)()

            ' 最初の条件を取得
            Dim prevBlock = iter.Next()
            Dim prevType = EmbeddedType.IfBlock

            Dim st = iter.CurrentIndex
            Dim ed = iter.CurrentIndex
            Dim lv = 0
            Dim update = False
            While iter.HasNext()
                Dim stat = iter.Current
                Select Case stat.kind
                    Case EmbeddedType.IfBlock
                        ' Ifブロックの開始
                        lv += 1

                    Case EmbeddedType.ElseIfBlock
                        ' Else Ifブロックの開始
                        If lv = 0 Then
                            exprs.Add(ParseIfExpression(iter, prevBlock, prevType, st, ed))
                            prevBlock = stat
                            prevType = EmbeddedType.ElseIfBlock
                            update = True
                        End If

                    Case EmbeddedType.ElseBlock
                        ' Elseブロックの処理
                        If lv = 0 Then
                            exprs.Add(ParseIfExpression(iter, prevBlock, prevType, st, ed))
                            prevBlock = stat
                            prevType = EmbeddedType.ElseBlock
                            update = True
                        End If

                    Case EmbeddedType.EndIfBlock
                        ' Ifブロックの終了
                        If lv > 0 Then
                            lv -= 1
                        Else
                            exprs.Add(ParseIfExpression(iter, prevBlock, prevType, st, ed))
                            Exit While ' ネストが終了でループも終了
                        End If

                    Case Else
                        ' 他の埋め込みブロックは無視するか、エラーを投げることも可能ですが、ここでは無視します。
                End Select
                iter.Next()

                If update Then
                    st = iter.CurrentIndex
                    update = False
                End If
                ed = iter.CurrentIndex
            End While

            ' Ifステートメントを解析します
            Return New IfStatementExpression(exprs.ToArray())
        End Function

        ''' <summary>
        ''' Ifブロックの式を解析します。
        ''' このメソッドは、Ifブロック、ElseIfブロック、Elseブロックの条件式を解析し、対応する式を返します。
        ''' </summary>
        ''' <param name="iter">イテレーター。</param>
        ''' <param name="prevBlock">前のIf式。</param>
        ''' <param name="prevType">前のIf式の型。</param>
        ''' <param name="st">開始位置。</param>
        ''' <param name="ed">終了位置。</param>
        ''' <returns>解析した式。</returns>
        Private Function ParseIfExpression(
            iter As ParserIterator(Of EmbeddedBlock),
            prevBlock As EmbeddedBlock,
            prevType As EmbeddedType,
            st As Integer,
            ed As Integer
        ) As IExpression
            ' ブロック内の要素のイテレータを作成します
            Dim inIter = iter.GetRangeIterator(st, ed)

            ' 条件式を解析します
            Select Case prevType
                Case EmbeddedType.IfBlock, EmbeddedType.ElseIfBlock
                    ' IfブロックまたはElseIfブロックの条件式を解析
                    Dim condition = ParserModule.DirectExecutes(prevBlock.str)
                    ' Ifブロックの実行部を解析
                    Dim innerExpr = ParseEmbeddedText(inIter)
                    ' If条件式を作成
                    Return New IfExpression(condition, innerExpr)

                Case EmbeddedType.ElseBlock
                    ' Elseブロックの実行部を作成
                    Dim innerExpr = ParseEmbeddedText(inIter)
                    Return New ElseExpression(innerExpr)

                Case Else
                    Throw New AnalysisException("条件式の解析に失敗しました。")
            End Select
        End Function

        ''' <summary>
        ''' Forステートメントを解析します。
        ''' Forステートメントは、繰り返し処理を行うために使用されます。
        ''' </summary>
        ''' <param name="iter">イテレータ。</param>
        ''' <returns>Forステートメント。</returns>
        ''' <remarks>
        ''' このメソッドは、Forブロックの開始から終了までの範囲を解析し、対応する式を返します。
        ''' </remarks>
        Private Function ParseForStatement(iter As ParserIterator(Of EmbeddedBlock)) As IExpression
            ' forブロックの開始を取得
            Dim forCondition = iter.Next()

            ' forの繰り返し範囲を取得
            Dim st = iter.CurrentIndex
            Dim ed = iter.CurrentIndex
            Dim lv = 0
            Dim endFor = False
            While iter.HasNext()
                Select Case iter.Current.kind
                    Case EmbeddedType.ForBlock
                        ' Forが開始された場合、ネストレベルを増やす
                        lv += 1

                    Case EmbeddedType.EndForBlock
                        ' Forが終了された場合、ネストレベルを減らす
                        If lv > 0 Then
                            lv -= 1
                        Else
                            endFor = True
                            Exit While ' ネストが終了でループも終了
                        End If

                    Case Else
                        ' 他の埋め込みブロックは無視するか、エラーを投げることも可能ですが、ここでは無視します。
                End Select

                iter.Next()
                ed = iter.CurrentIndex
            End While

            If endFor Then
                ' 入力文字列を単語に分割します
                Dim words = LexicalModule.SplitWords(forCondition.str)

                ' forの繰り返し条件を解析します
                Dim iterWords = New ParserIterator(Of LexicalModule.Word)(words)
                Dim forExpr = ParserModule.ParseForStatement(iterWords)

                ' forの繰り返す範囲を式として解析します
                Dim bodyIter = iter.GetRangeIterator(st, ed)
                Dim bodyExpr = ParseEmbeddedText(bodyIter)

                Return New ForExpression(forExpr.varName, forExpr.collectionExpr, bodyExpr)
            Else
                ' Forブロックが閉じられていない場合はエラーを返す
                Throw New AnalysisException("Forブロックが閉じられていません。")
            End If
        End Function

    End Module

End Namespace
