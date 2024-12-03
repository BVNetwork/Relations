using EPiCode.Relations.Core;

namespace EPiCode.Relations.Admin.ViewModels;

public class ValidationResultViewModel
{
    public Relation InvalidRelation { get; set; }
    public string Status { get; set; }
    public string ContentNameLeft { get; set; }
    public string ContentNameRight { get; set; }
}