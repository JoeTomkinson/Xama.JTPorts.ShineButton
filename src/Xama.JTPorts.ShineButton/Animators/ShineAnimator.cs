using Android.Animation;
using Android.Graphics;
using Xama.JTPorts.EasingInterpolator;

namespace ShineButton.Classes
{
    public class ShineAnimator : ValueAnimator
    {
        private float MAX_VALUE = 1.5f;
        private long ANIM_DURATION = 1500;
        private Canvas canvas;

        /// <summary>
        /// 
        /// </summary>
        public Canvas Canvas
        {
            set { canvas = value; }
        }

        public ShineAnimator()
        {
            SetFloatValues(1f, MAX_VALUE);
            SetDuration(ANIM_DURATION);
            StartDelay = 200;
            SetInterpolator(new EasingInterpolator(Ease.QuartOut));
        }

        public ShineAnimator(long duration, float max_value, long delay)
        {
            SetFloatValues(1f, max_value);
            SetDuration(duration);
            StartDelay = delay;
            SetInterpolator(new EasingInterpolator(Ease.QuartOut));
        }

        public void StartAnim(ShineView shineView, int centerAnimX, int centerAnimY)
        {
            Start();
        }
    }
}