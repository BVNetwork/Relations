using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Shell.Services.Rest;

namespace EPiCode.Relations.Queries
{
    [RestStore("relations")]
    public class RelatedPagesRest : RestControllerBase
    {
        private readonly IContentLoader _contentLoader;

        public RelatedPagesRest(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }
        
        public RestResult Get(string page)
        {
            var matches = _contentLoader.GetChildren<PageData>(ContentReference.StartPage);
            return Rest(matches.Select(m => new { Name = m.Name, Id = m.ContentLink.ID }));
        }
    }
}