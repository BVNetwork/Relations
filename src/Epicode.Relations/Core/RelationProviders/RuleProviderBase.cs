using System.Configuration.Provider;

namespace EPiCode.Relations.Core.RelationProviders
{
    public abstract class RuleProviderBase : ProviderBase
    {
        protected string _name;

        public override string Name
        {
            get { return ProviderName; }
        }

        public string ProviderName { get; set; }

        protected string _description;

        public override string Description
        {
            get { return _description; } 
        }

        public abstract Rule AddNewRule(string name, string pageTypeLeft, string pageTypeRight, string ruleTextLeft, string ruleTextRight);
        public abstract void DeleteAll();
        public abstract void DeleteRule(string name);
        public abstract System.Collections.Generic.IEnumerable<Rule> GetAllRules();
        public abstract System.Collections.Generic.List<Rule> GetAllRulesList();
        public abstract Rule GetRule(EPiServer.Data.Identity id);
        public abstract Rule GetRule(string name);
        public abstract System.Collections.Generic.IEnumerable<Rule> GetRules(int pageID);
        public abstract System.Collections.Generic.IEnumerable<Rule> GetRules(string pageTypeName);
        public abstract System.Collections.Generic.IEnumerable<Rule> GetRulesLeft(int pageID);
        public abstract System.Collections.Generic.IEnumerable<Rule> GetRulesRight(int pageID);
        public abstract bool IsDescendent(int pageID, int startID);
        public abstract bool IsLeftRule(int pageID, Rule rule);
        public abstract bool IsLeftRule(string pageTypeName, Rule rule);
        public abstract bool RuleExists(EPiServer.Data.Identity id);
        public abstract bool RuleExists(string name);
        public abstract EPiServer.Data.Identity Save(Rule rel);
        public abstract EPiServer.Core.PageDataCollection SearchRelations(Rule rule, int pageID, string searchKeyWord, EPiServer.Core.PageReference hierarchyStart);
        public abstract EPiServer.Core.PageDataCollection SearchRelations(Rule rule, int pageID, string searchKeyWord, EPiServer.Core.PageReference hierarchyStart, bool isLeftRule);
        public abstract EPiServer.Core.PageDataCollection SearchRelations(Rule rule, int pageID, string searchKeyWord, bool isLeftRule);
    }
}