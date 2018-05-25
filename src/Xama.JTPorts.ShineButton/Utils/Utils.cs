using Android.App;
using Android.Views;

namespace ShineButton.Classes
{
    internal class Utils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        internal static bool IsFullScreen(Activity activity)
        {
            var flag = activity.Window.Attributes.Flags;

            if (flag == WindowManagerFlags.Fullscreen && (Application.Context as Activity).Window.Attributes.Flags == WindowManagerFlags.Fullscreen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        internal static bool IsTranslucentNavigation(Activity activity)
        {
            var flag = activity.Window.Attributes.Flags;

            if (flag == WindowManagerFlags.TranslucentNavigation && (Application.Context as Activity).Window.Attributes.Flags == WindowManagerFlags.TranslucentNavigation)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        internal static bool IsWindowsNotLimit(Activity activity)
        {
            var flag = activity.Window.Attributes.Flags;

            if (flag == WindowManagerFlags.LayoutNoLimits && (Application.Context as Activity).Window.Attributes.Flags == WindowManagerFlags.LayoutNoLimits)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}