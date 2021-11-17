using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class QuestPointer : MonoBehaviour
{
    [SerializeField] 
    private Camera _uiCamera = null;
    
    [SerializeField] 
    private Sprite _arrowSprite = null;
    [SerializeField] 
    private Sprite _crossSprite = null;
    
    private GameObject _pointer = null;
    
    private Vector3 _targetPos = Vector3.zero;
    
    private RectTransform _pointerRectTransform = null;
    
    private Image _pointerImage = null;
    
    private const float BorderSize = 100f;
    
    private void Awake()
    {
        _pointer = GetComponent<QuestPointer>().gameObject;
        _pointerRectTransform = _pointer.GetComponent<RectTransform>();
        _pointerImage = _pointer.GetComponent<Image>();
        
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Camera.main == null) 
            return;

        var targetPosScreenPoint = Camera.main.WorldToScreenPoint(_targetPos);
        var isOffScreen = targetPosScreenPoint.x <= BorderSize || 
                          targetPosScreenPoint.x >= Screen.width - BorderSize ||
                          targetPosScreenPoint.y <= BorderSize || 
                          targetPosScreenPoint.y >= Screen.height - BorderSize;

        if (isOffScreen)
        {
            RotatePointer();
            _pointerImage.sprite = _arrowSprite;
            
            Vector3 cappedTargetScreenPos = targetPosScreenPoint;
            if (cappedTargetScreenPos.x <= BorderSize) cappedTargetScreenPos.x = BorderSize;
            if (cappedTargetScreenPos.x >= Screen.width - BorderSize) cappedTargetScreenPos.x = Screen.width - BorderSize;
            if (cappedTargetScreenPos.y <= BorderSize) cappedTargetScreenPos.y = BorderSize;
            if (cappedTargetScreenPos.y >= Screen.height - BorderSize) cappedTargetScreenPos.y = Screen.height - BorderSize;

            var pointerWorldPos = _uiCamera.ScreenToWorldPoint(cappedTargetScreenPos);
            _pointerRectTransform.position = pointerWorldPos;
            _pointerRectTransform.localPosition = new Vector3(_pointerRectTransform.localPosition.x, _pointerRectTransform.localPosition.y, 0f);
        }
        else
        {
            _pointerImage.sprite = _crossSprite;
            
            var pointerWorldPos = _uiCamera.ScreenToWorldPoint(targetPosScreenPoint);
            _pointerRectTransform.position = pointerWorldPos;
            _pointerRectTransform.localPosition = new Vector3(_pointerRectTransform.localPosition.x, _pointerRectTransform.localPosition.y, 0f);
            
            _pointerRectTransform.localEulerAngles = Vector3.zero;
        }

    }
    
    private void RotatePointer()
    {
        var toPos = _targetPos;
        if (Camera.main == null) 
            return;
        var fromPos = Camera.main.transform.position;
        fromPos.z = 0f;

        var dir = (toPos - fromPos).normalized;
        var angle = UtilsClass.GetAngleFromVectorFloat(dir);
        _pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public void Show(Vector3 targetPos)
    {
        gameObject.SetActive(true);
        _targetPos = targetPos;
    }
}
