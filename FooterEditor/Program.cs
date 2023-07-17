if (args.Length != 3)
{
    Console.WriteLine("Invalid arguments count");
}
else
{
    string actionName  = args[0];
    string fileName    = args[1];    
    string propValPair = args[2];

    try
    {
        IFileFooterEditor fileFooterEditor = new FileFooterEditor(fileName);
        fileFooterEditor.Execute(actionName, propValPair);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Err:");
        Console.WriteLine(ex.Message);
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}

