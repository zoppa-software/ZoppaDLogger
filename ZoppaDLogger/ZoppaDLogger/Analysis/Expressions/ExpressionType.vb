Option Strict On
Option Explicit On

Namespace Analysis

    ''' <summary>
    ''' 式の型を定義する列挙型です。
    ''' 式は、リスト、変数、条件式、演算子など、さまざまな形式を持つことができます。
    ''' </summary>
    Public Enum ExpressionType

        ''' <summary>リスト式。</summary>
        ListExpress

        ''' <summary>非展開式式。</summary>
        NoneEmbeddedExpress

        ''' <summary>展開式式（エスケープ）</summary>
        UnfoldExpress

        ''' <summary>非展開式式（アンエスケープ）</summary>
        NoEscapeUnfoldExpress

        ''' <summary>変数式リスト。</summary>
        VariableListExpress

        ''' <summary>変数式。</summary>
        VariableExpress

        ''' <summary>IF式。</summary>
        IfExpress

        ''' <summary>IF条件条件式。</summary>
        IfConditionExpress

        ''' <summary>IF Else式。</summary>
        ElseExpress

        ''' <summary>三項演算式。</summary>
        TernaryExpress

        ''' <summary>括弧式。</summary>
        ParenExpress

        ''' <summary>二項演算式。</summary>
        BinaryExpress

        ''' <summary>単項演算式（前置き）</summary>
        UnaryExpress

        ''' <summary>数値式。</summary>
        NumberExpress

        ''' <summary>文字列式。</summary>
        StringExpress

        ''' <summary>非エスケープ文字列式。</summary>
        NoEscapeStringExpress

        ''' <summary>真偽値式。</summary>
        BooleanExpress

        ''' <summary>配列変数式リスト。</summary>
        ArrayVariableExpress

        ''' <summary>配列式。</summary>
        ArrayExpress

        ''' <summary>識別子式。</summary>
        IdentifierExpress

        ''' <summary>For式。</summary>
        ForExpress

        ''' <summary>Select式。</summary>
        SelectExpress

        ''' <summary>Selectトップ式。</summary>
        SelectTopExpress

        ''' <summary>Select Case式。</summary>
        SelectCaseExpress

        ''' <summary>Select Default式。</summary>
        SelectDefaultExpress

        ''' <summary>関数引数式。</summary>
        FunctionArgsExpress

        ''' <summary>関数式。</summary>
        FunctionExpress

    End Enum

End Namespace
