﻿using System;
using System.Web.UI.WebControls;
using EPiCode.Relations.Core;
using EPiServer.Core;
using EPiCode.Relations.Helpers;

namespace EPiCode.Relations.Admin
{
    public partial class RelationsAdmin : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if(SecurityHelper.IsRelationsAdmin() == false)
                throw new AccessDeniedException();
        }

        public void EditRule(string ruleName)
        {
            CurrentTabTextBox.Text = "edit";
            EditRuleControl.CurrentRule = RuleEngine.Instance.GetRule(ruleName);
            EditRuleControl.SetupEditControls();
            
        }

        public string CurrentTab
        {
            get
            {
                if (string.IsNullOrEmpty(CurrentTabTextBox.Text))
                    CurrentTabTextBox.Text = "overview";
                return CurrentTabTextBox.Text; }

        }

        protected string IsActiveTab(string tab)
        {
            return (tab == CurrentTab) ? "active" : "";

        }

        protected void SetTab_OnCommand(object sender, CommandEventArgs e)
        {
            CurrentTabTextBox.Text = e.CommandArgument.ToString();
            TabsPlaceHolder.DataBind();
        }

        protected void CreateNewRuleButton_OnClick(object sender, EventArgs e)
        {
            CurrentTabTextBox.Text = "edit";
            EditRuleControl.CurrentRule = new Rule();
            EditRuleControl.SetupEditControls();
        }
    }
}