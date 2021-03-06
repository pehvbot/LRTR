﻿using System;
using UnityEngine;
using KSP.UI.Screens;

namespace LRTR
{
    public class TopWindow : UIBase
    {
        // GUI
        static Rect windowPos = new Rect(500, 240, 0, 0);
        private MaintenanceGUI maintUI = new MaintenanceGUI();
        private Crew.FSGUI fsUI = new LRTR.Crew.FSGUI();
        private CareerLogGUI logUI = new CareerLogGUI();
        private static Tabs currentTab;

        public TopWindow()
        {
            // Reset the tab on scene changes
            currentTab = default(Tabs);
        }

        public void OnGUI()
        {
            windowPos = GUILayout.Window("RP0Top".GetHashCode(), windowPos, DrawWindow, "RP-1");
        }

        public static void SwitchTabTo(Tabs newTab)
        {
            currentTab = newTab;
        }

        private void tabSelector()
        {
            GUILayout.BeginHorizontal();
            try {
                if (showTab(Tabs.Maintenance) && toggleButton("Maintenance", currentTab == Tabs.Maintenance))
                    currentTab = Tabs.Maintenance;
                if (showTab(Tabs.Training) && toggleButton("Astronauts", currentTab == Tabs.Training))
                    currentTab = Tabs.Training;
                if (showTab(Tabs.Courses) && toggleButton("Courses", currentTab == Tabs.Courses))
                    currentTab = Tabs.Courses;
                if (showTab(Tabs.CareerLog) && toggleButton("Career Log", currentTab == Tabs.CareerLog))
                    currentTab = Tabs.CareerLog;
            } finally {
                GUILayout.EndHorizontal();
            }
        }

        public void DrawWindow(int windowID)
        {
            try {
                GUILayout.BeginVertical();
                try {
                    /* If totalUpkeep is zero, we probably haven't calculated the upkeeps yet, so recalculate now */
                    if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER && MaintenanceHandler.Instance.totalUpkeep == 0d)
                        MaintenanceHandler.Instance.UpdateUpkeep();

                    tabSelector();
                    if (showTab(currentTab)) {
                        switch (currentTab) {
                        case Tabs.Maintenance:
                            maintUI.summaryTab();
                            break;
                        case Tabs.Facilities:
                            maintUI.facilitiesTab();
                            break;
                        case Tabs.Astronauts:
                            maintUI.astronautsTab();
                            break;
                        case Tabs.Training:
                            currentTab = fsUI.summaryTab();
                            break;
                        case Tabs.Courses:
                            currentTab = fsUI.coursesTab();
                            break;
                        case Tabs.NewCourse:
                            currentTab = fsUI.newCourseTab();
                            break;
                        case Tabs.Naut:
                            fsUI.nautTab();
                            break;
                        case Tabs.CareerLog:
                            logUI.RenderTab();
                            break;
                        default: // can't happen
                            break;
                        }
                    }
                } finally {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();
                }
            } finally {
                GUI.DragWindow();
            }
        }
    }
}

