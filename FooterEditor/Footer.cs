using System.Text.RegularExpressions;

namespace FooterEditor
{
    public class Footer : IFooter
    {
        private Dictionary<string, string> _properties = new Dictionary<string, string>();
        private string _lineSep = "\n";
        private string _propValSep = "=";
        private Tuple<string, string> _nameSep = Tuple.Create("[","]");
        
        private string _defaultName = "SafeticaProperties";
        public string? Name { get; private set;}
        public Footer(string footerStr)
        {
            ParseInput(footerStr);
        }

        private void ParseInput(string footerStr)
        {
            IEnumerable<string> lines = footerStr?.Split(_lineSep).Select(l => l.Trim()) ?? new string[0];
            int rowIndex = 0;
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (rowIndex == 0)
                    {
                        ExtractName(line);
                    }
                    else
                    {
                        string [] propValPair = line.Split(_propValSep);
                        if (propValPair.Length == 2)
                        {
                            AddProperty(propValPair[0], propValPair[1]);
                        }
                        else
                        {
                            Console.WriteLine($"Unable to extract property value pair from '{line}'.");
                        }
                    }
                    rowIndex++;
                }
            }
        }

        private void ExtractName(string text)
        {
            string pattern = $@"\{_nameSep.Item1}(.*?)\{_nameSep.Item2}";
            Match match = Regex.Match(text, pattern);

            if (match.Success)
            {
                Name = match.Groups[1].Value;
            }
            else
            {
                Name = _defaultName;
                Console.WriteLine($"Unable to extract footer name from '{text}' default footer name is set.");
            }
        }

        public void AddProperty(string prop, string value)
        {
            if (!_properties.ContainsKey(prop))
            {
                _properties.Add(prop, value);
            }
            else
            {
                Console.WriteLine($"{prop} already exists. Use edit or chose different name.");
            }
        }

        public void EditPropety(string prop, string value)
        {
            if (_properties.ContainsKey(prop))
            {
                _properties[prop] = value;
            }
            else
            {
                AddProperty(prop, value);
            }
        }

        public void RemoveProperty(string prop)
        {
            _properties.Remove(prop);
        }

        public override string ToString()
        {
            
            IEnumerable<string> kvPairsStr = _properties.Select(kvPair => $"{kvPair.Key}{_propValSep}{kvPair.Value}");                        
            string propsString = string.Join(_lineSep,kvPairsStr);
            if (string.IsNullOrEmpty(propsString))
            {
                return null;
            }
            else
            {
                string nameStr = $"{_nameSep.Item1}{Name}{_nameSep.Item2}{_lineSep}";
                return $"{nameStr}{propsString}";
            }

        }
    }
}
