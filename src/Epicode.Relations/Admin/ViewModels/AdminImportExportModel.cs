namespace EPiCode.Relations.Admin.ViewModels;

public class AdminImportExportModel
{
    public string Export { get; set; }
    public string Status { get; set; }
    public bool IncludeRules { get; set; }
    public bool IncludeRelations { get; set; }
}