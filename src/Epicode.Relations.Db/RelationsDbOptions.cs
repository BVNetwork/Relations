namespace EPiCode.Relations.Db;

public class RelationsDbOptions
{
    public bool RemoveDeletedPageTypesFromRules { get; set; } = true;
    public bool AutomaticMigrationsEnabled { get; set; } = true;
}
