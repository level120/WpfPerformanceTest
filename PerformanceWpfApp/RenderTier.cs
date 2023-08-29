namespace PerformanceWpfApp
{
    /// <summary>
    /// 렌더링 호환성
    /// </summary>
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.rendercapability.tier" />
    public enum RenderTier
    {
        /// <summary>
        /// 가장 낮은 렌더링 호환성
        /// </summary>
        /// <remarks>
        /// No graphics hardware acceleration is available for the application on the device.
        /// All graphics features use software acceleration.
        /// The DirectX version level is less than version 9.0.
        /// </remarks>
        Tier0 = 0x00000000,

        /// <summary>
        /// 그래픽 성능이 제한된 렌더링 호환성
        /// </summary>
        /// <remarks>
        /// Most of the graphics features of WPF will use hardware acceleration
        /// if the necessary system resources are available and have not been exhausted.
        /// This corresponds to a DirectX version that is greater than or equal to 9.0.
        /// </remarks>
        Tier1 = 0x00010000,

        /// <summary>
        /// 가장 높은 렌더링 호환성
        /// </summary>
        /// <remarks>
        /// Most of the graphics features of WPF will use hardware acceleration
        /// provided the necessary system resources have not been exhausted.
        /// This corresponds to a DirectX version that is greater than or equal to 9.0.
        /// </remarks>
        Tier2 = 0x00020000
    }
}