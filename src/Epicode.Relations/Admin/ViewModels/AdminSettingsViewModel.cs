using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPiCode.Relations.Admin.ViewModels;

public class AdminSettingsViewModel
{
    public string CurrentRelationProvider { get; set; }
    public string CurrentRuleProvider { get; set; }
    public IEnumerable<SelectListItem> RelationProviders { get; set; }
    public IEnumerable<SelectListItem> RuleProviders { get; set; }
}