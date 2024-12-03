using System.Collections.Generic;
using EPiCode.Relations.Core;

namespace EPiCode.Relations.Admin.ViewModels;

public class AdminViewModel
{
    public IEnumerable<Rule> Rules { get; set; }
}