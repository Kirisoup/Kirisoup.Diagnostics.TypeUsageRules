namespace Kirisoup.Diagnostics.TypeUsageRules;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
public sealed class NoNewAttribute : Attribute;

public sealed partial class Analyzer : DiagnosticAnalyzer
{
	private static readonly DiagnosticDescriptor noNew = new(
		id: "SoupTUR0002",
		title: "Prevent the use of default constructor of struct",
		messageFormat: "Struct {0} should not be initiallized with the default constructor",
		category: "Usage",
		defaultSeverity: DiagnosticSeverity.Warning,
		isEnabledByDefault: true);

    private static void AnalyzeNew(SyntaxNodeAnalysisContext context) => Analyze(
		context, noNew,
		static context => context.Node switch {
				ObjectCreationExpressionSyntax expr
					when expr.ArgumentList?.Arguments.Count is not > 0 => 
					context.SemanticModel.GetTypeInfo(expr, context.CancellationToken).Type,
				ImplicitObjectCreationExpressionSyntax { ArgumentList.Arguments.Count: not > 0 } expr
					when expr.ArgumentList?.Arguments.Count is not > 0 => 
					context.SemanticModel.GetTypeInfo(expr, context.CancellationToken).ConvertedType,
				_ => null
			},
		attr => attr.AttributeClass is {
				Name: nameof(NoNewAttribute),
				ContainingAssembly.Name: THIS
			});
}