using AxGrid.FSM;

namespace AxGrid.Hello.States
{
    [State("Init")]
    public class InitState : FSMState
    {
        [Enter]
        public void Enter()
        {
            Log.Info("Init objects");
            Settings.Model.Set("StartCounterValue", 10);
            Parent.Change("Ready");
        }
    }
}