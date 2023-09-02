using System;
using System.Collections.Generic;

namespace PerformanceWpfApp
{
    /// <summary>
    /// Disable GPU Acceleration Devices.
    /// </summary>
    public sealed class DisableGpuAccelerationDeviceSet : Dictionary<string, DisableGpuAccelerationDeviceInfo>
    {
        private readonly HashSet<string> _internalGpus;

        /// <inheritdoc />
        public DisableGpuAccelerationDeviceSet()
        {
            _internalGpus = new HashSet<string>
            {
                "Intel(R) Iris(R) Xe Graphics",
            };

            SetKnownIssueDevices();
        }

        /// <summary>
        /// Set Well-Known Issue Devices.
        /// </summary>
        public void SetKnownIssueDevices()
        {
            Add(
                // https://github.com/dotnet/wpf/issues/3817
                "Intel(R) Iris(R) Xe Graphics",
                DisableGpuAccelerationDeviceInfo.CreateInternalGpu(
                    "Intel(R) Iris(R) Xe Graphics",
                    "11th Gen Intel(R) Core(TM)",
                    new Version("30.0.100.9684")));
        }

        /// <summary>
        /// Check Internal GPU or External GPU.
        /// </summary>
        /// <param name="videoControllerName">GPU 이름.</param>
        /// <returns><see cref="DriverType"/></returns>
        public DriverType GetGpuType(string videoControllerName)
        {
            return _internalGpus.Contains(videoControllerName)
                ? DriverType.Internal
                : DriverType.External;
        }

        /// <summary>
        /// 내장 GPU 대상으로 하드웨어 가속 비활성화 여부를 체크합니다.
        /// </summary>
        /// <param name="videoControllerName">GPU 이름.</param>
        /// <param name="processorName">CPU 이름.</param>
        /// <param name="driverVersion">그래픽 드라이버 버전.</param>
        /// <returns>하드웨어 가속 비활성화 여부.</returns>
        public bool TryCheckIfDisable(
            string videoControllerName, string processorName, Version driverVersion = null)
        {
            if (!TryGetValue(videoControllerName, out var target))
            {
                return false;
            }

            // 내장 GPU지만 CPU 정보를 알 수 없는 경우 비활성화 처리.
            if (string.IsNullOrEmpty(processorName))
            {
                return true;
            }

            return processorName.StartsWith(target.ProcessorName, StringComparison.OrdinalIgnoreCase) &&
                   (target.DriverVersion == null || CheckVersion(target.DriverVersion, driverVersion));
        }

        /// <summary>
        /// 외장 GPU 대상으로 하드웨어 가속 비활성화 여부를 체크합니다.
        /// </summary>
        /// <param name="videoControllerName">GPU 이름.</param>
        /// <param name="driverVersion">그래픽 드라이버 버전.</param>
        /// <returns>하드웨어 가속 비활성화 여부.</returns>
        public bool TryCheckIfDisable(
            string videoControllerName, Version driverVersion = null)
        {
            if (!TryGetValue(videoControllerName, out var target))
            {
                return false;
            }

            return target.DriverVersion == null || CheckVersion(target.DriverVersion, driverVersion);
        }

        private bool CheckVersion(Version issueVersion, Version targetVersion)
        {
            return (targetVersion ?? new Version()) < issueVersion;
        }
    }
}
