using SiraUtil.Affinity;

namespace SiraUtil.Tools.FPFC
{
    internal class InputSpoofFPFCListener : IFPFCListener, IAffinity
    {
        private bool _active;
        private readonly DevicelessVRHelper _devicelessVRHelper = new();

        public void Enabled()
        {
            _active = true;
        }

        public void Disabled()
        {
            _active = false;
        }

        [AffinityPrefix]
        [AffinityPatch(typeof(UnityXRHelper), nameof(IVRPlatformHelper.GetTriggerValue))]
        protected bool GetTriggerValueOverridePatch(ref float __result)
        {
            __result = _devicelessVRHelper.GetTriggerValue(default);
            return !_active;
        }

        // OculusVRHelper was removed from the game as of 1.45.1 (Beat Saber consolidated
        // all VR runtimes, Oculus included, onto UnityXRHelper/OpenXR), so the Oculus-specific
        // patch that used to live here is no longer needed -- GetTriggerValueOverridePatch
        // above now covers Oculus headsets too.
    }
}