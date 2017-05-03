using System.Configuration.Provider;

namespace EPiCode.Relations.Core.RelationProviders
{
    public class RelationProviderCollection : ProviderCollection
    {

        public override void Add(ProviderBase provider)
        {
            if (provider == null) {
                //throw new ArgumentNullException("The provider parameter cannot be null.");
                return;
            }

            if (!(provider is RelationProviderBase))
                //throw new ArgumentException("The provider parameter must be of type RelationProviderBase.");
                return;

            base.Add(provider);
        }

        new public RelationProviderBase this[string name]
        {
            get { return (RelationProviderBase)base[name]; }
        }

        public void CopyTo(RelationProviderBase[] array, int index)
        {
            base.CopyTo(array, index);
        }
    }
}