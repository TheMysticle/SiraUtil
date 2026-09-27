using BGLib.AppFlow.Initialization;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SiraUtil.Zenject.Harmony
{
    [HarmonyPatch(typeof(Context))]
    internal class ContextDecorator
    {
        // This set is used to catch any late installing decorators.
        private static readonly HashSet<Context> _recentlyInstalledDecorators = [];
        internal static Action<Context, IEnumerable<Type>>? ContextInstalling;
        internal static Action<Context, List<MonoBehaviour>>? SceneBindingsInstalled;

        [HarmonyPatch(nameof(Context.InstallInstallers))]
        [HarmonyPatch([typeof(List<InstallerBase>), typeof(List<Type>), typeof(List<ScriptableObjectInstaller>), typeof(List<MonoInstaller>), typeof(List<MonoInstaller>)])]
        [HarmonyPrefix]
        internal static void InstallInstallers(Context __instance, List<InstallerBase> normalInstallers, List<Type> normalInstallerTypes, List<ScriptableObjectInstaller> scriptableObjectInstallers, List<MonoInstaller> installers, List<MonoInstaller> installerPrefabs)
        {
            // Check if this is a late bound decorator installation.
            if (_recentlyInstalledDecorators.Contains(__instance))
            {
                _recentlyInstalledDecorators.Remove(__instance);
                return;
            }

            // Adds every installer that's being installed to the type registrator.
            HashSet<Type> installerBindings = [];
            foreach (InstallerBase normalInstaller in normalInstallers)
            {
                installerBindings.Add(normalInstaller.GetType());
            }

            foreach (Type normalInstallerType in normalInstallerTypes)
            {
                installerBindings.Add(normalInstallerType);
            }

            foreach (ScriptableObjectInstaller scriptableObjectInstaller in scriptableObjectInstallers)
            {
                installerBindings.Add(scriptableObjectInstaller.GetType());
            }

            foreach (MonoInstaller installer in installers)
            {
                installerBindings.Add(installer.GetType());
            }

            foreach (MonoInstaller installerPrefab in installerPrefabs)
            {
                installerBindings.Add(installerPrefab.GetType());
            }

            // AsyncSceneContext itself still exists as of 1.45.1 -- what changed is its internal
            // structure: the old flat `_asyncInstallers` list is gone, replaced by `_registry`
            // (an AsyncInstallerRegistry, populated by RunAsync() -> LoadInstallersAsync()
            // before base.Run() -> InstallInstallers() runs, so it's always ready by the time
            // this prefix fires). Without folding these in, ContextInstalling reports an
            // installer set missing every async-loaded installer for this context, which made
            // our own InstallFilter.ShouldInstall() checks act on incomplete information --
            // confirmed as the cause of an intermittent Zenject injection failure
            // (InvalidCastException inside ModestTree.Assert.IsEqual, deep in
            // DiContainer.InjectExplicitInternal) that left several core gameplay systems
            // (BeatmapObjectSpawnController, NoteCutSoundEffectManager, SaberClashEffect, etc.)
            // uninjected for the rest of the session -- via an actual crash log from a real
            // gameplay session on 1.45.1.
            if (__instance is AsyncSceneContext asyncSceneContext && asyncSceneContext._registry != null)
            {
                foreach (IInstaller asyncInstaller in asyncSceneContext._registry.installers)
                {
                    installerBindings.Add(asyncInstaller.GetType());
                }
            }

            if (__instance is SceneDecoratorContext decorator)
            {
                _recentlyInstalledDecorators.Add(decorator);
            }

            ContextInstalling?.Invoke(__instance, installerBindings);
        }

        [HarmonyPatch(nameof(Context.InstallSceneBindings))]
        [HarmonyPostfix]
        internal static void InstallSceneBindings(Context __instance, List<MonoBehaviour> injectableMonoBehaviours)
        {
            SceneBindingsInstalled?.Invoke(__instance, injectableMonoBehaviours);
        }
    }
}