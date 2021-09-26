using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


//original code copied from Kerbalism: https://github.com/Kerbalism/Kerbalism
namespace LRTR
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
				Debug.LogError("[LRTRCONFIG] Could not find LRTRCONFIG node.");
				return;
			}

			// get configs from DB
			UrlDir.UrlFile root = null;
			foreach (UrlDir.UrlConfig url in GameDatabase.Instance.root.AllConfigs) { root = url.parent; break; }

			// get mod directories
			UrlDir gameData = GameDatabase.Instance.root.children.First(dir => dir.type == UrlDir.DirectoryType.GameData);
				
			foreach (ConfigNode feature in paramsNode.GetNodes())
			{
				bool enableFeature = false;
				if (feature.GetValue("enabled") != null)
				{
					enableFeature = feature.GetValue("enabled").ToLower() == "true";
				}
				string[] disabledBy = feature.GetValues("disabledBy");

				if(enableFeature)
                {
					foreach (string modName in disabledBy)
					{
						if(String.Compare(modName.Substring(0,1),"!", true)==0)
                        {
							enableFeature = false;
							foreach(UrlDir subDir in gameData.children)
                            {
								if(String.Compare(modName.Substring(1),subDir.name,true)==0)
                                {
									enableFeature = true;
                                }
                            }
							if (!enableFeature)
								Debug.Log("[LRTRCONFIG] " + feature.name + " disabled without " + modName.Substring(1));
						}
						else
                        {
							foreach(UrlDir subDir in gameData.children)
                            {
								if (String.Compare(modName, subDir.name, true) == 0)
								{
									enableFeature = false;
									Debug.Log("[LRTRCONFIG] " + feature.name + " disabled by " + modName);
								}
							}
                        }
					}
				}

				if (enableFeature)
				{
					Inject(root, "LRTR", feature.name);
				}
			}
		}

		// inject an MM patch on-the-fly, so that NEEDS[TypeId] can be used in MM patches
		static void Inject(UrlDir.UrlFile root, string type, string id)
		{
			Debug.Log("[LRTRCONFIG] " +id+ " enabled");
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
