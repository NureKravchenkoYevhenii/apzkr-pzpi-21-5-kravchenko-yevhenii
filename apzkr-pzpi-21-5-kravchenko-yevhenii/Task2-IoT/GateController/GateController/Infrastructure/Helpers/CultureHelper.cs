using System.Globalization;

namespace GateController.Infrastructure.Helpers;
public static class CultureHelper
{
    public static void ChangeCulture(
        int cultureIndex)
    {
        string newLanguage = cultureIndex switch
        {
            1 => "en-US",
            _ => "uk-UA",
        };
        var newCulture = CultureInfo.GetCultureInfo(newLanguage);

        Thread.CurrentThread.CurrentCulture = newCulture;
        Thread.CurrentThread.CurrentUICulture = newCulture;
    }
}
