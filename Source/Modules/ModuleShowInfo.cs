using LRTR.Crew;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LRTR
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class ModuleShowInfoUpdater : MonoBehaviour
    {
        protected bool run = true;
        private void Update()
        {
            if (run)
            {
                run = false;
                GameObject.Destroy(this);
            }
        }
    }

    public class ModuleShowInfo : PartModule
    {
        public const string sModuleName = "Show Info";
        public const string dragCubeGroup = "Drag Cubes";

        [KSPField(guiName = "X+", groupName = dragCubeGroup, groupDisplayName = dragCubeGroup)] private string XP;
        [KSPField(guiName = "X-", groupName = dragCubeGroup)] private string XN;
        [KSPField(guiName = "Y+", groupName = dragCubeGroup)] private string YP;
        [KSPField(guiName = "Y-", groupName = dragCubeGroup)] private string YN;
        [KSPField(guiName = "Z+", groupName = dragCubeGroup)] private string ZP;
        [KSPField(guiName = "Z-", groupName = dragCubeGroup)] private string ZN;
        [KSPField] public float updateInterval = 0.25f;

        private bool showCubeInfo = false;

        public override string GetModuleDisplayName() => "Unlock Requirements";

        public override string GetInfo()
        {
            string data = null, apInfo = null;
            if (part.partInfo is AvailablePart ap)
            {
                apInfo = $"Tech Required: {ap.TechRequired}";
                if (part.CrewCapacity > 0)
                    apInfo = $"Training course: {(TrainingDatabase.SynonymReplace(part.name, out string name) ? name : ap.title)}\n{apInfo}";
            }
            string res = $"Part name: {part.name}\n{apInfo}\n{data}";
            return res;
        }

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
                StartCoroutine(DragCubeDisplay());
        }

        private IEnumerator DragCubeDisplay()
        {
            while (HighLogic.LoadedSceneIsFlight)
            {
                if (showCubeInfo != PhysicsGlobals.AeroDataDisplay)
                {
                    showCubeInfo = PhysicsGlobals.AeroDataDisplay;
                    Fields[nameof(XP)].guiActive = showCubeInfo;
                    Fields[nameof(XN)].guiActive = showCubeInfo;
                    Fields[nameof(YP)].guiActive = showCubeInfo;
                    Fields[nameof(YN)].guiActive = showCubeInfo;
                    Fields[nameof(ZP)].guiActive = showCubeInfo;
                    Fields[nameof(ZN)].guiActive = showCubeInfo;
                }
                if (showCubeInfo)
                    BuildCubeData();
                yield return new WaitForSeconds(updateInterval);
            }
        }

        private void BuildCubeData()
        {
            DragCubeList cubes = part.DragCubes;
            XP = $"{cubes.WeightedArea[0]:F2} ({cubes.GetCubeAreaDir(DragCubeList.GetFaceDirection(DragCube.DragFace.XP)):F2})";
            XN = $"{cubes.WeightedArea[1]:F2} ({cubes.GetCubeAreaDir(DragCubeList.GetFaceDirection(DragCube.DragFace.XN)):F2})";
            YP = $"{cubes.WeightedArea[2]:F2} ({cubes.GetCubeAreaDir(DragCubeList.GetFaceDirection(DragCube.DragFace.YP)):F2})";
            YN = $"{cubes.WeightedArea[3]:F2} ({cubes.GetCubeAreaDir(DragCubeList.GetFaceDirection(DragCube.DragFace.YN)):F2})";
            ZP = $"{cubes.WeightedArea[4]:F2} ({cubes.GetCubeAreaDir(DragCubeList.GetFaceDirection(DragCube.DragFace.ZP)):F2})";
            ZN = $"{cubes.WeightedArea[5]:F2} ({cubes.GetCubeAreaDir(DragCubeList.GetFaceDirection(DragCube.DragFace.ZN)):F2})";
        }
    }
}