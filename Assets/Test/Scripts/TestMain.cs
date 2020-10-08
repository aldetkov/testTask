using AxGrid.Base;
using AxGrid.Hello.States;
using System.Collections.Generic;
using UnityEngine;

namespace AxGrid.Test
{
    public class TestMain : MonoBehaviourExt
    {
        [OnAwake]
        public void Init()
        {
            Settings.Fsm = new FSM.FSM(); // Создали FSM
            Settings.Fsm.Add(new InitCardsState()); // Добавили состояния
            Settings.Fsm.Add(new ReadyCardsState());
        }

        [OnStart]
        public void StartFSM()
        {
            // Запустим FSM
            Settings.Fsm.Start("InitCards");
        }

        [OnUpdate]
        public void UpdateFSM()
        {
            Settings.Fsm.Update(Time.deltaTime);
        }
    }
}