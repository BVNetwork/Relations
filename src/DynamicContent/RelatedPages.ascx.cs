using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.PlugIn;
using EPiServer.DynamicContent;
using EPiCode.Relations.Helpers;
using EPiServer;
using EPiServer.Core;
using EPiCode.Relations.CustomProperties;
using EPiCode.Relations.Core;

namespace EPiCode.Relations.EPiCode.Relations.DynamicContent
{
    [DynamicContentPlugIn(
        DisplayName = "Related pages",
        Description = "Displays related pages to this page by two hops")]
    public partial class RelatedPages : EPiServer.UserControlBase, IDynamicContent
    {
        public string MaxCount { get; set; }
        public string CurrentRule {get; set;}
        public Rule.Direction CurrentDirection { get; set; }

        public void GetRelations()
        {
            PageDataCollection relatedPages = PageHelper.GetPagesRelated(CurrentPage.PageLink, CurrentRule, CurrentDirection);
            EPiServer.Filters.FilterForVisitor.Filter(relatedPages);
            RelatedPagesRepeater.DataSource = relatedPages;
            RelatedPagesRepeater.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetRelations();
        }


        public RelatedPages() {
            Properties = new PropertyDataCollection(2);
            Properties.Add("Relation", new RuleSelector());
            Properties.Add("Max count", new PropertyString());
            Properties["Relation"].Value = "";
            Properties["Max count"].Value = "3";
    
        }


        public System.Web.UI.Control GetControl(PageBase hostPage)
        {
            var control = (RelatedPages)hostPage.LoadControl("~/EPiCode/Relations/DynamicContent/RelatedPages.ascx");


            if (Properties["Relation"] != null && Properties["Relation"].Value != null && !string.IsNullOrEmpty(Properties["Relation"].Value.ToString()))
            {
                string currentRule = Properties["Relation"].Value.ToString();
                string[] values = currentRule.Split(';');
                if (values != null && values.Length > 1)
                {
                    control.CurrentRule = values[0];
                    control.CurrentDirection = (values[1] == "Left") ? Rule.Direction.Left : Rule.Direction.Right;
                }
            }

            control.MaxCount = Properties["Max count"].Value as string;
            control.GetRelations();
            return control;
        }

        public PropertyDataCollection Properties
        {
            get;
            set;
        }

        public string Render(PageBase hostPage)
        {
            throw new NotSupportedException();
        }

        public bool RendersWithControl
        {
            get { return true; }
        }

        public string State
        {
            get
            {
                return string.Format("{0}|{1}",
                                     Properties["Relation"],
                                     Properties["Max count"].Value);            
            }

            set {
                var values = value.Split('|');
                if (values.Length == 2)
                {
                    RuleSelector rule = new RuleSelector();
                    Properties["Relation"].Value = values[0];
                    Properties["Max count"].Value = values[1];
                }

            }
        }

        /// <summary>
        /// The current page object
        /// </summary>
        private static PageData CurrentPage
        {
            get
            {
                var page = HttpContext.Current.Handler as EPiServer.PageBase;

                if (page == null)
                {
                    return null;
                }

                return page.CurrentPage;
            }
        }


    }
}