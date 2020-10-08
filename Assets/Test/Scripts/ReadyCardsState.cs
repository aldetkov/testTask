using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace AxGrid.Test
{

    [State("ReadyCards")]
    public class ReadyCardsState : FSMState
    {

        public ReadyCardsState()
        {
            Settings.Model.EventManager.AddAction("OnSelectCard", new DEventMethod(OnMoveCard)); // подписываемся на событие выбора карточки
            Settings.Model.EventManager.AddAction("OnfieldsNameListCardsAllChanged", OnCreateNewListCard); // подписываемся на событие создания нового списка карточек
        }
        [Bind]
        public void OnBtn(string name)
        {
            switch (name)
            {
                case "Add":
                    List<string> fieldNames = Settings.Model.Get<List<string>>("fieldsNameListCardsAll"); 
                    if (fieldNames.Count > 0)
                    {
                        Settings.Model.Get<List<Card>>(fieldNames[0]).Add(new Card()); // создаём карточку в первом списке
                        Settings.Model.Refresh(fieldNames[0]);
                    }
                    break;
            }
        }

        // создаём новый список с карточками
        public void OnCreateNewListCard()
        {
            List<string> fieldNames = Settings.Model.Get<List<string>>("fieldsNameListCardsAll");
            if (fieldNames.Count >0) Settings.Model.Set(fieldNames[fieldNames.Count - 1], new List<Card>());
        }

        /// <summary>
        /// Ищем карточку по которой нажали
        /// </summary>
        public void OnMoveCard(params object[] array)
        {
            int id = array.Length > 0 ? (int)array[0]:-1;
            List<string> fieldNames = Settings.Model.Get<List<string>>("fieldsNameListCardsAll");

            // ищем карточку по id во всех списках
            for (int i = 0; i < fieldNames.Count; i++)
            {
                int indexCard = FindCard(Settings.Model.Get<List<Card>>(fieldNames[i]), id); 

                if (indexCard != -1)
                {
                    // перемещаем карточку в другой список
                    Settings.Model.Get<List<Card>>(fieldNames[i<fieldNames.Count-1?i+1:0]).Add(new Card());
                    Settings.Model.Refresh(fieldNames[i<fieldNames.Count-1?i+1:0]);
                    Settings.Model.Get<List<Card>>(fieldNames[i]).RemoveAt(indexCard);
                    Settings.Model.Refresh(fieldNames[i]);
                    break;
                }
            }

        }
        /// <summary>
        /// Поиск карточки по id
        /// </summary>
        private int FindCard(List<Card> cards, int id) 
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].Value == id) return i;
            }
            return -1;
        }
    }
}