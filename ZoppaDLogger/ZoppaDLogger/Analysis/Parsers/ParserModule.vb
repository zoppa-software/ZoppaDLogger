﻿Option Strict On
Option Explicit On

Imports System.Transactions
Imports ZoppaDLogger.Analysis.LexicalModule
Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' パーサーモジュールを定義するモジュールです。
    ''' このモジュールは、式の解析や変数の管理など、解析に関連する機能を提供します。
    ''' </summary>
    Public Module ParserModule

        ''' <summary>文字列を解析し、結果を取得します。</summary>
        ''' <param name="input">解析する文字列。</param>
        ''' <returns>解析結果。</returns>
        ''' <remarks>
        ''' このメソッドは、指定された文字列を解析し、結果を返します。
        ''' </remarks>
        Public Function Executes(input As String) As AnalysisResults
            Return Executes(U8String.NewString(input))
        End Function

        ''' <summary>文字列を解析し、結果を取得します。</summary>
        ''' <param name="input">解析する文字列。</param>
        ''' <returns>解析結果。</returns>
        ''' <remarks>
        ''' このメソッドは、指定された文字列を解析し、結果を返します。
        ''' </remarks>
        Public Function Executes(input As U8String) As AnalysisResults
            ' 入力文字列を単語に分割します
            Dim words = input.SplitWords()

            ' 単語のイテレーターを作成します
            Dim iter As New ParserIterator(Of LexicalModule.Word)(words)

            ' 式を解析します
            Dim exper = ParseTernaryOperator(iter)
            If iter.HasNext() Then
                Throw New AnalysisException("式の解析に失敗しました。")
            End If
            Return New AnalysisResults(input, exper)
        End Function

        ''' <summary>埋め込みテキストを解析します。</summary>
        ''' <param name="input">解析する文字列。</param>
        ''' <returns>解析結果。</returns>
        ''' <remarks>
        ''' このメソッドは、埋め込みテキストを解析し、結果を返します。
        ''' </remarks>
        Public Function Translate(input As U8String) As AnalysisResults
            ' 入力文字列を埋込ブロックに分割します
            Dim blocks = input.SplitEmbeddedText()

            ' 単語のイテレーターを作成します
            Dim iter As New ParserIterator(Of LexicalEmbeddedModule.EmbeddedBlock)(blocks)

            ' 式を解析します
            Dim exper = ParseEmbeddedText(iter)
            If iter.HasNext() Then
                Throw New AnalysisException("埋込式の解析に失敗しました。")
            End If
            Return New AnalysisResults(input, exper)
        End Function

    End Module

End Namespace
