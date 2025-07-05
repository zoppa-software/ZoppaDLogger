Option Strict On
Option Explicit On

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
        Private Function ParseEmbeddedText(iter As ParserIterator(Of LexicalModule.Word)) As IExpression
            Dim exprs As New List(Of IExpression)()

            Return New ListExpress(exprs.ToArray())
        End Function

    End Module

End Namespace
