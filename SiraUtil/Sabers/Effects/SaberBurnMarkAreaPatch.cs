namespace SiraUtil.Sabers.Effects
{
    // DISABLED as of the 1.45.1 port: SaberBurnMarkArea's line-renderer-based burn mark
    // implementation was rewritten to use render textures instead (it now has _sabers,
    // _renderTextures, _fadeOutMaterial, WorldToNormalized/GetBurnMarkPos instead of
    // _lineRenderers/_saberBurnMarkLinePrefab and OnEnable/OnDisable). This class used to:
    //   - postfix OnEnable/OnDisable to show/hide line renderers beyond the first two sabers
    //   - transpile LateUpdate to compare all line renderers instead of just the first two
    //   - transpile OnDestroy to also destroy the extra line renderers
    // None of that IL shape exists anymore, so the transpilers here would throw at Harmony
    // patch-apply time (via ThrowIfInvalid) against the new method bodies. Since SiraUtil
    // applies all of its Harmony patches in one Harmony.PatchAll(assembly) call in Plugin.cs,
    // a throwing transpiler here could abort patching for the rest of the assembly too -- so
    // this is fully disabled (not just left broken) pending a real reimplementation against
    // the new render-texture-based approach, which needs decompiling the new Initialize/
    // LateUpdate/OnDestroy bodies and in-headset testing with more than 2 sabers active
    // (multiplayer) to verify. Until then, native 2-saber burn marks work unmodified; only
    // SiraUtil's more-than-2-sabers extension is unavailable.
    internal class SaberBurnMarkAreaPatch
    {
    }
}
