using System;
using UnityEngine;

namespace LRTR
{
    public class MaintenanceGUI : UIBase
    {
        private enum MaintenancePeriod { Day, Month, Year };

        private Vector2 _nautListScroll = new Vector2();
        private MaintenancePeriod _selectedPeriod = MaintenancePeriod.Year;
        private readonly GUIContent _infoBtnContent = new GUIContent("ⓘ", "View details");

        private string PeriodFactor
        {
            get
            {
                return _selectedPeriod switch
                {
                    MaintenancePeriod.Day => "day",
                    MaintenancePeriod.Month => "month",
                    MaintenancePeriod.Year => "year",
                    _ => "",
                };
            }
        }

        private string PeriodDispFormat => _selectedPeriod == MaintenancePeriod.Day ? "N1" : "N0";

        private void RenderPeriodSelector()
        {
            GUILayout.BeginHorizontal();

            if (RenderToggleButton("Day", _selectedPeriod == MaintenancePeriod.Day))
                _selectedPeriod = MaintenancePeriod.Day;
            if (RenderToggleButton("Month", _selectedPeriod == MaintenancePeriod.Month))
                _selectedPeriod = MaintenancePeriod.Month;
            if (RenderToggleButton("Year", _selectedPeriod == MaintenancePeriod.Year))
                _selectedPeriod = MaintenancePeriod.Year;

            GUILayout.EndHorizontal();
        }

        private double GetPeriod()
        {
            double year = 426;
            ConfigNode paramsNode = null;
            foreach (ConfigNode lrtrConfig in GameDatabase.Instance.GetConfigNodes("LRTRCONFIG"))
                paramsNode = lrtrConfig;

            if (paramsNode == null)
            {
                Debug.LogError("[LRTRCONFIG] Could not find LRTRCONFIG node.");
            }

            if (paramsNode.GetValue("daysPerYear") != null)
                year = Convert.ToDouble(paramsNode.GetValue("daysPerYear"));

            switch (PeriodFactor)
            {
                case "day": return 1; 
                case "month": return 30;
                default: return year;
            };
        }

        public void RenderSummaryTab()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Maintenance costs (per ", HighLogic.Skin.label);
            RenderPeriodSelector();
            GUILayout.Label(")", HighLogic.Skin.label);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Facilities", HighLogic.Skin.label, GUILayout.Width(160));
                GUILayout.Label((MaintenanceHandler.Instance.FacilityUpkeep * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
                if (GUILayout.Button(_infoBtnContent, InfoButton))
                {
                    TopWindow.SwitchTabTo(UITab.Facilities);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Integration", HighLogic.Skin.label, GUILayout.Width(160));
                GUILayout.Label((MaintenanceHandler.Instance.IntegrationUpkeep * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
                if (GUILayout.Button(_infoBtnContent, InfoButton))
                {
                    TopWindow.SwitchTabTo(UITab.Integration);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Research Teams", HighLogic.Skin.label, GUILayout.Width(160));
                GUILayout.Label((MaintenanceHandler.Instance.ResearchUpkeep * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Astronauts", HighLogic.Skin.label, GUILayout.Width(160));
                GUILayout.Label((MaintenanceHandler.Instance.NautTotalUpkeep * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
                if (GUILayout.Button(_infoBtnContent, InfoButton))
                {
                    TopWindow.SwitchTabTo(UITab.Astronauts);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Total (after subsidy)", BoldLabel, GUILayout.Width(160));
                GUILayout.Label(((MaintenanceHandler.Instance.TotalUpkeep + MaintenanceHandler.Settings.maintenanceOffset) * GetPeriod()).ToString(PeriodDispFormat), BoldRightLabel, GUILayout.Width(160));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();
        }

        public void RenderFacilitiesTab()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Facilities costs (per ", HighLogic.Skin.label);
            RenderPeriodSelector();
            GUILayout.Label(")", HighLogic.Skin.label);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Launch Pads", HighLogic.Skin.label, GUILayout.Width(160));
                GUILayout.Label((MaintenanceHandler.Instance.PadCost * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();

            for (int i = 0; i < MaintenanceHandler.PadLevelCount; i++)
            {
                if (MaintenanceHandler.Instance.PadCosts[i] == 0d)
                    continue;
                GUILayout.BeginHorizontal();
                try
                {
                    GUILayout.Label(String.Format("  level {0} × {1}", i + 1, MaintenanceHandler.Instance.KCTPadCounts[i]), HighLogic.Skin.label, GUILayout.Width(160));
                    GUILayout.Label((MaintenanceHandler.Instance.PadCosts[i] * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Runway", HighLogic.Skin.label, GUILayout.Width(160));
                GUILayout.Label((MaintenanceHandler.Instance.RunwayCost * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Vertical Assembly Building", HighLogic.Skin.label, GUILayout.Width(160));
                GUILayout.Label((MaintenanceHandler.Instance.VabCost * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Spaceplane Hangar", HighLogic.Skin.label, GUILayout.Width(160));
                GUILayout.Label((MaintenanceHandler.Instance.SphCost * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Research & Development", HighLogic.Skin.label, GUILayout.Width(160));
                GUILayout.Label((MaintenanceHandler.Instance.RndCost * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Mission Control", HighLogic.Skin.label, GUILayout.Width(160));
                GUILayout.Label((MaintenanceHandler.Instance.McCost * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Tracking Station", HighLogic.Skin.label, GUILayout.Width(160));
                GUILayout.Label((MaintenanceHandler.Instance.TsCost * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Astronaut Complex", HighLogic.Skin.label, GUILayout.Width(160));
                GUILayout.Label((MaintenanceHandler.Instance.AcCost * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Total", BoldLabel, GUILayout.Width(160));
                GUILayout.Label((MaintenanceHandler.Instance.FacilityUpkeep * GetPeriod()).ToString(PeriodDispFormat), BoldRightLabel, GUILayout.Width(160));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();
        }

        public void RenderIntegrationTab()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Integration Team costs (per ", HighLogic.Skin.label);
            RenderPeriodSelector();
            GUILayout.Label(")", HighLogic.Skin.label);
            GUILayout.EndHorizontal();

            foreach (string site in MaintenanceHandler.Instance.KCTBuildRates.Keys)
            {
                double rate = MaintenanceHandler.Instance.KCTBuildRates[site];
                GUILayout.BeginHorizontal();
                try
                {
                    GUILayout.Label(site, HighLogic.Skin.label, GUILayout.Width(160));
                    GUILayout.Label((rate * MaintenanceHandler.Settings.kctBPMult * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Total", BoldLabel, GUILayout.Width(160));
                GUILayout.Label((MaintenanceHandler.Instance.IntegrationUpkeep * GetPeriod()).ToString(PeriodDispFormat), BoldRightLabel, GUILayout.Width(160));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();
        }

        private void RenderNautList()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Name", HighLogic.Skin.label, GUILayout.Width(144));
            GUILayout.Label("Retires NET", HighLogic.Skin.label, GUILayout.Width(160));
            GUILayout.EndHorizontal();

            foreach (string name in Crew.CrewHandler.Instance.KerbalRetireTimes.Keys)
            {
                GUILayout.BeginHorizontal();
                try
                {
                    GUILayout.Space(20);
                    double rt = Crew.CrewHandler.Instance.KerbalRetireTimes[name];
                    GUILayout.Label(name, HighLogic.Skin.label, GUILayout.Width(144));
                    GUILayout.Label(Crew.CrewHandler.Instance.RetirementEnabled ? KSPUtil.PrintDate(rt, false) : "(n/a)", HighLogic.Skin.label, GUILayout.Width(160));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                GUILayout.EndHorizontal();
            }
        }

        public void RenderAstronautsTab()
        {
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Astronaut costs (per ", HighLogic.Skin.label);
                RenderPeriodSelector();
                GUILayout.Label(")", HighLogic.Skin.label);
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            try
            {
                int nautCount = HighLogic.CurrentGame.CrewRoster.GetActiveCrewCount();
                GUILayout.Label($"Corps: {nautCount:N0} astronauts", HighLogic.Skin.label, GUILayout.Width(160));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            GUILayout.EndHorizontal();

            _nautListScroll = GUILayout.BeginScrollView(_nautListScroll, GUILayout.Width(360), GUILayout.Height(280));
            RenderNautList();
            GUILayout.EndScrollView();

            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                GUILayout.BeginHorizontal();
                try
                {
                    GUILayout.Label("Astronaut base cost", HighLogic.Skin.label, GUILayout.Width(160));
                    GUILayout.Label((MaintenanceHandler.Instance.NautBaseUpkeep * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                try
                {
                    GUILayout.Label("Astronaut operational cost", HighLogic.Skin.label, GUILayout.Width(160));
                    GUILayout.Label((MaintenanceHandler.Instance.NautInFlightUpkeep * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                try
                {
                    GUILayout.Label("Astronaut training cost", HighLogic.Skin.label, GUILayout.Width(160));
                    GUILayout.Label((MaintenanceHandler.Instance.TrainingUpkeep * GetPeriod()).ToString(PeriodDispFormat), RightLabel, GUILayout.Width(160));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                try
                {
                    GUILayout.Label("Total", BoldLabel, GUILayout.Width(160));
                    GUILayout.Label((MaintenanceHandler.Instance.NautTotalUpkeep * GetPeriod()).ToString(PeriodDispFormat), BoldRightLabel, GUILayout.Width(160));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}
