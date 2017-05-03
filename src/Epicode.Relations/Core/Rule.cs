using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data.Dynamic;
using EPiServer.Data;

namespace EPiCode.Relations.Core
{
    [EPiServerDataStore(AutomaticallyCreateStore = true, AutomaticallyRemapStore = true)]
    public class Rule : IComparable
    {
        public Identity Id { get; set; }
        public string RuleName { get; set; }
        public string PageTypeLeft { get; set; }
        public string RuleTextLeft { get; set; }
        public string PageTypeRight { get; set; }
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
        
        //Is the current page on left or right side of rule?
        public Direction RuleDirection;

        public enum Direction { 
            Left,
            Right,
            Both
        }

        protected void Initialize()
        {
            Id = Identity.NewIdentity(Guid.NewGuid());
            RuleVisibleLeft = true;
            RuleVisibleRight = true;
            EditModeAccessLevel = EPiServer.Security.AccessLevel.Read.ToString();
        }

        public Rule(string name, string pageTypeLeft, string pageTypeRight) {
            Initialize();
            RuleName = name;
            PageTypeLeft = pageTypeLeft;
            PageTypeRight = pageTypeRight;
        }
        
        public Rule() {
            Initialize();
        }

        public List<System.Xml.Linq.XElement> Serialize() {
            return new EPiServer.Data.Serialization.DynamicDataSerializer().Serialize(Id, typeof(Rule).GetStore().Name).ToList<System.Xml.Linq.XElement>();
        }

        public int CompareTo(object rule)
        {
            Rule r = (Rule)rule;
            return RuleName.CompareTo(r.RuleName);
        }

    }
}