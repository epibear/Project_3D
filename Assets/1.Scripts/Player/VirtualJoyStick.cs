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
	public int value = 150; //�׸��������� ��
	
	public Image bgImg { get; private set; } //��� �̹���
	private Image stickImg; //��ƽ �̹���
	private Vector3 inputVector; //��ǥ ����
	public bool isInputEnable { get; set; } //�����̰� �մ��� üũ�ϴ°ǰ�? 

	public Vector3 joyStickInputPos { get; private set; } //�Էµ� ��ǥ ��

	void Start()
	{
		//�ʱ�ȭ
		isInputEnable = false; //���̽�ƽ�� �����϶��� 
		joyStickInputPos = Vector3.zero;
		bgImg = this.GetComponent<Image>();
		stickImg = this.transform.GetChild(1).GetComponent<Image>();
		GameManager.GetInstance().ac_EventOff += EventOff;
		//�ʱ�ȭ
	}
	private void EventOff() {
		isInputEnable = false;
		inputVector = Vector3.zero;
		stickImg.rectTransform.anchoredPosition = Vector3.zero;
	}

	public virtual void OnDrag(PointerEventData pointEvent)
	{
		Vector2 pos;
		joyStickInputPos = pointEvent.position; //������ �о����
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
			bgImg.rectTransform, pointEvent.position, pointEvent.pressEventCamera, out pos))
		{
			isInputEnable = true;	
			pos.x /= bgImg.rectTransform.sizeDelta.x; //������ ������ ��
			pos.y /= bgImg.rectTransform.sizeDelta.y;

			inputVector = new Vector3(pos.x * 2, pos.y * 2, 0);

			inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
			
			stickImg.rectTransform.anchoredPosition =
				new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3), //3 == ��ƽ ���� ��ƽ�� ũ�� ����
							inputVector.y * (bgImg.rectTransform.sizeDelta.y / 3));
		}
	}

	public virtual void OnPointerDown(PointerEventData pointEvent)
	{
		joyStickInputPos = Vector3.zero;
		OnDrag(pointEvent);
	}

	public virtual void OnPointerUp(PointerEventData pointEvent)
	{//�ʱ�ȭ ����� ��ġ
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

