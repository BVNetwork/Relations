namespace EPiCode.Relations.Admin.ViewModels;

public class RuleViewModel
{
    public string RuleId { get; set; }
    public string RuleName { get; set; }
    public string PageTypeLeft { get; set; }
    public string RuleTextLeft { get; set; }
    public string PageTypeRight { get; set; }
    public string RuleTextRight { get; set; }
    public string RelationHierarchyStartLeft { get; set; }
    public string RelationHierarchyStartRight { get; set; }
    public bool VisibleLeft { get; set; }
    public bool VisibleRight { get; set; }
    public string RuleDescriptionLeft { get; set; }
    public string RuleDescriptionRight { get; set; }
    public string SortOrderLeft { get; set; }
    public string SortOrderRight { get; set; }
    public string UniquePrLanguage { get; set; }
    public string EditModeAccessLevel { get; set; } 
}