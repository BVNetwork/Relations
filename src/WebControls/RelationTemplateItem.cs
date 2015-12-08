using System;
using System.Web.UI;

namespace EPiCode.Relations.WebControls
{
    public class RelationTemplateItem : Control, System.Web.UI.INamingContainer, IDataItemContainer
    {
        private object _CurrentDataItem;

        public RelationTemplateItem(object currentItem)
        {
            _CurrentDataItem = currentItem;

        }

        public object DataItem
        {
            get { return _CurrentDataItem; }
        }

        public int DataItemIndex
        {
            get
            {
                throw new Exception
                    ("The method or operation is not implemented.");
            }
        }

        public int DisplayIndex
        {
            get
            {
                throw new Exception
                    ("The method or operation is not implemented.");
            }
        }

    }
}