using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.PlugIn;
using EPiServer.DynamicContent;
using EPiCode.Relations.Helpers;
using EPiServer.Core;
using EPiCode.Relations.CustomProperties;
using EPiServer;
using EPiCode.Relations.Core;

namespace EPiCode.Relations.EPiCode.Relations.DynamicContent
{
    /*
    [DynamicContentPlugIn(
        DisplayName = "Related pages two hops",
        Description = "Displays related pages to this page by two hops",
        ViewUrl = "~/EPiCode/Relations/DynamicContent/RelatedPagesTwoHop.ascx")]
    public partial class RelatedPagesTwoHop : EPiServer.UserControlBase, IDynamicContent
    {
        public string MaxCount { get; set; }
        public string FirstRule { get; set; }
        public Rule.Direction FirstDirection { get; set; }
        public string SecondRule { get; set; }
        public Rule.Direction SecondDirection { get; set; }

        public void GetRelations() {
            RelatedPagesRepeater.DataSource = PageHelper.GetPagesRelated(CurrentPage.PageLink, FirstRule, FirstDirection, SecondRule, SecondDirection);
            RelatedPagesRepeater.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetRelations();
        }

        public RelatedPagesTwoHop() {
            Properties = new PropertyDataCollection(2);
            Properties.Add("FirstRelation", new RuleSelector());
            Properties.Add("SecondRelation", new RuleSelector());
            Properties.Add("Max count", new PropertyString());

            Properties["FirstRelation"].Value = "";
            Properties["SecondRelation"].Value = "";
            Properties["Max count"].Value = "3";
    
        }


        public System.Web.UI.Control GetControl(PageBase hostPage)
        {
            var control = (RelatedPagesTwoHop)hostPage.LoadControl("/EPiCode/Relations/DynamicContent/RelatedPagesTwoHop.ascx");

            string firstRule = Properties["FirstRelation"].Value.ToString();
            string secondRule = Properties["SecondRelation"].Value.ToString();

            if (!string.IsNullOrEmpty(firstRule))
            {
                string[] values = firstRule.Split(';');
                if (values != null && values.Length > 1)
                {
                    control.FirstRule = values[0];
                    control.FirstDirection = (values[1] == "Left") ? Rule.Direction.Left : Rule.Direction.Right;
                }
            }

            if (!string.IsNullOrEmpty(firstRule))
            {
                string[] values = secondRule.Split(';');
                if (values != null && values.Length > 1)
                {
                    control.SecondRule = values[0];
                    control.SecondDirection = (values[1] == "Left") ? Rule.Direction.Left : Rule.Direction.Right;
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
                return string.Format("{0}|{1}|{2}",
                                     Properties["FirstRelation"],
                                     Properties["SecondRelation"],
                                     Properties["Max count"].Value);            
            }

            set {
                var values = value.Split('|');
                if (values.Length == 3)
                {
                    RuleSelector rule = new RuleSelector();
                    Properties["FirstRelation"].Value = values[0];
                    Properties["SecondRelation"].Value = values[1];
                    Properties["Max count"].Value = values[2];
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

    }*/
}