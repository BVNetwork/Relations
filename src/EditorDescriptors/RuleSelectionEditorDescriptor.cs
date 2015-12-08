using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using System;
using System.Collections.Generic;

namespace EPiCode.Relations.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(string), UIHint="Rules")]
    public class RuleSelectionEditorDescriptor : EditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            
            SelectionFactoryType = typeof(RuleSelectionFactory);

            ClientEditingClass = "epi.cms.contentediting.editors.SelectionEditor";

            base.ModifyMetadata(metadata, attributes);

        }
    }
}