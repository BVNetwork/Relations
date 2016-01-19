using EPiServer.ServiceLocation;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Cms.Shell.UI.Rest.ContentQuery;
using EPiServer.Shell.ContentQuery;
using EPiServer.Shell.Search;
using EPiServer.Core;
using EPiCode.Relations.Core;
using EPiServer;
using EPiCode.Relations.Diagnostics;

namespace EPiCode.Relations.EditorDescriptors
{
    [ServiceConfiguration(typeof(IContentQuery))]
    public class RelatedPagesQuery : ContentQueryBase
    {
        private readonly IContentRepository _contentRepository;
        private readonly SearchProvidersManager _searchProvidersManager;
        private readonly LanguageSelectorFactory _languageSelectorFactory;

        public RelatedPagesQuery(
            IContentQueryHelper queryHelper,
            IContentRepository contentRepository,
            SearchProvidersManager searchProvidersManager,
            LanguageSelectorFactory languageSelectorFactory)
            : base(ServiceLocator.Current.GetInstance<IContentRepository>(), queryHelper)
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
            get { return "RelationsQuery"; }
        }

        protected override IEnumerable<IContent> GetContent(ContentQueryParameters parameters)
        {
            if(HttpContext.Current.Session != null)
            HttpContext.Current.Session["ValidationResult"] = "";
            bool isLeftRule = (HttpUtility.HtmlDecode(parameters.AllParameters["direction"]) == "left");
            var queryText = HttpUtility.HtmlDecode(parameters.AllParameters["queryText"]);
            var relationPageLeft = HttpUtility.HtmlDecode(parameters.AllParameters["relationPageLeft"]);
            string relationPageRightUrl = HttpUtility.HtmlDecode(parameters.AllParameters["relationPageRight"]);


            //string relationPageRight = "";
            /*
            if (!string.IsNullOrEmpty(relationPageRightUrl)) { 
                
                Match match = regex.Match(HttpUtility.HtmlDecode(parameters.AllParameters["relationPageRight"]));
                if (match.Success)
                {
                    relationPageRight = (match.Groups[1].Value);
                }
            }*/
            var relationRule = HttpUtility.HtmlDecode(parameters.AllParameters["relationRule"]);
            var action = HttpUtility.HtmlDecode(parameters.AllParameters["action"]);
            PageReference contextPage = (relationPageLeft != null) ? new PageReference(relationPageLeft) : null;





            PageDataCollection result = new PageDataCollection();

            if (contextPage != null && relationPageLeft != null)
                try
                {
                    List<int> relations = new List<int>();
                    if(RuleEngine.Instance.GetRule(relationRule).RuleTextLeft == RuleEngine.Instance.GetRule(relationRule).RuleTextRight)
                        relations = RelationEngine.Instance.GetRelationPagesForPage(contextPage.ID, RuleEngine.Instance.GetRule(relationRule));
                    else
                    relations = isLeftRule ? RelationEngine.Instance.GetRelationPagesForPage(contextPage.ID, RuleEngine.Instance.GetRule(relationRule), Rule.Direction.Left).Distinct<int>().ToList<int>() : RelationEngine.Instance.GetRelationPagesForPage(contextPage.ID, RuleEngine.Instance.GetRule(relationRule), Rule.Direction.Right).Distinct<int>().ToList<int>();
                    foreach (int pageid in relations)
                    {
                        try
                        {
                            result.Add(PageEngine.GetPage(pageid));
                        }
                        catch
                        {
                            Logging.Warning(string.Format("Error fetching page {0} related to {1}", pageid, contextPage.ID));
                        }
                    }
                }
                catch {
                    Logging.Warning(string.Format("Error fetching relations from page {0}", contextPage.ID));
                }



            return result;
        }

    }


}