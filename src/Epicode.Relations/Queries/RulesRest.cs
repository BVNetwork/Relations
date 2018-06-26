using System.Collections.Generic;
using System.Linq;
using EPiCode.Relations.Core;
using EPiCode.Relations.Helpers;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Security;
using EPiServer.Shell.Services.Rest;

namespace EPiCode.Relations.EditorDescriptors
{
    [RestStore("rules")]
    public class RulesRest : RestControllerBase
    {
        public RestResult Get(int? id, ItemRange range)
        {
            if (id.HasValue)
            {
                
                if (new ContentReference(id.Value) != null && EPiServer.DataFactory.Instance.Get<IContent>(new ContentReference(id.Value)) as PageData != null)
                {
                    PageData currentPage = EPiServer.DataFactory.Instance.GetPage(new PageReference(id.Value));
                    var pageId = currentPage.ContentLink.ID;
                    var _rules = new List<RuleDescription>();

                    List<Rule> _rulesLeft = RuleEngine.Instance.GetRulesLeft(pageId) as List<Rule>;
                    List<Rule> _rulesRight = RuleEngine.Instance.GetRulesRight(pageId) as List<Rule>;

                    AccessControlList list = currentPage.ACL;

                    foreach (Rule leftRule in _rulesLeft)
                    {
                        var ruleSortOrder = GetRuleSortOrder(leftRule.SortOrderLeft);

                        RuleDescription rd = new RuleDescription
                        {
                            RuleName = leftRule.RuleTextLeft, 
                            RuleDesc = leftRule.RuleDescriptionLeft,
                            RuleSortOrder = ruleSortOrder,
                            RuleDirection = "left", 
                            RuleGuid = leftRule.Id.ExternalId.ToString(), 
                            RuleId = leftRule.RuleName
                        };
                        if (HasAccess(list, leftRule.EditModeAccessLevel) && 
                            leftRule.RuleVisibleLeft)
                            _rules.Add(rd);
                    }

                    foreach (Rule rightRule in _rulesRight)
                    {
                        var ruleSortOrder = GetRuleSortOrder(rightRule.SortOrderRight);

                        RuleDescription rd = new RuleDescription
                        {
                            RuleName = rightRule.RuleTextRight, 
                            RuleDesc = rightRule.RuleDescriptionRight,
                            RuleSortOrder = ruleSortOrder,
                            RuleDirection = "right", 
                            RuleGuid = rightRule.Id.ExternalId.ToString(), 
                            RuleId = rightRule.RuleName
                        };

                        if (HasAccess(list, rightRule.EditModeAccessLevel) && 
                            rightRule.RuleVisibleRight &&
                            rightRule.RuleTextLeft != rightRule.RuleTextRight)
                        {
                            _rules.Add(rd);
                        }
                    }

                    return Rest(_rules.Select(m => new
                    {
                        Guid = m.RuleGuid, 
                        Name = TryTranslate( m.RuleName), 
                        Id = m.RuleId, 
                        Description = TryTranslate(m.RuleDesc), 
                        Direction = m.RuleDirection,
                        SortOrder = m.RuleSortOrder
                    }));
                }
            }
            return null;
        }

        private static string GetRuleSortOrder(int sortOrder)
        {
            
            var showSortOrder = ConfigurationHelper.GetAppSettingsConfigValueBool("Relations.ShowSortOrder", false);

            if (showSortOrder && (FilterSortOrder)sortOrder != FilterSortOrder.None)
            {
                return ( (FilterSortOrder) sortOrder).ToString();                
            }

            return string.Empty;
        }

        private bool HasAccess(AccessControlList list, string editModeAccessLevel)
        {
            AccessLevel al;
            if (AccessLevel.TryParse(editModeAccessLevel, true, out al))
                return list.QueryDistinctAccess(al);
            return true;

        }

        private string TryTranslate(string ruleName)
        {
            string result = TranslationHelper.Translate("/relations/rules/" + ruleName);
            if (string.IsNullOrEmpty(result) || result.StartsWith("["))
                result = ruleName;
            return result;
        }

        private class RuleDescription
        {
            public string RuleName;
            public string RuleDirection;
            public string RuleDesc;
            public string RuleId;
            public string RuleGuid;
            public string RuleSortOrder;
        }
    }


}