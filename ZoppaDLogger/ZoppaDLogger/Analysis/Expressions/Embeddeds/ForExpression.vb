﻿Option Strict On
Option Explicit On

Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' For式を表す構造体です。
    ''' この構造体は、変数名、コレクション式、および本体式を保持します。
    ''' </summary>
    ''' <remarks>
    ''' For式は、指定されたコレクションの各要素に対して本体式を実行するために使用されます。
    ''' </remarks>
    Structure ForExpression
        Implements IExpression

        ' 変数名
        Private ReadOnly _varName As U8String

        ' コレクション式
        Private ReadOnly _collectionExpr As IExpression

        ' 本体式
        Private ReadOnly _bodyExpr As IExpression

        ''' <summary>コンストラクタ。</summary>
        ''' <param name="varName">変数名。</param>
        ''' <param name="collectionExpr">コレクション式。</param>
        ''' <param name="bodyExpr">本体式。</param>
        ''' <exception cref="ArgumentNullException">引数がnullの場合にスローされます。</exception>
        Public Sub New(varName As U8String, collectionExpr As IExpression, bodyExpr As IExpression)
            If collectionExpr Is Nothing Then
                Throw New ArgumentNullException(NameOf(collectionExpr))
            End If
            If bodyExpr Is Nothing Then
                Throw New ArgumentNullException(NameOf(bodyExpr))
            End If
            Me._varName = varName
            Me._collectionExpr = collectionExpr
            Me._bodyExpr = bodyExpr
        End Sub

        ''' <summary>式の型を取得します。</summary>
        ''' <returns>式の型。</returns>
        Public ReadOnly Property Type As ExpressionType Implements IExpression.Type
            Get
                Return ExpressionType.ForExpression
            End Get
        End Property

        ''' <summary>式の値を取得します。</summary>
        ''' <param name="venv">変数環境。</param>
        ''' <returns>For式の結果の値。</returns>
        ''' <remarks>
        ''' このメソッドは、コレクション内の各要素に対して本体式を評価し、結果を連結して返します。
        ''' </remarks>
        Public Function GetValue(venv As AnalysisEnvironment) As IValue Implements IExpression.GetValue
            ' コレクションの値を取得
            Dim collection = _collectionExpr.GetValue(venv)
            If collection.Type <> ValueType.Array Then
                Throw New AnalysisException("コレクションは配列でなければなりません。")
            End If

            ' for式の結果
            Dim buffer = New List(Of Byte)()

            Using venv.GetScope()
                For Each item In collection.Array
                    ' 各アイテムに対して変数を登録
                    Select Case item.Type
                        Case ValueType.Str
                            venv.RegistStr(_varName, item.Str)
                        Case ValueType.Number
                            venv.RegistNumber(_varName, item.Number)
                        Case ValueType.Bool
                            venv.RegistBool(_varName, item.Bool)
                        Case ValueType.Obj
                            venv.RegistObject(_varName, item.Obj)
                        Case Else
                            Throw New AnalysisException("無効なコレクションの要素型です。")
                    End Select

                    ' ボディの式を評価
                    Dim bodyValue = _bodyExpr.GetValue(venv)

                    ' ボディの評価結果を文字列に変換してバッファに追加
                    buffer.AddRange(bodyValue.Str.GetByteEnumerable())
                Next

                Return New StringValue(U8String.NewStringChangeOwner(buffer.ToArray()))
            End Using
        End Function

    End Structure

End Namespace
