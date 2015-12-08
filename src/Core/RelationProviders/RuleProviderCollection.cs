using System.Configuration.Provider;

namespace EPiCode.Relations.Core.RelationProviders
{
    public class RuleProviderCollection : ProviderCollection
    {
        private object lockObject = new object();
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                return;
                //throw new ArgumentNullException("The provider parameter cannot be null.");

            if (!(provider is RuleProviderBase))
                return;
                //throw new ArgumentException("The provider parameter must be of type RuleProviderBase.");
            lock (lockObject)
            {
                if (base[provider.Name] == null)
                    base.Add(provider);
            }
        }

        new public RuleProviderBase this[string name]
        {
            get { return (RuleProviderBase)base[name]; }
        }


        public void CopyTo(RuleProviderBase[] array, int index)
        {
            lock (lockObject)
            {

                base.CopyTo(array, index);
            }
        }
    }
}