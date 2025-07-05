Option Strict On
Option Explicit On

Imports System.Reflection
Imports ZoppaDLogger.Strings

Namespace Analysis

    ''' <summary>
    ''' フィールドアクセスを表す式の構造体です。
    ''' この構造体は、オブジェクトのプロパティにアクセスするために使用されます。
    ''' </summary>
    ''' <remarks>
    ''' この式は、オブジェクトのプロパティにアクセスし、その値を取得します。
    ''' </remarks>
    Structure FieldAccessExpress
        Implements IExpression

        ' 変数
        Private ReadOnly _target As IExpression

        ' プロパティ名リスト
        Private ReadOnly _propertyName As U8String

        ''' <summary>フィールドアクセス式のコンストラクタ。</summary>
        ''' <param name="target">アクセスするインスタンス。</param>
        ''' <param name="propertyNames">プロパティ名リスト。</param>
        Public Sub New(target As IExpression, propertyName As U8String)
            _target = target
            _propertyName = propertyName
        End Sub

        ''' <summary>式の型を取得します。</summary>
        ''' <returns>式の型。</returns>
        Public ReadOnly Property Type As ExpressionType Implements IExpression.Type
            Get
                Return ExpressionType.FieldAccessExpress
            End Get
        End Property

        ''' <summary>式の値を取得します。</summary>
        ''' <param name="venv">変数環境。</param>
        ''' <returns>配列アクセスの結果としての値。</returns>
        ''' <remarks>
        ''' このメソッドは、変数名とインデックスを使用して配列の要素にアクセスし、その値を返します。
        ''' </remarks>
        Public Function GetValue(venv As AnalysisEnvironment) As IValue Implements IExpression.GetValue
            ' 変数を取得
            Dim obj = _target.GetValue(venv).Obj
            ' プロパティ名を使用してオブジェクトのプロパティにアクセス
            Dim propInfo As PropertyInfo = obj.GetType().GetProperty(_propertyName.ToString())
            If propInfo Is Nothing Then
                Throw New InvalidOperationException($"プロパティ '{_propertyName}' が見つかりません。")
            End If

            ' プロパティの値を取得
            obj = propInfo.GetValue(obj, Nothing)

            ' 最終的な値をIValueに変換して返す
            If TypeOf obj Is IValue Then
                Return DirectCast(obj, IValue)
            End If
            Return ConvertToValue(obj)
        End Function

    End Structure

End Namespace
