using EPiServer.Core;
using EPiServer.Shell.Services.Rest;
using System.Linq;

namespace EPiCode.Relations.EditorDescriptors
{
    [RestStore("relations")]
    public class RelatedPagesRest : RestControllerBase
    {
        public RestResult Get(string page)
        {
            var matches = EPiServer.DataFactory.Instance.GetChildren(ContentReference.StartPage);
            return Rest(matches.Select(m => new { Name = m.Name, Id = m.ContentLink.ID }));
        }
    }
}