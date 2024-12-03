using System.Collections.Generic;
using System.Linq;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Core;
using EPiServer;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.DependencyInjection;

namespace EPiCode.Relations.EditorDescriptors
{
    public class RelatedPagesSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            if (ContentReference.IsNullOrEmpty(ContentReference.StartPage))
            {
                return new List<ISelectItem>();
            }

            var contactPages = ServiceLocator.Current.GetService<IContentLoader>().GetChildren<PageData>(ContentReference.StartPage);

            return new List<SelectItem>(contactPages.Select(c => new SelectItem {Value = c.PageLink, Text = c.Name}));
        }
    }
}