using System.Collections.Generic;
using EPiCode.Relations.Core;
using EPiServer;
using EPiServer.Core;
using EPiServer.Shell.Services.Rest;

namespace EPiCode.Relations.Queries
{
    [RestStore("overview")]
    public class OverviewRest : RestControllerBase
    {
        private readonly IContentLoader _contentLoader;

        public OverviewRest(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }
        
        public RestResult Get(int? id, ItemRange range)
        {
            if (id.HasValue)
            {
                string result = "";
                PageData currentPage = _contentLoader.Get<PageData>(new PageReference(id.Value));
                var pageId = currentPage.ContentLink.ID;
                var _rules = new List<Rule>();
                int cnt = 0;

                List<Rule> _rulesLeft = RuleEngine.Instance.GetRulesLeft(pageId) as List<Rule>;
                List<Rule> _rulesRight = RuleEngine.Instance.GetRulesRight(pageId) as List<Rule>;

                foreach (Rule rule in _rulesLeft)
                {
                    if (rule.RuleTextLeft != rule.RuleTextRight)
                    {
                        result += "<b>" + rule.RuleTextLeft + "</b><br/>";
                        List<int> relations = RelationEngine.Instance.GetRelationPagesForPage(pageId, rule);
                        foreach (int pgid in relations)
                        {
                            result += "- " + _contentLoader.Get<PageData>(new PageReference(pgid)).Name +
                                      "<br/>";
                            cnt++;
                        }

                    }
                }

                foreach (Rule rule in _rulesRight)
                {
                    result += "<b>" + rule.RuleTextRight + "</b><br/>";
                    List<int> relations = RelationEngine.Instance.GetRelationPagesForPage(pageId, rule);
                    foreach (int pgid in relations)
                    {
                        result += "- " + _contentLoader.Get<PageData>(new PageReference(pgid)).Name + "<br/>";
                        cnt++;
                    }
                }
                result = "Number of relations: " + cnt + "<br/><br/>" + result;

                return Rest(result);

            }
            return null;
        }
    }
}