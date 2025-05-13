using System;
using System.Collections.Generic;
using System.Linq;
using EPiCode.Relations.Core;
using EPiCode.Relations.Helpers;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Security;
using EPiServer.Shell.Services.Rest;

namespace EPiCode.Relations.Queries
{
    [RestStore("rules")]
    public class RulesRest : RestControllerBase
    {
        private readonly IContentLoader _contentLoader;

        public RulesRest(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public RestResult Get(string id)
        {
            var rules = new List<RuleDescription>();

            if (string.IsNullOrWhiteSpace(id) == false)
            {
                var contentReference = new ContentReference(id).ToReferenceWithoutVersion();
                if (_contentLoader.TryGet(contentReference, out PageData currentPage))
                {
                    var pageId = contentReference.ID;

                    var rulesLeft = RuleEngine.Instance.GetRulesLeft(pageId).ToList();
                    var rulesRight = RuleEngine.Instance.GetRulesRight(pageId).ToList();

                    var list = currentPage.ACL;

                    foreach (var leftRule in rulesLeft)
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

                        if (HasAccess(list, leftRule.EditModeAccessLevel) && leftRule.RuleVisibleLeft)
                        {
                            rules.Add(rd);
                        }
                    }

                    foreach (Rule rightRule in rulesRight)
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
                            rules.Add(rd);
                        }
                    }
                }
            }

            return Rest(
                new
                {
                    Rules = rules.Select(m => new
                    {
                        Guid = m.RuleGuid,
                        Name = TryTranslate(m.RuleName),
                        Id = m.RuleId,
                        Description = TryTranslate(m.RuleDesc),
                        Direction = m.RuleDirection,
                        SortOrder = m.RuleSortOrder
                    }),
                    Id = id,
                    rules.Count
                });
        }

        private static string GetRuleSortOrder(int sortOrder)
        {            
            var showSortOrder = ConfigurationHelper.GetAppSettingsConfigValueBool("Relations:ShowSortOrder", false);

            if (showSortOrder && (FilterSortOrder)sortOrder != FilterSortOrder.None)
            {
                return TranslationHelper.Translate("/system/editutil/sortorder" + sortOrder, ((FilterSortOrder)sortOrder).ToString());                
            }

            return string.Empty;
        }

        private bool HasAccess(AccessControlList list, string editModeAccessLevel)
        {
            if (Enum.TryParse(editModeAccessLevel, true, out AccessLevel al))
            {
                return list.QueryDistinctAccess(al);
            }
                
            return true;
        }

        private string TryTranslate(string ruleName)
        {
            var result = TranslationHelper.Translate("/relations/rules/" + ruleName);
            if (string.IsNullOrEmpty(result) || result.StartsWith("["))
            {
                result = ruleName;
            }
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