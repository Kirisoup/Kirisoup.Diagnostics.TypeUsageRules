global using System.Collections.Immutable;
global using Microsoft.CodeAnalysis;
global using Microsoft.CodeAnalysis.Diagnostics;
global using Microsoft.CodeAnalysis.CSharp;
global using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kirisoup.Diagnostics.TypeUsageRules;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed partial class Analyzer : DiagnosticAnalyzer
{
	private const string THIS = "Kirisoup.Diagnostics.PreventDefault";

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [
		noDefault,
		noNew];

	public override void Initialize(AnalysisContext context) {
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution();
		context.RegisterSyntaxNodeAction(AnalyzeDefault,
			SyntaxKind.DefaultExpression,
			SyntaxKind.DefaultLiteralExpression);
		context.RegisterSyntaxNodeAction(AnalyzeNew,
			SyntaxKind.ObjectCreationExpression,
			SyntaxKind.ImplicitObjectCreationExpression);
	}

	private static void Analyze(
		SyntaxNodeAnalysisContext context,
		DiagnosticDescriptor descriptor,
		Func<SyntaxNodeAnalysisContext, ITypeSymbol?> getType,
		Func<AttributeData, bool> attrpred)
	{
		switch (getType(context)) {
		case not { IsValueType: true }: return;
		case var type when !type.GetAttributes().Any(attr => attrpred(attr)): return;
		case var type: 
			context.ReportDiagnostic(Diagnostic.Create(
				descriptor,
				context.Node.GetLocation(),
				type.Name));
			return;
		}
	}
}