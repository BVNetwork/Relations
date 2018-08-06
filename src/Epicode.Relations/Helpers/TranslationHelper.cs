using EPiCode.Relations.Core;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;

namespace EPiCode.Relations.Helpers
{
    public static class TranslationHelper
    {
        public static string Translate(string xpath)
        {
            return ServiceLocator.Current.GetInstance<LocalizationService>().GetString(xpath);
        }

        public static string Translate(string xpath, string fallback)
        {
            return ServiceLocator.Current.GetInstance<LocalizationService>().GetString(xpath, fallback);
        }

        public static string GetValidationText(Validator.ValidationResult validation)
        {
            string errorMessage;

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