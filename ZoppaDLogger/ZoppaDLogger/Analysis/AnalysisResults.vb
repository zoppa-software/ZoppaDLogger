Option Strict On
Option Explicit On

Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' ParseAnswerクラスは、解析結果を表すクラスです。
    ''' このクラスは、解析されたデータや結果を格納するために使用されます。
    ''' </summary>
    Public NotInheritable Class AnalysisResults
        Implements IComparable(Of AnalysisResults)

        ' 解析した文字列
        Private ReadOnly _input As U8String

        ''' <summary>比較を行います。</summary>
        ''' <param name="other">比較対象。</param>
        ''' <returns>比較結果。</returns>
        Public Function CompareTo(other As AnalysisResults) As Integer Implements IComparable(Of AnalysisResults).CompareTo
            Return Me._input.CompareTo(other._input)
        End Function

    End Class

End Namespace
