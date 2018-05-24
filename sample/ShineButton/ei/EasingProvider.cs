using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ShineButton.ei
{
    class EasingProvider
    {

        public static float get(Ease ease, float elapsedTimeRate)
        {
            switch (ease)
            {
                case Ease.Linear:
                    return elapsedTimeRate;
                case Ease.QuadIn:
                    return getPowIn(elapsedTimeRate, 2);
                case Ease.QuadOut:
                    return getPowOut(elapsedTimeRate, 2);
                case Ease.QuadInOut:
                    return getPowInOut(elapsedTimeRate, 2);
                case Ease.CubicIn:
                    return getPowIn(elapsedTimeRate, 3);
                case Ease.CubicOut:
                    return getPowOut(elapsedTimeRate, 3);
                case Ease.CubicInOut:
                    return getPowInOut(elapsedTimeRate, 3);
                case Ease.QuartIn:
                    return getPowIn(elapsedTimeRate, 4);
                case Ease.QuartOut:
                    return getPowOut(elapsedTimeRate, 4);
                case Ease.QuartInOut:
                    return getPowInOut(elapsedTimeRate, 4);
                case Ease.QuintIn:
                    return getPowIn(elapsedTimeRate, 5);
                case Ease.QuintOut:
                    return getPowOut(elapsedTimeRate, 5);
                case Ease.QuintInOut:
                    return getPowInOut(elapsedTimeRate, 5);
                case Ease.SineIn:
                    return (float)(1f - Math.Cos(elapsedTimeRate * Math.PI / 2f));
                case Ease.SineOut:
                    return (float)Math.Sin(elapsedTimeRate * Math.PI / 2f);
                case Ease.SineInOut:
                    return (float)(-0.5f * (Math.Cos(Math.PI * elapsedTimeRate) - 1f));
                case Ease.BackIn:
                    return (float)(elapsedTimeRate * elapsedTimeRate * ((1.7 + 1f) * elapsedTimeRate - 1.7));
                case Ease.BackOut:
                    return (float)(--elapsedTimeRate * elapsedTimeRate * ((1.7 + 1f) * elapsedTimeRate + 1.7) + 1f);
                case Ease.BackInOut:
                    return getBackInOut(elapsedTimeRate, 1.7f);
                case Ease.CircIn:
                    return (float)-(Math.Sqrt(1f - elapsedTimeRate * elapsedTimeRate) - 1);
                case Ease.CircOut:
                    return (float)Math.Sqrt(1f - (--elapsedTimeRate) * elapsedTimeRate);
                case Ease.CircInOut:
                    if ((elapsedTimeRate *= 2f) < 1f)
                    {
                        return (float)(-0.5f * (Math.Sqrt(1f - elapsedTimeRate * elapsedTimeRate) - 1f));
                    }
                    return (float)(0.5f * (Math.Sqrt(1f - (elapsedTimeRate -= 2f) * elapsedTimeRate) + 1f));
                case Ease.BounceIn:
                    return getBounceIn(elapsedTimeRate);
                case Ease.BounceOut:
                    return getBounceOut(elapsedTimeRate);
                case Ease.BounceInOut:
                    if (elapsedTimeRate < 0.5f)
                    {
                        return getBounceIn(elapsedTimeRate * 2f) * 0.5f;
                    }
                    return getBounceOut(elapsedTimeRate * 2f - 1f) * 0.5f + 0.5f;
                case Ease.ElasticIn:
                    return getElasticIn(elapsedTimeRate, 1, 0.3);

                case Ease.ElasticOut:
                    return getElasticOut(elapsedTimeRate, 1, 0.3);

                case Ease.ElasticInOut:
                    return getElasticInOut(elapsedTimeRate, 1, 0.45);

                default:
                    return elapsedTimeRate;
            }
        }

        /// <summary>
        /// elapsedTimeRate Elapsed time / Total time, pow The exponent to use (ex. 3 would return a cubic ease) returns eased value
        /// </summary>
        /// <param name="elapsedTimeRate"></param>
        /// <param name="pow"></param>
        /// <returns></returns>
        private static float getPowIn(float elapsedTimeRate, double pow)
        {
            return (float)Math.Pow(elapsedTimeRate, pow);
        }

        /**
 * @param elapsedTimeRate Elapsed time / Total time
 * @param pow             pow The exponent to use (ex. 3 would return a cubic ease).
 * @return easedValue
 */
        private static float getPowOut(float elapsedTimeRate, double pow)
        {
            return (float)((float)1 - Math.Pow(1 - elapsedTimeRate, pow));
        }

        /**
  * @param elapsedTimeRate Elapsed time / Total time
  * @param pow             pow The exponent to use (ex. 3 would return a cubic ease).
  * @return easedValue
  */
        private static float getPowInOut(float elapsedTimeRate, double pow)
        {
            if ((elapsedTimeRate *= 2) < 1)
            {
                return (float)(0.5 * Math.Pow(elapsedTimeRate, pow));
            }

            return (float)(1 - 0.5 * Math.Abs(Math.Pow(2 - elapsedTimeRate, pow)));
        }

        /**
 * @param elapsedTimeRate Elapsed time / Total time
 * @param amount          amount The strength of the ease.
 * @return easedValue
 */
        private static float getBackInOut(float elapsedTimeRate, double amount)
        {
            amount *= 1.525;
            if ((elapsedTimeRate *= 2) < 1)
            {
                return (float)(0.5 * (elapsedTimeRate * elapsedTimeRate * ((amount + 1) * elapsedTimeRate - amount)));
            }
            return (float)(0.5 * ((elapsedTimeRate -= 2) * elapsedTimeRate * ((amount + 1) * elapsedTimeRate + amount) + 2));
        }

        /**
 * @param elapsedTimeRate Elapsed time / Total time
 * @return easedValue
 */
        private static float getBounceIn(float elapsedTimeRate)
        {
            return 1f - getBounceOut(1f - elapsedTimeRate);
        }

        /**
 * @param elapsedTimeRate Elapsed time / Total time
 * @return easedValue
 */
        private static float getBounceOut(double elapsedTimeRate)
        {
            if (elapsedTimeRate < 1 / 2.75)
            {
                return (float)(7.5625 * elapsedTimeRate * elapsedTimeRate);
            }
            else if (elapsedTimeRate < 2 / 2.75)
            {
                return (float)(7.5625 * (elapsedTimeRate -= 1.5 / 2.75) * elapsedTimeRate + 0.75);
            }
            else if (elapsedTimeRate < 2.5 / 2.75)
            {
                return (float)(7.5625 * (elapsedTimeRate -= 2.25 / 2.75) * elapsedTimeRate + 0.9375);
            }
            else
            {
                return (float)(7.5625 * (elapsedTimeRate -= 2.625 / 2.75) * elapsedTimeRate + 0.984375);
            }
        }

        /**
 * @param elapsedTimeRate Elapsed time / Total time
 * @param amplitude       Amplitude of easing
 * @param period          Animation of period
 * @return easedValue
 */
        private static float getElasticIn(float elapsedTimeRate, double amplitude, double period)
        {
            if (elapsedTimeRate == 0 || elapsedTimeRate == 1) return elapsedTimeRate;
            double pi2 = Math.PI * 2;
            double s = period / pi2 * Math.Asin(1 / amplitude);
            return (float)-(amplitude * Math.Pow(2f, 10f * (elapsedTimeRate -= 1f)) * Math.Sin((elapsedTimeRate - s) * pi2 / period));
        }

        /**
 * @param elapsedTimeRate Elapsed time / Total time
 * @param amplitude       Amplitude of easing
 * @param period          Animation of period
 * @return easedValue
 */
        private static float getElasticOut(float elapsedTimeRate, double amplitude, double period)
        {
            if (elapsedTimeRate == 0 || elapsedTimeRate == 1) return elapsedTimeRate;

            double pi2 = Math.PI * 2;
            double s = period / pi2 * Math.Asin(1 / amplitude);
            return (float)(amplitude * Math.Pow(2, -10 * elapsedTimeRate) * Math.Sin((elapsedTimeRate - s) * pi2 / period) + 1);
        }

        /**
 * @param elapsedTimeRate Elapsed time / Total time
 * @param amplitude       Amplitude of easing
 * @param period          Animation of period
 * @return easedValue
 */
        private static float getElasticInOut(float elapsedTimeRate, double amplitude, double period)
        {
            double pi2 = Math.PI * 2;

            double s = period / pi2 * Math.Asin(1 / amplitude);
            if ((elapsedTimeRate *= 2) < 1)
            {
                return (float)(-0.5f * (amplitude * Math.Pow(2, 10 * (elapsedTimeRate -= 1f)) * Math.Sin((elapsedTimeRate - s) * pi2 / period)));
            }
            return (float)(amplitude * Math.Pow(2, -10 * (elapsedTimeRate -= 1)) * Math.Sin((elapsedTimeRate - s) * pi2 / period) * 0.5 + 1);

        }

    }
}