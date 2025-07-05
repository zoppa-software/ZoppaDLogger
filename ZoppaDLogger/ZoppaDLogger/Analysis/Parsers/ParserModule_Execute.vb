Option Strict On
Option Explicit On

Imports ZoppaDLogger.Strings

Namespace Analysis

    Partial Module ParserModule

        ''' <summary>
        ''' 三項演算子の式を解析します。
        ''' この関数は、三項演算子の式を解析し、結果を `IExpression` として返します。
        ''' 三項演算子の式は、条件式、真の場合の式、偽の場合の式を持ちます。
        ''' 例えば、`condition ? true_expr : false_expr` の形式です。
        ''' </summary>
        ''' <param name="iter">単語のイテレーター。</param>
        ''' <returns>
        ''' この関数は、三項演算子の式を解析し、結果を `IExpression` として返します。
        ''' 三項演算子の式は、条件式、真の場合の式、偽の場合の式を持ちます。
        ''' 例えば、`condition ? true_expr : false_expr` の形式です。
        ''' </returns>
        Private Function ParseTernaryOperator(iter As ParserIterator(Of LexicalModule.Word)) As IExpression
            ' 三項演算子の式を解析します
            Dim condition As IExpression = ParseLogical(iter)

            ' 三項演算子の条件が見つかった場合、真偽値の式を解析します
            If iter.HasNext() AndAlso iter.Current.kind = WordType.Question Then
                iter.Next() ' '?' をスキップ

                ' 真の場合の式を解析
                Dim trueExpr As IExpression = ParseLogical(iter)

                ' ':' が存在するか確認
                If iter.HasNext() AndAlso iter.Current.kind = WordType.Colon Then
                    iter.Next() ' ':' をスキップ

                    ' 偽の場合の式を解析
                    Dim falseExpr As IExpression = ParseLogical(iter)

                    ' 三項演算子の式を生成
                    Return New TernaryExpress(condition, trueExpr, falseExpr)
                Else
                    Throw New AnalysisException("三項演算子の偽の場合の式がありません。")
                End If
            End If
            ' 三項演算子ではない場合、条件式を返す
            Return condition
        End Function

        '        /// 三項演算子の式を解析します。
        '/// この関数は、三項演算子の式を解析し、結果を `Expression` として返します。
        '/// 三項演算子の式は、条件式、真の場合の式、偽の場合の式を持ちます。
        '/// 例えば、`condition ? true_expr : false_expr` の形式です。
        'pub fn ternaryOperatorParser(
        '    allocator: Allocator,
        '    store: *ExpressionStore,
        '    iter: *Iterator(Lexical.Word),
        ') ParserError!*Expression {
        '    Const condition = try logicalParser(allocator, store, iter);
        '    If (iter.peek()) |word| {
        '        If (word.kind == .Question) {
        '            _ = iter.next();

        '            // 三項演算子の左辺と右辺を解析
        '            Const true_expr = try logicalParser(allocator, store, iter);
        '            If (iter.hasNext() And iter.peek().?.kind == .Colon) {
        '                _ = iter.next();
        '            } else {
        '                Return ParserError.TernaryOperatorParseFailed;
        '            }
        '            Const false_expr = try logicalParser(allocator, store, iter);

        '            // 三項演算子の式を生成
        '            Const expr = store.get({}) Catch Return ParserError.OutOfMemoryExpression;
        '            expr.*= .{ .TernaryExpress = .{ .condition = condition, .true_expr = true_expr, .false_expr = false_expr } };
        '            Return expr;
        '        }
        '    }
        '    Return condition;
        '}

        ''' <summary>
        ''' 論理演算子の式を解析します。
        ''' </summary>
        ''' <param name="iter">単語のイテレーター。</param>
        ''' <returns>
        ''' この関数は、論理演算子の式を解析し、結果を `IExpression` として返します。
        ''' 論理演算子の式は、左辺と右辺の式を持ち、演算子として `and`、`or`、`xor` などを使用します。
        ''' </returns>
        Private Function ParseLogical(iter As ParserIterator(Of LexicalModule.Word)) As IExpression
            ' ' 左辺を解析します
            Dim left As IExpression = ParseComparison(iter)

            ' 論理演算子が見つかった場合、右辺を解析します
            While iter.HasNext()
                Dim word = iter.Current
                Select Case word.kind
                    Case WordType.AndOperator, WordType.OrOperator, WordType.XorOperator
                        ' 演算子をスキップ
                        iter.Next()

                        ' 右辺を解析
                        Dim right As IExpression = ParseComparison(iter)

                        ' 二項演算式を作成
                        left = New BinaryExpress(word.kind, left, right)
                    Case Else
                        Exit While
                End Select
            End While
            Return left
        End Function

        ''' <summary>
        ''' 比較演算子を解析します。
        ''' </summary>
        ''' <param name="iter">単語のイテレーター。</param>
        ''' <returns>解析された式。</returns>
        ''' <remarks>
        ''' この関数は、比較演算子を解析し、対応する式を返します。
        ''' 比較演算子は、左辺と右辺の式を比較するために使用されます。
        ''' </remarks>
        Private Function ParseComparison(iter As ParserIterator(Of LexicalModule.Word)) As IExpression
            ' 左辺を解析します
            Dim left As IExpression = ParseAdditionOrSubtraction(iter)

            ' 比較演算子が見つかった場合、右辺を解析します
            While iter.HasNext()
                Dim word = iter.Current
                Select Case word.kind
                    Case WordType.GreaterEqual, WordType.LessEqual, WordType.GreaterThan, WordType.LessThan, WordType.Equal, WordType.NotEqual
                        ' 演算子をスキップ
                        iter.Next()

                        ' 右辺を解析
                        Dim right As IExpression = ParseAdditionOrSubtraction(iter)

                        ' 二項演算式を作成
                        left = New BinaryExpress(word.kind, left, right)
                    Case Else
                        Exit While
                End Select
            End While
            Return left
        End Function

        ''' <summary>
        ''' 加算または減算の演算子を解析します。
        ''' </summary>
        ''' <param name="iter">単語のイテレーター。</param>
        ''' <returns>解析された式。</returns>
        ''' <remarks>
        ''' この関数は、加算や減算の演算子を解析し、対応する式を返します。
        ''' </remarks>
        Private Function ParseAdditionOrSubtraction(iter As ParserIterator(Of LexicalModule.Word)) As IExpression
            ' 左辺を解析します
            Dim left As IExpression = ParseMultiplyOrDivision(iter)

            ' 加算または減算の演算子が見つかった場合、右辺を解析します
            While iter.HasNext()
                Dim word = iter.Current
                If word.kind = WordType.Plus OrElse word.kind = WordType.Minus Then
                    ' 演算子をスキップ
                    iter.Next()

                    ' 右辺を解析
                    Dim right As IExpression = ParseMultiplyOrDivision(iter)

                    ' 二項演算式を作成
                    left = New BinaryExpress(word.kind, left, right)
                Else
                    Exit While
                End If
            End While
            Return left
        End Function

        ''' <summary>
        ''' 乗算または除算の演算子を解析します。
        ''' </summary>
        ''' <param name="iter">単語のイテレーター。</param>
        ''' <returns>解析された式。</returns>
        ''' <remarks>
        ''' この関数は、乗算や除算の演算子を解析し、対応する式を返します。
        ''' </remarks>
        Private Function ParseMultiplyOrDivision(iter As ParserIterator(Of LexicalModule.Word)) As IExpression
            ' 左辺を解析します
            Dim left As IExpression = ParseFactor(iter)

            ' 乗算または除算の演算子が見つかった場合、右辺を解析します
            While iter.HasNext()
                Dim word = iter.Current
                If word.kind = WordType.Multiply OrElse word.kind = WordType.Divide Then
                    ' 演算子をスキップ
                    iter.Next()

                    ' 右辺を解析
                    Dim right As IExpression = ParseFactor(iter)

                    ' 二項演算式を作成
                    left = New BinaryExpress(word.kind, left, right)
                Else
                    Exit While
                End If
            End While
            Return left
        End Function

        ''' <summary>
        ''' 式の要素を解析します。
        ''' </summary>
        ''' <param name="iter">単語のイテレーター。</param>
        ''' <returns>解析された式。</returns>
        ''' <remarks>
        ''' この関数は、単語のイテレーターを使用して、式の要素を解析します。
        ''' 数値、文字列、真偽値などの基本的な要素を処理します。
        ''' </remarks>
        Private Function ParseFactor(iter As ParserIterator(Of LexicalModule.Word)) As IExpression
            Dim word = iter.Current
            Select Case word.kind
                Case WordType.Number
                    ' 数値を解析
                    iter.Next()
                    Return New NumberExpress(ParseNumber(word.str))

                Case WordType.StringLiteral
                    ' 文字列を解析
                    iter.Next()
                    Return New StringExpress(ParseString(word.str))

                Case WordType.TrueLiteral
                    ' 真偽値の真を解析
                    iter.Next()
                    Return New BooleanExpress(True)

                Case WordType.FalseLiteral
                    ' 真偽値の偽を解析
                    iter.Next()
                    Return New BooleanExpress(False)

                Case WordType.Plus, WordType.Minus, WordType.Not
                    ' 単項演算子を解析
                    iter.Next()
                    Return New UnaryExpress(word.kind, ParseFactor(iter))

                Case WordType.LeftParen
                    ' ()括弧内の式を解析
                    Dim expr = ParseParen(iter)
                    If iter.HasNext() AndAlso iter.Current.kind = WordType.RightParen Then
                        ' 右括弧が存在する場合、式を閉じる
                        iter.Next()
                        Return expr
                    Else
                        Throw New AnalysisException("括弧が閉じられていません。")
                    End If

                Case WordType.LeftBracket
                    ' []括弧内の式を解析
                    Dim expr = New ArrayFieldExpress(ParseCommaSplitParameter(WordType.LeftBracket, WordType.RightBracket, iter))
                    If iter.HasNext() AndAlso iter.Current.kind = WordType.RightBracket Then
                        ' 右括弧が存在する場合、式を閉じる
                        iter.Next()
                        Return expr
                    Else
                        Throw New AnalysisException("括弧が閉じられていません。")
                    End If

                Case WordType.Identifier
                    iter.Next()

                    ' 関数呼び出しの場合
                    If iter.HasNext() AndAlso iter.Current.kind = WordType.LeftParen Then
                        Dim expr = New FunctionCallExpress(word.str, ParseCommaSplitParameter(WordType.LeftParen, WordType.RightParen, iter))
                        If iter.HasNext() AndAlso iter.Current.kind = WordType.RightParen Then
                            ' 右括弧が存在する場合、式を閉じる
                            iter.Next()
                            Return expr
                        Else
                            Throw New AnalysisException("関数呼び出しが閉じられていません。")
                        End If
                    End If

                    Dim vexpr As IExpression = New VariableExpress(word.str)
                    While iter.HasNext()
                        Select Case iter.Current.kind
                            Case WordType.LeftBracket
                                ' 配列フィールドアクセスの場合
                                Dim expr = New ArrayAccessExpress(vexpr, ParseBracket(iter))
                                If iter.HasNext() AndAlso iter.Current.kind = WordType.RightBracket Then
                                    ' 右括弧が存在する場合、式を閉じる
                                    iter.Next()
                                    vexpr = expr
                                Else
                                    Throw New AnalysisException("配列フィールドが閉じられていません。")
                                End If

                            Case WordType.Period
                                ' フィールドアクセスの場合
                                iter.Next()
                                If iter.Current.kind = WordType.Identifier Then
                                    vexpr = New FieldAccessExpress(vexpr, iter.Current.str)
                                    iter.Next()
                                Else
                                    Throw New AnalysisException("フィールド名が指定されていません。")
                                End If
                            Case Else
                                Exit While
                        End Select
                    End While
                    Return vexpr

                Case Else
                    Throw New AnalysisException("要素が解析できません。")
            End Select
        End Function

        ''' <summary>数値を解析します。</summary>
        ''' <param name="wordStr">解析する単語。</param>
        ''' <returns>解析された数値。</returns>
        ''' <remarks>
        ''' この関数は、単語から数値を抽出し、整数または小数として返します。
        ''' 小数点が含まれる場合は、適切に処理されます。
        ''' </remarks>
        Public Function ParseNumber(wordStr As U8String) As Double
            If wordStr.Length <= 0 Then
                Throw New InvalidCastException("数値が指定されていません。")
            End If

            Dim iter = wordStr.GetIterator()
            Dim sign As Integer = 1
            Dim dv As ULong = 0
            Dim dd As UInteger = 0

            ' 符号をチェック
            If iter.Current.Value.Raw0 = &H2D Then
                ' 負の数値を検出
                iter.MoveNext()
                If Not iter.HasNext() Then
                    Throw New InvalidCastException("負の数値が不正です。")
                End If
                sign = -1
            ElseIf iter.Current.Value.Raw0 = &H2B Then
                ' 正の数値を検出
                iter.MoveNext()
                If Not iter.HasNext() Then
                    Throw New InvalidCastException("正の数値が不正です。")
                End If
            End If

            ' 数値を解析
            While iter.HasNext()
                Dim cb = iter.MoveNext().Value
                If cb.Raw0 = &H2E Then
                    ' 小数点を検出
                    If dd > 0 Then
                        Throw New InvalidCastException("小数点の位置が不正です。")
                    End If
                    dd = 1
                ElseIf cb.Raw0 >= &H30 AndAlso cb.Raw0 <= &H39 Then
                    dv = CULng(dv * 10 + (cb.Raw0 - &H30))
                    If dd > 0 Then
                        dd = CUInt(dd * 10)
                    End If
                ElseIf cb.Raw0 = &H5F Then
                    ' アンダースコアは無視
                Else
                    Throw New InvalidCastException("数値の解析に失敗しました。")
                End If
            End While
            Return sign * If(dd = 0, dv, dv / dd)
        End Function

        ''' <summary>
        ''' 文字列リテラルを解析します。
        ''' </summary>
        ''' <param name="wordStr">解析する文字列。</param>
        ''' <returns>解析された文字列。</returns>
        ''' <remarks>
        ''' この関数は、文字列リテラルを解析し、エスケープシーケンスを処理します。
        ''' クォートで囲まれた文字列を正しく処理します。
        ''' </remarks>
        Public Function ParseString(wordStr As U8String) As U8String
            Dim buffer As New List(Of Byte)()
            Dim iter = wordStr.GetIterator()

            ' 最初のクォートを確認
            Dim quote = iter.MoveNext()
            If quote.Value.Raw0 <> &H22 AndAlso quote.Value.Raw0 <> &H27 Then
                Throw New InvalidCastException("文字列リテラルはダブルクォート、シングルクォートで始まる必要があります。")
            End If

            ' クォートの後の文字を解析
            While iter.HasNext()
                Dim c = iter.MoveNext().Value
                If c.Size = 1 AndAlso c.Raw0 = quote.Value.Raw0 Then
                    Dim nc = iter.Current
                    If nc.HasValue AndAlso nc.Value.Size = 1 AndAlso
                       nc.Value.Raw0 = quote.Value.Raw0 Then
                        ' クォートが連続している場合、バッファに追加
                        buffer.Add(quote.Value.Raw0)
                        iter.MoveNext() ' 次のクォートをスキップ
                    Else
                        ' クォートが閉じられた場合、ループを終了
                        Exit While
                    End If
                ElseIf c.Size = 1 AndAlso c.Raw0 = &H5C Then
                    ' エスケープシーケンスを処理
                    If Not iter.HasNext() Then
                        Throw New InvalidCastException("エスケープシーケンスが不完全です。")
                    End If
                    Select Case iter.MoveNext().Value.Raw0
                        Case &H6E : buffer.Add(&HA) ' 改行
                        Case &H74 : buffer.Add(&H9) ' タブ
                        Case &H5C : buffer.Add(&H5C) ' バックスラッシュ
                        Case &H22 : buffer.Add(&H22) ' ダブルクォート
                        Case &H27 : buffer.Add(&H27) ' シングルクォート
                        Case &H7B : buffer.Add(&H7B) ' 左中括弧
                        Case &H7D : buffer.Add(&H7D) ' 右中括弧
                        Case Else : Throw New InvalidCastException("不明なエスケープシーケンスです。")
                    End Select
                Else
                    ' 通常の文字を追加
                    Select Case c.Size
                        Case 1
                            buffer.Add(c.Raw0)
                        Case 2
                            buffer.Add(c.Raw0)
                            buffer.Add(c.Raw1)
                        Case 3
                            buffer.Add(c.Raw0)
                            buffer.Add(c.Raw1)
                            buffer.Add(c.Raw2)
                        Case Else
                            buffer.Add(c.Raw0)
                            buffer.Add(c.Raw1)
                            buffer.Add(c.Raw2)
                            buffer.Add(c.Raw3)
                    End Select
                End If
            End While
            Return U8String.NewString(buffer.ToArray())
        End Function

        ''' <summary>
        ''' ()括弧内の式を解析します。
        ''' </summary>
        ''' <param name="iter">単語のイテレーター。</param>
        ''' <returns>解析された式。</returns>
        ''' <remarks>
        ''' この関数は、()括弧内の式を解析し、結果を `IExpression` として返します。
        ''' ()括弧は、他の式をグループ化化するために使用されます。
        ''' </remarks>
        Private Function ParseParen(iter As ParserIterator(Of Word)) As IExpression
            ' 左括弧をスキップ
            iter.Next()

            ' ()括弧内の式を走査するためのイテレーターを作成
            Dim range = ParenInnerRange(WordType.LeftParen, WordType.RightParen, iter)
            Dim inIter = iter.GetRangeIterator(range.start, range.end)

            ' カッコ内の式を解析し、結果式を返す
            Return New ParenExpress(ParseTernaryOperator(inIter))
        End Function

        ''' <summary>
        ''' []括弧内の式を解析します。
        ''' </summary>
        ''' <param name="iter">単語のイテレーター。</param>
        ''' <returns>解析された式。</returns>
        ''' <remarks>
        ''' この関数は、[]括弧内の式を解析し、結果を `IExpression` として返します。
        ''' []括弧は、配列やリストの要素にアクセスするために使用されます。
        ''' </remarks>
        Private Function ParseBracket(iter As ParserIterator(Of Word)) As IExpression
            ' 左括弧をスキップ
            iter.Next()

            ' ()括弧内の式を走査するためのイテレーターを作成
            Dim range = ParenInnerRange(WordType.LeftBracket, WordType.RightBracket, iter)
            Dim inIter = iter.GetRangeIterator(range.start, range.end)

            ' カッコ内の式を解析し、結果式を返す
            Return New ParenExpress(ParseTernaryOperator(inIter))
        End Function

        ''' <summary>
        ''' 括弧の対応をネストレベルでカウントして判定し、カッコ内の文字列を取得します。
        ''' </summary>
        ''' <param name="lParen">左括弧の種類。</param>
        ''' <param name="rParen">右括弧の種類。</param>
        ''' <param name="iter">解析する単語のイテレーター。</param>
        ''' <returns>括弧の開始位置と終了位置のタプル。</returns>
        ''' <remarks>
        ''' この関数は、指定された括弧のネストレベルをカウントし、対応する括弧の範囲を返します。
        ''' </remarks>
        Private Function ParenInnerRange(lParen As WordType,
                                         rParen As WordType,
                                         iter As ParserIterator(Of LexicalModule.Word)) As (start As Integer, [end] As Integer)
            Dim startIndex As Integer = iter.CurrentIndex
            Dim endIndex As Integer = iter.CurrentIndex
            Dim lv As Integer = 0
            While iter.HasNext()
                Dim word = iter.Current
                Select Case word.kind
                    Case lParen
                        ' 左括弧が見つかった場合、ネストレベルを増やす
                        lv += 1

                    Case rParen
                        ' 右括弧が見つかった場合、ネストレベルを減らす
                        If lv > 0 Then
                            lv -= 1
                        Else
                            ' ネストが終了でループも終了
                            Exit While
                        End If

                    Case Else
                        ' 他の単語は無視
                End Select

                ' 次の単語に進む
                iter.Next()
                endIndex = iter.CurrentIndex
            End While
            Return (startIndex, endIndex)
        End Function

        ''' <summary>
        ''' カンマで区切られた式のリストを解析します。
        ''' </summary>
        ''' <param name="lParen">左括弧の種類。</param>
        ''' <param name="rParen">右括弧の種類。</param>
        ''' <param name="iter">解析する単語のイテレーター。</param>
        ''' <returns>解析された式の配列。</returns>
        ''' <remarks>
        ''' この関数は、カンマで区切られた式を解析し、結果を `IExpression` の配列として返します。
        ''' </remarks>
        Private Function ParseCommaSplitParameter(lParen As WordType, rParen As WordType, iter As ParserIterator(Of Word)) As IExpression()
            Dim exper As New List(Of IExpression)()

            ' 左括弧をスキップ
            iter.Next()

            ' ()括弧内の式を走査するためのイテレーターを作成
            Dim range = ParenInnerRange(lParen, rParen, iter)
            Dim inIter = iter.GetRangeIterator(range.start, range.end)

            While (inIter.HasNext())
                ' 配列内の要素を取得
                exper.Add(ParseTernaryOperator(inIter))

                ' カンマまたは右括弧を判定して配列を評価
                If inIter.HasNext() Then
                    Select Case inIter.Current.kind
                        Case WordType.Comma
                            ' カンマをスキップ
                            inIter.Next()

                        Case rParen
                            ' 右括弧が見つかった場合、ループを終了
                            Exit While
                        Case Else
                            Throw New AnalysisException("無効な式です。")
                    End Select
                End If
            End While

            Return exper.ToArray()
        End Function

    End Module

End Namespace
