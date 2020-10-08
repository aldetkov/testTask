using UnityEngine;

namespace AxGrid.Test
{

    public class Card
    {
        public int value;
        public string cardName;
        public string spriteName;

        public Card()
        {
            value = Random.Range(0, 10);
            cardName = "Gold";
            spriteName = "red_card_06";
        }

    }
}