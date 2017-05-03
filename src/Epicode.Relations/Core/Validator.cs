using System.Web;
using EPiServer.Core;

namespace EPiCode.Relations.Core
{
    public class Validator
    {
        public enum ValidationResult { 
            Ok,
            PageNotFound,
            NotAllowed,
            AlreadyExists,
            RuleNotFound
        }

        public static ValidationResult Validate(string ruleName, int pageLeft, int pageRight)
        {
            return Validate(ruleName, pageLeft, pageRight, false);
        }

        public static ValidationResult Validate(string ruleName, int pageLeft, int pageRight, bool validateExisting)
        {
            PageData pageDataLeft = PageEngine.GetPage(pageLeft);
            PageData pageDataRight = PageEngine.GetPage(pageRight);
            Rule rule = RuleEngine.Instance.GetRule(ruleName);
            if (rule == null || string.IsNullOrEmpty(rule.PageTypeLeft) || string.IsNullOrEmpty(rule.PageTypeRight))
                return ValidationResult.RuleNotFound;
            if (pageDataLeft == null || pageDataRight == null)
                return ValidationResult.PageNotFound;
            if(validateExisting == false)
                if (RelationEngine.Instance.RelationExists(ruleName, pageLeft, pageRight))
                    return ValidationResult.AlreadyExists;
            if (RuleValidation(rule, pageDataRight, pageDataLeft) == false)
                return ValidationResult.NotAllowed;
            return ValidationResult.Ok;
        }


        private static bool RuleValidation(Rule rule, PageData pageLeft, PageData pageRight) {
            return (RuleSideValidation(pageLeft, rule, false) && RuleSideValidation(pageRight, rule, true));
        }

        private static bool RuleSideValidation(PageData pageToValidate, Rule rule, bool validateLeftSide) {
            if (ValidatePageType(rule, pageToValidate.PageTypeName, validateLeftSide))
            {
                int startPage = (validateLeftSide ? rule.RelationHierarchyStartLeft : rule.RelationHierarchyStartRight);
                if (startPage < 1 || IsDescendent(pageToValidate.PageLink.ID, startPage))
                    return true;
            }
            return false;
        }

        private static bool IsDescendent(int pageID, int startID) {
            return RuleEngine.Instance.IsDescendent(pageID, startID);
        }

        private static bool ValidatePageType(Rule rule, string pageTypeName, bool validateLeftSide) {
            string pageTypes = HttpUtility.UrlDecode(validateLeftSide ? rule.PageTypeLeft : rule.PageTypeRight);
            return (pageTypes.Contains(pageTypeName));
        }

        private static bool ValidateRelationExists(string rule, int pageLeft, int pageRight) {
            return RelationEngine.Instance.RelationExists(rule, pageLeft, pageRight);
        }


    }
}