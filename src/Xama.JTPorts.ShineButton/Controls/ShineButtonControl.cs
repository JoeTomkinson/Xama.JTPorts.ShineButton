using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using System;
using Xama.JTPorts.ShineButton;
using Xama.JTPorts.ShineButton.Models;

namespace ShineButton.Classes
{
    public class ShineButtonControl : PorterShapeImageView
    {
        #region CLASS LEVEL VARIABLES

        private static String TAG = nameof(ShineButtonControl);
        private Color btnColor;
        private DisplayMetrics metrics = new DisplayMetrics();
        public Activity activity;
        private ShineView shineView;
        private ValueAnimator shakeAnimator;
        private ShineView.ShineParams shineParams = new ShineView.ShineParams();
        private int bottomHeight;
        private int realBottomHeight;

        #endregion

        #region PUBLIC PROPERTIES

        /// <summary>
        /// 
        /// </summary>
        public EventHandler clickEvent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EventHandler Checked { get; set; }

        /// <summary>
        /// Set the buttons image starting colour mask.
        /// </summary>
        public Color ButtonColour
        {
            get { return this.btnColor; }
            set
            {
                this.btnColor = value;
                SourceColour = this.btnColor;
            }
        }

        /// <summary>
        /// Checked state fill colour of the button image.
        /// </summary>
        public Color ButtonFillColour { get; set; }

        /// <summary>
        ///  Get current checked state of control.
        /// </summary>
        /// <returns></returns>
        public bool IsChecked { get; private set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public bool AllowRandomColour
        {
            get { return shineParams.UseRandomColor; }
            set { shineParams.UseRandomColor = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public long AnimationDuration
        {
            get { return shineParams.AnimDuration; }
            set { shineParams.AnimDuration = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Color BigShineColour
        {
            get { return shineParams.BigShineColor; }
            set { shineParams.BigShineColor = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public long ClickAnimationDuration
        {
            get { return shineParams.ClickAnimDuration; }
            set { shineParams.ClickAnimDuration = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableFlashing
        {
            get { return shineParams.EnableFlashing; }
            set { shineParams.EnableFlashing = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ShineCount
        {
            get { return shineParams.ShineCount; }
            set { shineParams.ShineCount = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float ShineDistanceMultiple
        {
            get { return shineParams.ShineDistanceMultiple; }
            set { shineParams.ShineDistanceMultiple = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float ShineTurnAngle
        {
            get { return shineParams.ShineTurnAngle; }
            set { shineParams.ShineTurnAngle = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Color SmallShineColour
        {
            get { return shineParams.SmallShineColor; }
            set { shineParams.SmallShineColor = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float ShineOffsetAngle
        {
            get { return shineParams.SmallShineOffsetAngle; }
            set { shineParams.SmallShineOffsetAngle = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ShineSize
        {
            get { return shineParams.ShineSize; }
            set { shineParams.ShineSize = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ShapeResource
        {
            set
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Shape = Resources.GetDrawable(value, null);
                }
                else
                {
                    //TOOD: this is depreciated in xamarin android native, I don't think it needs to be here, I believe the above now attempts to support backwards compatibility.
                    Shape = Resources.GetDrawable(value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICheckedListener OnCheckStateChangeListener { get; set; }

        /// <summary>
        /// Supply random colour selection for the random shine animation.
        /// </summary>
        public ColourSet RandomColourSelection { get; set; } = null;

        #endregion

        #region PUBLIC CONSTRUCTORS

        public ShineButtonControl(Context context) : base(context)
        {
            Activity activityCaller = context as Activity;

            if (activityCaller != null)
            {
                Init((Activity)context);
            }
        }

        public ShineButtonControl(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            InitButton(context, attrs);
        }

        public ShineButtonControl(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            InitButton(context, attrs);
        }

        public void InitButton(Context context, IAttributeSet attrs)
        {
            Activity activityCaller = context as Activity;

            if (activityCaller != null)
            {
                Init((Activity)context);
            }

            TypedArray a = context.ObtainStyledAttributes(attrs, Resource.Styleable.ShineButton);

            btnColor = a.GetColor(Resource.Styleable.ShineButton_btn_color, Color.Gray);
            ButtonFillColour = a.GetColor(Resource.Styleable.ShineButton_btn_fill_color, Color.Black);

            shineParams.UseRandomColor = a.GetBoolean(Resource.Styleable.ShineButton_allow_random_color, false);
            shineParams.AnimDuration = a.GetInteger(Resource.Styleable.ShineButton_shine_animation_duration, (int)shineParams.AnimDuration);
            shineParams.BigShineColor = a.GetColor(Resource.Styleable.ShineButton_big_shine_color, shineParams.BigShineColor);
            shineParams.ClickAnimDuration = a.GetInteger(Resource.Styleable.ShineButton_click_animation_duration, (int)shineParams.ClickAnimDuration);
            shineParams.EnableFlashing = a.GetBoolean(Resource.Styleable.ShineButton_enable_flashing, false);
            shineParams.ShineCount = a.GetInteger(Resource.Styleable.ShineButton_shine_count, shineParams.ShineCount);
            shineParams.ShineDistanceMultiple = a.GetFloat(Resource.Styleable.ShineButton_shine_distance_multiple, shineParams.ShineDistanceMultiple);
            shineParams.ShineTurnAngle = a.GetFloat(Resource.Styleable.ShineButton_shine_turn_angle, shineParams.ShineTurnAngle);
            shineParams.SmallShineColor = a.GetColor(Resource.Styleable.ShineButton_small_shine_color, shineParams.SmallShineColor);
            shineParams.SmallShineOffsetAngle = a.GetFloat(Resource.Styleable.ShineButton_small_shine_offset_angle, shineParams.SmallShineOffsetAngle);
            shineParams.ShineSize = a.GetDimensionPixelSize(Resource.Styleable.ShineButton_shine_size, shineParams.ShineSize);

            a.Recycle();

            SourceColour = btnColor;
        }

        public void Init(Activity activity)
        {
            this.activity = activity;

            this.Click += (s, e) =>
            {
                if (!IsChecked)
                {
                    IsChecked = true;
                    ShowAnim();
                }
                else
                {
                    IsChecked = false;
                    Cancel();
                }

                if (OnCheckStateChangeListener != null)
                {
                    OnCheckStateChangeListener.CheckedChanged(this, IsChecked);
                }
            };
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Set whether the controls state is checked or unchecked, and whether an animation should run.
        /// </summary>
        /// <param name="hasBeenChecked"></param>
        /// <param name="anim"></param>
        public void SetChecked(bool hasBeenChecked, bool anim)
        {
            SetChecked(hasBeenChecked, anim, true);
        }

        /// <summary>
        /// SetChecked overflow method for a none callback, none animated state change.
        /// </summary>
        /// <param name="hasBeenChecked"></param>
        public void SetChecked(bool hasBeenChecked)
        {
            SetChecked(hasBeenChecked, false, false);
        }

        /// <summary>
        /// Cancel an on-going animation.
        /// </summary>
        public void Cancel()
        {
            SourceColour = btnColor;
            if (shakeAnimator != null)
            {
                shakeAnimator.End();
                shakeAnimator.Cancel();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="real"></param>
        /// <returns></returns>
        public int GetBottomHeight(bool real)
        {
            if (real)
            {
                return realBottomHeight;
            }
            return bottomHeight;
        }

        #endregion

        #region OVERRIDE METHODS

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            CalPixels();
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        #endregion

        #region PRIVATE CLASS METHODS

        /// <summary>
        /// Internal method for firing off the animations based on check status of control.
        /// </summary>
        /// <param name="hasBeenChecked"></param>
        /// <param name="anim"></param>
        /// <param name="callBack"></param>
        private void SetChecked(bool hasBeenChecked, bool anim, bool callBack)
        {
            IsChecked = hasBeenChecked;
            if (hasBeenChecked)
            {
                SourceColour = ButtonFillColour;
                IsChecked = true;
                if (anim) ShowAnim();
            }
            else
            {
                SourceColour = btnColor;
                IsChecked = false;
                if (anim) Cancel();
            }
            if (callBack)
            {
                OnListenerUpdate(hasBeenChecked);
            }
        }

        private void ShowAnim()
        {
            if (activity != null)
            {
                ViewGroup rootView = (ViewGroup)activity.FindViewById(Window.IdAndroidContent);
                shineView = new ShineView(activity, this, shineParams, RandomColourSelection);
                rootView.AddView(shineView, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
                DoShareAnim();
            }
            else
            {
                Log.Error(TAG, "Please init.");
            }
        }

        internal void RemoveView(View view)
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

        private void DoShareAnim()
        {
            shakeAnimator = ValueAnimator.OfFloat(0.4f, 1f, 0.9f, 1f);
            shakeAnimator.SetInterpolator(new LinearInterpolator());
            shakeAnimator.SetDuration(500);
            shakeAnimator.StartDelay = 180;

            Invalidate();

            shakeAnimator.Update += (s, e) =>
            {
                ScaleX = (float)e.Animation.AnimatedValue;
                ScaleY = (float)e.Animation.AnimatedValue;
            };

            shakeAnimator.AnimationStart += (s, e) =>
            {
                SourceColour = ButtonFillColour;
            };

            shakeAnimator.AnimationEnd += (s, e) =>
            {
                SourceColour = IsChecked ? ButtonFillColour : btnColor;
            };

            shakeAnimator.AnimationCancel += (s, e) =>
            {
                SourceColour = btnColor;
            };

            shakeAnimator.Start();
        }

        private void CalPixels()
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

        private void OnListenerUpdate(bool hasBeenChecked)
        {
            if (OnCheckStateChangeListener != null)
            {
                OnCheckStateChangeListener.CheckedChanged(this, hasBeenChecked);
            }
        }

        #endregion
    }
}