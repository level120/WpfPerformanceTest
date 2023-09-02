using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Interop;

namespace PerformanceWpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            RenderCapability.TierChanged += (_, __) => OptimizeRenderOptions();

            OptimizeRenderOptions();

            Task.Delay(3000).Wait();
        }

        private void OptimizeRenderOptions()
        {
            switch ((RenderTier)RenderCapability.Tier)
            {
                // 일반 PC
                case RenderTier.Tier2 when !DeviceHelper.UseDisableAccelerationGpu():
                    RenderOptions.ProcessRenderMode = RenderMode.Default;
                    break;

                // RDP, 저사양 PC 또는 렌더링 이슈가 있었던 PC
                case RenderTier.Tier1:
                case RenderTier.Tier0:
                default:
                    RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
                    break;
            }
        }
    }
}
