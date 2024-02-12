using BepInEx;
using LethalCompanyInputUtils.Api;
using UnityEngine.InputSystem;

namespace HeckinChloe.QuickRadarSwitch;

[BepInPlugin("HeckinChloe.QuickRadarSwitch", "QuickRadarSwitch", "1.0.0")]
[BepInDependency("com.rune580.LethalCompanyInputUtils", "0.6.3")]
public sealed class Plugin : BaseUnityPlugin
{
    private sealed class InputActions : LcInputActions
    {
        [InputAction("<Keyboard>/leftArrow", Name = "Previous Target")]
        internal InputAction PreviousTarget { get; set; }

        [InputAction("<Keyboard>/rightArrow", Name = "Next Target")]
        internal InputAction NextTarget { get; set; }
    }

    private static readonly InputActions Inputs = new();

    private void Awake()
    {
        Inputs.PreviousTarget.performed += _ => TrySwitchRadarTarget(step: -1);
        Inputs.NextTarget.performed += _ => TrySwitchRadarTarget(step: 1);
    }

    private void TrySwitchRadarTarget(int step)
    {
        var player = GameNetworkManager.Instance.localPlayerController;

        if ((player != null) && player.inTerminalMenu)
        {
            var mapScreen = StartOfRound.Instance.mapScreen;
            var currentIndex = mapScreen.targetTransformIndex;
            var targetCount = mapScreen.radarTargets.Count;
            var newIndex = (currentIndex + step + targetCount) % targetCount;

            if (newIndex != mapScreen.targetTransformIndex)
            {
                mapScreen.SwitchRadarTargetAndSync(newIndex);
            }
        }
    }
}
