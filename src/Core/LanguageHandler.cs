using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;

namespace EPiCode.Relations.Core
{
    public static class LanguageHandler
    {

        public static string GetErrorMessage(string errorKey)
        {
            return Translate("/epicode.relations/error/" + errorKey.ToString());
        }

        public static string Translate(string key)
        {
            return ServiceLocator.Current.GetInstance<LocalizationService>().GetString(key, string.Empty);
        }

        public static string Translate(string key, string fallback) {
            string text = Translate(key);
            if (string.IsNullOrEmpty(text))
                text = fallback;
            return text ?? string.Empty;
        }

        public static string TranslateRuleName(Rule rule)
        {
            return LanguageHandler.Translate("/epicode.relations/rules/" + rule.RuleName + "/name", rule.RuleName);
        }

        public static string TranslateRuleTextLeft(Rule rule)
        {
            return LanguageHandler.Translate("/epicode.relations/rules/" + rule.RuleName + "/left/text", rule.RuleTextLeft);
        }

        public static string TranslateRuleTextRight(Rule rule)
        {
            return LanguageHandler.Translate("/epicode.relations/rules/" + rule.RuleName + "/right/text", rule.RuleTextRight);
        }

        public static string TranslateRuleDescriptionLeft(Rule rule)
        {
            return LanguageHandler.Translate("/epicode.relations/rules/" + rule.RuleName + "/left/description", rule.RuleDescriptionLeft);
        }

        public static string TranslateRuleDescriptionRight(Rule rule)
        {
            return LanguageHandler.Translate("/epicode.relations/rules/" + rule.RuleName + "/right/description", rule.RuleDescriptionRight);
        }
    }
}