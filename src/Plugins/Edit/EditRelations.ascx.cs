using System;
using System.Collections.Generic;
using EPiServer.PlugIn;
using EPiCode.Relations.Core;

namespace EPiCode.Relations.Plugins.Edit
{
    [GuiPlugIn(DisplayName = "Relations ", Description = "Edit relations related to this page", Area = PlugInArea.EditPanel, Url = "~/EPiCode/Relations/Plugins/Edit/EditRelations.ascx", LanguagePath="/epicode.relations/editPlugin/relations")]
    public partial class EditRelations : EPiServer.UserControlBase, ICustomPlugInLoader
    {
        List<Rule> _rules;
        List<Rule> Rules
        {
            get
            {
                if (_rules == null)
                {
                    _rules = new List<Rule>();

                    List<Rule> _rulesLeft = RuleEngine.Instance.GetRulesLeft(CurrentPage.PageLink.ID) as List<Rule>;
                    List<Rule> _rulesRight = RuleEngine.Instance.GetRulesRight(CurrentPage.PageLink.ID) as List<Rule>;

                    foreach (Rule leftRule in _rulesLeft) {
                        if (leftRule.RuleTextLeft != leftRule.RuleTextRight) {
                            _rules.Add(leftRule);
                        }
                    }

                    _rules.AddRange(_rulesRight);

                }

                return _rules;
            }
        }


        public PlugInDescriptor[] List()
        {
            PlugInDescriptor[] descriptors = null;
            if (Rules.Count > 0)
            {
                descriptors = new PlugInDescriptor[1];
                descriptors[0] = PlugInDescriptor.Load(this.GetType());
            }
            return descriptors;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Rules.Count > 0)
                {
                    RulesRepeater.DataSource = Rules;
                    RulesRepeater.DataBind();
                }
                else
                {
                    ErrorMessageContainer.Visible = true;
                    ErrorMessageContainer.DataBind();
                }
            }
            base.OnLoad(e);
        }

        protected Rule GetRule(string ruleName)
        {
            return RuleEngine.Instance.GetRule(ruleName);
        }

    }
}