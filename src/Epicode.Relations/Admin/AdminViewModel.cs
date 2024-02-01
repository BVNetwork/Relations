using System.Collections.Generic;
using EPiCode.Relations.Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPiCode.Relations;

public class AdminViewModel
{
    public IEnumerable<Rule> Rules { get; set; }
}

public class AdminEditViewModel
{
    public RuleViewModel RuleModel { get; set; }
    public IEnumerable<SelectListItem> PageTypes { get; set; }
    public List<SelectListItem> SortOrderLeft { get; set; }
    public List<SelectListItem> SortOrderRight { get; set; }
    public List<Rule> Rules { get; set; }
    public int NumberOfRelations { get; set; }
    public List<SelectListItem> AccessOptions { get; set; }
}

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

public class ValidatorViewModel
{
    public List<ValidationResultViewModel> InvalidRelations { get; set; }
    public string Status { get; set; }
}

public class ValidationResultViewModel
{
    public Relation InvalidRelation { get; set; }
    public string Status { get; set; }
    public string ContentNameLeft { get; set; }
    public string ContentNameRight { get; set; }
}