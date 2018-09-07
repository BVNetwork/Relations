using System;
using EPiCode.Relations.Core;
using EPiCode.Relations.Db.Mapping;
using EPiServer.Data;

namespace EPiCode.Relations.Db.Mapping
{
    internal class RelationTranslator : EntityMapperTranslator<Relation, Data.Relation>
    {
        protected override Relation ServiceToBusiness(Data.Relation value)
        {
            return new Relation
                {
                    Id = Identity.NewIdentity(value.RelationId),
                    LanguageBranch = value.LanguageBranch,
                    PageIDLeft = value.PageIdLeft,
                    PageIDRight = value.PageIdRight,
                    RuleName = value.RuleName
                };
        }

        protected override Data.Relation BusinessToService(Relation value)
        {
            return new Data.Relation
            {
                RelationId = value.Id.ExternalId,
                LanguageBranch = value.LanguageBranch,
                PageIdLeft = value.PageIDLeft,
                PageIdRight = value.PageIDRight,
                RuleName = value.RuleName
            };
        }
    }
}
