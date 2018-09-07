using System.Collections.Generic;
using System.Linq;
using EPiCode.Relations.Db.Mapping;

namespace EPiCode.Relations.Db.Extentions
{
    public static class EnumerableExtentions
    {
        public static IEnumerable<TBusinessEntity> Translate<TBusinessEntity, TServiceEntity>(this IEnumerable<TServiceEntity> serviceEntities,
                                                                                              EntityMapperTranslator<TBusinessEntity, TServiceEntity> entityMapperTranslator)
        {
            return serviceEntities.Select(o => entityMapperTranslator.Translate<TBusinessEntity>(o));
        }
    }
}
