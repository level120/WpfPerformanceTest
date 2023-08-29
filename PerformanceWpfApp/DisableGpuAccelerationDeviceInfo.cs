using System;

namespace PerformanceWpfApp
{
    /// <summary>
    /// 하드웨어 가속 비활성화 장비 정보.
    /// </summary>
    public sealed class DisableGpuAccelerationDeviceInfo
    {
        private DisableGpuAccelerationDeviceInfo(
            string processorName,
            string videoControllerName,
            Version driverVersion,
            DriverType driverType)
        {
            ProcessorName = processorName;
            VideoControllerName = videoControllerName;
            DriverVersion = driverVersion;
            DriverType = driverType;
        }

        /// <summary>
        /// ProcessorName.
        /// </summary>
        public string ProcessorName { get; }

        /// <summary>
        /// Video Controller Name.
        /// </summary>
        public string VideoControllerName { get; }

        /// <summary>
        /// Driver Version.
        /// </summary>
        public Version DriverVersion { get; }

        /// <summary>
        /// Driver Type.
        /// </summary>
        public DriverType DriverType { get; }

        /// <summary>
        /// 내장 GPU로부터 정보 생성
        /// </summary>
        /// <param name="videoControllerName">GPU 이름.</param>
        /// <param name="processorName">CPU 이름.</param>
        /// <param name="driverVersion">문제가 해결된 그래픽 드라이버 버전(없으면 null).</param>
        /// <returns><see cref="DisableGpuAccelerationDeviceInfo"/></returns>
        public static DisableGpuAccelerationDeviceInfo CreateInternalGpu(
            string videoControllerName, string processorName, Version driverVersion = null)
        {
            return new DisableGpuAccelerationDeviceInfo(
                processorName, videoControllerName, driverVersion, DriverType.Internal);
        }

        /// <summary>
        /// 외장 GPU로부터 정보 생성
        /// </summary>
        /// <param name="videoControllerName">GPU 이름.</param>
        /// <param name="driverVersion">문제가 해결된 그래픽 드라이버 버전(없으면 null).</param>
        /// <returns><see cref="DisableGpuAccelerationDeviceInfo"/></returns>
        public static DisableGpuAccelerationDeviceInfo CreateExternalGpu(
            string videoControllerName, Version driverVersion = null)
        {
            return new DisableGpuAccelerationDeviceInfo(
                null, videoControllerName, driverVersion, DriverType.External);
        }
    }
}