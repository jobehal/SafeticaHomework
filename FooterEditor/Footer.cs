using System.Text.RegularExpressions;

namespace FooterEditor
{
    public class Footer : IFooter
    {
        private Dictionary<string, string> _properties = new Dictionary<string, string>();
        public Dictionary<string, string> Properties { get => _properties;}
        
        private string _lineSep;
        private string _propValSep;        
        private string _defaultName;

        //  
        //  CTOR  
        //  
        public Footer(string footerStr, string footerTag, string lineSep = "\n", string propValSep = "=")
        {
            _defaultName = footerTag;
            _lineSep = lineSep;
            _propValSep = propValSep;
            ParseInput(footerStr);
        }

        public void AddProperty(string prop, string value)
        {
            if (string.IsNullOrWhiteSpace(prop))
            {
                throw new ArgumentException("Prop Name is not defined.");                
            }
            
            if (!_properties.ContainsKey(prop))
            {
                _properties.Add(prop.Trim(), value);
            }
            else
            {
                throw new ArgumentException($"{prop} already exists. Use edit or chose different name.");
            }
        }

        public void EditPropety(string prop, string value)
        {   
            string trimmedProp = prop.Trim();
            if (_properties.ContainsKey(trimmedProp))
            {
                _properties[trimmedProp] = value;
            }
            else
            {
                AddProperty(trimmedProp, value);
            }
        }

        public void RemoveProperty(string prop)
        {
            string trimmedProp = prop.Trim(); 
            if (_properties.ContainsKey(trimmedProp))
            {
                _properties.Remove(trimmedProp);
            }
            else
            {
                Console.WriteLine($"Property {trimmedProp} not found - no propety was deleted.");
            }
        }

        public override string ToString()
        {
            string nameStr = $"{_defaultName}{_lineSep}";
            
            IEnumerable<string> kvPairsStr = _properties.Select(kvPair => $"{kvPair.Key}{_propValSep}{kvPair.Value}");                        
            string propsString = string.Join(_lineSep,kvPairsStr);
            if (string.IsNullOrEmpty(propsString))
            {
                return "";
            }
            else
            {
                return $"{nameStr}{propsString}";
            }

        }
        
        private void ParseInput(string footerStr)
        {
            IEnumerable<string> lines = footerStr?.Split(_lineSep).Select(l => l.Trim()) ?? new string[0];
            int rowIndex = 0;
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (rowIndex == 0)
                {
                    // check footer head
                    CheckFooterName(line);
                }
                else
                {
                    // parse keyvalue pairs
                    string [] propValPair = line.Split(_propValSep);
                    if (propValPair.Length == 2)
                    {
                        if (!_properties.ContainsKey(propValPair[0]))
                        {
                            AddProperty(propValPair[0], propValPair[1]);
                        }
                    }
                    else
                    {
                        throw new ArgumentException($"Unable to extract property value pair from '{line}'.");
                    }
                }
                rowIndex++;
            }
        }
        
        private void CheckFooterName(string text)
        {
            if (!text.Contains(_defaultName))
            {
                throw new ArgumentException("Input does not contains required head.");
            }
        }
    }
}
