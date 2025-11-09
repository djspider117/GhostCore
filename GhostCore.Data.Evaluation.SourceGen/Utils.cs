using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;

namespace GhostCore.Data.Evaluation.SourceGen;

public class Utils
{
    public static bool CompareTypeNames(TypeSyntax typeSyntax, string targetTypeName)
    {
        if (typeSyntax is IdentifierNameSyntax identifierSyntax)
        {
            return identifierSyntax.Identifier.ValueText == targetTypeName;
        }
        else if (typeSyntax is QualifiedNameSyntax qualifiedSyntax)
        {
            return qualifiedSyntax.Right.Identifier.ValueText == targetTypeName;
        }

        return false;
    }

    public static object GetNamespace(ITypeSymbol symbol)
    {
        var symbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
        return symbol.ContainingNamespace.ToDisplayString(symbolDisplayFormat);
    }

    public static string FieldToPropertyName(string fieldName)
    {
        var sb = new StringBuilder();
        var isNextUpper = true;
        foreach (var c in fieldName)
        {
            if (c == '_')
            {
                isNextUpper = true;
            }
            else
            {
                sb.Append(isNextUpper ? char.ToUpper(c) : c);
                isNextUpper = false;
            }
        }
        return sb.ToString();
    }

    public static bool IsReferenceType(TypeSyntax typeSyntax)
    {
        if (typeSyntax is PredefinedTypeSyntax predefinedType)
        {
            return predefinedType.Keyword.IsKind(SyntaxKind.StringKeyword); // string is a reference type
        }

        if (typeSyntax is ArrayTypeSyntax || typeSyntax is PointerTypeSyntax)
        {
            return true; // Arrays and pointers are reference types
        }

        return false;
    }

    public static string GetArgumentValue(ExpressionSyntax expression)
    {
        return expression switch
        {
            LiteralExpressionSyntax literal => literal.Token.ValueText,
            IdentifierNameSyntax identifier => identifier.Identifier.Text,
            // Add more cases to handle different argument types as needed
            _ => "Unknown",
        };
    }
}
