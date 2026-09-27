using SiraUtil.Sabers;
using SiraUtil.Sabers.Effects;
using SiraUtil.Tools.SongControl;
using Zenject;

namespace SiraUtil.Installers
{
    // This class contains bindings that should work for EVERY song gamemode (Standard, Campaign, Tutorial, and Multiplayer)
    internal class SiraGameplayInstaller : Installer
    {
        public override void InstallBindings()
        {
            // SongControl stuff
            Container.BindInterfacesTo<SongControlManager>().AsSingle();

            // Saber API
            Container.BindInterfacesTo<ObstacleSaberSparkleEffectManagerLatch>().AsSingle();
            Container.BindInterfacesTo<SaberBurnMarkSparklesLatch>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaberModelProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaberModelManager>().AsSingle();
            Container.BindInterfacesTo<SaberClashEffectAdjuster>().AsSingle();

            // SaberBurnMarkAreaLatch is disabled as of the 1.45.1 port: SaberBurnMarkArea's
            // burn-mark rendering was rewritten (render textures instead of LineRenderers --
            // see SaberBurnMarkAreaPatch.cs for details), so this class's field-reflection
            // based more-than-2-sabers support no longer has anything valid to touch and
            // would throw at runtime. Native 2-saber burn marks still work unmodified.
            // Container.BindInterfacesTo<SaberBurnMarkAreaLatch>().AsSingle();

            Container.Bind<SiraSaberFactory>().AsSingle();
        }
    }
}