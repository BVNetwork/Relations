using System;
using System.Web.UI.WebControls;
using EPiCode.Relations.Core;
using EPiServer.Core;
using log4net;
using EPiServer.Data;

namespace EPiCode.Relations.Plugins.Admin.Units
{
    public partial class RelationValidationControl : System.Web.UI.UserControl
    {
        private PageData _pageLeft;
        private PageData _pageRight;
        
        public Relation CurrentRelation { get; set; }

        protected PageData PageLeft {
            get {
                if (_pageLeft == null) { 
                    PageReference pr = new PageReference(CurrentRelation.PageIDLeft);
                    if (pr != null && pr != PageReference.EmptyReference)
                        _pageLeft = EPiServer.DataFactory.Instance.GetPage(pr);
                }
                return _pageLeft;
            }
        }

        protected PageData PageRight
        {
            get
            {
                if (_pageRight == null)
                {
                    PageReference pr = new PageReference(CurrentRelation.PageIDRight);
                    if (pr != null && pr != PageReference.EmptyReference)
                        _pageRight = EPiServer.DataFactory.Instance.GetPage(pr);
                }
                return _pageRight;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string GetPageName(int pageID) {
            PageReference pr = new PageReference(pageID);
            PageData page;
            if (pr != null && pr != PageReference.EmptyReference) {
                try
                {
                    page = EPiServer.DataFactory.Instance.GetPage(pr);
                    if (page != null)
                        return page.PageName;
                }
                catch (Exception e) {
                    LogManager.GetLogger("DefaultLogger").Error("EPiCode.Relations.RelationValidationControl - Page in relation not found : " + e.Message);                    
                }
            }
            return "["+pageID.ToString()+"]";
        }

        protected void RemoveRelation_Click(object sender, CommandEventArgs e)
        {
            Relation relationToDelete = RelationEngine.Instance.GetRelation(Identity.Parse(e.CommandArgument.ToString()));
            RelationEngine.Instance.DeleteRelation(relationToDelete);
            this.Visible = false;
        }

    }
}