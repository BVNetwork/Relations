using EPiCode.Relations.Core.RelationProviders;


namespace EPiCode.Relations.Core
{
    public class RelationEngine
    {

        public static RelationProviderBase Instance
        {
            get
            {
                return RelationProviderManager.Provider;
            }
        }

    }
}