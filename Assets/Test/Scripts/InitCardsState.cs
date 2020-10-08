using System.Collections.Generic;
using AxGrid.FSM;

namespace AxGrid.Test
{

    [State("InitCards")]
    public class InitCardsState : FSMState
    {

        [Enter]
        public void InitModel()
        {
            Settings.Model.Set("fieldsNameListCardsAll", new List<string>());
            Parent.Change("ReadyCards");
        }

    }
}