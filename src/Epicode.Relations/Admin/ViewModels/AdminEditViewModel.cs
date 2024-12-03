using System.Collections.Generic;
using EPiCode.Relations.Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPiCode.Relations.Admin.ViewModels;

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