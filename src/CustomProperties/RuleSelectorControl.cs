using System;
using System.Collections.Generic;
using System.Text;
using EPiServer.Core;
using EPiCode.Relations.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiCode.Relations.Core;

namespace EPiCode.Relations.CustomProperties
{
    /// <summary>
    /// PropertyControl implementation used for rendering CustomPropertyTest data.
    /// </summary>
    public class RuleSelectorControl : EPiServer.Web.PropertyControls.PropertyStringControl
    {
        public DropDownList ddlRules;
        public TextBox ddlTextBox;

        public override void CreateEditControls() {
            ddlRules = new DropDownList();
            ddlRules.Attributes["id"] = "ddlRule" + this.PropertyData.Name;

            ddlTextBox = new TextBox();

            Rule.Direction direction = Rule.Direction.Both;
            string SelectedRule = "";
            if (RuleSelector.Value != null && !string.IsNullOrEmpty(RuleSelector.Value.ToString()))
            {
                string[] propertyValue = RuleSelector.Value.ToString().Split(';');
                if (propertyValue != null && propertyValue.Length > 1)
                {
                    ddlTextBox.Text = RuleSelector.Value.ToString();
                    SelectedRule = propertyValue[0];
                    direction = (Rule.Direction)Enum.Parse(typeof(Rule.Direction), propertyValue[1]);
                }
            }
            List<Rule> allRules = RuleEngine.Instance.GetAllRulesList();
            ddlRules.Items.Clear();
            ddlRules.Items.Add(new ListItem(string.Empty, Core.LanguageHandler.Translate("selectRule", "Select rule")));
            foreach (Rule rule in allRules)
            {
                ddlRules.Items.Add(new ListItem(Core.LanguageHandler.TranslateRuleName(rule), rule.RuleName));
            }


            if (RuleSelector.Value != null)
                foreach (ListItem li in ddlRules.Items)
                {
                    if (li.Value == SelectedRule)
                        li.Selected = true;
                }

            string ruleName = ddlRules.SelectedValue;
            Rule selectedRule = RuleEngine.Instance.GetRule(ruleName);

            if (string.IsNullOrEmpty(ddlTextBox.Text))
                ddlTextBox.Text = ruleName + ";Left";

            StringBuilder ddlDirectionString = new StringBuilder("");
            ddlDirectionString.Append("<select class='episize240' id='" + "ddlDir" + this.PropertyData.Name + "'>");
            ddlDirectionString.Append("<option value='0' "+((direction==Rule.Direction.Left) ? "selected" : "") +">"+selectedRule.RuleTextLeft+"</option>");
            ddlDirectionString.Append("<option value='1' " +((direction==Rule.Direction.Right) ? "selected" : "") +">" + selectedRule.RuleTextRight+"</option>");
            ddlDirectionString.Append("</select>");

            LiteralControl ddlDirection = new LiteralControl(ddlDirectionString.ToString());

            StringBuilder directionValues = new StringBuilder();
            directionValues.Append("<script type='text/javascript'>");
            directionValues.Append("$(function() {");
                directionValues.Append("var rules"+this.PropertyData.Name+" = { ");
                for(int i = 0; i < allRules.Count; i++)    
                {
                    Rule rule = allRules[i];
                    directionValues.Append(rule.RuleName+": [");
                    directionValues.Append("'"+rule.RuleTextLeft+"',");
                    directionValues.Append("'"+rule.RuleTextRight+"'");
                    directionValues.Append("]");
                    if (i < allRules.Count - 1)
                        directionValues.Append(",");
                }
                directionValues.Append("};");
                directionValues.Append("$(document).ready(function() {$('select[id=\"" + "ddlRule" + this.PropertyData.Name + "\"]').change(function() {");
                    //What do do when rule is changed
                    directionValues.Append("var options = '';");
                    directionValues.Append("$.each(rules" + this.PropertyData.Name + "[$(this).val()] || [], function(i, v) { ");
                        directionValues.Append("options += '<option value=\"'+i+'\">' + v + '</option>';");
                    directionValues.Append("});");
                    directionValues.Append("$('select[id=\"" + "ddlDir" + this.PropertyData.Name + "\"]').html(options);");
                    directionValues.Append("$('input[id=\"" + "ddlTextBox" + this.PropertyData.Name + "\"]').val($('select[id=\"" + "ddlRule" + this.PropertyData.Name + "\"]').val()+';Left');");
                    directionValues.Append("});");
                    directionValues.Append("});");
                    //What do do when direction is changed
                    directionValues.Append("$(document).ready(function() {$('select[id=\"" + "ddlDir" + this.PropertyData.Name + "\"]').change(function() {");
                    directionValues.Append("$('input[id=\"" + "ddlTextBox" + this.PropertyData.Name + "\"]').val(");
                    directionValues.Append("$('select[id=\"" + "ddlRule" + this.PropertyData.Name + "\"]').val()");
                    directionValues.Append("+';'+");
                    directionValues.Append("($('select[id=\"" + "ddlDir" + this.PropertyData.Name + "\"]').val() == '0' ? 'Left' : 'Right')");    
                    directionValues.Append(");");
                    directionValues.Append("});");
                    directionValues.Append("});");

            directionValues.Append("});");
            directionValues.Append("</script>");
            this.Controls.Add(new LiteralControl("<div style='border:1px solid #999;padding:5px;background-color:#eee;width:300px;margin-bottom:10px;'><div style='float:left;width:120px;'>Rule:</div><div float:left;>"));
            this.Controls.Add(ddlRules);
            this.Controls.Add(new LiteralControl("</div><div style='float:left;width:120px;margin-top:5px;'>Direction:</div><div float:left;margin-top:5px;>"));
            this.Controls.Add(ddlDirection);
            this.Controls.Add(new LiteralControl(directionValues.ToString()));
            this.Controls.Add(new LiteralControl("</div>"));
            if(this.PropertyData.Value != null)
                ddlTextBox.Text = this.PropertyData.Value.ToString();
            ddlTextBox.Attributes["id"] = "ddlTextBox" + this.PropertyData.Name;
            ddlTextBox.Attributes["style"] = "display:none;";
            this.Controls.Add(ddlTextBox);
            this.Controls.Add(new LiteralControl("</div>"));
            SetupEditControls();
        }

        protected override void SetupEditControls()
        {

        }

        public override void CreateDefaultControls() {
            string[] rule = RuleSelector.Value.ToString().Split(';');
            if (rule.Length > 1)
            {
                StringBuilder html = new StringBuilder();
                html.Append("<ul");
                html.Append(">");

                PageDataCollection pages = PageHelper.GetPagesRelated(CurrentPage.PageLink, rule[0], (Rule.Direction)Enum.Parse(typeof(Rule.Direction), rule[1]));
                foreach (PageData pd in pages)
                {
                    html.Append(string.Format("<li><a href=\"{0}\">{1}</li>", pd.LinkURL, pd.PageName));
                }
                html.Append("</ul>");
                this.Controls.Add(new LiteralControl(html.ToString()));
            }
        }

        /// <summary>
        /// Gets the RuleSelector instance for this IPropertyControl.
        /// </summary>
        /// <value>The property that is to be displayed or edited.</value>
        public RuleSelector RuleSelector
        {
            get
            {
                return PropertyData as RuleSelector;
            }
        }
        public override void ApplyChanges()
        {
            PropertyData.Value = ddlTextBox.Text;
        }

    }
}
