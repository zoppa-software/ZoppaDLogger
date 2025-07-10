Option Strict On
Option Explicit On

Imports System.Runtime.CompilerServices
Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' LexicalModuleは、プログラムの字句解析に関連する機能を提供します。
    ''' ここでは、字句解析に必要な型や関数を定義します。
    ''' </summary>
    ''' <remarks>
    ''' このモジュールは、字句解析のための基本的な構造を提供し、
    ''' プログラム内で使用されるキーワードや記号を定義します。
    ''' </summary>
    Public Module LexicalModule

        ''' <summary>trueキーワードを表す定数。</summary>
        Public ReadOnly TrueKeyword As U8String = U8String.NewString("true")

        ''' <summary>falseキーワードを表す定数。</summary>
        Public ReadOnly FalseKeyword As U8String = U8String.NewString("false")

        ''' <summary>notキーワードを表す定数。</summary>
        Private ReadOnly NotKeyword As U8String = U8String.NewString("not")

        ''' <summary>andキーワードを表す定数。</summary>
        Private ReadOnly AndKeyword As U8String = U8String.NewString("and")

        ''' <summary>orキーワードを表す定数。</summary>
        Private ReadOnly OrKeyword As U8String = U8String.NewString("or")

        ''' <summary>xorキーワードを表す定数。</summary>
        Private ReadOnly XorKeyword As U8String = U8String.NewString("xor")

        ''' <summary>inキーワードを表す定数。</summary>
        Private ReadOnly InKeyword As U8String = U8String.NewString("in")

        ''' <summary>nullキーワードを表す定数。</summary>
        Private ReadOnly NullKeyword As U8String = U8String.NewString("null")

        ' 単語分割に使用する文字の配列を定義します。
        Private ReadOnly _splitChars As New Lazy(Of Boolean())(
            Function()
                Dim res = New Boolean(255) {}
                res(AscW(" "c)) = True : res(&H9) = True : res(&HA) = True
                res(&HD) = True : res(0) = True : res(AscW("+"c)) = True
                res(AscW("-"c)) = True : res(AscW("*"c)) = True : res(AscW("/"c)) = True
                res(AscW("="c)) = True : res(AscW("<"c)) = True : res(AscW(">"c)) = True
                res(AscW("("c)) = True : res(AscW(")"c)) = True : res(AscW("["c)) = True
                res(AscW("]"c)) = True : res(AscW("!"c)) = True : res(AscW(","c)) = True
                res(AscW("#"c)) = True : res(AscW("$"c)) = True : res(AscW("?"c)) = True
                res(AscW(":"c)) = True : res(AscW(";"c)) = True : res(AscW("\"c)) = True
                res(AscW("."c)) = True : res(AscW("'"c)) = True : res(AscW(""""c)) = True
                Return res
            End Function
        )

        ''' <summary>
        ''' 単語の種類を定義する列挙型です。
        ''' この列挙型は、プログラム内で使用されるキーワードや記号を表します。
        ''' </summary>
        ''' <summary>
        ''' 文字列を単語に分割します。
        ''' </summary>
        ''' <param name="input">入力文字列。</param>
        ''' <returns>分割された単語の配列。</returns>
        <Extension()>
        Public Function SplitWords(input As U8String) As Word()
            Dim words As New List(Of Word)()

            ' 分割文字テーブルを作成します。
            ' 分割文字は、空白文字や特定の記号（例: +, -, *, /, = など）です。
            Dim splitChars = _splitChars.Value

            Dim iter = input.GetIterator()
            While iter.HasNext()
                If iter.Current IsNot Nothing Then
                    Dim c = iter.Current.Value

                    If c.IsWhiteSpace Then
                        ' 空白文字の場合はスキップします。
                        iter.MoveNext()
                    ElseIf c.Size = 1 Then
                        ' 1文字の場合はトークン解析します
                        words.Add(SwitchOneCharToWord(splitChars, input, iter, c))
                    Else
                        ' それ以外の文字列はキーワードまたは識別子とみなす
                        words.Add(GetWordString(splitChars, input, iter))
                    End If
                End If
            End While

            ' 分割された単語リストを返します
            Return words.ToArray()
        End Function

        ''' <summary>
        ''' 先頭1文字から文字列を作成し、単語に変換します。
        ''' </summary>
        ''' <param name="splitChars">分割文字の配列。</param>
        ''' <param name="input">入力文字列。</param>
        ''' <param name="iter">文字列イテレータ。</param>
        ''' <param name="c">現在の文字。</param>
        ''' <returns>変換された単語。</returns>
        ''' <remarks>
        ''' この関数は、1文字の文字列を特定の単語に変換します。
        ''' </remarks>
        Private Function SwitchOneCharToWord(splitChars As Boolean(), input As U8String, iter As U8String.U8StringIterator, c As U8Char) As Word
            Select Case c.Raw0
                Case &H21 ' !
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.Not}
                Case &H22 ' "
                    Return New Word With {.str = GetStringLiteralToken(input, iter, &H22), .kind = WordType.StringLiteral}
                Case &H23 ' #
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.Hash}
                Case &H24 ' $
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.Dollar}
                Case &H27 ' '
                    Return New Word With {.str = GetStringLiteralToken(input, iter, &H27), .kind = WordType.StringLiteral}
                Case &H28 ' (
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.LeftParen}
                Case &H29 ' )
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.RightParen}
                Case &H2A ' *
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.Multiply}
                Case &H2B ' +
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.Plus}
                Case &H2C ' ,
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.Comma}
                Case &H2D ' -
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.Minus}
                Case &H2E ' .
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.Period}
                Case &H2F ' /
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.Divide}
                Case &H30 To &H39 ' 0-9
                    Return New Word With {.str = GetNumberLiteralToken(input, iter), .kind = WordType.Number}
                Case &H3A ' :
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.Colon}
                Case &H3B ' ;
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.Semicolon}
                Case &H3C ' <
                    Return GetLessWord(input, iter)
                Case &H3D ' =
                    Return GetEqualWord(input, iter)
                Case &H3E ' >
                    Return GetGreaterWord(input, iter)
                Case &H3F ' ?
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.Question}
                Case &H5B ' [
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.LeftBracket}
                Case &H5C ' \
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.Backslash}
                Case &H5D ' ]
                    Return New Word With {.str = GetOneCharString(input, iter), .kind = WordType.RightBracket}
                Case Else
                    ' それ以外の文字は単語として処理
                    Return GetWordString(splitChars, input, iter)
            End Select
        End Function

        ''' <summary>
        ''' 1文字の文字列を取得します。
        ''' </summary>
        ''' <param name="input">入力文字列。</param>
        ''' <param name="iter">文字列イテレータ。</param>
        ''' <returns>1文字の文字列。</returns>
        ''' <remarks>
        ''' この関数は、イテレータの現在位置から1文字を取得し、新しいU8Stringを返します。
        ''' </remarks>
        Private Function GetOneCharString(input As U8String, iter As U8String.U8StringIterator) As U8String
            Dim res = U8String.NewSlice(input, iter.CurrentIndex, 1)
            iter.MoveNext()
            Return res
        End Function

        ''' <summary>
        ''' 小なり演算子を取得します。
        ''' </summary>
        ''' <param name="input">入力文字列。</param>
        ''' <param name="iter">文字列イテレータ。</param>
        ''' <returns>小なり演算子のWord。</returns>
        ''' <remarks>
        ''' この関数は、イテレータの現在位置から小なり演算子を解析し、新しいWordを返します。
        ''' </remarks>
        Private Function GetLessWord(input As U8String, iter As U8String.U8StringIterator) As Word
            If iter.HasNext() Then
                Dim lc = iter.MoveNext()
                Dim nc = iter.Current
                If nc.HasValue AndAlso nc.Value.Size = 1 Then
                    Select Case nc.Value.Raw0
                        Case &H3D ' =
                            ' <= 演算子
                            Dim res = New Word With {
                                .str = U8String.NewSlice(input, iter.CurrentIndex - 1, 2),
                                .kind = WordType.LessEqual
                            }
                            iter.MoveNext() ' '=' をスキップ
                            Return res
                        Case &H3E ' >
                            ' <> 演算子
                            Dim res = New Word With {
                                .str = U8String.NewSlice(input, iter.CurrentIndex - 1, 2),
                                .kind = WordType.NotEqual
                            }
                            iter.MoveNext() ' '>' をスキップ
                            Return res
                    End Select
                End If
            End If
            ' 単なる < 演算子
            Return New Word With {.str = U8String.NewSlice(input, iter.CurrentIndex - 1, 1), .kind = WordType.LessThan}
        End Function

        ''' <summary>
        ''' 等価演算子を取得します。
        ''' </summary>
        ''' <param name="input">入力文字列。</param>
        ''' <param name="iter">文字列イテレータ。</param>
        ''' <returns>等価演算子のWord。</returns>
        ''' <remarks>
        ''' この関数は、イテレータの現在位置から等価演算子を解析し、新しいWordを返します。
        ''' </remarks>
        Private Function GetEqualWord(input As U8String, iter As U8String.U8StringIterator) As Word
            If iter.HasNext() Then
                Dim lc = iter.MoveNext()
                Dim nc = iter.Current
                If nc.HasValue AndAlso nc.Value.Size = 1 Then
                    If nc.Value.Raw0 = &H3D Then ' '='
                        ' == 演算子
                        Dim res = New Word With {
                            .str = U8String.NewSlice(input, iter.CurrentIndex - 1, 2),
                            .kind = WordType.Equal
                        }
                        iter.MoveNext() ' '=' をスキップ
                        Return res
                    End If
                End If
            End If
            ' 単なる = 演算子
            Return New Word With {.str = U8String.NewSlice(input, iter.CurrentIndex - 1, 1), .kind = WordType.Assign}
        End Function

        ''' <summary>
        ''' 大なり演算子を取得します。
        ''' </summary>
        ''' <param name="input">入力文字列。</param>
        ''' <param name="iter">文字列イテレータ。</param>
        ''' <returns>大なり演算子のWord。</returns>
        ''' <remarks>
        ''' この関数は、イテレータの現在位置から大なり演算子を解析し、新しいWordを返します。
        ''' </remarks>
        Private Function GetGreaterWord(input As U8String, iter As U8String.U8StringIterator) As Word
            If iter.HasNext() Then
                Dim lc = iter.MoveNext()
                Dim nc = iter.Current
                If nc.HasValue AndAlso nc.Value.Size = 1 Then
                    Select Case nc.Value.Raw0
                        Case &H3D ' =
                            ' >= 演算子
                            Dim res = New Word With {
                                .str = U8String.NewSlice(input, iter.CurrentIndex - 1, 2),
                                .kind = WordType.GreaterEqual
                            }
                            iter.MoveNext() ' '=' をスキップ
                            Return res
                    End Select
                End If
            End If
            ' 単なる > 演算子
            Return New Word With {.str = U8String.NewSlice(input, iter.CurrentIndex - 1, 1), .kind = WordType.GreaterThan}
        End Function

        ''' <summary>
        ''' 文字列から単語を取得します。
        ''' イテレータを使用して、空白文字または分割文字が見つかるまで文字を読み取り、その文字列をWord構造体に変換します。
        ''' </summary>
        ''' <param name="splitChars">分割文字の配列。</param>
        ''' <param name="input">入力文字列。</param>
        ''' <param name="iter">文字列イテレータ。</param>
        ''' <returns>取得された単語。</returns>
        ''' <remarks>
        ''' この関数は、イテレータの現在位置から単語を取得し、新しいWord構造体を返します。
        ''' </remarks>
        Private Function GetWordString(splitChars As Boolean(), input As U8String, iter As U8String.U8StringIterator) As Word
            Dim startIndex = iter.CurrentIndex

            ' イテレータを進めて、空白文字または分割文字が見つかるまで読み取る
            While iter.HasNext()
                Dim c = iter.Current
                If c IsNot Nothing Then
                    ' 空白文字または分割文字が見つかったら終了
                    If c.Value.IsWhiteSpace OrElse splitChars(c.Value.Raw0) Then
                        Exit While
                    End If
                    iter.MoveNext()
                Else
                    ' イテレータの終端に到達した場合も終了
                    Exit While
                End If
            End While

            ' 文字列を取得して、キーワードかどうかを判定
            Dim keyword = U8String.NewSlice(input, startIndex, iter.CurrentIndex - startIndex)
            If keyword = TrueKeyword Then ' ture
                Return New Word With {.str = keyword, .kind = WordType.TrueLiteral}
            ElseIf keyword = FalseKeyword Then ' false
                Return New Word With {.str = keyword, .kind = WordType.FalseLiteral}
            ElseIf keyword = NotKeyword Then ' not
                Return New Word With {.str = keyword, .kind = WordType.Not}
            ElseIf keyword = AndKeyword Then ' and
                Return New Word With {.str = keyword, .kind = WordType.AndOperator}
            ElseIf keyword = OrKeyword Then ' or
                Return New Word With {.str = keyword, .kind = WordType.OrOperator}
            ElseIf keyword = XorKeyword Then ' xor
                Return New Word With {.str = keyword, .kind = WordType.XorOperator}
            ElseIf keyword = InKeyword Then ' in
                Return New Word With {.str = keyword, .kind = WordType.InKeyword}
            ElseIf keyword = NullKeyword Then
                Return New Word With {.str = keyword, .kind = WordType.NullLiteral}
            Else
                ' それ以外の文字列は識別子とみなす
                Return New Word With {.str = keyword, .kind = WordType.Identifier}
            End If
        End Function

        ''' <summary>
        ''' 数値リテラルトークンを取得します。
        ''' </summary>
        ''' <param name="input">入力文字列。</param>
        ''' <param name="iter">文字列イテレータ。</param>
        ''' <returns>取得された数値トークン。</returns>
        ''' <remarks>
        ''' この関数は、イテレータの現在位置から数値を読み取り、新しいU8Stringを返します。
        ''' </remarks>
        Private Function GetNumberLiteralToken(input As U8String, iter As U8String.U8StringIterator) As U8String
            Dim startIndex = iter.CurrentIndex
            Dim hasDecimalPoint As Boolean = False
            Dim hasDigit As Boolean = False

            ' 数字または小数点が続く限り読み取ります
            While iter.HasNext()
                Dim pc = iter.Current
                If pc.HasValue Then
                    Dim c As U8Char = pc.Value
                    If c.Size = 1 AndAlso c.Raw0 >= &H30 AndAlso c.Raw0 <= &H39 Then ' 0-9
                        ' 数字のトークンを作成
                        hasDigit = True
                        iter.MoveNext()
                    ElseIf c.Size = 1 AndAlso c.Raw0 = &H2E Then
                        ' 小数点を許可
                        If Not hasDecimalPoint Then
                            hasDecimalPoint = True
                            iter.MoveNext()
                        Else
                            Exit While ' 2つ目の小数点は無視
                        End If
                    ElseIf c.Size = 1 AndAlso c.Raw0 = &H5F Then ' _
                        If hasDigit Then
                            ' アンダースコアは数字の一部として扱う
                            hasDigit = False
                            iter.MoveNext()
                        Else
                            ' アンダースコアが連続している場合はエラーとする（例: "12__34"）
                            Throw New AnalysisException("数値リテラルでアンダースコアが連続している")
                        End If
                    Else
                        Exit While ' 数字以外の文字は終了
                    End If
                End If
            End While

            ' トークンを返す（数字または小数点を含む）
            Return U8String.NewSlice(input, startIndex, iter.CurrentIndex - startIndex)
        End Function

        ''' <summary>
        ''' 文字列リテラルトークンを取得します。
        ''' </summary>
        ''' <param name="input">入力文字列。</param>
        ''' <param name="iter">文字列イテレータ。</param>
        ''' <param name="quote">引用符のバイト値（例: ' または "）。</param>
        ''' <returns>取得された文字列トークン。</returns>
        ''' <remarks>
        ''' この関数は、イテレータの現在位置から文字列リテラルを読み取り、新しいU8Stringを返します。
        ''' </remarks>
        Private Function GetStringLiteralToken(input As U8String, iter As U8String.U8StringIterator, quote As Byte) As U8String
            Dim startIndex = iter.CurrentIndex
            Dim hasQuote As Boolean = False

            iter.MoveNext()

            ' 文字列リテラルの終了を探す
            While iter.HasNext()
                Dim pc = iter.MoveNext()
                If pc.HasValue Then
                    Dim c As U8Char = pc.Value
                    If c.Size = 1 AndAlso c.Raw0 = &H5C AndAlso iter.Current.HasValue Then
                        ' エスケープ文字が見つかった場合は次の文字を無視
                        iter.MoveNext()
                    ElseIf c.Size = 1 AndAlso c.Raw0 = quote AndAlso
                        iter.Current?.Size = 1 AndAlso iter.Current?.Raw0 = quote Then
                        ' 連続する引用符が見つかった場合は、引用符を無視して次の文字へ
                        iter.MoveNext()
                    ElseIf c.Size = 1 AndAlso c.Raw0 = quote Then
                        ' 文字リテラルが見つかった場合は終了
                        hasQuote = True
                        Exit While
                    End If
                Else
                    ' イテレータの終端に到達した場合は終了
                    Exit While
                End If
            End While

            If Not hasQuote Then
                Throw New AnalysisException("文字列リテラルが閉じられていません。")
            End If
            Return U8String.NewSlice(input, startIndex, iter.CurrentIndex - startIndex)
        End Function

        ''' <summary>
        ''' 単語を表現する構造体です。
        ''' この構造体は、単語の文字列とその種類を保持します。
        ''' </summary>
        ''' <remarks>
        ''' 単語は、プログラム内で使用されるキーワードや識別子などを表します。
        ''' </remarks>
        Public Structure Word

            ''' <summary>単語の文字列。</summary>
            Public str As U8String

            ''' <summary>単語の種類。</summary>
            Public kind As WordType

        End Structure

    End Module

End Namespace
