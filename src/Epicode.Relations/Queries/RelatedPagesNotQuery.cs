using EPiServer.ServiceLocation;
using System.Collections.Generic;
using System.Web;
using EPiServer.Cms.Shell.UI.Rest.ContentQuery;
using EPiServer.Shell.ContentQuery;
using EPiServer.Shell.Search;
using EPiServer.Core;
using EPiCode.Relations.Core;
using EPiServer;

namespace EPiCode.Relations.EditorDescriptors
{
    [ServiceConfiguration(typeof(IContentQuery))]
    public class RelatedPagesNotQuery : ContentQueryBase
    {
        private readonly IContentRepository _contentRepository;
        private readonly SearchProvidersManager _searchProvidersManager;
        private readonly LanguageSelectorFactory _languageSelectorFactory;

        public RelatedPagesNotQuery(
            IContentQueryHelper queryHelper,
            IContentRepository contentRepository,
            SearchProvidersManager searchProvidersManager,
            LanguageSelectorFactory languageSelectorFactory)
            : base(ServiceLocator.Current.GetInstance<IContentRepository>(),queryHelper)
        {
            _contentRepository = contentRepository;
            _searchProvidersManager = searchProvidersManager;
            _languageSelectorFactory = languageSelectorFactory;
        }

        /// <summary>        
        /// The key to trigger this query.        
        /// </summary>        
        public override string Name
        {
            get { return "NotRelationsQuery"; }
        }

        protected override IEnumerable<IContent> GetContent(ContentQueryParameters parameters)
        {
            bool isLeftRule = (HttpUtility.HtmlDecode(parameters.AllParameters["direction"]) == "left");
            var queryText = HttpUtility.HtmlDecode(parameters.AllParameters["queryText"]);
            var relationPageLeft = HttpUtility.HtmlDecode(parameters.AllParameters["relationPageLeft"]);
            var relationPageRight = HttpUtility.HtmlDecode(parameters.AllParameters["relationPageRight"]);
            var relationRule = HttpUtility.HtmlDecode(parameters.AllParameters["relationRule"]);
            PageReference contextPage = (relationPageLeft != null) ? new PageReference(relationPageLeft): null;

            IEnumerable<IContent> result = new PageDataCollection();

            if (contextPage != null)
                result = RuleEngine.Instance.SearchRelations(RuleEngine.Instance.GetRule(relationRule), contextPage.ID, queryText, isLeftRule);

            return result as PageDataCollection;
        }


    }


}