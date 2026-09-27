namespace SiraUtil.Sabers.Effects
{
    // DISABLED as of the 1.45.1 port, and no longer bound in SiraGameplayInstaller: this
    // class extended SaberBurnMarkArea's line-renderer arrays to support more than 2 sabers
    // (e.g. multiplayer), but SaberBurnMarkArea's burn-mark rendering was rewritten to use
    // render textures instead of LineRenderers as of 1.45.1 (see SaberBurnMarkAreaPatch.cs
    // for the full explanation). The fields this class reflected into
    // (_lineRenderers, _saberBurnMarkLinePrefab) no longer exist, so there's nothing left to
    // extend. Needs a real reimplementation against the new render-texture-based approach,
    // which needs decompiling the new Initialize/AddSaber-equivalent logic and in-headset
    // testing with more than 2 sabers active to verify. Until then, native 2-saber burn
    // marks work unmodified.
    internal class SaberBurnMarkAreaLatch
    {
    }
}
