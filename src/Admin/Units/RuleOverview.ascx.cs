using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiCode.Relations.Core;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace EPiCode.Relations.Admin.Units
{
    public partial class RuleOverview : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RulesRepeater.DataSource = RuleEngine.Instance.GetAllRulesList();
            RulesRepeater.DataBind();

        }

        protected void EditRuleBtn_OnClick(object sender, CommandEventArgs e)
        {
            (Parent.Parent as RelationsAdmin).EditRule(e.CommandArgument.ToString());
        }

        protected int GetPossibleRelations(Rule rule, bool isLeftRule, Control warningControl)
        {
            try
            {
                return RuleEngine.Instance.SearchRelations(rule, 0, "", isLeftRule).Count;
            }
            catch (Exception e)
            {
                warningControl.Controls.Add(new LiteralControl("<div class='alert alert-danger'>Rule error: "+e.Message+" - when checking rule '"+rule.RuleName+"'</div>"));
            }   
        

            return 0;
        }

        protected PageData GetPage(int page)
        {
            try
            {
                return ServiceLocator.Current.GetInstance<IContentRepository>().Get<IContent>(new ContentReference(page)) as PageData;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        protected string GetPageName(int pageId)
        {
            PageData page = GetPage(pageId);
            if (page != null)
                return page.Name;
            return "Page not found!";
        }

        protected string GetPageTypeList(Rule rule, bool isLeftRule)
        {
            string pageType = isLeftRule ? rule.PageTypeLeft : rule.PageTypeRight;
            if (string.IsNullOrEmpty(pageType) == false)
            {
                string[] pageTypes =  HttpUtility.UrlDecode(pageType).Split(';');
                pageType = "";
                foreach (string p in pageTypes)
                {
                    if(!string.IsNullOrEmpty(p))
                    pageType += "" +p + "<br/>";
                }
            }
            return pageType;
        }
    }
}