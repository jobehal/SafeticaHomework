// 
// Assign arguments
//
string[] arguments = new string[0];
bool showWait = false;
if (args.Length != 3)
{
    showWait = true;

    Console.Write(GetHelp());
    Console.WriteLine("Enter the method, filename, and property=value:");

    string userInput = Console.ReadLine();
    arguments = userInput.Split(' ');

    if (arguments.Length != 3)
    {
        Console.WriteLine("Invalid number of arguments.");
        WaitForKeyPress("Press any key to terminate...");
        return;
    }    
}
else
{
    arguments = args;
}
//
//  Call the editor
//
if (arguments?.Length == 3)
{
    string method      = arguments[0];
    string fileName    = arguments[1];    
    string propValPair = arguments[2];

    string headTag = GetHeadTag();
    int maxLenght  = GetMaxLength();

    //print blank line
    Console.WriteLine("\n");
    try
    {
        IFileFooterEditor fileFooterEditor = new FileFooterEditor(fileName, headTag, maxLenght);
        fileFooterEditor.Execute(method, propValPair);        
        if (showWait)
        {
            WaitForKeyPress("Press any key to continue...");            
        }
    }
    catch (Exception ex)
    {   
        Console.WriteLine(ex.Message);
        WaitForKeyPress("Press any key to continue...");
    }
}
//
//
//
string GetHeadTag() => "[SafeticaProperties]";
int GetMaxLength() => 1024;

string GetHelp()
{
    string helpText = @"
|---------------------------------------- HELP -------------------------------------------------------------------
|
Usage: FooterEditor.exe <method> <filename> <property=value>

Description:
This program performs adds/modifies file footer on files using the specified method and property value pair.

Arguments:
- method         : The method to perform. Supported methods are 'add', 'edit', and 'remove'.
- filename       : The name of the file to operate on.
- property=value : The key sensitive property-value pair to use for the operation. 
                   Property and value are separated by '='

Examples:
FooterEditor.exe add    myFileName.txt property1=value1
FooterEditor.exe edit   myFileName.txt property1=value2
FooterEditor.exe remove myFileName.txt property;
|
|------------------------------------------------------------------------------------------------------------------


";   
    return helpText;
}

void WaitForKeyPress(string msg)
{
    Console.WriteLine(msg);
    Console.ReadKey();
}
