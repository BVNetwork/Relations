using EPiCode.Relations.Core.RelationProviders;

namespace EPiCode.Relations.Core
{
    public class RuleEngine 
    {

        public static RuleProviderBase Instance
        {
            get
            {
                return RuleProviderManager.Provider;
            }
        }


    }
}