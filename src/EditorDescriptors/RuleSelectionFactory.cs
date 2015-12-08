using System.Collections.Generic;
using System.Linq;
using EPiServer.Shell.ObjectEditing;
using EPiCode.Relations.Core;

namespace EPiCode.Relations.EditorDescriptors
{
    public class RuleSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var contactPages = RuleEngine.Instance.GetAllRules();

            return new List<SelectItem>(contactPages.Select(c => new SelectItem {Value = c.RuleName, Text = c.RuleName}));
        }
    }
}