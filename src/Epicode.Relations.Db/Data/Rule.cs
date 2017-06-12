using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPiCode.Relations.Db.Data
{
    public class Rule
    {
        public Rule()
        {
            
        }

        [Key]
        public Guid RuleId { get; set; }
        public string RuleName { get; set; }        
        public string RuleTextLeft { get; set; }
        public string RuleTextRight { get; set; }
        public int RelationHierarchyStartLeft { get; set; }
        public int RelationHierarchyStartRight { get; set; }
        public bool RuleVisibleLeft { get; set; }
        public bool RuleVisibleRight { get; set; }
        public string RuleDescriptionLeft { get; set; }
        public string RuleDescriptionRight { get; set; }
        public int SortOrderLeft { get; set; }
        public int SortOrderRight { get; set; }
        public bool UniquePrLanguage { get; set; }
        public string EditModeAccessLevel { get; set; }

        public List<RulePageType> PageTypeList { get; set; }
        
        //Is the current page on left or right side of rule?
        public Core.Rule.Direction RuleDirection;

        public Core.Rule TranslateToRule()
        {
            return new Core.Rule{ RuleName = RuleName };
        }
    }



}
