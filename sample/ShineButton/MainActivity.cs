using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using ShineButton.Classes;
using Android.Graphics;
using Xama.JTPorts.ShineButton.Models;

namespace ShineButton
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_main);

			Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

			FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            // grab our buttons
            var buttonLike = FindViewById<ShineButtonControl>(Resource.Id.po_like);
            var buttonGhandi = FindViewById<ShineButtonControl>(Resource.Id.po_ghandi);
            var buttonHeart = FindViewById<ShineButtonControl>(Resource.Id.po_heart);
            var buttonIronman = FindViewById<ShineButtonControl>(Resource.Id.po_ironman);
            var buttonSave = FindViewById<ShineButtonControl>(Resource.Id.po_save);
            var buttonSmile = FindViewById<ShineButtonControl>(Resource.Id.po_smile);
            var buttonStar = FindViewById<ShineButtonControl>(Resource.Id.po_star);

            // sets the shine animation to flash.
            buttonLike.EnableFlashing = true;

            // Supplies custom set of random animations
            buttonIronman.RandomColourSelection = new ColourSet(Color.Red, Color.DarkRed, Color.IndianRed, Color.PaleVioletRed, Color.MediumVioletRed, Color.OrangeRed);
            buttonIronman.ShineCount = 20;

            // this will force the shine animations to pick from a base set of 10 colours.
            buttonStar.AllowRandomColour = true;
        }

		public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            //View view = (View) sender;
            //Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }
	}
}

