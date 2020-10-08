using System.Collections.Generic;
using AxGrid;
using AxGrid.FSM;

namespace AxGrid.Test
{

    [State("InitCards")]
    public class InitCardsState : FSMState
    {

        [Enter]
        public void InitModel()
        {
            Settings.Model.Set("Cards1", new List<Card>());
            Settings.Model.Set("Cards2", new List<Card>());
            Settings.Model.Set("CardsCount", 0);
            Parent.Change("ReadyCards");
        }

    }
}