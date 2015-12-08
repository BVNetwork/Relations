using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Core;
using EPiCode.Relations.Core;

namespace EPiCode.Relations.Plugins.Edit.Units
{
    public partial class RelationSelector : EPiServer.UserControlBase
    {
        public Rule CurrentRule { get; set; }

        private Rule.Direction _ruleDirection;

        protected bool IsLeftRule
        {
            get {
                return _ruleDirection == Rule.Direction.Left ? true : false;
            }
            set {
                _ruleDirection = (value) ? Rule.Direction.Left : Rule.Direction.Right;
            }
        }

        protected bool IsRightRule
        {
            get
            {
                return !IsLeftRule;
            }
            set
            {
                IsLeftRule = !value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorMessageContainer.Visible = false;
            ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl(Button1);
            if (CurrentRule != null)
            {
                RuleName.Text = CurrentRule.RuleName;
                IsLeftRule = (CurrentRule.RuleDirection == Rule.Direction.Left);
                RuleDirectionLiteral.Text = _ruleDirection.ToString();
            }
            else if (!string.IsNullOrEmpty(RuleName.Text))
            {
                CurrentRule = RuleEngine.Instance.GetRule(RuleName.Text);
                _ruleDirection = (Rule.Direction)Enum.Parse(typeof(Rule.Direction), RuleDirectionLiteral.Text);
            }

            if (_ruleDirection == Rule.Direction.Both)
            {
                // Rule direction not set - check page against rule
                IsLeftRule = RuleEngine.Instance.IsLeftRule(CurrentPage.PageLink.ID, CurrentRule);
            }

            if (IsLeftRule && !CurrentRule.RuleVisibleLeft || !IsLeftRule && !CurrentRule.RuleVisibleRight)
            {
                this.Visible = false;
                return;
            }

            if (!IsPostBack)
                PerformSearch_Click(null, null);

            string pageTypes = "";
            
            if(IsLeftRule)
                pageTypes =  HttpUtility.UrlDecode(CurrentRule.PageTypeRight);
            else
                pageTypes = HttpUtility.UrlDecode(CurrentRule.PageTypeLeft);

            if (!IsPostBack)
                if (pageTypes.Split(';').Length > 2)
                {
                    PageTypesDropDown.DataSource = pageTypes.Split(';');
                    PageTypesDropDown.DataBind();
                    PageTypesContainer.Visible = true;
                    PageTypesDropDown.SelectedIndex = PageTypesDropDown.Items.Count - 1;
                }

            RelatedRelations.DataSource = GetRelationsForPage(CurrentPage.PageLink.ID, CurrentRule);
            RelatedRelations.DataBind();
        }

        private List<Relation> GetRelationsForPage(int page, Rule rule) {
            List<Relation> relations = new List<Relation>();
            if (rule.RuleTextLeft == rule.RuleTextRight)
            {
                relations.AddRange(RelationEngine.Instance.GetRelationsForPage(page, rule));
            }
            else
                if (IsLeftRule)
                    relations = RelationEngine.Instance.GetRelationsForPage(page, rule, Rule.Direction.Left);
                else
                    relations = RelationEngine.Instance.GetRelationsForPage(page, rule, Rule.Direction.Right);
            return relations;
        }

        protected string GetRuleText()
        {
            return (IsLeftRule ? CurrentRule.RuleTextLeft : CurrentRule.RuleTextRight);
        }

        protected void PageTypes_Changed(object sender, EventArgs e) {
            PerformSearch_Click(null, null);
        }

        protected PageDataCollection SearchRelations()
        {
            string CacheKey = CurrentRule.Id.ToString() + "_" + CurrentPage.PageLink.ID+"_"+PageTypesDropDown.SelectedValue+"_"+_ruleDirection.ToString();
            PageDataCollection pages = EPiServer.CacheManager.Get(CacheKey) as PageDataCollection;
            PageDataCollection result = new PageDataCollection();
            if (pages == null || 1 == 1)
            {
                pages = RuleEngine.Instance.SearchRelations(CurrentRule, CurrentPage.PageLink.ID, SearchKeyWord.Text, IsLeftRule);
                if (PageTypesDropDown.SelectedValue.Length > 2)
                {
                    foreach (PageData pd in pages)
                        if (pd.PageTypeName == PageTypesDropDown.SelectedValue)
                            result.Add(pd);
                }
                else
                    result = pages;
                EPiServer.CacheManager.Add(CacheKey, result);
            }
            return result;
        }

        private void AddRelation(string ruleName, int currentPage, int relatedPage) { 
            if(IsLeftRule)
                RelationEngine.Instance.AddRelation(CurrentRule.RuleName, relatedPage, currentPage);
            else
                RelationEngine.Instance.AddRelation(CurrentRule.RuleName, currentPage, relatedPage);
        }

        protected void AddRelations_Click(object sender, EventArgs e)
        {
            ErrorMessageContainer.Visible = false;
            if (!string.IsNullOrEmpty(NewPageTextBox.Text))
            {
                if (Validate(CurrentRule, int.Parse(NewPageTextBox.Text), CurrentPage.PageLink.ID))
                {
                    AddRelation(CurrentRule.RuleName, int.Parse(NewPageTextBox.Text), CurrentPage.PageLink.ID);
                }
                else {
                    ErrorMessageContainer.Visible = true;                
                }
                NewPageTextBox.Text = string.Empty;
            }
            else
            {
                foreach (ListItem li in AllRelations.Items)
                {
                    if (li.Selected)
                        if(Validate(CurrentRule, int.Parse(li.Value), CurrentPage.PageLink.ID))
                        {
                            AddRelation(CurrentRule.RuleName, int.Parse(li.Value), CurrentPage.PageLink.ID);
                        }
                        else 
                        {
                            ErrorMessageContainer.Visible = true;
                        }
                }
            }
            RelatedRelations.DataSource = GetRelationsForPage(CurrentPage.PageLink.ID, CurrentRule);
            RelatedRelations.DataBind();
        }

        protected bool Validate(Rule rule, int pageLeft, int pageRight) {
            
            Validator.ValidationResult validationResult;
            if(IsLeftRule)
                validationResult = Validator.Validate(rule.RuleName, pageRight, pageLeft);
            else
                validationResult = Validator.Validate(rule.RuleName, pageLeft, pageRight);
            if (validationResult == Validator.ValidationResult.Ok)
                return true;
            ErrorMessage.Text = LanguageHandler.GetErrorMessage(validationResult.ToString());
            return false;
        }

        protected void PerformSearch_Click(object sender, EventArgs e) {
            PageDataCollection pdc = SearchRelations();
            PageDataCollection result = new PageDataCollection();
            if (!string.IsNullOrEmpty(SearchKeyWord.Text))
            {
                foreach (PageData pd in pdc)
                {
                    if (pd.PageName.ToLower().Contains(SearchKeyWord.Text.ToString().ToLower()))
                        result.Add(pd);
                }
            }
            else
                result = pdc;

            AllRelations.DataSource = result;

            AllRelations.DataBind();
        }
    }
}