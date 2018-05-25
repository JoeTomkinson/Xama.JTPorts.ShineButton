using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Views;
using System;
using Xama.JTPorts.EasingInterpolator;

namespace ShineButton.Classes
{
    public class ShineView : View
    {
        #region CLASS LEVEL VARIABLES

        private static readonly String TAG = nameof(ShineView);

        private static long FRAME_REFRESH_DELAY = 25; //default 10ms ,change to 25ms for saving cpu.

        private ShineAnimator shineAnimator;
        private ValueAnimator clickAnimator;

        private ShineButtonControl shineButton;
        private Paint paint;
        private Paint paint2;
        private Paint paintSmall;

        private int colorCount
        {
            get { return colorRandom.Length; }
        }

        private static Color[] colorRandom = new Color[10];

        private int shineCount;
        private float smallOffsetAngle;
        private float turnAngle;
        private long animDuration;
        private long clickAnimDuration;
        private float shineDistanceMultiple;
        private Color smallShineColor = colorRandom[0];
        private Color bigShineColor = colorRandom[1];

        private int shineSize = 0;

        private bool allowRandomColor = false;
        private bool enableFlashing = false;

        private RectF rectF = new RectF();
        private RectF rectFSmall = new RectF();

        private Random random = new Random();
        private int centerAnimX;
        private int centerAnimY;
        private int btnWidth;
        private int btnHeight;

        private double thirdLength;
        private float value;
        private float clickValue = 0;
        private bool isRun = false;
        private float distanceOffset = 0.2f;

        #endregion

        #region PUBLIC CONSTRUCTOR

        public ShineView(Context context, ShineButtonControl shineButton, ShineParams shineParams) : base(context)
        {
            InitShineParams(shineParams, shineButton);

            this.shineAnimator = new ShineAnimator(animDuration, shineDistanceMultiple, clickAnimDuration);
            ValueAnimator.FrameDelay = FRAME_REFRESH_DELAY;
            this.shineButton = shineButton;

            paint = new Paint();
            paint.Color = bigShineColor;
            paint.StrokeWidth = 20;
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeCap = Paint.Cap.Round;

            paint2 = new Paint();
            paint2.Color = Color.White;
            paint2.StrokeWidth = 20;
            paint2.StrokeCap = Paint.Cap.Round;

            paintSmall = new Paint();
            paintSmall.Color = smallShineColor;
            paintSmall.StrokeWidth = 10;
            paintSmall.SetStyle(Paint.Style.Stroke);
            paintSmall.StrokeCap = Paint.Cap.Round;

            clickAnimator = ValueAnimator.OfFloat(0f, 1.1f);
            ValueAnimator.FrameDelay = FRAME_REFRESH_DELAY;
            clickAnimator.SetDuration(clickAnimDuration);
            clickAnimator.SetInterpolator(new EasingInterpolator(Ease.QuartOut));

            clickAnimator.Update += (s, e) =>
            {
                clickValue = (float)e.Animation.AnimatedValue;
                Invalidate();
            };

            clickAnimator.AnimationEnd += (s, e) =>
            {
                clickValue = 0;
                Invalidate();
            };

            shineAnimator.AnimationEnd += (s, e) =>
            {
                shineButton.RemoveView(this);
            };
        }

        #endregion

        #region INITIALISATION LOGIC

        private void InitShineParams(ShineParams shineParams, ShineButtonControl shineButton)
        {
            shineCount = shineParams.ShineCount;
            turnAngle = shineParams.ShineTurnAngle;
            smallOffsetAngle = shineParams.SmallShineOffsetAngle;
            enableFlashing = shineParams.EnableFlashing;
            allowRandomColor = shineParams.AllowRandomColor;
            shineDistanceMultiple = shineParams.ShineDistanceMultiple;
            animDuration = shineParams.AnimDuration;
            clickAnimDuration = shineParams.ClickAnimDuration;
            smallShineColor = shineParams.SmallShineColor;
            bigShineColor = shineParams.BigShineColor;
            shineSize = shineParams.ShineSize;

            if (smallShineColor == 0)
            {
                smallShineColor = colorRandom[6];
            }

            if (bigShineColor == 0)
            {
                bigShineColor = shineButton.BigShineColour;
            }
        }

        #endregion

        #region METHOD OVERRIDES

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            for (int i = 0; i < shineCount; i++)
            {
                if (allowRandomColor)
                {
                    paint.Color = colorRandom[Math.Abs(colorCount / 2 - i) >= colorCount ? colorCount - 1 : Math.Abs(colorCount / 2 - i)];
                }
                canvas.DrawArc(rectF, 360f / shineCount * i + 1 + ((value - 1) * turnAngle), 0.1f, false, GetConfigPaint(paint));
            }

            for (int i = 0; i < shineCount; i++)
            {
                if (allowRandomColor)
                {
                    paint.Color = colorRandom[Math.Abs(colorCount / 2 - i) >= colorCount ? colorCount - 1 : Math.Abs(colorCount / 2 - i)];
                }
                canvas.DrawArc(rectFSmall, 360f / shineCount * i + 1 - smallOffsetAngle + ((value - 1) * turnAngle), 0.1f, false, GetConfigPaint(paintSmall));

            }
            paint.StrokeWidth = btnWidth * (clickValue) * (shineDistanceMultiple - distanceOffset);
            if (clickValue != 0)
            {
                paint2.StrokeWidth = btnWidth * (clickValue) * (shineDistanceMultiple - distanceOffset) - 8;
            }
            else
            {
                paint2.StrokeWidth = 0;
            }

            canvas.DrawPoint(centerAnimX, centerAnimY, paint);
            canvas.DrawPoint(centerAnimX, centerAnimY, paint2);

            if (shineAnimator != null && !isRun)
            {
                isRun = true;
                ShowAnimation(shineButton);
            }
        }

        #endregion

        #region PRIVATE METHODS

        private void ShowAnimation(ShineButtonControl shineButton)
        {
            btnWidth = shineButton.Width;
            btnHeight = shineButton.Height;
            thirdLength = GetThirdLength(btnHeight, btnWidth);
            int[] location = new int[2];
            shineButton.GetLocationInWindow(location);

            Rect visibleFrame = new Rect();

            if (Utils.IsWindowsNotLimit(shineButton.activity))
            {
                shineButton.activity.Window.DecorView.GetLocalVisibleRect(visibleFrame);
            }
            else
            {
                shineButton.activity.Window.DecorView.GetWindowVisibleDisplayFrame(visibleFrame);
            }

            centerAnimX = location[0] + btnWidth / 2 - visibleFrame.Left; // If navigation bar is not displayed on left, visibleFrame.left is 0.

            if (Utils.IsTranslucentNavigation(shineButton.activity))
            {
                if (Utils.IsFullScreen(shineButton.activity))
                {
                    centerAnimY = visibleFrame.Height() - shineButton.GetBottomHeight(false) + btnHeight / 2;
                }
                else
                {
                    centerAnimY = visibleFrame.Height() - shineButton.GetBottomHeight(true) + btnHeight / 2;
                }
            }
            else
            {
                centerAnimY = MeasuredHeight - shineButton.GetBottomHeight(false) + btnHeight / 2;
            }

            shineAnimator.Update += (s, e) =>
            {
                value = (float)e.Animation.AnimatedValue;

                if (shineSize != 0 && shineSize > 0)
                {
                    paint.StrokeWidth = (shineSize) * (shineDistanceMultiple - value);
                    paintSmall.StrokeWidth = ((float)shineSize / 3 * 2) * (shineDistanceMultiple - value);
                }
                else
                {
                    paint.StrokeWidth = (btnWidth / 2) * (shineDistanceMultiple - value);
                    paintSmall.StrokeWidth = (btnWidth / 3) * (shineDistanceMultiple - value);
                }

                rectF.Set(centerAnimX - (btnWidth / (3 - shineDistanceMultiple) * value), centerAnimY - (btnHeight / (3 - shineDistanceMultiple) * value), centerAnimX + (btnWidth / (3 - shineDistanceMultiple) * value), centerAnimY + (btnHeight / (3 - shineDistanceMultiple) * value));
                rectFSmall.Set(centerAnimX - (btnWidth / ((3 - shineDistanceMultiple) + distanceOffset) * value), centerAnimY - (btnHeight / ((3 - shineDistanceMultiple) + distanceOffset) * value), centerAnimX + (btnWidth / ((3 - shineDistanceMultiple) + distanceOffset) * value), centerAnimY + (btnHeight / ((3 - shineDistanceMultiple) + distanceOffset) * value));

                Invalidate();
            };

            shineAnimator.StartAnim(this, centerAnimX, centerAnimY);
            clickAnimator.Start();
        }

        private Paint GetConfigPaint(Paint paint)
        {
            if (enableFlashing)
            {
                paint.Color = colorRandom[random.Next(colorCount - 1)];
            }

            return paint;
        }

        private double GetThirdLength(int btnHeight, int btnWidth)
        {
            int all = btnHeight * btnHeight + btnWidth * btnWidth;
            return Math.Sqrt(all);
        }

        #endregion

        public class ShineParams
        {
            public ShineParams(Color[] randomColors = null)
            {
                if (randomColors == null)
                {
                    colorRandom[0] = Color.ParseColor("#FFFF99");
                    colorRandom[1] = Color.ParseColor("#FFCCCC");
                    colorRandom[2] = Color.ParseColor("#996699");
                    colorRandom[3] = Color.ParseColor("#FF6666");
                    colorRandom[4] = Color.ParseColor("#FFFF66");
                    colorRandom[5] = Color.ParseColor("#F44336");
                    colorRandom[6] = Color.ParseColor("#666666");
                    colorRandom[7] = Color.ParseColor("#CCCC00");
                    colorRandom[8] = Color.ParseColor("#666666");
                    colorRandom[9] = Color.ParseColor("#999933");
                }
                else
                {
                    colorRandom = randomColors;
                }
            }

            public bool AllowRandomColor = false;
            public long AnimDuration = 1500;
            public Color BigShineColor = Color.White;
            public long ClickAnimDuration = 200;
            public bool EnableFlashing = false;
            public int ShineCount = 7;
            public float ShineTurnAngle = 20;
            public float ShineDistanceMultiple = 1.5f;
            public float SmallShineOffsetAngle = 20;
            public Color SmallShineColor = Color.White;
            public int ShineSize = 0;

            /// <summary>
            /// Exposed random colour array to allow selection of own random colours.
            /// </summary>
            public Color[] RandomColourSelection
            {
                get { return colorRandom; }
                set { colorRandom = value; }
            }
        }
    }
}