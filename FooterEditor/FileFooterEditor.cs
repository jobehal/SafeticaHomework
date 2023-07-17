﻿using FooterEditor;
public class FileFooterEditor : IFileFooterEditor
{
    private int footerMaxLenght = 1024;
    private string footerHeadTag = "[SafeticaProperties]";

    private IFileHandler reader;
    private string actionName;
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
                Console.WriteLine("No property defined");
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
                Console.WriteLine("No value defined");
                return null;
            }
        }
    }

    public FileFooterEditor(string fileName)
    {
            reader = new FileHandler(fileName);

    }  
    
    private void CallAction(IFooter footer) 
    { 
        switch (actionName.ToLower())
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
        
    public void Execute(string actionName, string propValPair)
    {
        //set parameters
        this.actionName = actionName;
        this.propValPair = propValPair.Split("=");

        string endBytes = reader.ReadFromEnd(footerMaxLenght);

        Tuple<int, string> splited = reader.SplitBySubstring(endBytes, footerHeadTag);
        int startIndex = splited.Item1;
        string footerString = splited.Item2;
        
        IFooter footer = new Footer(footerString, footerHeadTag);
        CallAction(footer);

        string modified = footer.ToString();
        if (modified.Length <= footerMaxLenght)
        {
            reader.WriteToEnd(modified, startIndex);
        }
        else
        {
            throw new ArgumentException("Footer is too long. No change will be made");
        }
    }

}
