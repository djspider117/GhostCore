using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace GhostCore.Data.Evaluation.SourceGen;

[Generator]
public class EvaluatableSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(FilterSymbols,
                transform: static (context, _) =>
                {
                    var classSyntax = (ClassDeclarationSyntax)context.Node;
                    var model = context.SemanticModel;
                    var classSymbol = model.GetDeclaredSymbol(classSyntax) as INamedTypeSymbol;
                    return classSymbol;
                })
            .Where(static info => info is not null)
            .Collect();

        context.RegisterSourceOutput(classDeclarations, CreateSources);
    }

    private bool FilterSymbols(SyntaxNode node, CancellationToken ctok)
    {
        if (node is not ClassDeclarationSyntax cdecl)
            return false;

        var isNodeData = cdecl.AttributeLists.SelectMany(x => x.Attributes)?.Any(x => x.Name?.ToString()?.Contains("Evaluatable") ?? false) ?? false;
        var shouldExclude = cdecl.AttributeLists.SelectMany(x => x.Attributes)?.Any(x => x.Name?.ToString()?.Contains("ExcludeFromSourceGeneration") ?? false) ?? false;
        var baselistCount = (cdecl.BaseList?.Types.Count ?? 0) == 0; // TEMP: for now, stuff that's not derived only

        return !shouldExclude && isNodeData && baselistCount;
    }

    private void CreateSources(SourceProductionContext ctx, ImmutableArray<INamedTypeSymbol?> classes)
    {
        foreach (var classSymbol in classes)
        {
            if (classSymbol is null)
                continue;

            var generatedCode = GenerateCode(classSymbol);
            if (generatedCode == null)
                continue;

            ctx.AddSource($"{classSymbol.Name}.g.cs", SourceText.From(generatedCode, Encoding.UTF8));
        }
    }

    private static string? GenerateCode(INamedTypeSymbol classSymbol)
    {
        if (classSymbol?.BaseType is null)
            return null;

        var className = classSymbol.Name;
        var baseType = classSymbol.BaseType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
        var inputs = classSymbol
            .GetMembers().OfType<IPropertySymbol>()
            .Where(field => field.GetAttributes().Any(a => a.AttributeClass?.Name.StartsWith(nameof(InputAttribute).Replace("Attribute", string.Empty)) ?? false))
            .ToList();

        var outputs = classSymbol
            .GetMembers().OfType<IPropertySymbol>()
            .Where(field => field.GetAttributes().Any(a => a.AttributeClass?.Name.Contains(nameof(OutputAttribute).Replace("Attribute", string.Empty)) ?? false))
            .ToList();


        if (classSymbol.IsGenericType)
        {
            var typeArguments = classSymbol.TypeParameters
                .Select(tp => tp.Name)
                .ToArray();
            className = $"{classSymbol.Name}<{string.Join(", ", typeArguments)}>";
        }

        var sb = new StringBuilder();
        sb.AppendLine("#pragma warning disable");
        sb.AppendLine("using GhostCore.Data.Evaluation;");
        sb.AppendLine();
        if (!classSymbol.ContainingNamespace.IsGlobalNamespace)
        {
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine($"{{");
        }
        sb.AppendLine($"    public partial class {className} : EvaluatableBase");
        sb.AppendLine($"    {{");
        sb.AppendLine($"        public {className}() : base(GetTypeDefinition()) {{ }}");
        sb.AppendLine($"        private static DynamicTypeDefinition GetTypeDefinition()");
        sb.AppendLine($"        {{");
        sb.AppendLine($"            return DynamicTypeDefinitionBuilder.Create(nameof({className}))"); // todo: do custom names

        foreach (var prop in inputs ?? [])
        {
            var typeDs = prop.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            sb.AppendLine($"                .WithInputProperty(nameof({prop.Name}), MetadataCache.GetOrRegisterTypeId(typeof({typeDs})))");
        }
        foreach (var prop in outputs ?? [])
        {
            sb.AppendLine($"                .WithOutputProperty(nameof({prop.Name}), MetadataCache.GetOrRegisterTypeId(typeof({prop.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})))");
        }
        sb.AppendLine($"                .Build();");
        sb.AppendLine($"        }}");
        sb.AppendLine();
        sb.AppendLine($"        protected override void UpdateInputs(EvaluationContext context)");
        sb.AppendLine($"        {{");
        sb.AppendLine($"            PortConnection? pc = null;");
        foreach (var prop in inputs ?? [])
        {
            sb.AppendLine($"            if (InputConnections.TryGetValue(GetInputPort(nameof({prop.Name})), out pc))");
            sb.AppendLine($"            {{");
            sb.AppendLine($"                var val = pc.SourceObject.GetOutputValue(pc.SourcePort, context);");
            sb.AppendLine($"                if (val != null)");
            sb.AppendLine($"                    {prop.Name} = ({prop.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})val;");
            sb.AppendLine($"            }}");
            sb.AppendLine();
        }
        sb.AppendLine($"        }}");
        sb.AppendLine();
        sb.AppendLine($"        public override object? GetOutputValueInternal(PortInfo port, EvaluationContext context)");
        sb.AppendLine($"        {{");
        foreach (var prop in outputs ?? [])
        {
            sb.AppendLine($"            if (port.Name == nameof({prop.Name}))");
            sb.AppendLine($"                return {prop.Name};");
            sb.AppendLine();
        }
        sb.AppendLine($"            return null;");
        sb.AppendLine($"        }}");
        sb.AppendLine();
        sb.AppendLine($"        public override object?[] GetOutputValuesInternal(EvaluationContext context)");
        sb.AppendLine($"        {{");
        sb.AppendLine($"            return [{string.Join(",", outputs.Select(x => x.Name))}];");
        sb.AppendLine($"        }}");
        sb.AppendLine($"    }}");

        if (!classSymbol.ContainingNamespace.IsGlobalNamespace)
        {
            sb.AppendLine($"}}");
        }
        sb.AppendLine("#pragma warning enable");

        var rv = sb.ToString();
        return rv;
    }
}