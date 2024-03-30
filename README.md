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

# Getting Started
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

Now that there's two values for english, you can add as many languages as desired. Click on the down arrow again, and select another language code.

![image](https://github.com/tezemi/ZeroPercentInternalization/assets/59236027/da78527b-b65f-42a5-a8d3-c7bc275c452a)

In this example, I've added French and Japanese, and they can now be selected alongside English. The keys are the same across every language, but you'll need to specify the new values.

![image](https://github.com/tezemi/ZeroPercentInternalization/assets/59236027/a7ab726d-d1d8-4872-b196-7afcf0bf33a3)

To delete a language, you can click the minus sign to the right of the language field.

## Referencing Internalized Text
There are two ways to reference the text stored in a zpit. You make either use the LocalizedText component on a UI text, or use the LocalizedString type.

### LocalizedText
More to come soon 😉
