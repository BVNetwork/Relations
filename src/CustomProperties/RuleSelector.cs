using System;
using EPiServer.Core;
using EPiServer.PlugIn;

namespace EPiCode.Relations.CustomProperties
{
    [Serializable]
    [PageDefinitionTypePlugIn]
    public class RuleSelector : PropertyString
    {
        public override IPropertyControl CreatePropertyControl()
        {
            return new RuleSelectorControl();
        }
    }
}
