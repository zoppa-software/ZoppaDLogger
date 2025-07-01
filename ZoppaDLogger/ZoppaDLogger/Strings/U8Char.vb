Option Strict On
Option Explicit On

Namespace Strings

    ''' <summary>
    ''' UTF-8文字を表す構造体。
    ''' この構造体は、UTF-8エンコーディングの文字を表現します。
    ''' </summary>
    Public Structure U8Char

        ' 文字を表現するバイト値
        Private ReadOnly _raw0, _raw1, _raw2, _raw3 As Byte

        ''' <summary>
        ''' 文字を表すバイト数を取得します。
        ''' </summary>
        ''' <returns>文字を表すバイト数の配列。</returns>
        Public ReadOnly Property Size As Integer
            Get
                Return Utf8ByteSequenceLength(Me._raw0, 1)
            End Get
        End Property

        ''' <summary>
        ''' 文字を表すバイト値の整数値を取得します。
        ''' </summary>
        ''' <returns>文字を表す整数値。</returns>
        Public ReadOnly Property IntegerValue As UInteger
            Get
                ' UTF-8の値を取得
                If Me._raw1 = 0 Then
                    Return Me._raw0
                ElseIf Me._raw2 = 0 Then
                    Return Me._raw0 << 8 + Me._raw1
                ElseIf Me._raw3 = 0 Then
                    Return Me._raw0 << 16 + Me._raw1 << 8 + Me._raw2
                Else
                    Return Me._raw0 << 24 + Me._raw1 << 16 + Me._raw2 << 8 + Me._raw3
                End If
            End Get
        End Property

        ''' <summary>
        ''' 文字が空白文字（スペース、タブなど）であるかどうかを取得します。
        ''' </summary>
        ''' <returns>空白文字の場合はTrue、それ以外はFalse。</returns>
        ''' <remarks>
        ''' このプロパティは、UTF-8エンコーディングの文字が空白文字であるかどうかを判定します。
        ''' 空白文字は、スペースやタブなどの空白を含みます。
        ''' </remarks>
        Public ReadOnly Property IsWhiteSpace As Boolean
            Get
                If Me._raw1 = 0 Then
                    ' 1バイト文字の場合、スペースやタブ、改行ならば真
                    Return Me._raw0 = &H20 OrElse Me._raw0 = &H9 OrElse Me._raw0 = &HA OrElse Me._raw0 = &HD
                ElseIf (Me._raw0 = &HE2 AndAlso Me._raw1 = &H80 AndAlso Me._raw2 = &H80) OrElse (Me._raw0 = &HE3 AndAlso Me._raw1 = &H80 AndAlso Me._raw2 = &H80) Then
                    ' ' U+3000（全角スペース）やU+2000（スペース）
                    Return True
                Else
                    ' 上記以外
                    Return False ' 4バイト文字は空白文字ではない
                End If
            End Get
        End Property

        ''' <summary>
        ''' UTF-8文字を新しく作成します。
        ''' </summary>
        ''' <param name="raw0">最初のバイト。</param>
        ''' <param name="raw1">2番目のバイト（オプション）。</param>
        ''' <param name="raw2">3番目のバイト（オプション）。</param>
        ''' <param name="raw3">4番目のバイト（オプション）。</param>
        Private Sub New(raw0 As Byte, Optional raw1 As Byte = 0, Optional raw2 As Byte = 0, Optional raw3 As Byte = 0)
            Me._raw0 = raw0
            Me._raw1 = raw1
            Me._raw2 = raw2
            Me._raw3 = raw3
        End Sub

        ''' <summary>
        ''' Char（UTF-16）から新しいU8Charを作成します。
        ''' CharをUTF-8バイト配列に変換し、U8Charを生成します。
        ''' </summary>
        ''' <param name="source">対象のChar。</param>
        ''' <returns>新しいU8Char。</returns>
        Public Shared Function NewChar(source As Char) As U8Char
            Return NewChar(System.Text.Encoding.UTF8.GetBytes(New Char() {source}), 0)
        End Function

        ''' <summary>
        ''' バイト配列から新しいU8Charを作成します。
        ''' バイト配列の長さに応じて、1〜4バイトのUTF-8文字を生成します。
        ''' </summary>
        ''' <param name="bytes">対象のバイト配列。</param>
        ''' <returns>新しいU8Char。</returns>
        ''' <exception cref="ArgumentNullException">バイト配列がnullまたは空の場合にスローされます。</exception>
        ''' <exception cref="ArgumentException">無効なバイト配列の長さの場合にスローされます。</exception>
        Public Shared Function NewChar(bytes() As Byte) As U8Char
            Return NewChar(bytes, 0)
        End Function

        ''' <summary>
        ''' バイト配列の指定位置から新しいU8Charを作成します。
        ''' 指定位置から1〜4バイトのUTF-8文字を生成します。
        ''' </summary>
        ''' <param name="bytes">対象のバイト配列。</param>
        ''' <param name="start">開始位置。</param>
        ''' <returns>新しいU8Char。</returns>
        ''' <exception cref="ArgumentNullException">バイト配列がnullまたは長さが不足している場合にスローされます。</exception>
        ''' <exception cref="ArgumentException">無効なバイト配列の長さの場合にスローされます。</exception>
        Public Shared Function NewChar(bytes() As Byte, start As Integer) As U8Char
            If bytes Is Nothing OrElse bytes.Length - start <= 0 Then
                Throw New ArgumentNullException("bytes", "バイト長が不足しています")
            End If
            Select Case Utf8ByteSequenceLength(bytes(start))
                Case 1
                    Return New U8Char(bytes(start + 0))
                Case 2
                    Return New U8Char(bytes(start + 0), bytes(start + 1))
                Case 3
                    Return New U8Char(bytes(start + 0), bytes(start + 1), bytes(start + 2))
                Case 4
                    Return New U8Char(bytes(start + 0), bytes(start + 1), bytes(start + 2), bytes(start + 3))
                Case Else
                    Throw New ArgumentException("無効なバイト配列の長さです", "bytes")
            End Select
        End Function

        ''' <summary>
        ''' UTF8文字のバイト数を取得します。
        ''' 1バイト文字は1、2バイト文字は2、3バイト文字は3を返します。
        ''' 無効なバイト（0x80-0xBF）はスキップされます。
        ''' </summary>
        ''' <param name="firstByte">判定する文字。</param>
        ''' <param name="skipLen">無効な文字のバイト数。</param>
        ''' <returns>バイト数。</returns>
        Public Shared Function Utf8ByteSequenceLength(firstByte As Byte, Optional skipLen As Integer = 1) As Integer
            Select Case firstByte
                Case &H0 To &H7F
                    Return 1 ' 1バイト文字
                Case &H80 To &HBF
                    Return skipLen ' 無効なバイト
                Case &HC2 To &HDF
                    Return 2 ' 2バイト文字
                Case &HE0 To &HEF
                    Return 3 ' 3バイト文字
                Case &HF0 To &HF7
                    Return 4 ' 4バイト文字
                Case Else
                    Return skipLen ' 無効なバイト
            End Select
        End Function

        ''' <summary>
        ''' 2つのU8Charが等しいかどうかを比較します。
        ''' </summary>
        ''' <param name="obj">比較対象のオブジェクト。</param>
        ''' <returns>等しい場合はTrue、それ以外はFalse。</returns>
        Public Overrides Function Equals(obj As Object) As Boolean
            If TypeOf obj Is U8Char Then
                Dim other As U8Char = CType(obj, U8Char)
                Return Me._raw0 = other._raw0 AndAlso
                       Me._raw1 = other._raw1 AndAlso
                       Me._raw2 = other._raw2 AndAlso
                       Me._raw3 = other._raw3
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' = 演算子をオーバーロードします。
        ''' 2つのU8Charが等しいかどうかを比較します。
        ''' </summary>
        ''' <returns>等しければ真。</returns>
        Public Overloads Shared Operator =(ByVal c1 As U8Char, ByVal c2 As U8Char) As Boolean
            Return c1._raw0 = c2._raw0 AndAlso
                   c1._raw1 = c2._raw1 AndAlso
                   c1._raw2 = c2._raw2 AndAlso
                   c1._raw3 = c2._raw3
        End Operator

        ''' <summary>
        ''' <> 演算子をオーバーロードします。
        ''' 2つのU8Charが等しくないかどうかを比較します。
        ''' </summary>
        ''' <returns>等しくなければ真。</returns>
        Public Overloads Shared Operator <>(ByVal c1 As U8Char, ByVal c2 As U8Char) As Boolean
            Return c1._raw0 <> c2._raw0 OrElse
                   c1._raw1 <> c2._raw1 OrElse
                   c1._raw2 <> c2._raw2 OrElse
                   c1._raw3 <> c2._raw3
        End Operator

        ''' <summary>
        ''' 文字列表現を取得します。
        ''' このメソッドは、U8CharをUTF-8バイト配列として文字列に変換します。
        ''' </summary>
        ''' <returns>文字列。</returns>
        Public Overrides Function ToString() As String
            Dim bytes() As Byte = {Me._raw0, Me._raw1, Me._raw2, Me._raw3}
            Return System.Text.Encoding.UTF8.GetString(bytes, 0, Me.Size)
        End Function

    End Structure

End Namespace
