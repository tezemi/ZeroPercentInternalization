# Zero Percent Internalization
Zero Percent Internalization is a lightweight text internalization/localization library for Unity. ZPI stores text files as Unity-tracked assets using a key-value pair system for looking up text based on the configured language. These assets are stored as human-readable JSON so translators may make changes without Unity or Unity experience.

# Installation
The easiest way to install ZPI is to install it as a package.
1. Window -> Package Manager
2. Click the "+" button.
3. Select "Add package from git URL."
4. Enter the following URL: `https://github.com/tezemi/ZeroPercentInternalization.git`
5. Click "Add."

# Dependencies
- ZPI requires Unity UI and TextMeshPro, these can be downloaded through the Unity Package Manager, and should be downloaded automatically.
- ZPI also requires [JSON.NET](https://www.newtonsoft.com/json). This will **not be downloaded automatically, and must be installed manually**. How you decide to include it in your project is up to you, although I recommend [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity).

# Usage
## Creating an Internalized Text Asset
ZPI works by making use of assets called "internalized texts," saved locally as ".zpit" (pronounced "zip-it" 😉). You can create one using the context menu.

![image](https://github.com/tezemi/ZeroPercentInternalization/assets/59236027/571f228c-71ec-4112-a701-70eb9bd36e71)

When you create a new zpit, it will not have any languages. To add one, click the arrow next to the language field, then select the language code of the language you wish to add.

![image](https://github.com/tezemi/ZeroPercentInternalization/assets/59236027/29a01710-7e02-4f27-8c07-9de3e458d735)

So for example, if you are adding English text, you would select the "EN" code.

Once the language has been added, you may start adding keys and values by clicking the plus icon.
- Keys are what the game uses to reference text strings.
- Values are what will actually be displayed.

Here's an example:

![image](https://github.com/tezemi/ZeroPercentInternalization/assets/59236027/08f1ef66-3645-466c-80a3-1b9072bfe473)

Click the minus button to remove a key if needed.

Now that there's two values for English, you can add as many languages as desired. Click on the down arrow again, and select another language code.

![image](https://github.com/tezemi/ZeroPercentInternalization/assets/59236027/da78527b-b65f-42a5-a8d3-c7bc275c452a)

In this example, I've added French and Japanese, and they can now be selected alongside English. The keys are the same across every language, but you'll need to specify the new values.

![image](https://github.com/tezemi/ZeroPercentInternalization/assets/59236027/a7ab726d-d1d8-4872-b196-7afcf0bf33a3)

To delete a language, you can click the minus sign to the right of the language field.

## Referencing Internalized Text
There are two ways to reference the text stored in a zpit. You may either use the Localized Text component on a UI text, or use the LocalizedString type.

### Localized Text
This is a component that can be attached to a Unity UI Text component. On the game object, click Add Component > Zero Percent Internalization > Localized Text.

![image](https://github.com/tezemi/ZeroPercentInternalization/assets/59236027/298c2673-6999-4994-8232-561120db478c)

Assign the internalized text you want to reference to the first field, then select a key from the dropdown. The UI text will automatically be assigned to the value the key references. It will update automatically if you change the value.

![GIF](https://github.com/tezemi/ZeroPercentInternalization/assets/59236027/1b53c456-ce13-408f-ba7b-bf503248c89f)

For TextMeshPro components, simply use the "Localized Text (TMP)," which functions exactly the same.

### LocalizedString
LocalizedString is a type that can be used as a Unity property on your components. It allows you to assign an internalized text and key, but otherwise acts like a normal string.

![image](https://github.com/tezemi/ZeroPercentInternalization/assets/59236027/b50bef29-2322-4912-9307-afa8d44fee76)
![image](https://github.com/tezemi/ZeroPercentInternalization/assets/59236027/54cd6018-a6b9-454c-bfe1-12d340f7f3eb)

In this example, the four LocalizedString fields can be assigned in the editor, but used like a string:

![image](https://github.com/tezemi/ZeroPercentInternalization/assets/59236027/2f9c2303-322a-4c32-b8b0-a51c82e26e75)
![image](https://github.com/tezemi/ZeroPercentInternalization/assets/59236027/6e3673f0-3b2a-4153-9105-a1b315febe76)

## Changing the Language
Changing the language during runtime is simple. The current language is stored as a PlayerPref, however, you'll want to use the configuration class to assign a new language.
```CS
ZeroPercentInternalizationConfiguration.Language = Language.FR;
```
This line will change the language to French. It will also update all Localized Texts and LocalizedStrings. Next time the game starts, the language will automatically be set to the new language.

![GIF2](https://github.com/tezemi/ZeroPercentInternalization/assets/59236027/46430e92-74e7-48c1-9b51-61f56ecbc7e8)

## Changing the Default Language
The default language is English, so if you want the default language to be English, you won't need to change anything. If you want a different language, however, the method is fairly straightforward. When your game starts, run the following code:
```CS
if (!PlayerPrefs.HasKey("ZeroPercentInternalizationConfiguration.Language"))
{
  ZeroPercentInternalizationConfiguration.Language = Language.JA;
}
```
This will check to see if the language key has been set, if not, it will set it to the new language, effectively changing the default language for the game. Where you choose to do this is up to you, but a method that gets run once when the game starts is best, for example:
```CS
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
private static void Startup()
{
  // ...
}
```

## Editing Text Directly (JSON)
If you or someone on your team doesn't want to use Unity, you can .zpit files directly. This is what one looks like:
```JSON
{
  "Languages": [
    32,
    50
  ],
  "_languageMaps": [
    {
      "Language": "EN",
      "TextEntries": [
        {
          "Key": "NewKey0",
          "Value": "Hello"
        },
        {
          "Key": "NewKey1",
          "Value": "World!"
        }
      ]
    },
    {
      "Language": "FR",
      "TextEntries": [
        {
          "Key": "NewKey0",
          "Value": "Hello in French"
        },
        {
          "Key": "NewKey1",
          "Value": "World! in French"
        }
      ]
    }
  ],
  "name": "NewInternalizedText",
  "hideFlags": 8
}
```
Most of these properties are not important when it comes to translating. You'll only need to change what's inside the "_languageMaps" array. Each element will start with a "Language" property, followed by the language code. Just like with editing in Unity, the "keys" need to stay the same across all languages, but the values are what will be changed per language.

## Language Code List
```CS
AF,
SQ,
AR_DZ,
AR_BH,
AR_EG,
AR_IQ,
AR_JO,
AR_KW,
AR_LB,
AR_LY,
AR_MA,
AR_OM,
AR_QA,
AR_SA,
AR_SY,
AR_TN,
AR_AE,
AR_YE,
EU,
BE,
BG,
CA,
ZH_HK,
ZH_CN,
ZH_SG,
ZH_TW,
HR,
CS,
DA,
NL_BE,
NL,
EN,
EN_AU,
EN_BZ,
EN_CA,
EN_IE,
EN_JM,
EN_NZ,
EN_ZA,
EN_TT,
EN_GB,
EN_US,
ET,
FO,
FA,
FI,
FR_BE,
FR_CA,
FR_LU,
FR,
FR_CH,
GD,
DE_AT,
DE_LI,
DE_LU,
DE,
DE_CH,
EL,
HE,
HI,
HU,
IS,
ID,
GA,
IT,
IT_CH,
JA,
KO,
KU,
LV,
LT,
MK,
ML,
MS,
MT,
NO,
NB,
NN,
PL,
PT_BR,
PT,
PA,
RM,
RO,
RO_MD,
RU,
RU_MD,
SR,
SK,
SL,
SB,
ES_AR,
ES_BO,
ES_CL,
ES_CO,
ES_CR,
ES_DO,
ES_EC,
ES_SV,
ES_GT,
ES_HN,
ES_MX,
ES_NI,
ES_PA,
ES_PY,
ES_PE,
ES_PR,
ES,
ES_UY,
ES_VE,
SV,
SV_FI,
TH,
TS,
TN,
TR,
UK,
UR,
VE,
VI,
CY,
XH,
JI,
ZU
```



