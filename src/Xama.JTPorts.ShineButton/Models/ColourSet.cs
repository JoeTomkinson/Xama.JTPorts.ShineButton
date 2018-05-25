using Android.Graphics;

namespace Xama.JTPorts.ShineButton.Models
{
    public class ColourSet
    {
        internal Color[] ColourSelection = new Color[5];

        public ColourSet(Color colourOne, Color colourTwo, Color colourThree, Color colourFour, Color colourFive, Color colourSix)
        {
            ColourSelection[0] = colourOne;
            ColourSelection[0] = colourTwo;
            ColourSelection[0] = colourThree;
            ColourSelection[0] = colourFour;
            ColourSelection[0] = colourFive;
            ColourSelection[0] = colourSix;
        }
    }
}