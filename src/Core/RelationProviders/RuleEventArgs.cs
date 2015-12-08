using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiCode.Relations.Core.RelationProviders
{
    public class RuleEventArgs
    {
        public string RuleName;
        public Rule CurrentRule;
        public bool CancelEvent;
    }
}
