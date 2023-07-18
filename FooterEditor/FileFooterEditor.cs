using FooterEditor;
public class FileFooterEditor : IFileFooterEditor
{
    //
    // Defult Values
    //
    private int footerMaxLenght = 1024;
    private string footerHeadTag = "[SafeticaProperties]";

    private IFileHandler reader;
    private string method;
    private string[] propValPair;
    
    private string Property 
    { 
        get
        {
            if (propValPair.Length>0)
            { 
                return propValPair[0];
            }
            else
            {   
                return null;
            }
        }
    }

    private string Value
    {
        get
        {
            if (propValPair.Length == 2)
            {
                return propValPair[1];
            }
            else
            {
                throw new ArgumentException($"Property {propValPair[0]} does not hav value");
            }
        }
    }
    //  
    //  CTOR  
    //  
    public FileFooterEditor(string fileName)
    {
            reader = new FileHandler(fileName);
    }  
            
    public void Execute(string method, string propValPair)
    {
        // Set Parameters
        this.method = method;
        this.propValPair = propValPair.Split("=");

        // Get Footer string
        string endBytes = reader.ReadFromEnd(footerMaxLenght);

        Tuple<int, string> splited = reader.SplitBySubstring(endBytes, footerHeadTag);
        int startIndex = splited.Item1;
        string footerString = splited.Item2;
        
        // Modify Footer
        IFooter footer = new Footer(footerString, footerHeadTag);
        CallMethod(footer);

        // Write Footer
        string modified = footer.ToString();
        if (modified.Length <= footerMaxLenght)
        {
            reader.WriteToEnd(modified, startIndex);
        }
        else
        {
            throw new ArgumentException("Footer is too long. No change will be made");
        }
        Console.WriteLine($"Method {method} finished properly.");
    }

    private void CallMethod(IFooter footer) 
    { 
        switch (method.ToLower())
        {
            case "add":
                footer.AddProperty(Property, Value);
                break;
            case "edit":
                footer.EditPropety(Property, Value);                
                break;
            case "remove":
                footer.RemoveProperty(Property);                
                break;
            default:
                throw new ArgumentException("Invalid operation.");
                break;
        }
    }
}

