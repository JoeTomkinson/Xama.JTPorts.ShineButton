using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using System;
using static Android.Views.View;

namespace ShineButton.Classes
{
    public class ShineButtonControl : PorterShapeImageView
    {
        #region CLASS LEVEL VARIABLES

        private static String TAG = nameof(ShineButtonControl);
        private bool isChecked = false;
        private Color btnColor;
        private Color btnFillColor;
        private int DEFAULT_WIDTH = 50;
        private int DEFAULT_HEIGHT = 50;
        private DisplayMetrics metrics = new DisplayMetrics();
        public Activity activity;
        private ShineView shineView;
        private ValueAnimator shakeAnimator;
        private ShineView.ShineParams shineParams = new ShineView.ShineParams();
        private OnCheckedChangeListener listener;
        private int bottomHeight;
        private int realBottomHeight;

        public EventHandler clickEvent;

        #endregion

        #region CONSTRUCTORS

        public ShineButtonControl(Context context) : base(context)
        {
            Activity activityCaller = context as Activity;

            if (activityCaller != null)
            {
                init((Activity)context);
            }
        }

        public ShineButtonControl(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            initButton(context, attrs);
        }

        public ShineButtonControl(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            initButton(context, attrs);
        }

        #endregion

        public void initButton(Context context, IAttributeSet attrs)
        {
            Activity activityCaller = context as Activity;

            if (activityCaller != null)
            {
                init((Activity)context);
            }

            TypedArray a = context.ObtainStyledAttributes(attrs, Resource.Styleable.ShineButton);

            btnColor = a.GetColor(Resource.Styleable.ShineButton_btn_color, Color.Gray);
            btnFillColor = a.GetColor(Resource.Styleable.ShineButton_btn_fill_color, Color.Black);

            shineParams.allowRandomColor = a.GetBoolean(Resource.Styleable.ShineButton_allow_random_color, false);
            shineParams.animDuration = a.GetInteger(Resource.Styleable.ShineButton_shine_animation_duration, (int)shineParams.animDuration);
            shineParams.bigShineColor = a.GetColor(Resource.Styleable.ShineButton_big_shine_color, shineParams.bigShineColor);
            shineParams.clickAnimDuration = a.GetInteger(Resource.Styleable.ShineButton_click_animation_duration, (int)shineParams.clickAnimDuration);
            shineParams.enableFlashing = a.GetBoolean(Resource.Styleable.ShineButton_enable_flashing, false);
            shineParams.shineCount = a.GetInteger(Resource.Styleable.ShineButton_shine_count, shineParams.shineCount);
            shineParams.shineDistanceMultiple = a.GetFloat(Resource.Styleable.ShineButton_shine_distance_multiple, shineParams.shineDistanceMultiple);
            shineParams.shineTurnAngle = a.GetFloat(Resource.Styleable.ShineButton_shine_turn_angle, shineParams.shineTurnAngle);
            shineParams.smallShineColor = a.GetColor(Resource.Styleable.ShineButton_small_shine_color, shineParams.smallShineColor);
            shineParams.smallShineOffsetAngle = a.GetFloat(Resource.Styleable.ShineButton_small_shine_offset_angle, shineParams.smallShineOffsetAngle);
            shineParams.shineSize = a.GetDimensionPixelSize(Resource.Styleable.ShineButton_shine_size, shineParams.shineSize);
            
            a.Recycle();

            setSrcColor(btnColor);
        }

        public int getBottomHeight(bool real)
        {
            if (real)
            {
                return realBottomHeight;
            }
            return bottomHeight;
        }

        public Color getColor()
        {
            return btnFillColor;
        }

        /// <summary>
        /// Public IsChecked variable to determine the click status of the control.
        /// </summary>
        /// <returns></returns>
        public bool IsChecked()
        {
            return isChecked;
        }
        
        public void setBtnColor(Color btnColor)
        {
            this.btnColor = btnColor;
            setSrcColor(this.btnColor);
        }

        public void setBtnFillColor(Color btnFillColor)
        {
            this.btnFillColor = btnFillColor;
        }

        /// <summary>
        /// Set whether the controls state is checked or unchecked.
        /// </summary>
        /// <param name="hasBeenChecked"></param>
        /// <param name="anim"></param>
        public void setChecked(bool hasBeenChecked, bool anim)
        {
            setChecked(hasBeenChecked, anim, true);
        }

        /// <summary>
        /// Internal method for firing off the animations based on check status of control.
        /// </summary>
        /// <param name="hasBeenChecked"></param>
        /// <param name="anim"></param>
        /// <param name="callBack"></param>
        private void setChecked(bool hasBeenChecked, bool anim, bool callBack)
        {
            isChecked = hasBeenChecked;
            if (hasBeenChecked) {
                setSrcColor(btnFillColor);
                isChecked = true;
                if (anim) showAnim();
            }
            else
            {
                setSrcColor(btnColor);
                isChecked = false;
                if (anim) setCancel();
            }
            if (callBack)
            {
                onListenerUpdate(hasBeenChecked);
            }
        }

        public void setChecked(bool hasBeenChecked)
        {
            setChecked(hasBeenChecked, false, false);
        }

        private void onListenerUpdate(bool hasBeenChecked)
        {
            if (listener != null)
            {
                listener.onCheckedChanged(this, hasBeenChecked);
            }
        }

        public void setCancel()
        {
            setSrcColor(btnColor);
            if (shakeAnimator != null)
            {
                shakeAnimator.End();
                shakeAnimator.Cancel();
            }
        }

        public void setAllowRandomColor(bool allowRandomColor)
        {
            shineParams.allowRandomColor = allowRandomColor;
        }

        public void setAnimDuration(int durationMs)
        {
            shineParams.animDuration = durationMs;
        }

        public void setBigShineColor(Color color)
        {
            shineParams.bigShineColor = color;
        }

        public void setClickAnimDuration(int durationMs)
        {
            shineParams.clickAnimDuration = durationMs;
        }

        public void enableFlashing(Boolean enable)
        {
            shineParams.enableFlashing = enable;
        }

        public void setShineCount(int count)
        {
            shineParams.shineCount = count;
        }

        public void setShineDistanceMultiple(float multiple)
        {
            shineParams.shineDistanceMultiple = multiple;
        }

        public void setShineTurnAngle(float angle)
        {
            shineParams.shineTurnAngle = angle;
        }

        public void setSmallShineColor(Color color)
        {
            shineParams.smallShineColor = color;
        }

        public void setSmallShineOffAngle(float angle)
        {
            shineParams.smallShineOffsetAngle = angle;
        }

        public void setShineSize(int size)
        {
            shineParams.shineSize = size;
        }

        public void setOnCheckStateChangeListener(OnCheckedChangeListener listener)
        {
            this.listener = listener;
        }
       
        public void init(Activity activity)
        {
            this.activity = activity;

            this.Click += (s, e) => {
                if (!isChecked)
                {
                    isChecked = true;
                    showAnim();
                }
                else
                {
                    isChecked = false;
                    setCancel();
                }

                if (listener != null)
                {
                    listener.onCheckedChanged(this, isChecked);
                }
            };
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            calPixels();
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
        }


        public void showAnim()
        {
            if (activity != null)
            {
                ViewGroup rootView = (ViewGroup)activity.FindViewById(Window.IdAndroidContent);
                shineView = new ShineView(activity, this, shineParams);
                rootView.AddView(shineView, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
                doShareAnim();
            }
            else
            {
                Log.Error(TAG, "Please init.");
            }
        }

        public void removeView(View view)
        {
            if (activity != null)
            {
                ViewGroup rootView = (ViewGroup)activity.FindViewById(Window.IdAndroidContent);
                rootView.RemoveView(view);
            }
            else
            {
                Log.Error(TAG, "Please init.");
            }
        }

        public void setShapeResource(int raw)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                setShape(Resources.GetDrawable(raw, null));
            }
            else
            {
                setShape(Resources.GetDrawable(raw)); //TOOD: this is depreciated in xamarin android native, create the work around.
            }
        }

        private void doShareAnim()
        {
            shakeAnimator = ValueAnimator.OfFloat(0.4f, 1f, 0.9f, 1f);
            shakeAnimator.SetInterpolator(new LinearInterpolator());
            shakeAnimator.SetDuration(500);
            shakeAnimator.StartDelay = 180;

            Invalidate();

            shakeAnimator.Update += (s, e) =>
            {
                //TODO: not sure if this will work, needs testing thoroughly
                ScaleX = (float)e.Animation.AnimatedValue;
                ScaleY = (float)e.Animation.AnimatedValue;
            };

            shakeAnimator.AnimationStart += (s, e) =>
            {
                setSrcColor(btnFillColor);
            };

            shakeAnimator.AnimationEnd += (s, e) =>
            {
                setSrcColor(isChecked ? btnFillColor : btnColor);
            };

            shakeAnimator.AnimationCancel += (s, e) =>
            {
                setSrcColor(btnColor);
            };

            shakeAnimator.Start();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        private void calPixels()
        {
            if (activity != null && metrics != null)
            {
                activity.WindowManager.DefaultDisplay.GetMetrics(metrics);
                int[] location = new int[2];
                GetLocationInWindow(location);
                Rect visibleFrame = new Rect();
                activity.Window.DecorView.GetWindowVisibleDisplayFrame(visibleFrame);
                realBottomHeight = visibleFrame.Height() - location[1];
                bottomHeight = metrics.HeightPixels - location[1];
            }
        }

        public interface OnCheckedChangeListener
        {
            void onCheckedChanged(View view, bool hasBeenchecked);
        }
    }
}