using AxGrid;
using AxGrid.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AxGrid.Test
{

    [RequireComponent(typeof(GraphicRaycaster))] // необходим для определения клика по карточке
    public class UIPanelCard : MonoBehaviourExt
    {
        [Header("Панели для хранение карточек")]
        [SerializeField] private RectTransform panelDown = null;
        [SerializeField] private RectTransform panelTop = null;

        [Header("Префаб карточки")]
        [SerializeField] private GameObject prefab = null;

        [Header("основной евент систем")]
        public EventSystem eventSystem;

        // возможно от это стоит избавиться, но я не представляю как
        List<RectTransform> prefabsDown;
        List<RectTransform> prefabsTop;

        GraphicRaycaster raycaster;
        PointerEventData mousePosEventData;


        [OnStart]
        public void start()
        {
            raycaster = GetComponent<GraphicRaycaster>();
            Settings.Model.EventManager.AddAction("OnCardsCountChanged", OnAddUICard);

            prefabsDown = new List<RectTransform>();
            prefabsTop = new List<RectTransform>();

        }

        [OnUpdate]
        public void update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                OnMouseTap();
            }
        }

        [OnDestroy]
        public void destroy()
        {
            Settings.Model.EventManager.RemoveAction("OnCardsCountChanged", OnAddUICard);
        }


        public void OnAddUICard()
        {
            List<Card> cards = Settings.Model.Get("Cards1") as List<Card>;
            if (cards.Count > 0)
            {
                Card cardModel = cards[cards.Count - 1];
                GameObject cardGO = Instantiate(prefab, panelDown.transform.position, Quaternion.identity, panelDown);
                cardGO.GetComponent<UICard>().InitUICard(cardModel.cardName, Resources.Load<Sprite>(cardModel.spriteName));
                prefabsDown.Add(cardGO.GetComponent<RectTransform>());
                OnChangePosAll();
            }
        }

        private void OnMouseTap()
        {
            //считываем попадания при нажатии
            mousePosEventData = new PointerEventData(eventSystem);
            mousePosEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(mousePosEventData, results);

            // если было попадаение по карточки, то ищем её в списке нижних карточек
            int indexCard = results.Count > 0 ? prefabsDown.IndexOf(results[0].gameObject.GetComponent<RectTransform>()) : -1;

            if (indexCard >= 0)
                ChangeListCard(prefabsDown, "Cards1", prefabsTop, "Cards2", panelTop, indexCard);
            // если не нашли там, то проверяем в списке верхних карточек
            else if ((indexCard = results.Count > 0 ? prefabsTop.IndexOf(results[0].gameObject.GetComponent<RectTransform>()) : -1) >= 0)
                ChangeListCard(prefabsTop, "Cards2", prefabsDown, "Cards1", panelDown, indexCard);


        }

        //*** Возможно здесь стоило делать всё через данный в модели без списков в этом классе, но как нам тогда двигать карточки потом? 
        //*** Заносить RectTransform в класс Card? Вот этот момент был сложен для меня, поэтому пока сделал так
        /// <summary>
        /// Переносит карточку в другой список
        /// </summary>
        private void ChangeListCard(List<RectTransform> outList, string outNameModel, List<RectTransform> inList, string inNameModel, RectTransform parent, int index)
        {
            inList.Add(outList[index]);

            // вносим изменения в модель
            Settings.Model.Get<List<Card>>(inNameModel).Add(Settings.Model.Get<List<Card>>(outNameModel)[index]);
            Settings.Model.Get<List<Card>>(outNameModel).RemoveAt(index);

            // Проверка правильности отражения данных модели
            print($"Cards1.Count: {Settings.Model.Get<List<Card>>(inNameModel).Count}");
            print($"Cards2.Count: {Settings.Model.Get<List<Card>>(outNameModel).Count}");

            // вносим изменения в UI
            outList[index].SetParent(parent);
            outList.RemoveAt(index);
            OnChangePosAll();
        }


        public void OnChangePosAll()
        {
            MovingCard(prefabsTop);
            MovingCard(prefabsDown);
        }

        /// <summary>
        /// Плавно перемещает карточку на своё место
        /// </summary>
        public void MovingCard(List<RectTransform> prefabsList)
        {
            if (prefabsList.Count > 0)
            {
                MoveCard(prefabsList, Time.fixedDeltaTime * 3);

                // Если первая карточка не на своём месте, то двигаем карточки дальше
                if (Vector2.Distance(prefabsList[0].anchoredPosition, new Vector2(-prefabsList[0].sizeDelta.x / 2 * (prefabsList.Count - 1), 0)) < 1f)
                {
                    MoveCard(prefabsList, 1);
                }
                else StartCoroutine(MovingCardCor(prefabsList));
            }
        }

        // Для движения раз в fixedDeltaTime;
        IEnumerator MovingCardCor(List<RectTransform> prefabsList)
        {
            yield return new WaitForFixedUpdate();
            MovingCard(prefabsList);
        }
        private void MoveCard(List<RectTransform> prefabsList, float rateLerpMove)
        {
            for (int i = 0; i < prefabsList.Count; i++)
            {
                float step = prefabsList[i].sizeDelta.x / 2; // шаг смещения
                Vector2 targetPos = new Vector2(-step * (prefabsList.Count - 1) + 2 * step * i, 0);
                prefabsList[i].anchoredPosition = Vector2.Lerp(prefabsList[i].anchoredPosition, targetPos, rateLerpMove);
            }
        }
    }
}