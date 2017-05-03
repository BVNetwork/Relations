using System;
using EPiServer.Data.Dynamic;
using EPiServer.Data;

namespace EPiCode.Relations.Core
{
    [EPiServerDataStore(AutomaticallyCreateStore = true, AutomaticallyRemapStore = true)]
    public class Relation
    {
        public Identity Id { get; set; }
        public string RuleName { get; set; }
        public int PageIDLeft { get; set; }
        public int PageIDRight { get; set; }
        public string LanguageBranch { get; set; }


        protected void Initialize()
        {
            Id = Identity.NewIdentity(Guid.NewGuid());
        }

        public Relation() {
            Initialize();
        }

    }
}