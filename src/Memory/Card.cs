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

namespace Memory
{
    public class Card
    {
        private CardState State;

        private int CardValue;

        public Card(int value)
        {
            this.State = CardState.Hiden;
            this.CardValue = value;
        }

        public void Show()
        {
            this.State = CardState.Shown;
        }

        public void Hide()
        {
            this.State = CardState.Hiden;
        }

        public bool IsShown()
        {
            if (this.State == CardState.Shown)
            {
                return true;
            }

            return false;
        }

        public int GetValue()
        {
            return this.CardValue;
        }
    }
}