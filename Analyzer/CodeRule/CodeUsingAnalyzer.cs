
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeRule;

public static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor CoreCannotReferenceModel = new DiagnosticDescriptor(
        id: "UPUP_RULE001",
        title: "框架namespcace 调用限制",
        messageFormat: "'{0}'",
        category: "Usage",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);
}

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public class CodeUsingAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => ImmutableArray.Create(DiagnosticDescriptors.CoreCannotReferenceModel);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);
    }
    
    private static void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
    {
        var syntaxRoot = context.Tree.GetRoot(context.CancellationToken);

        // 检查是否存在namespace Core声明
        var hasCoreNamespace = syntaxRoot.DescendantNodes()
            .OfType<NamespaceDeclarationSyntax>()
            .Any(n => n.Name.ToString().EndsWith(".Core") || n.Name.ToString() == "Core");

        // 检查是否存在namespace Model声明
        var hasModelNamespace = syntaxRoot.DescendantNodes()
            .OfType<NamespaceDeclarationSyntax>()
            .Any(n => n.Name.ToString().EndsWith(".Model") || n.Name.ToString() == "Model");
        
        // 检查是否存在using Model声明
        var hasUsingModel = syntaxRoot.DescendantNodes()
            .OfType<UsingDirectiveSyntax>()
            .Any(u => u.Name.ToString().EndsWith(".Model") || u.Name.ToString() == "Model");
        
        // 是否存在using ViewCtrl声明
        var hasUsingViewCtrl = syntaxRoot.DescendantNodes()
            .OfType<UsingDirectiveSyntax>()
            .Any(u => u.Name.ToString().EndsWith(".ViewCtrl") || u.Name.ToString() == "ViewCtrl");
        
        // core namespace 不能引用model namespace
        if (hasCoreNamespace && hasUsingModel)
        {
            // 如果同时存在，则报告诊断
            var diagnostic = Diagnostic.Create(DiagnosticDescriptors.CoreCannotReferenceModel, context.Tree.GetLocation(syntaxRoot.FullSpan), "Core namespace should not use Model namespace.");
            context.ReportDiagnostic(diagnostic);
        }
        // core namespace 不能引用viewctrl namespace
        if (hasCoreNamespace && hasUsingViewCtrl)
        {
            // 如果同时存在，则报告诊断
            var diagnostic = Diagnostic.Create(DiagnosticDescriptors.CoreCannotReferenceModel, context.Tree.GetLocation(syntaxRoot.FullSpan), "Core namespace should not use ViewCtrl namespace.");
            context.ReportDiagnostic(diagnostic);
        }
        // model namespace 不能引用viewctrl namespace
        if (hasModelNamespace && hasUsingViewCtrl)
        {
            // 如果同时存在，则报告诊断
            var diagnostic = Diagnostic.Create(DiagnosticDescriptors.CoreCannotReferenceModel, context.Tree.GetLocation(syntaxRoot.FullSpan), "Model namespace should not use ViewCtrl namespace.");
            context.ReportDiagnostic(diagnostic);
        }
    }
}
