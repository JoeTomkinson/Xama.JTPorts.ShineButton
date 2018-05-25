using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using System;

namespace ShineButton.Classes
{
    public abstract class PorterImageView : Android.Support.V7.Widget.AppCompatImageView
    {
        private static String TAG = nameof(PorterImageView);
        private static PorterDuffXfermode PORTER_DUFF_XFERMODE = new PorterDuffXfermode(PorterDuff.Mode.DstIn);

        private Canvas maskCanvas;
        private Bitmap maskBitmap;
        private Paint maskPaint;
        private Canvas drawableCanvas;
        private Bitmap drawableBitmap;
        private Paint drawablePaint;
        private Color paintColor = Color.Gray;
        private bool invalidated = true;

        #region PUBLIC PROPERTIES

        /// <summary>
        /// 
        /// </summary>
        public Color SourceColour
        {
            get { return paintColor; }
            set
            {
                paintColor = value;
                SetImageDrawable(new ColorDrawable(value));

                if (drawablePaint != null)
                {
                    drawablePaint.Color = value;
                    Invalidate();
                }
            }
        }

        #endregion

        #region PUBLIC CONSTRUCTORS

        public PorterImageView(Context context) : base(context)
        {
            Setup(context, null, 0);
        }

        public PorterImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Setup(context, attrs, 0);
        }

        public PorterImageView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Setup(context, attrs, 0);
        }

        #endregion

        internal void Setup(Context context, IAttributeSet attrs, int defStyle)
        {
            if (GetScaleType() == ScaleType.FitCenter)
            {
                SetScaleType(ScaleType.CenterCrop);
            }

            maskPaint = new Paint(PaintFlags.AntiAlias);
            maskPaint.Color = Color.Black;
        }

        #region CLASS OVERRIDES

        public override void Invalidate()
        {
            invalidated = true;
            base.Invalidate();
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            CreateMaskCanvas(w, h, oldw, oldh);
        }

        protected override void OnDraw(Canvas canvas)
        {
            if (!IsInEditMode)
            {
                int saveCount = canvas.SaveLayer(new RectF(0.0f, 0.0f, Width, Height), null);

                try
                {
                    if (invalidated)
                    {
                        Drawable drawable = Drawable;
                        if (drawable != null)
                        {
                            invalidated = false;
                            Matrix imageMatrix = ImageMatrix;
                            if (imageMatrix == null)
                            {
                                drawable.Draw(drawableCanvas);
                            }
                            else
                            {
                                int drawableSaveCount = drawableCanvas.SaveCount;
                                drawableCanvas.Save();
                                drawableCanvas.Concat(imageMatrix);
                                drawable.Draw(drawableCanvas);
                                drawableCanvas.RestoreToCount(drawableSaveCount);
                            }

                            drawablePaint.Reset();
                            drawablePaint.FilterBitmap = false;
                            drawablePaint.SetXfermode(PORTER_DUFF_XFERMODE);
                            drawableCanvas.DrawBitmap(maskBitmap, 0.0f, 0.0f, drawablePaint);
                        }
                    }

                    if (!invalidated)
                    {
                        drawablePaint.SetXfermode(null);
                        canvas.DrawBitmap(drawableBitmap, 0.0f, 0.0f, drawablePaint);
                    }
                }
                catch (Exception e)
                {
                    String log = "Exception occured while drawing " + Id;
                    Log.Error(TAG, log, e);
                }
                finally
                {
                    canvas.RestoreToCount(saveCount);
                }
            }
            else
            {
                base.OnDraw(canvas);
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if (widthMeasureSpec == 0)
            {
                widthMeasureSpec = 50;
            }
            if (heightMeasureSpec == 0)
            {
                heightMeasureSpec = 50;
            }

            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        #endregion

        private void CreateMaskCanvas(int width, int height, int oldw, int oldh)
        {
            bool sizeChanged = width != oldw || height != oldh;
            bool isValid = width > 0 && height > 0;

            if (isValid && (maskCanvas == null || sizeChanged))
            {
                maskCanvas = new Canvas();
                maskBitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
                maskCanvas.SetBitmap(maskBitmap);

                maskPaint.Reset();
                PaintMaskCanvas(maskCanvas, maskPaint, width, height);

                drawableCanvas = new Canvas();
                drawableBitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
                drawableCanvas.SetBitmap(drawableBitmap);
                drawablePaint = new Paint(PaintFlags.AntiAlias);
                drawablePaint.Color = paintColor;
                invalidated = true;
            }
        }

        protected abstract void PaintMaskCanvas(Canvas maskCanvas, Paint maskPaint, int width, int height);
    }
}