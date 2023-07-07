using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class EditWindow : MonoBehaviour
{
    public GameObject Save;
    public GameObject Popup;
    public TMP_InputField FieldFirst;
    public TMP_InputField FieldSecond;
    public TMP_InputField FieldThird;
    public TMP_Text Header;

    private PointOfInterest curPoint;
    private List<PointOfInterest> points;
    private bool isCabinetButtonActive = false;
    private bool isInterestButtonActive = false;
    private IOFileWork ioFile;
    
    void Start()
    {
        points = new List<PointOfInterest>();
        ioFile = new IOFileWork( @"\file.json");
        StartWithPoints();
    }

    private void StartWithPoints()
    {
        var properties = ioFile.Read();
        foreach(var item in properties)
        {
            PointOfInterest poi;
            if(item.PointClass == 1)
            {
                poi = new PointOfInterest(item, 
                    new PopupInternalText("Кабинет", "Введите номер кабинета", "Введите назначение кабинета", "Введите заведующего"),
                    Color.black, OpenPopup);
            }
            else
            {
                poi = new PointOfInterest(item, 
                    new PopupInternalText("Точка интереса", "Введите название", "Введите краткое описание", "Введите аналоги названия"),
                    Color.red, OpenPopup);
            }
            var button = poi.CreatePoint();
            points.Add(poi);
            button.transform.SetParent(transform, false);
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(isCabinetButtonActive && !IsMouseOverUI())
            {
                SpawnButton(Color.black, new PopupInternalText("Кабинет", "Введите номер кабинета", "Введите назначение кабинета", "Введите заведующего"));
            }
            else if(isInterestButtonActive && !IsMouseOverUI())
            {
                SpawnButton(Color.red, new PopupInternalText("Точка интереса", "Введите название", "Введите краткое описание", "Введите аналоги названия"));
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

    private void SpawnButton(Color color, PopupInternalText pit)
    {
        var pos = getPosition();
        PointOfInterest pointOfInterest = new PointOfInterest(pos, color, OpenPopup, pit);
        var button = pointOfInterest.CreatePoint();

        points.Add(pointOfInterest);

        button.transform.SetParent(transform, false);
    }

    public void CabinetPress()
    {
        isCabinetButtonActive = !isCabinetButtonActive;
    }

    public void InterestPress()
    {
        isInterestButtonActive = !isInterestButtonActive;
    }

    private void OpenPopup(PointOfInterest poi)
    {
        if(Popup != null)
        {
            curPoint = poi;

            Header.text = curPoint.curPopupText.Header;
            FieldFirst.placeholder.GetComponent<TextMeshProUGUI>().text = curPoint.curPopupText.HintFirst;
            FieldSecond.placeholder.GetComponent<TextMeshProUGUI>().text = curPoint.curPopupText.HintSecond;
            FieldThird.placeholder.GetComponent<TextMeshProUGUI>().text = curPoint.curPopupText.HintThird;
            
            FieldFirst.text = curPoint.curProperties.TextFirst;
            FieldSecond.text = curPoint.curProperties.TextSecond;
            FieldThird.text = curPoint.curProperties.TextThird;

            Popup.transform.SetAsLastSibling();
            Popup.SetActive(true);
        }
    }

    public void SaveAll()
    {   
        List<PointProperties> properties = new List<PointProperties>();
        points.ForEach(x => properties.Add(x.curProperties));
        ioFile.Write(properties);
    }

    private void ClosePopup()
    {
        if(Popup != null)
        {
            Popup.SetActive(false);
        }
    }

    public void SavePopup()
    {
        curPoint.curProperties.TextFirst = FieldFirst.text;
        curPoint.curProperties.TextSecond = FieldSecond.text;
        curPoint.curProperties.TextThird = FieldThird.text;

        ClosePopup();
    }

    public void DeletePoint()
    {
        DestroyImmediate(curPoint.curButton);
        ClosePopup();
    }
}
