using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using KSP.UI.Screens;

namespace LRTR.Utilities
{

	public static class MM40Injector
	{
		private static List<string> injectors = new List<string>();

		public static void AddInjector(string type, string id)
		{
			injectors.Add(type + id);
		}

		public static IEnumerable<string> ModuleManagerAddToModList()
		{
			return injectors;
		}
	}

	// the name is chosen so that the awake method is called after ModuleManager,
	// this is necessary because MM injects its loader at index 1, so we need to inject
	// our own after it, at index 1 (so that it runs just before MM)
	[KSPAddon(KSPAddon.Startup.Instantly, false)]
	public sealed class Loader : MonoBehaviour
	{
		public void Start()
		{
			PartResourceLibrary.Instance.LoadDefinitions();

			ConfigNode paramsNode = null;

            foreach (ConfigNode n in GameDatabase.Instance.GetConfigNodes("LRTRCONFIG"))
				paramsNode = n;

			if (paramsNode == null)
			{
				Debug.LogError("[LRTR]: Could not find LRTRCONFIG node.");
				return;
			}
			
			// get configs from DB
			UrlDir.UrlFile root = null;
			foreach (UrlDir.UrlConfig url in GameDatabase.Instance.root.AllConfigs) { root = url.parent; break; }

            // inject MM patches on-the-fly, so that profile/features can be queried with NEEDS[]
            if (paramsNode.GetValue("Contracts").ToLower() == "true")
			    Inject(root, "LRTR", "Contracts");

			if (paramsNode.GetValue("TechTree").ToLower() == "true")
				Inject(root, "LRTR", "TechTree");

			if (paramsNode.GetValue("Science").ToLower() == "true")
				Inject(root, "LRTR", "Science");

			if (paramsNode.GetValue("Rescale").ToLower() == "true")
				Inject(root, "LRTR", "Rescale");

			if (paramsNode.GetValue("CustomBarnKit").ToLower() == "true")
				Inject(root, "LRTR", "CustomBarnKit");
	    }

		// inject an MM patch on-the-fly, so that NEEDS[TypeId] can be used in MM patches
		static void Inject(UrlDir.UrlFile root, string type, string id)
		{
			Debug.LogError("[LRTRCONFIG] Injecting "+type+id);
			if (ModuleManager.MM_major >= 4)
			{
				MM40Injector.AddInjector(type, id);
			}
			else
			{
				root.configs.Add(new UrlDir.UrlConfig(root, new ConfigNode("@LRTR:FOR["+type+id+"]")));
			}
		}
	}

}
