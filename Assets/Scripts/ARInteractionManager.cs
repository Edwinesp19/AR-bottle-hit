using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARInteractionManager : MonoBehaviour
{
    [SerializeField] private Camera aRCamera;
    private ARRaycastManager aRRaycastManager;
    private List<ARRaycastHit> aRRaycastHits = new List<ARRaycastHit>();

    private GameObject aRPointer;
    private GameObject item3DModel;

    private GameObject itemSelected;

    private bool isInitialPosition;

    private bool IsOverUI;
    private bool IsOver3DModel;

    private Vector2 initialTouchPos;

    public GameObject Item3DModel
    {
        get => item3DModel;
        set
        {
            item3DModel = value;
            item3DModel.transform.position = aRPointer.transform.position;
            item3DModel.transform.parent = aRPointer.transform;
            aRPointer.transform.rotation = Quaternion.Euler(aRPointer.transform.eulerAngles.x, item3DModel.transform.eulerAngles.y, item3DModel.transform.eulerAngles.z);
            isInitialPosition = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        aRPointer = transform.GetChild(0).gameObject;
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        GameManager.instance.OnMoreMenu += SetItemPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInitialPosition)
        {
            Vector2 middlePointScreen = new Vector2(Screen.width / 2, Screen.height / 2);
            aRRaycastManager.Raycast(middlePointScreen, aRRaycastHits, TrackableType.Planes);
            if (aRRaycastHits.Count > 0)
            {
                transform.position = aRRaycastHits[0].pose.position;
                transform.rotation = aRRaycastHits[0].pose.rotation;
                aRPointer.SetActive(true);
                isInitialPosition = false;
            }
        }
        if (Input.touchCount > 0)
        {
            Touch touchOne = Input.GetTouch(0);

            if (touchOne.phase == TouchPhase.Began)
            {
                var touchPosition = touchOne.position;
                IsOverUI = isTapOverUI(touchPosition);
                IsOver3DModel = isTapOver3DModel(touchPosition);

            }
            if (touchOne.phase == TouchPhase.Moved)
            {
                if (aRRaycastManager.Raycast(touchOne.position, aRRaycastHits, TrackableType.Planes))
                {
                    Pose hitPose = aRRaycastHits[0].pose;
                    if (!IsOverUI && IsOver3DModel)
                    {
                        transform.position = hitPose.position;
                        aRPointer.transform.position = hitPose.position;
                    }
                }
            }

            if (Input.touchCount == 2)
            {
                Touch touchTwo = Input.GetTouch(1);
                if (touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
                {
                    initialTouchPos = touchTwo.position - touchOne.position;
                }

                if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)
                {
                    Vector2 currentTouchPos = touchTwo.position - touchOne.position;
                    float angle = Vector2.SignedAngle(initialTouchPos, currentTouchPos);

                    item3DModel.transform.rotation = Quaternion.Euler(0, item3DModel.transform.eulerAngles.y - angle, 0);
                    initialTouchPos = currentTouchPos;
                    aRPointer.transform.rotation = Quaternion.Euler(aRPointer.transform.eulerAngles.x, item3DModel.transform.eulerAngles.y, 0);


                }
            }
            if (IsOver3DModel && item3DModel == null && !IsOverUI)
            {
                GameManager.instance.ARPosition();
                item3DModel = itemSelected;
                itemSelected = null;
                aRPointer.SetActive(true);
                transform.position = item3DModel.transform.position;
                item3DModel.transform.parent = aRPointer.transform;

            }
        }
    }

    private bool isTapOver3DModel(Vector2 touchPosition)
    {
        Ray ray = aRCamera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit3DModel))
        {
            if (hit3DModel.collider.CompareTag("Item"))
            {
                itemSelected = hit3DModel.transform.gameObject;
                return true;
            }

        }
        return false;
    }

    private void SetItemPosition()
    {
        if (item3DModel != null)
        {
            item3DModel.transform.parent = null;
            aRPointer.SetActive(false);
            item3DModel = null;
        }
    }

    public void DeleteItem()
    {
        Destroy(item3DModel);
        aRPointer.SetActive(false);
        GameManager.instance.MoreMenu();
    }
    public bool isTapOverUI(Vector2 touchPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touchPosition.x, touchPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}
