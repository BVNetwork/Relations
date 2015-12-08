using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using EPiCode.Relations.Core;

namespace EPiCode.Relations.Plugins.Admin.Units
{
    public partial class RelationValidator : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected void Validate_Click(object sender, EventArgs e) {
            List<Relation> invalidRelations = new List<Relation>();
            List<Rule> rules = RuleEngine.Instance.GetAllRules().ToList();
            foreach (Rule rule in rules)
            {
                List<Relation> relations = RelationEngine.Instance.GetAllRelations(rule.RuleName);
                foreach (Relation relation in relations)
                {
                    if (Validator.Validate(relation.RuleName, relation.PageIDLeft, relation.PageIDRight, true) != Validator.ValidationResult.Ok)
                        invalidRelations.Add(relation);
                }
            }
            Relations.DataSource = invalidRelations;
            AllOkPanel.Visible = (invalidRelations.Count == 0);
       
            Relations.DataBind();
        
        }

        protected void RemoveInvalid_OnClick(object sender, EventArgs e)
        {
            List<Relation> invalidRelations = new List<Relation>();
            List<Rule> rules = RuleEngine.Instance.GetAllRules().ToList();
            foreach (Rule rule in rules)
            {
                List<Relation> relations = RelationEngine.Instance.GetAllRelations(rule.RuleName);
                foreach (Relation relation in relations)
                {
                    if (Validator.Validate(relation.RuleName, relation.PageIDLeft, relation.PageIDRight, true) != Validator.ValidationResult.Ok)
                        invalidRelations.Add(relation);
                }
            }
            int cnt = 0;
            foreach (Relation invalidRelation in invalidRelations)
            {
                RelationEngine.Instance.DeleteRelation(invalidRelation);
                cnt++;
            }
            AllOkPanel.Visible = true;
            AllOkPanel.Controls.Add(new LiteralControl("<br><br>Removed "+ cnt+" relations! <br><br>"));
        }
    }


}