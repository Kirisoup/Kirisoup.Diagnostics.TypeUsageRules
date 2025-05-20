namespace Kirisoup.Diagnostics.TypeUsageRules;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
public sealed class NoDefaultAttribute : Attribute;

public sealed partial class Analyzer : DiagnosticAnalyzer
{
	private static readonly DiagnosticDescriptor noDefault = new(
		id: "SoupTUR0001",
		title: "Prevent the use of default value of struct",
		messageFormat: "Struct {0} should not be initiallized with default value",
		category: "Usage",
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true);

    private static void AnalyzeDefault(SyntaxNodeAnalysisContext context) => Analyze(
		context, noDefault,
		static context => context.Node switch {
				DefaultExpressionSyntax expr => 
						context.SemanticModel.GetTypeInfo(expr.Type, context.CancellationToken).Type,
				LiteralExpressionSyntax expr => 
					context.SemanticModel.GetTypeInfo(expr, context.CancellationToken).ConvertedType,
				_ => null 
			},
		static attr => attr.AttributeClass is {
				Name: nameof(NoDefaultAttribute),
				ContainingAssembly.Name: THIS
			});
}