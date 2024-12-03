using System;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using EPiCode.Relations.Core;
using EPiServer;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Filters;
using EPiServer.ServiceLocation;
using EPiServer.Util;
using EPiServer.DataAbstraction;


namespace EPiCode.Relations.Plugins.Admin.Units
{
    public partial class EditRule : System.Web.UI.UserControl
    {



        public Rule CurrentRule { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            var repository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();
            var pageTypes = (from pagetypes in repository.List() where (pagetypes as PageType != null) select pagetypes);

            RulePageTypeLeft.DataSource = pageTypes;
            RulePageTypeRight.DataSource = pageTypes;
            SortOrderLeft.DataSource = Enum.GetNames(typeof(FilterSortOrder));
            SortOrderRight.DataSource = Enum.GetNames(typeof(FilterSortOrder));

            if (CurrentRule != null)
                SetupEditControls();
            else
            {
                CurrentRule = new Rule();
            }

            CurrentRule.PageTypeLeft = HttpUtility.UrlEncode(RulePageTypesLeftLiteral.Text);
            CurrentRule.PageTypeRight = HttpUtility.UrlEncode(RulePageTypesRightLiteral.Text);

            AccessLevelDropDownList.DataSource = Enum.GetNames(typeof(EPiServer.Security.AccessLevel));

            RulesRepeater.DataSource = RuleEngine.Instance.GetAllRulesList();
            RulesRepeater.DataBind();

            StatusLiteral.Text = "";

            if (!IsPostBack)
                DataBind();
        }


        public void SetupEditControls()
        {
            if (CurrentRule != null && CurrentRule.Id != null)
            {
                RuleId.Text = CurrentRule.Id.ToString();
                RuleName.Text = CurrentRule.RuleName;
                RulePageTypesLeftLiteral.Text = HttpUtility.UrlDecode(CurrentRule.PageTypeLeft);
                RulePageTypesRightLiteral.Text = HttpUtility.UrlDecode(CurrentRule.PageTypeRight);
                if (string.IsNullOrEmpty(CurrentRule.PageTypeLeft) == false)
                {
                    RulePageTypesLeftListBox.DataSource = HttpUtility.UrlDecode(CurrentRule.PageTypeLeft).Split(';');

                }
                else
                {
                    RulePageTypesLeftListBox.Items.Clear();
                }
                RulePageTypesLeftListBox.DataBind();
                if (string.IsNullOrEmpty(CurrentRule.PageTypeRight) == false)
                {
                    RulePageTypesRightListBox.DataSource = HttpUtility.UrlDecode(CurrentRule.PageTypeRight).Split(';');

                }
                else
                {
                    RulePageTypesRightListBox.Items.Clear();

                }
                RulePageTypesRightListBox.DataBind();
                RuleTextLeft.Text = CurrentRule.RuleTextLeft;
                RuleTextRight.Text = CurrentRule.RuleTextRight;
                if (GetPage(CurrentRule.RelationHierarchyStartLeft) != null)
                    RuleHierarchyStartPageReferenceLeft.PageLink =
                        new EPiServer.Core.PageReference(CurrentRule.RelationHierarchyStartLeft);
                else
                {
                    RuleHierarchyStartPageReferenceLeft.PageLink = PageReference.StartPage;
                }
                if (GetPage(CurrentRule.RelationHierarchyStartRight) != null)
                    RuleHierarchyStartPageReferenceRight.PageLink = new EPiServer.Core.PageReference(CurrentRule.RelationHierarchyStartRight);
                else
                {
                    RuleHierarchyStartPageReferenceRight.PageLink = PageReference.StartPage;
                }
                VisibleLeft.Checked = CurrentRule.RuleVisibleLeft;
                VisibleRight.Checked = CurrentRule.RuleVisibleRight;
                RuleDescriptionLeft.Text = CurrentRule.RuleDescriptionLeft;
                RuleDescriptionRight.Text = CurrentRule.RuleDescriptionRight;
                SortOrderLeft.SelectedIndex = SortOrderLeft.Items.IndexOf(SortOrderLeft.Items.FindByValue(((FilterSortOrder)CurrentRule.SortOrderLeft).ToString()));
                SortOrderRight.SelectedIndex = SortOrderRight.Items.IndexOf(SortOrderRight.Items.FindByValue(((FilterSortOrder)CurrentRule.SortOrderRight).ToString()));
                AccessLevelDropDownList.SelectedIndex = AccessLevelDropDownList.Items.IndexOf(AccessLevelDropDownList.Items.FindByValue(CurrentRule.EditModeAccessLevel));
                RulesRepeater.DataBind();

            }
        }

        protected void AddRule_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentRule = RuleEngine.Instance.GetRule(Identity.Parse(RuleId.Text));
                CurrentRule.RuleName = RuleName.Text;
                CurrentRule.RuleTextLeft = RuleTextLeft.Text;
                CurrentRule.RuleTextRight = RuleTextRight.Text;
                CurrentRule.PageTypeLeft = HttpUtility.UrlEncode(RulePageTypesLeftLiteral.Text);
                CurrentRule.PageTypeRight = HttpUtility.UrlEncode(RulePageTypesRightLiteral.Text);
                CurrentRule.RuleVisibleLeft = VisibleLeft.Checked;
                CurrentRule.RuleVisibleRight = VisibleRight.Checked;
                CurrentRule.RuleDescriptionLeft = RuleDescriptionLeft.Text;
                CurrentRule.RuleDescriptionRight = RuleDescriptionRight.Text;
                CurrentRule.SortOrderLeft = (int) Enum.Parse(typeof (FilterSortOrder), SortOrderLeft.SelectedValue);
                CurrentRule.SortOrderRight = (int) Enum.Parse(typeof (FilterSortOrder), SortOrderRight.SelectedValue);
                CurrentRule.EditModeAccessLevel = AccessLevelDropDownList.SelectedValue;
                if (RuleHierarchyStartPageReferenceLeft.PageLink != null &&
                    GetPage(RuleHierarchyStartPageReferenceLeft.PageLink.ID) != null)
                    CurrentRule.RelationHierarchyStartLeft = RuleHierarchyStartPageReferenceLeft.PageLink.ID;
                if (RuleHierarchyStartPageReferenceRight.PageLink != null &&
                    GetPage(RuleHierarchyStartPageReferenceRight.PageLink.ID) != null)
                    CurrentRule.RelationHierarchyStartRight = RuleHierarchyStartPageReferenceRight.PageLink.ID;
                RuleEngine.Instance.Save(CurrentRule);
                StatusLiteral.Text = "<div class='well'>Rule saved</div>";
                SetupEditControls();
            }
            catch (Exception ex)
            {
                StatusLiteral.Text = "<div class='alert alert-danger'><b>Something went wrong:</b><br/> "+ex.Message+"</div>";
                
            }
            //Response.Redirect("/Moduels/epicode.relations/Plugins/Admin/RelationRulePlugin.aspx");
        }

        protected void DeleteRule_Click(object sender, EventArgs e)
        {
            CurrentRule = RuleEngine.Instance.GetRule(Identity.Parse(RuleId.Text));
            RuleEngine.Instance.DeleteRule(CurrentRule.RuleName);
            CurrentRule = null;
            StatusLiteral.Text = "<div class='alert alert-danger'><b>Rule delted!</b></div>";
            SetupEditControls();
            //Response.Redirect("/Moduels/epicode.relations/Plugins/Admin/RelationRulePlugin.aspx");
        }

        protected void AddPageTypeRight_Click(object sender, EventArgs e)
        {
            CurrentRule.PageTypeRight += HttpUtility.UrlEncode(RulePageTypeRight.SelectedValue + ";");
            RulePageTypesRightLiteral.Text = HttpUtility.UrlDecode(CurrentRule.PageTypeRight);
            RulePageTypesRightListBox.DataSource = HttpUtility.UrlDecode(CurrentRule.PageTypeRight).Split(';');
            RulePageTypesRightListBox.DataBind();
        }


        protected void RemovePageTypeRight_Click(object sender, EventArgs e)
        {
            string pageTypesRemoved = RemovePageType(RulePageTypesRightLiteral.Text, RulePageTypesRightListBox.SelectedValue);
            RulePageTypesRightLiteral.Text = pageTypesRemoved;
            RulePageTypesRightListBox.DataSource = pageTypesRemoved.Split(';');
            RulePageTypesRightListBox.DataBind();
        }

        protected void RemovePageTypeLeft_Click(object sender, EventArgs e)
        {
            string pageTypesRemoved = RemovePageType(RulePageTypesLeftLiteral.Text, RulePageTypesLeftListBox.SelectedValue);
            RulePageTypesLeftLiteral.Text = pageTypesRemoved;
            RulePageTypesLeftListBox.DataSource = pageTypesRemoved.Split(';');
            RulePageTypesLeftListBox.DataBind();
        }


        protected void AddPageTypeLeft_Click(object sender, EventArgs e)
        {
            CurrentRule.PageTypeLeft += HttpUtility.UrlEncode(RulePageTypeLeft.SelectedValue + ";");
            RulePageTypesLeftLiteral.Text = HttpUtility.UrlDecode(CurrentRule.PageTypeLeft);
            RulePageTypesLeftListBox.DataSource = StringToArray(CurrentRule.PageTypeLeft);
            RulePageTypesLeftListBox.DataBind();
        }

        protected string RemovePageType(string pageTypes, string pageTypeToRemove)
        {
            string pageTypesResult = "";
            string[] pageTypesArray = pageTypes.Split(';');
            var newArray = from p in pageTypesArray where p.ToLower() != pageTypeToRemove.ToLower() select p;
            foreach (string pt in newArray.ToList<string>())
            {
                if (pt != string.Empty)
                    pageTypesResult += pt + ";";
            }
            return pageTypesResult;
        }

        protected string[] StringToArray(string pageTypes)
        {
            return HttpUtility.UrlDecode(pageTypes).Split(';');
        }

        protected void OnCommand(object sender, CommandEventArgs e)
        {
            CurrentRule = RuleEngine.Instance.GetRule(e.CommandArgument.ToString());
            SetupEditControls();
            RulesRepeater.DataBind();
        }


        protected PageData GetPage(int page)
        {
            PageData pageData = PageEngine.GetPage(page);
            if (pageData != null)
                return pageData;
            StatusLiteral.Text = "<div class='alert alert-danger'><b>Page "+page.ToString()+" could not be found. Using Start Page instead.</b></div>";
            return null;
        }

    }
}