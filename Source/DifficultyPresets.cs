using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using KSP;

namespace LRTR
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class DifficultyPresetChanger : MonoBehaviour
    {
        public void Awake()
        {
            ConfigNode paramsNode = null;
            foreach (ConfigNode n in GameDatabase.Instance.GetConfigNodes("GAMEPARAMETERS"))
                paramsNode = n;

            if (paramsNode == null)
            {
                Debug.LogError("[LRTR]: Could not find GAMEPARAMETERS node.");
                return;
            }

            GameParameters.SetDifficultyPresets();

            foreach (KeyValuePair<GameParameters.Preset, GameParameters> kvp in GameParameters.DifficultyPresets)
            {
                ConfigNode n = paramsNode.GetNode(kvp.Key.ToString());
                if (n != null)
                    kvp.Value.Load(n);
            }

            Debug.Log("[LRTR]: Reset difficulty presets.");
        }
    }

    public class LRTRSettings : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "General Settings"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "LRTR"; } }
        public override string DisplaySection { get { return "LRTR"; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomParameterUI("Crews require training")]
        public bool IsTrainingEnabled = true;

        [GameParameters.CustomParameterUI("Enable crew retirement", toolTip = "Re-enabling this option can cause some of the older crewmembers to instantly retire.")]
        public bool IsRetirementEnabled = true;

        [GameParameters.CustomFloatParameterUI("Contract deadline multiplier", toolTip = "Used to lengthen or shorten all contract deadlines.", minValue = 0.5f, maxValue = 5f, stepCount = 46, displayFormat = "N1", gameMode = GameParameters.GameMode.CAREER)]
        public float ContractDeadlineMult = 1f;

        [GameParameters.CustomFloatParameterUI("Maintenance cost multiplier", minValue = 0f, maxValue = 10f, stepCount = 101, displayFormat = "N1", gameMode = GameParameters.GameMode.CAREER)]
        public float MaintenanceCostMult = 1f;
        
        [GameParameters.CustomParameterUI("Enable career progress logging")]
        public bool CareerLogEnabled = true;

        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
            switch (preset)
            {
                case GameParameters.Preset.Easy:
                    IsTrainingEnabled = false;
                    IsRetirementEnabled = false;
                    ContractDeadlineMult = 1.7f;
                    break;
                case GameParameters.Preset.Normal:
                    IsTrainingEnabled = true;
                    IsRetirementEnabled = true;
                    ContractDeadlineMult = 1.3f;
                    break;
                case GameParameters.Preset.Moderate:
                    IsTrainingEnabled = true;
                    IsRetirementEnabled = true;
                    ContractDeadlineMult = 1f;
                    break;
                case GameParameters.Preset.Hard:
                    IsTrainingEnabled = true;
                    IsRetirementEnabled = true;
                    ContractDeadlineMult = 0.8f;
                    break;
            }
        }
    }
}
