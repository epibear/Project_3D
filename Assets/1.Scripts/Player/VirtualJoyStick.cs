using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class VirtualJoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
	private static VirtualJoyStick instance = null;
	public static VirtualJoyStick GetInstance() {
		return instance; }

    private void Awake()
    {
		if (instance != null)
			Destroy(instance);

		instance = this;
	}
    [Header("control value")]
	public int value = 150; //그림사이즈의 반
	
	public Image bgImg { get; private set; } //배경 이미지
	private Image stickImg; //스틱 이미지
	private Vector3 inputVector; //좌표 변수
	public bool isInputEnable { get; set; } //움직이고 잇는지 체크하는건가? 

	public Vector3 joyStickInputPos { get; private set; } //입력된 좌표 값

	void Start()
	{
		//초기화
		isInputEnable = false; //조이스틱을 움직일때만 
		joyStickInputPos = Vector3.zero;
		bgImg = this.GetComponent<Image>();
		stickImg = this.transform.GetChild(1).GetComponent<Image>();
		GameManager.GetInstance().ac_EventOff += EventOff;
		//초기화
	}
	private void EventOff() {
		isInputEnable = false;
		inputVector = Vector3.zero;
		stickImg.rectTransform.anchoredPosition = Vector3.zero;
	}

	public virtual void OnDrag(PointerEventData pointEvent)
	{
		Vector2 pos;
		joyStickInputPos = pointEvent.position; //움직임 읽어오기
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
			bgImg.rectTransform, pointEvent.position, pointEvent.pressEventCamera, out pos))
		{
			isInputEnable = true;	
			pos.x /= bgImg.rectTransform.sizeDelta.x; //비율로 나누는 것
			pos.y /= bgImg.rectTransform.sizeDelta.y;

			inputVector = new Vector3(pos.x * 2, pos.y * 2, 0);

			inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
			
			stickImg.rectTransform.anchoredPosition =
				new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3), //3 == 스틱 배경과 스틱의 크기 차이
							inputVector.y * (bgImg.rectTransform.sizeDelta.y / 3));
		}
	}

	public virtual void OnPointerDown(PointerEventData pointEvent)
	{
		joyStickInputPos = Vector3.zero;
		OnDrag(pointEvent);
	}

	public virtual void OnPointerUp(PointerEventData pointEvent)
	{//초기화 가운데로 위치
		isInputEnable = false;
		inputVector = Vector3.zero;
		stickImg.rectTransform.anchoredPosition = Vector3.zero;
	}

	public float GetHorizontalValue()
	{
		return inputVector.x;
	}
	public float GetVerticalValue()
	{
		return inputVector.y;
	}

}

