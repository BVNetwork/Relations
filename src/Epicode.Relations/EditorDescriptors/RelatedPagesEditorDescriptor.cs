using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using System;
using System.Collections.Generic;

namespace EPiCode.Relations.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(string))]
    public class RelatedPagesEditorDescriptor : EditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            
            SelectionFactoryType = typeof(RelatedPagesSelectionFactory);

            base.ModifyMetadata(metadata, attributes);

        }
    }
}