using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;
using System.Collections.Generic;
using UnityEngine;

namespace AxGrid.Test
{

    [State("ReadyCards")]
    public class ReadyCardsState : FSMState
    {
        [Bind]
        public void OnBtn(string name)
        {
            switch (name)
            {
                case "Add":
                    Settings.Model.Get<List<Card>>("Cards1").Add(new Card());
                    Settings.Model.Inc("CardsCount");
                    break;
            }
        }
    }
}