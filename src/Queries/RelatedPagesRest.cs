using System;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Runtime.Remoting.Messaging;
using EPiCode.Relations.Core;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.Services.Rest;
using System.Collections.Generic;
using System.Linq;

namespace EPiCode.Relations.EditorDescriptors
{
    [RestStore("relations")]
    public class RelatedPagesRest : RestControllerBase
    {
        public RestResult Get(string page)
        {
            var matches = EPiServer.DataFactory.Instance.GetChildren(PageReference.StartPage);
            return Rest(matches.Select(m => new { Name = m.Name, Id = m.ContentLink.ID }));
        }
    }

    public class RuleDescription
    {
        public string RuleName;
        public string RuleDirection;
        public string RuleDesc;
        public string RuleId;
        public string RuleGuid;
    }

    [RestStore("rules")]
    public class RulesRest : RestControllerBase
    {
        public RestResult Get(int? id, ItemRange range)
        {
            if (id.HasValue)
            {
                ContentReference currentContentRef = new PageReference(id.Value);
                if (new ContentReference(id.Value) != null && EPiServer.DataFactory.Instance.Get<IContent>(new ContentReference(id.Value)) as PageData != null)
                {
                    PageData currentPage = EPiServer.DataFactory.Instance.GetPage(new PageReference(id.Value));
                    var pageId = currentPage.ContentLink.ID;
                    var _rules = new List<RuleDescription>();

                    List<Rule> _rulesLeft = RuleEngine.Instance.GetRulesLeft(pageId) as List<Rule>;
                    List<Rule> _rulesRight = RuleEngine.Instance.GetRulesRight(pageId) as List<Rule>;

                    AccessControlList list = currentPage.ACL;

                    foreach (Rule leftRule in _rulesLeft)
                    {
                        RuleDescription rd = new RuleDescription() { RuleName = leftRule.RuleTextLeft, RuleDesc = leftRule.RuleDescriptionLeft, RuleDirection = "left", RuleGuid = leftRule.Id.ExternalId.ToString(), RuleId = leftRule.RuleName };
                        if (HasAccess(list, leftRule.EditModeAccessLevel) && leftRule.RuleVisibleLeft)
                            _rules.Add(rd);
                    }



                    foreach (Rule rightRule in _rulesRight)
                    {
                        RuleDescription rd = new RuleDescription() { RuleName = rightRule.RuleTextRight, RuleDesc = rightRule.RuleDescriptionRight, RuleDirection = "right", RuleGuid = rightRule.Id.ExternalId.ToString(), RuleId = rightRule.RuleName };
                        if (HasAccess(list, rightRule.EditModeAccessLevel) && rightRule.RuleVisibleRight && rightRule.RuleTextLeft != rightRule.RuleTextRight)
                            _rules.Add(rd);
                    }

                    /*

                    List<RuleDescription> result = new List<RuleDescription>();
                    int cnt = 0;

                    foreach (Rule rule in _rules)
                    {
                        bool isLeftRule = (RuleEngine.Instance.IsLeftRule(pageId, RuleEngine.Instance.GetRule(rule.RuleName)));
                        if ((from p in result where p.RuleName == rule.RuleName select p).Any())
                            result.Add(new RuleDescription() { RuleGuid = rule.Id.ExternalId.ToString(), RuleId = rule.RuleName, RuleDirection = "left", RuleName = rule.RuleTextLeft + "2", RuleDesc = (RuleEngine.Instance.IsLeftRule(pageId, RuleEngine.Instance.GetRule(rule.RuleName))) ? rule.RuleDescriptionLeft : rule.RuleDescriptionRight });
                        else
                            result.Add(new RuleDescription() { RuleGuid = rule.Id.ExternalId.ToString(), RuleId = rule.RuleName, RuleDirection = "left", RuleName = rule.RuleTextLeft, RuleDesc = (RuleEngine.Instance.IsLeftRule(pageId, RuleEngine.Instance.GetRule(rule.RuleName))) ? rule.RuleDescriptionLeft : rule.RuleDescriptionRight });
                        cnt++;
                    }

                    foreach (Rule rule in _rulesRight)
                    {
                        bool isLeftRule = (RuleEngine.Instance.IsLeftRule(pageId, RuleEngine.Instance.GetRule(rule.RuleName)));
                        if ((from p in result where p.RuleName == rule.RuleName select p).Any())
                            result.Add(new RuleDescription() { RuleGuid = rule.Id.ExternalId.ToString(), RuleId = rule.RuleName, RuleDirection = "right", RuleName = rule.RuleTextRight + "2", RuleDesc = (RuleEngine.Instance.IsLeftRule(pageId, RuleEngine.Instance.GetRule(rule.RuleName))) ? rule.RuleDescriptionLeft : rule.RuleDescriptionRight });
                        else
                            result.Add(new RuleDescription() { RuleGuid = rule.Id.ExternalId.ToString(), RuleId = rule.RuleName, RuleDirection = "right", RuleName = rule.RuleTextRight, RuleDesc = (RuleEngine.Instance.IsLeftRule(pageId, RuleEngine.Instance.GetRule(rule.RuleName))) ? rule.RuleDescriptionLeft : rule.RuleDescriptionRight });
                        cnt++;
                    }
                    */
                    return Rest(_rules.Select(m => new { Guid = m.RuleGuid, Name = TryTranslate( m.RuleName), Id = m.RuleId, Description = TryTranslate(m.RuleDesc), Direction = m.RuleDirection }));
                }
            }
            return null;
        }

        private bool HasAccess(AccessControlList list, string editModeAccessLevel)
        {
            AccessLevel al;
            if (AccessLevel.TryParse(editModeAccessLevel, true, out al))
                return list.QueryDistinctAccess(al);
            return true;

        }

        private string TryTranslate(string ruleName)
        {
            string result = TranslationHelper.Translate("/relations/rules/" + ruleName);
            if (string.IsNullOrEmpty(result) || result.StartsWith("["))
                result = ruleName;
            return result;
        }
    }



    [RestStore("overview")]
    public class OverviewRest : RestControllerBase
    {
        public RestResult Get(int? id, ItemRange range)
        {
            if (id.HasValue)
            {
                string result = "";
                PageData currentPage = EPiServer.DataFactory.Instance.GetPage(new PageReference(id.Value));
                var pageId = currentPage.ContentLink.ID;
                var _rules = new List<Rule>();
                int cnt = 0;

                List<Rule> _rulesLeft = RuleEngine.Instance.GetRulesLeft(pageId) as List<Rule>;
                List<Rule> _rulesRight = RuleEngine.Instance.GetRulesRight(pageId) as List<Rule>;

                foreach (Rule rule in _rulesLeft)
                {
                    if (rule.RuleTextLeft != rule.RuleTextRight)
                    {
                        result += "<b>" + rule.RuleTextLeft + "</b><br/>";
                        List<int> relations = RelationEngine.Instance.GetRelationPagesForPage(pageId, rule);
                        foreach (int pgid in relations)
                        {
                            result += "- " + EPiServer.DataFactory.Instance.GetPage(new PageReference(pgid)).Name +
                                      "<br/>";
                            cnt++;
                        }

                    }
                }

                foreach (Rule rule in _rulesRight)
                {
                    result += "<b>" + rule.RuleTextRight + "</b><br/>";
                    List<int> relations = RelationEngine.Instance.GetRelationPagesForPage(pageId, rule);
                    foreach (int pgid in relations)
                    {
                        result += "- " + EPiServer.DataFactory.Instance.GetPage(new PageReference(pgid)).Name + "<br/>";
                        cnt++;
                    }
                }
                result = "Number of relations: " + cnt + "<br/><br/>" + result;

                return Rest(result);

            }
            return null;
        }
    }

    [RestStore("relationremove")]
        public class RemoveRelationRest : RestControllerBase
        {


            public RestResult Get(int? id, ItemRange range)
            {
                // When requesting a specific item we return that item; otherwise return all items.

                return Rest("Hello world!");
            }



            public RestResult Post(string ruleName, string relationPageLeftString, string relationPageRightString, string ruleDirection)
            {
                int relationPageLeft = 0;
                int relationPageRight = 0;
                bool isLeftRule = true;
                string relationRule = "";

                //string[] newRelationValues = (newRelation.ToString()).Split(';');
                try
                {
                    relationRule = ruleName;
                    relationPageLeft = int.Parse(relationPageLeftString.Split('_')[0]);
                    relationPageRight = EPiServer.Web.PermanentLinkUtility.FindContentReference(
                        EPiServer.Web.PermanentLinkUtility.GetGuid(relationPageRightString)).ID;
                    isLeftRule = ruleDirection == "left";
                }
                catch (Exception e)
                {
                    return Rest(string.Format(TranslationHelper.Translate("/relations/edit/couldnotparse"), e.Message));
                }

                PageReference contextPage = new PageReference(relationPageLeft);
                if (contextPage != null && relationPageRight != null && relationPageLeft != null)

                    try
                    {
                        if (isLeftRule)
                        {
                            Relation rel = RelationEngine.Instance.GetRelation(relationRule,
                                relationPageLeft, relationPageRight);
                            RelationEngine.Instance.DeleteRelation(rel);
                        }
                        else
                        {
                            Relation rel = RelationEngine.Instance.GetRelation(relationRule,
                                relationPageRight, relationPageLeft);
                            RelationEngine.Instance.DeleteRelation(rel);

                        }
                    }
                    catch { }


                return Rest(TranslationHelper.Translate("/relations/edit/removed"));
            }


        }

        [RestStore("relationadd")]
        public class AddRelationRest : RestControllerBase
        {


            public RestResult Get(int? id, ItemRange range)
            {
                // When requesting a specific item we return that item; otherwise return all items.

                return Rest("Hello world!");
            }

            public RestResult Post(string ruleName, string relationPageLeftString, string relationPageRightString, string ruleDirection)
            {
                int relationPageLeft = 0;
                int relationPageRight = 0;
                bool isLeftRule = true;
                string relationRule = "";

                //string[] newRelationValues = (newRelation.ToString()).Split(';');
                try
                {
                    relationRule = ruleName;
                    relationPageLeft = int.Parse(relationPageLeftString.Split('_')[0]);
                    relationPageRight = EPiServer.Web.PermanentLinkUtility.FindContentReference(
                        EPiServer.Web.PermanentLinkUtility.GetGuid(relationPageRightString)).ID;
                    isLeftRule = ruleDirection == "left";
                }
                catch (Exception e)
                {
                    return Rest("Could not parse relation: " + e.Message);
                }

                PageReference contextPage = new PageReference(relationPageLeft);
                if (contextPage != null && relationPageRight != null && relationPageLeft != null)

                    try
                    {
                        if (isLeftRule)
                        {
                            Validator.ValidationResult validation = Validator.Validate(relationRule, contextPage.ID,
                                relationPageRight);
                            if (validation == Validator.ValidationResult.Ok)
                                RelationEngine.Instance.AddRelation(relationRule, contextPage.ID,
                                    relationPageRight);
                            else
                            {
                                string errorMessage = TranslationHelper.GetValidationText(validation);

                                return Rest(TranslationHelper.Translate("/relations/edit/error") + errorMessage);
                            }
                        }
                        else
                        {
                            Validator.ValidationResult validation = Validator.Validate(relationRule, relationPageRight,
                                contextPage.ID);
                            if (validation == Validator.ValidationResult.Ok)
                                RelationEngine.Instance.AddRelation(relationRule, relationPageRight,
                                    contextPage.ID);
                            else
                            {
                                string errorMessage = TranslationHelper.GetValidationText(validation);

                                return Rest(TranslationHelper.Translate("/relations/edit/error") + errorMessage);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        return Rest(TranslationHelper.Translate("/relations/edit/error") + e.Message);
                        
                    }


                return Rest(TranslationHelper.Translate("/relations/edit/added"));
            }


        }



        public static class TranslationHelper
        {
            public static string Translate(string xpath)
            {
                return ServiceLocator.Current.GetInstance<LocalizationService>().GetString(xpath);
            }
            public static string GetValidationText(Validator.ValidationResult validation)
            {
                string errorMessage = "";
                switch (validation)
                {
                    case Validator.ValidationResult.AlreadyExists:
                        errorMessage = Translate("/relations/edit/alreadyexists");
                        break;
                    case Validator.ValidationResult.NotAllowed:
                        errorMessage = Translate("/relations/edit/notallowed");
                        break;
                    default:
                        errorMessage = validation.ToString();
                        break;
                }
                return errorMessage;

            }
        }



}