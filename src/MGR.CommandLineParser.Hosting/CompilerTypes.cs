namespace System.Runtime.CompilerServices;

internal class RequiredMemberAttribute : Attribute { }
internal class CompilerFeatureRequiredAttribute : Attribute
{
    public CompilerFeatureRequiredAttribute(string name) { }
}