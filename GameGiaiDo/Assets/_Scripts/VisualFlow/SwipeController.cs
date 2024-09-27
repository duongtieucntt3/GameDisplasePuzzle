using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField]private int maxPage;
    public int currentPage;
    private Vector3 targetPos;
    [SerializeField] private Vector3 pageStep;
    [SerializeField] private RectTransform levelPagesRect;
    private LTDescr tween;
    [SerializeField] private float duration;
    [SerializeField] private LeanTweenType tweenType;
    private float dragThreshould;
    [SerializeField] Image[] barImage;
    [SerializeField] private Sprite barClosed, barOpen;
    [SerializeField] private Button previousbtn, nextBtn;

    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        dragThreshould = Screen.width / 15;
        UpdateBar();
        UpdateArrowButton();
    }
    public void Next()
    {
        if(currentPage< maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }
    public void Previous()
    {
        if(currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }
    private void MovePage()
    {
        if (tween != null)
            tween.reset();
        tween = levelPagesRect.LeanMoveLocal(targetPos, duration).setEase(tweenType);
        UpdateBar();
        UpdateArrowButton();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(Mathf.Abs(eventData.position.x - eventData.pressPosition.x)> dragThreshould)
        {
            if(eventData.position.x> eventData.pressPosition.x) Previous();
            else Next();
        }
        else
        {
            MovePage();
        }
    }
    private void UpdateBar()
    {
        foreach(var item in barImage)
        {
            item.sprite = barClosed;
        }
        barImage[currentPage - 1].sprite = barOpen;
    }
    private void UpdateArrowButton()
    {
        nextBtn.interactable = true;
        previousbtn.interactable = true;
        if (currentPage == 1) previousbtn.interactable = false;
        else if(currentPage == maxPage) nextBtn.interactable = false;
    }
}
