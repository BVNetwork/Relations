using System.Collections.Generic;

namespace EPiCode.Relations.Admin.ViewModels;

public class ValidatorViewModel
{
    public List<ValidationResultViewModel> InvalidRelations { get; set; }
    public string Status { get; set; }
}