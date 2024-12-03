using System;
using System.Collections.Generic;
using EPiCode.Relations.Core;
using System.Text;

namespace EPiCode.Relations.Plugins.Admin.Units
{
    public partial class ImportExport : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Export_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = "";
            DataTextBox.Text = "";
            List<Rule> rules = RuleEngine.Instance.GetAllRulesList();
            StringBuilder sb = new StringBuilder();
            foreach (Rule rule in rules)
            {
                if (CheckBoxRules.Checked)
                {
                    sb.Append("Rule;");
                    sb.Append(rule.RuleName + ";");
                    sb.Append(rule.Id + ";");
                    sb.Append(rule.PageTypeLeft + ";");
                    sb.Append(rule.PageTypeRight + ";");
                    sb.Append(rule.RelationHierarchyStartLeft + ";");
                    sb.Append(rule.RelationHierarchyStartRight + ";");
                    sb.Append(rule.RuleTextLeft + ";");
                    sb.Append(rule.RuleTextRight+";");
                    sb.Append(rule.RuleVisibleLeft + ";");
                    sb.Append(rule.RuleVisibleRight + ";");
                    sb.Append(rule.EditModeAccessLevel + ";");
                    sb.Append(rule.SortOrderLeft + ";");
                    sb.Append(rule.SortOrderRight + ";");
                    sb.Append("\n");
                    Status("Exporting rule " + rule.RuleName);
                }
                if (CheckBoxRelations.Checked)
                {
                    List<Relation> relations = RelationEngine.Instance.GetAllRelations(rule.RuleName);
                    int relationCount = 0;
                    foreach (Relation relation in relations)
                    {
                        sb.Append("Relation;");
                        sb.Append(relation.RuleName + ";");
                        sb.Append(relation.PageIDLeft + ";");
                        sb.Append(relation.PageIDRight + ";");
                        sb.Append(relation.LanguageBranch+";");
                        sb.Append("\n");
                        relationCount++;
                    }
                    Status("Exported " + relationCount.ToString() + " relations of rule " + rule.RuleName);
                }

            }


            DataTextBox.Text = sb.ToString();
        }

        protected void Import_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = "";
            int rulesImported = 0;
            int rulesNotImported = 0;
            int relationsImported = 0;
            int relationsNotImported = 0;
            if (DataTextBox.Text.Length > 0)
            {
                string[] rows = DataTextBox.Text.Split('\n');
                if (rows.Length > 0) {
                    Status("Found " + rows.Length.ToString() + " items to import.");
                    foreach (string row in rows) {
                        if (!string.IsNullOrEmpty(row))
                        {
                            string[] columns = row.Split(';');
                            {
                                if (columns.Length == 15 && columns[0] == "Rule" && CheckBoxRules.Checked) {
                                    string status = (ImportRule(columns));
                                    if (!string.IsNullOrEmpty(status))
                                    {
                                        Status(status);
                                        rulesNotImported++;
                                    }
                                    else
                                        rulesImported++;
                                }

                                if (columns.Length == 6 && columns[0] == "Relation" && CheckBoxRelations.Checked) {
                                    string status = (ImportRelation(columns));
                                    if (!string.IsNullOrEmpty(status))
                                    {
                                        Status(status);
                                        relationsNotImported++;
                                    }
                                    else
                                        relationsImported++;
                                }
                                    
                            }
                        }
                        else
                            Status("No data in row");
                    }
                    Status("*** Result ***");
                    Status("Rules imported: " + rulesImported.ToString() + " / "+(rulesImported+rulesNotImported).ToString());
                    Status("Relations imported: " + relationsImported.ToString()+ " / " + (relationsImported+relationsNotImported).ToString());
                }

            }
            else
            {
                Status("No data to import. Aborting.");
            }

        }

        protected string ImportRule(string[] row)
        {
            try
            {
                string name = row[1];

                //int left = int.Parse(row[1]);
                //int right = int.Parse(row[2]);
                string pageTypeLeft = row[3];
                string pageTypeRight = row[4];
                int relationHierarchyStartLeft = int.Parse(row[5]);
                int relationHierarchyStartRight = int.Parse(row[6]);
                string ruleTextLeft = row[7];
                string ruleTextRight = row[8];
                bool ruleVisibleLeft = bool.Parse(row[9]);
                bool ruleVisibleRight = bool.Parse(row[10]);
                string ruleEditModeAccessLevel = row[11];
                int ruleSortOrderLeft = int.Parse(row[12]);
                int ruleSortOrderRight = int.Parse(row[13]);

                Rule newRule;
                if (RuleEngine.Instance.RuleExists(name))
                {
                    newRule = RuleEngine.Instance.GetRule(name);
                    Status("Updating existing rule: " + name);
                }
                else
                {
                    newRule = RuleEngine.Instance.AddNewRule(name, pageTypeLeft, pageTypeRight, ruleTextLeft, ruleTextRight);
                    Status("Adding new rule: " + name);
                }
                newRule.PageTypeLeft = pageTypeLeft;
                newRule.PageTypeRight = pageTypeRight;
                newRule.RelationHierarchyStartLeft = relationHierarchyStartLeft;
                newRule.RelationHierarchyStartRight = relationHierarchyStartRight;
                newRule.RuleTextLeft = ruleTextLeft;
                newRule.RuleTextRight = ruleTextRight;
                newRule.RuleVisibleLeft = ruleVisibleLeft;
                newRule.RuleVisibleRight = ruleVisibleRight;
                newRule.EditModeAccessLevel = ruleEditModeAccessLevel;
                newRule.SortOrderLeft = ruleSortOrderLeft;
                newRule.SortOrderRight = ruleSortOrderRight;

                RuleEngine.Instance.Save(newRule);
                return string.Empty;
            }
            catch (Exception e){
                return "Something went wrong: " + e.Message;
            }
            //Validator.ValidationResult validation = Validator.Validate(name, left, right);
            //if (validation == Validator.ValidationResult.Ok)
            //{
            //    RelationEngine.AddRelation(name, left, right);
            //    return string.Empty;
            //}
        }


        protected string ImportRelation(string[] row) {
            string name = row[1];
            int left = int.Parse(row[2]);
            int right = int.Parse(row[3]);

            Validator.ValidationResult validation =  Validator.Validate(name, left, right);
            if (validation == Validator.ValidationResult.Ok)
            {
                RelationEngine.Instance.AddRelation(name, left, right);
                return string.Empty;
            }
            return ("Could not import relation: " + validation.ToString());
        }


        protected void Status(string text)
        {
            StatusLabel.Text += text + "<br/>";
        }

        //protected void Export_Click(object sender, EventArgs e)
        //{
        //    Response.Clear();
        //    Response.AddHeader("content-disposition", "attachment; filename=testfile.csv");
        //    Response.AddHeader("content-type", "text/csv");

        //    List<Rule> rules = RuleEngine.GetAllRulesList();

        //    StreamWriter writer = new StreamWriter(Response.OutputStream);
        //    foreach (Rule rule in rules)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append(rule.RuleName + ";");
        //        sb.Append(rule.Id + ";");
        //        sb.Append(rule.PageTypeLeft + ";");
        //        sb.Append(rule.PageTypeRight + ";");
        //        sb.Append(rule.RelationHierarchyStartLeft + ";");
        //        sb.Append(rule.RelationHierarchyStartRight + ";");
        //        writer.WriteLine(sb.ToString());
        //    }
        //    writer.Close();
        //    Response.End();
        //}

    }
}