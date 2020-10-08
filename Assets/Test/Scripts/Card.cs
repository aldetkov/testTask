using UnityEngine;

namespace AxGrid.Test
{

    public class Card
    {
        static int id;
        public int Value { get; private set; }
        public string cardName;
        public string spriteName;

        public Card()
        {
            Value = id;
            id++;
            cardName = "Gold";
            spriteName = "red_card_06";
        }

    }
}