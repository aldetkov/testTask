using AxGrid.FSM;
using AxGrid.Model;

namespace AxGrid.Hello.States
{
    [State("Ready")]
    public class ReadyState : FSMState
    {
        [Enter]
        public void Enter()
        {
            
        }

        [Bind]
        public void OnBtn(string name)
        {
            switch (name)
            {
                case "Inc":
                    Settings.Model.Inc("StartCounterValue");
                    break;
                case "Dec":
                    if (Settings.Model.GetInt("StartCounterValue", 0) > 0)
                        Settings.Model.Dec("StartCounterValue");
                    break;
            }
        }
    }
}