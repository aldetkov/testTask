﻿using AxGrid.Base;
using AxGrid.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AxGrid.Test
{
    public class CardCollecView : MonoBehaviourExt
    {

        private static CardView _cashCardView; // поле под удаленную из списка карточку
        [SerializeField] private GameObject prefab;
        List<CardView> views;
        [SerializeField] private string fieldName;
        [SerializeField] private string cardsFieldName;

        [OnStart]
        public void start()
        {
            Settings.Model.EventManager.AddAction($"On{fieldName}Changed", Refresh); // подписываемся на изменения поля в моделе
            views = new List<CardView>();
        }



        [OnDelay(0.02f)] //задержка для того, чтобы в модели успела создаться переменная
        public void DelayStart()
        {
            Settings.Model.Get<List<string>>("fieldsNameListCardsAll").Add(fieldName);
            Settings.Model.Refresh("fieldsNameListCardsAll");
        }

        /// <summary>
        /// Создание новой карточки
        /// </summary>
        void Create(Card dto)
        {
            CardView cardView = Instantiate(prefab, transform.position, Quaternion.identity, transform).GetComponent<CardView>();
            cardView.InitUICard(dto.cardName, Resources.Load<Sprite>(dto.spriteName), dto.Value);
            views.Add(cardView);

        }

        /// <summary>
        /// Удаление карточки по id
        /// </summary>
        public void OnDelete(CardView cardView)
        {
            views.Remove(cardView);
            print("remove" + name);
            if (_cashCardView == null)
            {
                print("set");
                _cashCardView = cardView;
            }
            MovingCard(views);
        }
        void Refresh()
        {
            var newList = Model.Get<List<Card>>(fieldName);
            if (newList.Count > views.Count)
            {
                print("add1");
                if (_cashCardView != null)
                {
                    views.Add(_cashCardView);
                    _cashCardView.RectTransf.SetParent(transform);
                    print("add2");
                    _cashCardView = null;
                }
                else Create(newList[newList.Count - 1]);
            }
            MovingCard(views);
        }

        /// <summary>
        /// Плавно перемещает карточку на своё место
        /// </summary>
        public void MovingCard(List<CardView> prefabsList)
        {
            if (prefabsList.Count > 0)
            {
                MoveCard(prefabsList, Time.fixedDeltaTime * 3);


                // Если первая карточка не на своём месте, то двигаем карточки дальше
                if (Vector2.Distance(prefabsList[0].RectTransf.anchoredPosition,
                    new Vector2(-prefabsList[0].RectTransf.sizeDelta.x / 2 * (prefabsList.Count - 1), 0)) < 2f)
                {
                    MoveCard(prefabsList, 1);
                }
                else StartCoroutine(MovingCardCor(prefabsList));
            }
        }

        // Для движения раз в fixedDeltaTime;
        IEnumerator MovingCardCor(List<CardView> prefabsList)
        {
            yield return new WaitForFixedUpdate();
            MovingCard(prefabsList);
        }
        private void MoveCard(List<CardView> prefabsList, float rateLerpMove)
        {
            for (int i = 0; i < prefabsList.Count; i++)
            {
                float step = prefabsList[i].RectTransf.sizeDelta.x / 2; // шаг смещения
                Vector2 targetPos = new Vector2(-step * (prefabsList.Count - 1) + 2 * step * i, 0);
                prefabsList[i].RectTransf.anchoredPosition = Vector2.Lerp(prefabsList[i].RectTransf.anchoredPosition, targetPos, rateLerpMove);
            }
        }
    }
}