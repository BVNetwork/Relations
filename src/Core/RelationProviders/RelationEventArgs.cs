using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiCode.Relations.Core.RelationProviders
{
    public class RelationEventArgs
    {
        public string RelationName;
        public Relation CurrentRelation;
        public bool CancelEvent;
    }
}
