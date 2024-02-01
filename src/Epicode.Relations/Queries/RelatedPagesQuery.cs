using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiCode.Relations.Core;
using EPiCode.Relations.Diagnostics;
using EPiServer;
using EPiServer.Cms.Shell.UI.Rest.ContentQuery;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ContentQuery;
using Microsoft.AspNetCore.Http;

namespace EPiCode.Relations.Queries
{
    [ServiceConfiguration(typeof(IContentQuery))]
    public class RelatedPagesQuery : ContentQueryBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RelatedPagesQuery(IContentRepository contentRepository, IContentQueryHelper queryHelper, IHttpContextAccessor httpContextAccessor)
            : base(contentRepository, queryHelper)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        /// <summary>        
        /// The key to trigger this query.        
        /// </summary>        
        public override string Name => "RelationsQuery";
        
        protected override IEnumerable<IContent> GetContent(ContentQueryParameters parameters)
        {
            // TODO NETCORE: check if this is needed
            // if (_httpContextAccessor.HttpContext?.Session != null)
            // {
            //     _httpContextAccessor.HttpContext.Session["ValidationResult"] = "";
            // }

            bool isLeftRule = HttpUtility.HtmlDecode(parameters.AllParameters["direction"]) == "left";
            var relationPageLeft = HttpUtility.HtmlDecode(parameters.AllParameters["relationPageLeft"]);
            var relationRule = HttpUtility.HtmlDecode(parameters.AllParameters["relationRule"]);

            var contextPage = new PageReference(relationPageLeft);

            var result = new PageDataCollection();

            try
            {
                List<int> relations;

                if (RuleEngine.Instance.GetRule(relationRule).RuleTextLeft ==
                    RuleEngine.Instance.GetRule(relationRule).RuleTextRight)
                {
                    relations = RelationEngine.Instance.GetRelationPagesForPage(contextPage.ID, RuleEngine.Instance.GetRule(relationRule));
                }
                else
                {
                    relations = isLeftRule ? 
                        RelationEngine.Instance.GetRelationPagesForPage(contextPage.ID, RuleEngine.Instance.GetRule(relationRule), Rule.Direction.Left).Distinct().ToList() : 
                        RelationEngine.Instance.GetRelationPagesForPage(contextPage.ID, RuleEngine.Instance.GetRule(relationRule), Rule.Direction.Right).Distinct().ToList();
                }

                foreach (int pageid in relations)
                {
                    try
                    {
                        result.Add(PageEngine.GetPage(pageid));
                    }
                    catch
                    {
                        Logging.Warning($"Error fetching page {pageid} related to {contextPage.ID}");
                    }
                }
            }
            catch 
            {
                Logging.Warning($"Error fetching relations from page {contextPage.ID}");
            }

            return result;
        }
    }
}