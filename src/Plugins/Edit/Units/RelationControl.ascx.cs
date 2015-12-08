using System;
using EPiCode.Relations.Core;
using EPiServer.Core;
using EPiServer;
using log4net;

namespace EPiCode.Relations.Plugins.Edit.Units
{
    public partial class RelationControl : EPiServer.UserControlBase
    {
        private PageData _relatedPage;

        public Relation CurrentRelation { get; set; }
        public PageData RelatedPage
        {
            get
            {
                if (_relatedPage != null)
                    return _relatedPage;
                if (CurrentRelation != null)
                    _relatedPage = (CurrentRelation.PageIDLeft != CurrentPage.PageLink.ID) ? GetPage(CurrentRelation.PageIDLeft) : GetPage(CurrentRelation.PageIDRight);
                return _relatedPage;
            }
        }

        protected void RemoveRelation_Click(object sender, EventArgs e)
        {
            RelationEngine.Instance.DeleteRelation(CurrentRelation);
            ControlPlaceHolder.Visible = false;
            ControlPlaceHolder.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected string GetPageName()
        {
            if (RelatedPage != null)
                return RelatedPage.PageName;
            return "Error getting relation";
        }

        protected PageData GetPage(int id)
        {
            try
            {
                PageReference page = new PageReference(id);
                if (page != null && page != PageReference.EmptyReference)
                {
                    PageData pd = DataFactory.Instance.GetPage(page);
                    if (pd != null)
                        return pd;
                }
                return null;
            }
            catch (Exception e) {
                LogManager.GetLogger("DefaultLogger").Error("EPiCode.Relations.RelationControl - Page in relation not found : "+e.Message);
                return null;
            }
        }
    }

}