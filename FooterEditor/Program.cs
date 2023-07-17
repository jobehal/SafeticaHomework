// See https://aka.ms/new-console-template for more information

Console.WriteLine("xxx");


string actionName  = null;
actionName  = "add";
actionName  = "edit";
//actionName  = "remove";
string fileName    = @"E:\Projekty2\SafeticaHomework\Test_FooterEditor\LongTestFile.txt";
string val = new string('a', 2000);
string propValPair = $"property3={val}";

try
{
    IFileFooterEditor fileFooterEditor = new FileFooterEditor(fileName, actionName, propValPair);
    fileFooterEditor.Execute();
}
catch (Exception ex)
{
    Console.WriteLine("Err:");
    Console.WriteLine(ex.Message);
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

