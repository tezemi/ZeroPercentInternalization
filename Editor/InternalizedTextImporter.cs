using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using Newtonsoft.Json;

namespace ZeroPercentInternalization.Editor
{
    [ScriptedImporter(Version, FileExtension)]
    public class InternalizedTextImporter : ScriptedImporter
    {
	    private const int Version = 1;
	    private const string FileExtension = "zpit";

	    public override void OnImportAsset(AssetImportContext context)
	    {
		    if (context == null)
			    throw new ArgumentNullException(nameof(context));

		    var asset = ScriptableObject.CreateInstance<InternalizedText>();
			string json = File.ReadAllText(context.assetPath);

			// If there is no JSON, this is a new text
		    if (!string.IsNullOrEmpty(json))
		    {
			    try
			    {
					// Can't deserialize scriptable objects directly, so create this temp object first
				    var definition = new { Language = Language.NONE, TextEntries = new List<TextEntry>() };
				    var temp = JsonConvert.DeserializeAnonymousType(json, definition);

				    asset = ScriptableObject.CreateInstance<InternalizedText>();

				    asset.Language = temp.Language;
				    asset.TextEntries = new List<TextEntry>(temp.TextEntries);
			    }
			    catch (Exception e)
			    {
				    context.LogImportError($"Could not read internalized text '{context.assetPath}'. It could be malformed. {Environment.NewLine}{e}");
			    }
		    }

		    context.AddObjectToAsset("<root>", asset, null);
			context.SetMainObject(asset);
	    }

	    [MenuItem("Assets/Create/Internalized Text")]
	    public static void CreateInputAsset()
	    {
		    ProjectWindowUtil.CreateAssetWithContent
		    (
			    "NewInternalizedText." + FileExtension,
				string.Empty
		    );
	    }
	}
}
