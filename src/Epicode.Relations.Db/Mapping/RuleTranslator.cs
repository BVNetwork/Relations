using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiCode.Relations.Db.Data;
using EPiServer.Data;
using Rule = EPiCode.Relations.Core.Rule;

namespace EPiCode.Relations.Db.Mapping
{
    internal class RuleTranslator : EntityMapperTranslator<Rule, Data.Rule>
    {
        readonly string[] _stringSeparators = { "%3b" };

        protected override Rule ServiceToBusiness(Data.Rule value)
        {
            var rule = new Rule
                {
                    RuleName = value.RuleName,
                    EditModeAccessLevel = value.EditModeAccessLevel,
                    PageTypeLeft = CreateRulePageTypeString(value.PageTypeList.Where(x => x.Direction == Rule.Direction.Left.ToString())),
                    PageTypeRight = CreateRulePageTypeString(value.PageTypeList.Where(x => x.Direction == Rule.Direction.Right.ToString())),
                    Id = Identity.NewIdentity(value.RuleId),
                    RelationHierarchyStartLeft = value.RelationHierarchyStartLeft,
                    RelationHierarchyStartRight = value.RelationHierarchyStartRight,
                    RuleDescriptionLeft = value.RuleDescriptionLeft,
                    RuleDescriptionRight = value.RuleDescriptionRight,
                    RuleDirection = value.RuleDirection,
                    RuleTextLeft = value.RuleTextLeft,
                    RuleTextRight = value.RuleTextRight,
                    RuleVisibleLeft = value.RuleVisibleLeft,
                    RuleVisibleRight = value.RuleVisibleRight,
                    SortOrderRight = value.SortOrderRight,
                    SortOrderLeft = value.SortOrderLeft,
                    UniquePrLanguage = value.UniquePrLanguage

                };
            return rule;
        }

        protected override Data.Rule BusinessToService(Rule value)
        {
            return new Data.Rule
                {
                    RuleName = value.RuleName,
                    EditModeAccessLevel = value.EditModeAccessLevel,
                    PageTypeList = CreateRulePageTypeList(value),                    
                    RuleId = value.Id.ExternalId,
                    RelationHierarchyStartLeft = value.RelationHierarchyStartLeft,
                    RelationHierarchyStartRight = value.RelationHierarchyStartRight,
                    RuleDescriptionLeft = value.RuleDescriptionLeft,
                    RuleDescriptionRight = value.RuleDescriptionRight,
                    RuleDirection = value.RuleDirection,
                    RuleTextLeft = value.RuleTextLeft,
                    RuleTextRight = value.RuleTextRight,
                    RuleVisibleLeft = value.RuleVisibleLeft,
                    RuleVisibleRight = value.RuleVisibleRight,
                    SortOrderRight = value.SortOrderRight,
                    SortOrderLeft = value.SortOrderLeft,
                    UniquePrLanguage = value.UniquePrLanguage
                };
        }

        private List<RulePageType> CreateRulePageTypeList(Rule rule)
        {
            var pageTypes = new List<RulePageType>();

            if (rule.PageTypeLeft == null)
                return pageTypes;

            var pageTypesLeftList = rule.PageTypeLeft.Split(_stringSeparators, StringSplitOptions.RemoveEmptyEntries);

            var pageTypesRightList = rule.PageTypeRight.Split(_stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            
            
            //Add Direction.Left
            pageTypes.AddRange(
                pageTypesLeftList.Select(pagetype => new RulePageType() { PageTypeName = pagetype, Direction = Rule.Direction.Left.ToString()})
                                 .ToList()
                );
            //Add Direction.Right
            pageTypes.AddRange(
                pageTypesRightList.Select(pagetype => new RulePageType() { PageTypeName = pagetype, Direction = Rule.Direction.Right.ToString() })
                                .ToList()
                );

            return pageTypes;
        }

        private string CreateRulePageTypeString(IEnumerable<RulePageType> allPageTypes)
        {
            if (allPageTypes == null)
                return String.Empty;

            var pageTypes = new StringBuilder();
            foreach (var rulePageType in allPageTypes.OrderBy(x => x.PageTypeName))
            {
                pageTypes.Append(rulePageType.PageTypeName + _stringSeparators[0]);
            }
            return pageTypes.ToString();
        }
    }
}
