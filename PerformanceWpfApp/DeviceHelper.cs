using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management;

namespace PerformanceWpfApp
{
    /// <summary>
    /// Device Helper.
    /// </summary>
    public class DeviceHelper
    {
        private const string Scope = @"root\CIMV2";

        /// <summary>
        /// 하드웨어 가속 비활성화 항목.
        /// </summary>
        public static DisableGpuAccelerationDeviceSet DisableGpuAccelerationDevices = new DisableGpuAccelerationDeviceSet();

        /// <summary>
        /// 프로세서의 이름을 순차적으로 가져옵니다.
        /// </summary>
        /// <returns>Processor names.</returns>
        public static IEnumerable<string> GetProcessorNames()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher(Scope, "SELECT Name FROM Win32_Processor"))
                {
                    return searcher.Get()
                        .OfType<ManagementObject>()
                        .Select(o => o["Name"] as string)
                        .Where(n => !string.IsNullOrEmpty(n));
                }
            }
            catch
            {
                return Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// 비디오 제어기를 순차적으로 가져옵니다.
        /// </summary>
        /// <returns>Video Controllers.</returns>
        public static IReadOnlyDictionary<string, Version> GetVideoControllers()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher(Scope, "SELECT Name, DriverVersion FROM Win32_VideoController"))
                {
                    var result = searcher.Get()
                        .OfType<ManagementObject>()
                        .Select(o => (o["Name"] as string, o["DriverVersion"] as string))
                        .Where(kv => !string.IsNullOrEmpty(kv.Item1) && !string.IsNullOrEmpty(kv.Item2))
                        .Select(kv => (kv.Item1, new Version(kv.Item2)))
                        .ToDictionary(kv => kv.Item1, kv => kv.Item2);

                    return new ReadOnlyDictionary<string, Version>(result);
                }
            }
            catch
            {
                return new ReadOnlyDictionary<string, Version>(new Dictionary<string, Version>());
            }
        }

        public static bool UseDisableAccelerationGpu()
        {
            var videoControllers = GetVideoControllers();
            var processorName = GetProcessorNames().FirstOrDefault();

            foreach (var videoController in videoControllers)
            {
                switch (DisableGpuAccelerationDevices.GetGpuType(videoController.Key))
                {
                    case DriverType.External
                        when DisableGpuAccelerationDevices.TryCheckIfDisable(videoController.Key, videoController.Value):
                    case DriverType.Internal
                        when DisableGpuAccelerationDevices.TryCheckIfDisable(videoController.Key, processorName, videoController.Value):
                        return true;
                }
            }

            return false;
        }
    }
}