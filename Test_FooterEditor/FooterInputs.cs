using System.Collections.Generic;

namespace Test_FooterEditor
{
    public static class FooterInputs
    {   
        public const string correctInput   = "[SafeticaProperties]\nproperty1=value1\nproperty2=value2";
        public static readonly Dictionary<string, string> correctProps = new Dictionary<string, string>
        {
                { "property1", "value1"},
                { "property2", "value2"}
        };

        public const string duplicatePropsInput = "[SafeticaProperties]\nproperty1=value1\nproperty2=value2\nproperty1=value1";
        public static readonly Dictionary<string, string> duplicateProps = new Dictionary<string, string>
        {
                { "property1", "value1"},
                { "property2", "value2"}
        };

        public const string corruptedPropsInput = "[SafeticaProperties]\nonlyProp\nproperty2=value2";
        public static readonly Dictionary<string, string> corruptedProps = new Dictionary<string, string>
        {
            { "property2", "value2"}
        };

        public const string corruptedOnlyValInput = "[SafeticaProperties]\n=onlyVal\nproperty2=value2";
        public static readonly Dictionary<string, string> corruptedOnlyValProps = new Dictionary<string, string>
        {
            { "property2", "value2"}
        };

        public const string emptyPropInput = "[SafeticaProperties]\nemptyProp=";
        public static readonly Dictionary<string, string> emptyProps = new Dictionary<string, string>
        {
            { "emptyProp", ""}
        };


        public const string wrongHeadInput = "[randomHead]\nproperty1=value1\nproperty2=value2";
        public const string noHeadInput    = "\nproperty1=value1\nproperty2=value2";
        

    }
}