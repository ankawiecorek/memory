using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;

namespace Memory
{
    [Activity(Label = "Memory", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        #region Constants

        private const int NumberOfCards = 16;
        private const string ImageButtonPrefix = "imageButton";
        private const string ImageFile = "img";

        #endregion

        #region Fields

        private JavaList<Card> deck;
        private bool isFirstCardShown;
        private int firstCardId;
        private int secondCardId;
        private System.Timers.Timer timer;

        #endregion

        #region Events

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            this.CreateDeck();
            this.BindEvents();
            this.isFirstCardShown = false;
        }

        void MatchCards()
        {
            System.Threading.Thread.Sleep(1000);
            RunOnUiThread(UpdateButtons);
        }

        void UpdateButtons()
        {
            this.deck[this.firstCardId].Hide();
            this.deck[this.secondCardId].Hide();
            int fistButtonId = this.GetResourceId(typeof(Resource.Id), ImageButtonPrefix + this.firstCardId.ToString());
            ImageButton fistButton = FindViewById<ImageButton>(fistButtonId);
            fistButton.SetImageResource(Resource.Drawable.card);
            int secondButtonId = this.GetResourceId(typeof(Resource.Id), ImageButtonPrefix + this.secondCardId.ToString());
            ImageButton secondButton = FindViewById<ImageButton>(secondButtonId);
            secondButton.SetImageResource(Resource.Drawable.card);
            this.firstCardId = -1;
            this.secondCardId = -1;
            this.isFirstCardShown = false;
            ButtonsEnabled(true);
        }

        void ButtonsEnabled(bool areEnabled)
        {
            for (int i = 0; i < NumberOfCards; i++)
            {
                int controlId = this.GetResourceId(typeof(Resource.Id), ImageButtonPrefix + i.ToString());
                ImageButton button = FindViewById<ImageButton>(controlId);
                button.Enabled = areEnabled;
            }
        }

        void button_Click(object sender, EventArgs e)
        {
            ImageButton button = sender as ImageButton;
            int cardId = 0;
            foreach (var field in typeof(Resource.Id).GetFields())
            {
                if (int.Parse(field.GetValue(typeof(Resource.Id)).ToString()) == button.Id)
                {
                    cardId = int.Parse(field.Name.Remove(field.Name.IndexOf(ImageButtonPrefix), ImageButtonPrefix.Length).ToString());
                    break;
                }
            }

            if (!this.deck[cardId].IsMatched)
            {
                if (this.deck[cardId].IsShown())
                {
                    button.SetImageResource(Resource.Drawable.card);
                    this.deck[cardId].Hide();
                    this.firstCardId = -1;
                    this.isFirstCardShown = false;
                }
                else
                {
                    var imageId = this.GetResourceId(typeof(Resource.Drawable), ImageFile + this.deck[cardId].GetValue());
                    button.SetImageResource(imageId);
                    this.deck[cardId].Show();
                    if (this.isFirstCardShown)
                    {
                        if (this.deck[this.firstCardId].GetValue() == this.deck[cardId].GetValue())
                        {
                            this.deck[this.firstCardId].IsMatched = true;
                            this.deck[cardId].IsMatched = true;
                            this.firstCardId = -1;
                            this.secondCardId = -1;
                            this.isFirstCardShown = false;
                        }
                        else
                        {
                            this.secondCardId = cardId;
                            ButtonsEnabled(false);
                            ThreadPool.QueueUserWorkItem(o => this.MatchCards());
                        }
                    }
                    else
                    {
                        this.firstCardId = cardId;
                        this.isFirstCardShown = true;
                    }
                }
            }
        }

        #endregion

        #region Private methods

        private void CreateDeck()
        {
            this.deck = new JavaList<Card>();
            for (int i = 0; i < NumberOfCards / 2; i++)
            {
                Card card1 = new Card(i);
                Card card2 = new Card(i);
                this.deck.Add(card1);
                this.deck.Add(card2);
            }

            this.Shuffle<Card>(this.deck);
        }

        private void BindEvents()
        {
            for (int i = 0; i < NumberOfCards; i++)
            {
                int controlId = this.GetResourceId(typeof(Resource.Id), ImageButtonPrefix + i.ToString());
                ImageButton button = FindViewById<ImageButton>(controlId);
                button.Click += button_Click;
            }
        }

        private int GetResourceId(Type resourceType, string controlName)
        {
            return int.Parse(resourceType.GetField(controlName).GetValue(resourceType).ToString());
        }

        private void Shuffle<T>(JavaList<T> list)
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

        #endregion
    }
}
