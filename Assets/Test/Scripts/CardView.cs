using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviourExt, IPointerClickHandler
{
    
    [SerializeField] private Text nameText = null;
    [SerializeField] private Image image = null;
    
    public int Value { get; private set; }
    public RectTransform RectTransf { get; private set; }

    public void InitUICard(string name, Sprite image, int id)
    {
        nameText.text = name;
        this.image.sprite = image;
        Value = id;
        RectTransf = GetComponent<RectTransform>();
    }
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
       //s Debug.Log("click" + name);
        Settings.Model.EventManager.Invoke("OnSelectCard", Value);
    }
}
