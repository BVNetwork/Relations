using System;
using System.Collections.Generic;
using System.Linq;

namespace EPiCode.Relations.Core.RelationProviders.TestProvider
{
    public class TestRuleProvider : RuleProviderBase
    {
        private Rule rule
        {
            get
            {
                Random rnd = new Random();
                int number = rnd.Next(0, rules.Count-1);
                return rules[number];
            }
        }

        private List<Rule> rules
        {
            get
            {
                List<Rule> _rules = new List<Rule>();
                _rules.Add(new Rule { RuleTextLeft = "Works in", RuleTextRight="Had employees", RuleName="Employment" });
                _rules.Add(new Rule { RuleTextLeft = "Writes", RuleTextRight = "Is written by", RuleName = "Writer" });
                _rules.Add(new Rule { RuleTextLeft = "Is managing", RuleTextRight = "Is managed by", RuleName = "Management" });
                _rules.Add(new Rule { RuleTextLeft = "Has topic", RuleTextRight = "Is topic for", RuleName = "Topic" });
                _rules.Add(new Rule { RuleTextLeft = "Has location", RuleTextRight = "Is location for", RuleName = "Location" });
                return _rules;
            }
        }

        private List<Rule> randomRules
        {
            get {
                List<Rule> _rules = new List<Rule>();
                Random rnd = new Random();
                int number = rnd.Next(2, rules.Count - 1);
                for (int i = 0; i < number; i++) {
                    _rules.Add(rules[rnd.Next(0, rules.Count - 1)]);
                }
                return _rules;
            }
        }

        public override Rule AddNewRule(string name, string pageTypeLeft, string pageTypeRight, string ruleTextLeft, string ruleTextRight)
        {
            return new Rule();
        }

        public override void DeleteAll()
        {
        }

        public override void DeleteRule(string name)
        {
        }

        public override IEnumerable<Rule> GetAllRules()
        {
            return rules;
        }

        public override List<Rule> GetAllRulesList()
        {
            return rules;
        }

        public override Rule GetRule(EPiServer.Data.Identity id)
        {
            return (from r in rules where r.Id == id select r).First<Rule>();
        }

        public override Rule GetRule(string name)
        {
            if(!string.IsNullOrEmpty(name))
                return (from r in rules where r.RuleName == name select r).First<Rule>();
            return new Rule();
        }

        public override IEnumerable<Rule> GetRules(int pageID)
        {
            return rules;
        }

        public override IEnumerable<Rule> GetRules(string pageTypeName)
        {
            return rules;
        }

        public override IEnumerable<Rule> GetRulesLeft(int pageID)
        {
            return randomRules;
        }

        public override IEnumerable<Rule> GetRulesRight(int pageID)
        {
            return randomRules;
        }

        public override bool IsDescendent(int pageID, int startID)
        {
            return true;
        }

        public override bool IsLeftRule(int pageID, Rule rule)
        {
            return true;
        }

        public override bool IsLeftRule(string pageTypeName, Rule rule)
        {
            return true;
        }

        public override bool RuleExists(EPiServer.Data.Identity id)
        {
            return true;
        }

        public override bool RuleExists(string name)
        {
            return true;
        }

        public override EPiServer.Data.Identity Save(Rule rel)
        {
            return EPiServer.Data.Identity.NewIdentity();
        }

        public override EPiServer.Core.PageDataCollection SearchRelations(Rule rule, int pageID, string searchKeyWord, EPiServer.Core.PageReference hierarchyStart)
        {
            return TestRelationProvider.AllPages;
        }

        public override EPiServer.Core.PageDataCollection SearchRelations(Rule rule, int pageID, string searchKeyWord, EPiServer.Core.PageReference hierarchyStart, bool isLeftRule)
        {
            return TestRelationProvider.AllPages;
        }

        public override EPiServer.Core.PageDataCollection SearchRelations(Rule rule, int pageID, string searchKeyWord, bool isLeftRule)
        {
            return TestRelationProvider.AllPages;
        }
    }
}