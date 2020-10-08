using AxGrid.Base;
using AxGrid.Hello.States;
using UnityEngine;

namespace AxGrid.Hello
{
    public class HelloMain : MonoBehaviourExt // Из за особенностей юнити нельзя использовать базовые методы Start, Avake, Update итп
    {
        [OnAwake]
        public void Init()
        {
            Settings.Fsm = new FSM.FSM(); // Создали FSM
            Settings.Fsm.Add(new InitState()); // Добавили состояния
            Settings.Fsm.Add(new ReadyState());
        }

        [OnStart]
        public void StartFSM()
        {
            // Запустим FSM
            Settings.Fsm.Start("Init");
        }

        [OnUpdate]
        public void UpdateFSM()
        {
            Settings.Fsm.Update(Time.deltaTime);
        }
    }
}
