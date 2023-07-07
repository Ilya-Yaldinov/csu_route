using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class PointOfInterest
{
    private Vector2 buttonSize = new Vector2(20f, 20f);
    private Vector3 position;
    private Color btnColor;
    private Action<PointOfInterest> buttonEvent;


    public PopupInternalText curPopupText {get; private set;}
    public GameObject curButton {get; private set;}
    public PointProperties curProperties {get; private set;}

    public PointOfInterest(Vector3 position, Color color, Action<PointOfInterest> action, PopupInternalText pit)
    {
        this.position = position;
        this.btnColor = color;
        this.buttonEvent = action;
        this.curPopupText = pit;
        this.curProperties = new PointProperties();
        this.curProperties.X = position.x;
        this.curProperties.Y = position.y;  
        if(color == Color.black){
            curProperties.PointClass = 1;
        }
        else
        {
            curProperties.PointClass = 2;
        }
    }

    public PointOfInterest(PointProperties pointProperties, PopupInternalText pit, Color color, Action<PointOfInterest> action)
    {
        this.curProperties = pointProperties;
        this.position = new Vector3(((float)pointProperties.X), (float)pointProperties.Y);
        this.curPopupText = pit;
        this.btnColor = color;
        this.buttonEvent = action;
    }

    public GameObject CreatePoint()
    {
        curButton = new GameObject("Button"); // Создаем новый объект кнопки

        // Добавляем компоненты RectTransform и Button
        RectTransform rectTransform = curButton.AddComponent<RectTransform>();
        Button button = curButton.AddComponent<Button>();

        // Устанавливаем размер кнопки
        rectTransform.sizeDelta = buttonSize;

        // Устанавливаем позицию кнопки
        rectTransform.anchoredPosition = position;

        // Устанавливаем цвет фона кнопки
        Image buttonImage = curButton.AddComponent<Image>();
        buttonImage.color = btnColor;

        AddEventListener(curButton);

        return curButton;
    }

    private void AddEventListener(GameObject button)
    {
        button.AddComponent(typeof(EventTrigger));
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener( (eventData) => { 
            buttonEvent(this);
        });
        trigger.triggers.Add(entry);
    }
}