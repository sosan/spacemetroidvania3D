using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OptionsManager : MonoBehaviour
{
    int currentLanguage = 0;
    [SerializeField] UILocalization languageUILocalization;


    private void OnEnable()
    {
        UpdateLanguageLabel();
    }

    private void UpdateLanguageLabel()
    {
        languageUILocalization.key = Localization.language;
        languageUILocalization.GetComponent<TMPro.TextMeshProUGUI>().text = Localization.Get(Localization.language);
    }

    public void NextLanguage()
    {
        currentLanguage = Localization.knownLanguages.ToList().IndexOf(Localization.language);

        if (currentLanguage < Localization.knownLanguages.Length - 1)
            currentLanguage++;
        else
            currentLanguage = 0;

        Localization.language = Localization.knownLanguages[currentLanguage];
        UpdateLanguageLabel();
    }

    public void PreviousLanguage()
    {
        currentLanguage = Localization.knownLanguages.ToList().IndexOf(Localization.language);

        if (currentLanguage > 0)
            currentLanguage--;
        else
            currentLanguage = Localization.knownLanguages.Length - 1;

        Localization.language = Localization.knownLanguages[currentLanguage];
        UpdateLanguageLabel();
    }
}
