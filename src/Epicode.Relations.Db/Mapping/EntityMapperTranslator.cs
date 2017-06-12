using System;

namespace EPiCode.Relations.Db.Mapping
{
    public abstract class EntityMapperTranslator<TBusinessEntity, TServiceEntity>
    {
        public bool CanTranslate(Type targetType, Type sourceType)
        {
            return (targetType == typeof(TBusinessEntity) && sourceType == typeof(TServiceEntity)) ||
                   (targetType == typeof(TServiceEntity) && sourceType == typeof(TBusinessEntity));
        }

        public TTarget Translate<TTarget>(object source)
        {
            return (TTarget)Translate(typeof(TTarget), source);
        }

        public object Translate(Type targetType, object source)
        {
            if (targetType == typeof(TBusinessEntity))
            {
                return ServiceToBusiness((TServiceEntity)source);
            }
            if (targetType == typeof(TServiceEntity))
            {
                return BusinessToService((TBusinessEntity)source);
            }
            throw new System.ArgumentException("Invalid type passed to Translator", "targetType");
        }

        protected abstract TServiceEntity BusinessToService(TBusinessEntity value);
        protected abstract TBusinessEntity ServiceToBusiness(TServiceEntity value);
    }
}
