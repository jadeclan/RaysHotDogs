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
using RaysHotDogs.Core.Model;
using RaysHotDogs.Core.Service;
using RaysHotDogs.Adapters;
using RaysHotDogs.Fragments;

namespace RaysHotDogs
{
    [Activity(Label = "HotDogMenuActivity")]
    public class HotDogMenuActivity : Activity
    {
        private ListView hotDogListView;
        private List<HotDog> allHotDogs;
        private HotDogDataService hotDogDataService;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.HotDogMenuView);

            // Specify you want to use tabs navigation mode
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            AddTab("Favorites", Resource.Drawable.Icon, new FavoriteHotDogFragment());
            AddTab("Meat Lovers", Resource.Drawable.Icon, new MeatLoversFragment());
            AddTab("Veggie Lovers", Resource.Drawable.Icon, new VeggieLoversFragment());
        }
        private void AddTab(string tabText, int iconRsourceId, Fragment view)
        {
            var tab = this.ActionBar.NewTab();
            tab.SetText(tabText);
            tab.SetIcon(iconRsourceId);

            tab.TabSelected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                var fragment = this.FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                {
                    e.FragmentTransaction.Remove(fragment);
                }
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
            };

            tab.TabUnselected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                e.FragmentTransaction.Remove(view);
            };
            // Add tab to action bar tab collection
            this.ActionBar.AddTab(tab);
        }


        private void HotDogListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var hotDog = allHotDogs[e.Position];
            var intent = new Intent(this, typeof(HotDogDetailActivity));
            intent.PutExtra("selectedHotDogId", hotDog.HotDogId);

            StartActivityForResult(intent, 100); //The second parameter is the request code
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if(resultCode == Result.Ok && requestCode == 100)
            {
                var selectedHotDog = hotDogDataService.GetHotDogById(data.GetIntExtra("selectedHotDogId",0));
                var dialog = new AlertDialog.Builder(this);
                dialog.SetTitle("Confirmation");
                dialog.SetMessage(string.Format("You've add {0} time(s) the {1}", data.GetIntExtra("amount",0), allHotDogs[data.GetIntExtra("selectedHotDogId",0)].Name));
                dialog.Show();
            }
        }
    }
}