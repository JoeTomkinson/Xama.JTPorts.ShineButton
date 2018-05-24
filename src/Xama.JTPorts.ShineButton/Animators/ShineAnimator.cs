using Android.Animation;
using Android.Graphics;
using ShineButton.ei;

namespace ShineButton.Classes
{
    public class ShineAnimator : ValueAnimator
    {

        private float MAX_VALUE = 1.5f;
        private long ANIM_DURATION = 1500;
        private Canvas canvas;

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

        public void startAnim(ShineView shineView, int centerAnimX, int centerAnimY)
        {
            Start();
        }

        public void setCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }
    }
}