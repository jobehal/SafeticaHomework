using System.Collections.Generic;

namespace Test_FooterEditor
{
    internal static class FooterInputs
    {   
        internal const string headTag = "[SafeticaProperties]";
        
        internal const string correctInput   = $"{headTag}\nproperty1=value1\nproperty2=value2";
        internal static readonly Dictionary<string, string> correctProps = new Dictionary<string, string>
        {
                { "property1", "value1"},
                { "property2", "value2"}
        };

        internal const string duplicatePropsInput = $"{headTag}\nproperty1=value1\nproperty2=value2\nproperty1=value1";
        internal static readonly Dictionary<string, string> duplicateProps = new Dictionary<string, string>
        {
                { "property1", "value1"},
                { "property2", "value2"}
        };

        internal const string corruptedPropsInput = $"{headTag}\nonlyProp\nproperty2=value2";
        internal static readonly Dictionary<string, string> corruptedProps = new Dictionary<string, string>
        {
            { "property2", "value2"}
        };

        internal const string corruptedOnlyValInput = $"{headTag}\n=onlyVal\nproperty2=value2";
        internal static readonly Dictionary<string, string> corruptedOnlyValProps = new Dictionary<string, string>
        {
            { "property2", "value2"}
        };

        internal const string emptyPropInput = $"{headTag}\nemptyProp=";
        internal static readonly Dictionary<string, string> emptyProps = new Dictionary<string, string>
        {
            { "emptyProp", ""}
        };
        internal const string wrongHeadInput = "[randomHead]\nproperty1=value1\nproperty2=value2";
        internal const string noHeadInput    = "\nproperty1=value1\nproperty2=value2";
        

    }
}