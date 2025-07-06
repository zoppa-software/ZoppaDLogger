Option Strict On
Option Explicit On

Imports System.Linq.Expressions
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
        Private Function ParseEmbeddedText(iter As ParserIterator(Of LexicalEmbeddedModule.EmbeddedBlock)) As IExpression
            ' 埋め込み式のリストを作成します
            Dim exprs As New List(Of IExpression)()

            ' 埋め込み式の解析を行います
            While iter.HasNext()
                Dim embedded = iter.Current
                Select Case embedded.kind
                    Case EmbeddedType.None
                        ' 埋め込み式以外
                        exprs.Add(New PlainTextExpress(embedded.str))
                        iter.Next()

                    Case EmbeddedType.Unfold
                        ' 展開埋め込み式
                        Dim inExpr = ParserModule.DirectExecutes(embedded.str.Mid(2, embedded.str.Length - 3))
                        exprs.Add(New UnfoldExpress(inExpr))
                        iter.Next()

                    Case EmbeddedType.NoEscapeUnfold
                        ' 非エスケープ展開埋め込み式
                        Dim inExpr = ParserModule.DirectExecutes(embedded.str.Mid(2, embedded.str.Length - 3))
                        exprs.Add(New NoEscapeUnfoldExpress(inExpr))
                        iter.Next()
                        '    Dim inExpr = ParseDirectExecutes(embedded.str.Substring(2, embedded.str.Length - 4))
                        '    Dim expr = New NoEscapeUnfoldExpress(inExpr)
                        '    exprs.Add(expr)
                        '    iter.Next()
                        'Case LexicalEmbeddedModule.EmbeddedKind.Variables
                        '    ' 変数埋め込み式を解析します
                        '    exprs.Add(ParseVariablesBlock(embedded))
                        '    iter.Next()
                        'Case LexicalEmbeddedModule.EmbeddedKind.IfBlock
                        '    ' Ifブロックを解析します
                        '    Dim expr = ParseIfBlock(iter)
                        '    If iter.HasNext() AndAlso iter.Current.kind = LexicalEmbeddedModule.EmbeddedKind.EndIfBlock Then
                        '        exprs.Add(expr)
                        '        iter.Next()
                        '    Else
                        '        Throw New AnalysisException("Ifブロックが閉じられていません。")
                        '    End If
                        'Case LexicalEmbeddedModule.EmbeddedKind.ForBlock
                        '    ' Forブロックを解析します
                        '    Dim expr = ParseForBlock(iter)
                        '    If iter.HasNext() AndAlso iter.Current.kind = LexicalEmbeddedModule.EmbeddedKind.EndForBlock Then
                        '        exprs.Add(expr)
                        '        iter.Next()
                        '    Else
                        '        Throw New AnalysisException("Forブロックが閉じられていません。")
                        '    End If
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
                        exprs.Add(New EmptyExpress())
                        iter.Next()
                End Select
            End While

            Return New ListExpress(exprs.ToArray())
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
        '            .None => {
        '                // 埋め込み式以外は文字列として扱います
        '                Const expr = store.get({}) Catch Return ParserError.OutOfMemoryExpression;
        '                expr.*= .{ .NoneEmbeddedExpress = embedded.str };
        '                exprs.append(expr) catch return ParserError.OutOfMemoryExpression;
        '                _ = iter.next();
        '            },
        '            .Unfold => {
        '                // 展開埋め込み式を解析します
        '                Const in_expr = try Parser.directExecutes(allocator, store, &embedded.str.mid(2, embedded.str.len() - 3));
        '                Const expr = store.get({}) Catch Return ParserError.OutOfMemoryExpression;
        '                expr.*= .{ .UnfoldExpress = in_expr };
        '                exprs.append(expr) catch return ParserError.OutOfMemoryExpression;
        '                _ = iter.next();
        '            },
        '            .NoEscapeUnfold => {
        '                // 非エスケープ展開埋め込み式を解析します
        '                Const in_expr = try Parser.directExecutes(allocator, store, &embedded.str.mid(2, embedded.str.len() - 3));
        '                Const expr = store.get({}) Catch Return ParserError.OutOfMemoryExpression;
        '                expr.*= .{ .NoEscapeUnfoldExpress = in_expr };
        '                exprs.append(expr) catch return ParserError.OutOfMemoryExpression;
        '                _ = iter.next();
        '            },
        '            .Variables => {
        '                // 変数埋め込み式を解析します
        '                exprs.append(try parseVariablesBlock(allocator, store, embedded)) catch return ParserError.OutOfMemoryExpression;
        '                _ = iter.next();
        '            },
        '            .IfBlock => {
        '                // Ifブロックを解析、最後の埋め込み式が EndIf であることを確認します
        '                Const expr = try parseIfBlock(allocator, store, iter);
        '                If (iter.hasNext() And iter.peek().?.kind == .EndIfBlock) {
        '                    exprs.append(expr) catch return ParserError.OutOfMemoryExpression;
        '                    _ = iter.next();
        '                } else {
        '                    Return ParserError.IfBlockNotClosed;
        '                }
        '            },
        '            .ElseIfBlock, .ElseBlock, .EndIfBlock >= {
        '                // ElseIfブロックまたはElseブロック、EndIfを解析
        '                Return ParserError.IfBlockNotStarted;
        '            },
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

    End Module

End Namespace
