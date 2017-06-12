using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core = EPiCode.Relations.Core;

namespace EPiCode.Relations.Db.Data
{
    public class RulePageType
    {
        public int Id { get; set; }
        public string PageTypeName { get; set; }
        public string Direction { get; set; }
        public virtual Rule Rule { get; set; }        
    }
}
