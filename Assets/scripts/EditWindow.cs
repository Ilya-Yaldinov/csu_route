using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class EditWindow : MonoBehaviour
{
    private bool _isCabinetButtonActive = false;
    private bool _isInterestButtonActive = false;    
    public GameObject Save;
    
    public GameObject Popup;
    public TMP_InputField fieldFirst;
    public TMP_InputField fieldSecond;
    public TMP_InputField fieldThird;

    void Start()
    {
        AddEventListener(Save);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(_isCabinetButtonActive && !IsMouseOverUI())
            {
                SpawnButton(Color.black);
            }
            else if(_isInterestButtonActive && !IsMouseOverUI())
            {
                SpawnButton(Color.red);
            }
        }
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private Vector3 getPosition()
    {
        Vector3 pos = Input.mousePosition;
        Debug.Log(pos);
        pos.x -= Screen.width / 2;
        pos.y -= Screen.height / 2;

        return pos;
    }

    private void SpawnButton(Color color)
    {
        var pos = getPosition();
        PointOfInterest pointOfInterest = new PointOfInterest(pos, color, OpenPopup);
        var button = pointOfInterest.CreatePoint();

        button.transform.SetParent(transform, false);
    }

    public void CabinetPress()
    {
        _isCabinetButtonActive = !_isCabinetButtonActive;
    }

    public void InterestPress()
    {
        _isInterestButtonActive = !_isInterestButtonActive;
    }

    private void OpenPopup()
    {
        if(Popup != null)
        {
            Popup.SetActive(true);
        }
    }

    private void ClosePopup()
    {
        if(Popup != null)
        {
            Popup.SetActive(false);
        }
    }

    private void AddEventListener(GameObject obj)
    {
        obj.AddComponent(typeof(EventTrigger));
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener( (eventData) => { ClosePopup(); });
        trigger.triggers.Add(entry);
    }
}

class PointOfInterest
{
    private Vector2 _buttonSize = new Vector2(20f, 20f);
    private Vector3 _position;
    private Color _btnColor;
    private Action _buttonEvent;

    public PointOfInterest(Vector3 position, Color color, Action action)
    {
        this._position = position;
        this._btnColor = color;
        this._buttonEvent = action;
    }

    public GameObject CreatePoint()
    {
        GameObject buttonObject = new GameObject("Button"); // Создаем новый объект кнопки

        // Добавляем компоненты RectTransform и Button
        RectTransform rectTransform = buttonObject.AddComponent<RectTransform>();
        Button button = buttonObject.AddComponent<Button>();

        // Устанавливаем размер кнопки
        rectTransform.sizeDelta = _buttonSize;

        // Устанавливаем позицию кнопки
        rectTransform.anchoredPosition = _position;

        // Устанавливаем цвет фона кнопки
        Image buttonImage = buttonObject.AddComponent<Image>();
        buttonImage.color = _btnColor;

        AddEventListener(buttonObject);

        return buttonObject;
    }

    private void AddEventListener(GameObject button)
    {
        button.AddComponent(typeof(EventTrigger));
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener( (eventData) => { _buttonEvent(); });
        trigger.triggers.Add(entry);
    }
}
