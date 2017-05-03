using System.Collections.Generic;
using System.Linq;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Core;
using EPiServer;

namespace EPiCode.Relations.EditorDescriptors
{
    public class RelatedPagesSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var contactPages = DataFactory.Instance.GetChildren(PageReference.StartPage);

            return new List<SelectItem>(contactPages.Select(c => new SelectItem {Value = c.PageLink, Text = c.Name}));
        }
    }
}