using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Memory
{
    [Activity(Label = "Memory", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        private string cardSuiteImage = "card.png";
        private string cardImage = "img?.png";

        private JavaList<Card> Talia;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            this.Talia = new JavaList<Card>();
            for (int i = 0; i < 8; i++)
            {
                Card card1 = new Card(i);
                Card card2 = new Card(i);
                this.Talia.Add(card1);
                this.Talia.Add(card2);
            }

            Shuffle<Card>(this.Talia);
        }

        public void Shuffle<T>(JavaList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public void ClickAction(View view)
        {
        }
    }
}

