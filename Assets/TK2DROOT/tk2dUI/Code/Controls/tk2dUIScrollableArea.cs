using UnityEngine;
using System.Collections;

/// <summary>
/// Scrollable Area Control. Can be actually by changing Value, external scrollbar or swipe gesture
/// </summary>
[AddComponentMenu("2D Toolkit/UI/tk2dUIScrollableArea")]
public class tk2dUIScrollableArea : MonoBehaviour
{
    /// <summary>
    /// XAxis - horizontal, YAxis - vertical
    /// </summary>
    public enum Axes { XAxis, YAxis }

    /// <summary>
    /// Length of all the content in the scrollable area
    /// </summary>
    [SerializeField]
    private float contentLength = 1;

    public float ContentLength
    {
        get { return contentLength; }
        set 
        {
            ContentLengthVisibleAreaLengthChange(contentLength, value,visibleAreaLength,visibleAreaLength);
        }
    }

    /// <summary>
    /// Length of visible area of content, what can be seen
    /// </summary>
    [SerializeField]
    private float visibleAreaLength = 1;

    public float VisibleAreaLength
    {
        get { return visibleAreaLength; }
        set 
        {
            ContentLengthVisibleAreaLengthChange(contentLength, contentLength, visibleAreaLength, value);
        }
    }

    /// <summary>
    /// Transform the will be moved to scroll content. All content needs to be a child of this Transform
    /// </summary>
    public GameObject contentContainer;

    /// <summary>
    /// Scrollbar to be attached. Not required.
    /// </summary>
    public tk2dUIScrollbar scrollBar;

    /// <summary>
    /// Used to record swipe scrolling events
    /// </summary>
    public tk2dUIItem backgroundUIItem;

    /// <summary>
    /// Axes scrolling will happen on
    /// </summary>
    public Axes scrollAxes = Axes.YAxis;

    /// <summary>
    /// If swipe (gesture) scrolling is enabled
    /// </summary>
    public bool allowSwipeScrolling = true;

    /// <summary>
    /// If mouse will is enabled, hover needs to be active
    /// </summary>
    public bool allowScrollWheel = true;

    private bool isBackgroundButtonDown = false;
    private bool isBackgroundButtonOver = false;

    private Vector3 swipeScrollingPressDownStartLocalPos = Vector3.zero;
    private Vector3 swipeScrollingContentStartLocalPos = Vector3.zero;
    private Vector3 swipeScrollingContentDestLocalPos = Vector3.zero;
    private bool isSwipeScrollingInProgress = false;
    private const float SWIPE_SCROLLING_FIRST_SCROLL_THRESHOLD = .02f; //at what point swipe scrolling will start moving the list
    private const float WITHOUT_SCROLLBAR_FIXED_SCROLL_WHEEL_PERCENT = .1f; //if not scrollbar attached how much scroll wheel will move list
    private Vector3 swipePrevScrollingContentPressLocalPos = Vector3.zero;
    private float swipeCurrVelocity = 0; //velocity of current frame (used for inertia swipe scrolling)
    private float snapBackVelocity = 0;

    /// <summary>
    /// If scrollable area is being scrolled
    /// </summary>
    public event System.Action<tk2dUIScrollableArea> OnScroll;

    private float percent = 0; //0-1

    /// <summary>
    /// Scroll position percent 0-1
    /// </summary>
    public float Value
    {
        get { return percent; }
        set
        {
            value = Mathf.Clamp(value, 0f, 1f);
            if (value != percent)
            {
                UnpressAllUIItemChildren();
            }
            percent = value;
            if (scrollBar != null) { scrollBar.SetScrollPercentWithoutEvent(percent); }
            SetContentPosition();
        }
    }

    void OnEnable()
    {
        if (scrollBar != null)
        {
            scrollBar.OnScroll += ScrollBarMove;
        }

        if (backgroundUIItem != null)
        {
            backgroundUIItem.OnDown += BackgroundButtonDown;
            backgroundUIItem.OnRelease += BackgroundButtonRelease;
            backgroundUIItem.OnHoverOver += BackgroundButtonHoverOver;
            backgroundUIItem.OnHoverOut += BackgroundButtonHoverOut;
        }
    }

    void OnDisable()
    {
        if (scrollBar != null)
        {
            scrollBar.OnScroll -= ScrollBarMove;
        }

        if (backgroundUIItem != null)
        {
            backgroundUIItem.OnDown -= BackgroundButtonDown;
            backgroundUIItem.OnRelease -= BackgroundButtonRelease;
            backgroundUIItem.OnHoverOver -= BackgroundButtonHoverOver;
            backgroundUIItem.OnHoverOut -= BackgroundButtonHoverOut;
        }

        if (isBackgroundButtonOver)
        {
            if (tk2dUIManager.Instance != null)
            {
                tk2dUIManager.Instance.OnScrollWheelChange -= BackgroundHoverOverScrollWheelChange;
            }
            isBackgroundButtonOver = false;
        }

        if (isBackgroundButtonDown)
        {
            if (tk2dUIManager.Instance != null)
            {
                tk2dUIManager.Instance.OnInputUpdate -= BackgroundOverUpdate;
            }
            isBackgroundButtonDown = false;
        }

        swipeCurrVelocity = 0;
    }

    void Start()
    {
        UpdateScrollbarActiveState();
    }

    private void BackgroundHoverOverScrollWheelChange(float mouseWheelChange)
    {
        if (mouseWheelChange > 0)
        {
            if (scrollBar)
            {
                scrollBar.ScrollUpFixed();
            }
            else
            {
                Value -= WITHOUT_SCROLLBAR_FIXED_SCROLL_WHEEL_PERCENT;
            }
        }
        else if (mouseWheelChange < 0)
        {
            if (scrollBar)
            {
                scrollBar.ScrollDownFixed();
            }
            else
            {
                Value += WITHOUT_SCROLLBAR_FIXED_SCROLL_WHEEL_PERCENT;
            }
        }
    }

    private void ScrollBarMove(tk2dUIScrollbar scrollBar)
    {
        Value = scrollBar.Value;
        isSwipeScrollingInProgress = false;
        if (isBackgroundButtonDown)
        {
            BackgroundButtonRelease();
        }
    }

    Vector3 ContentContainerOffset {
        get { 
            return Vector3.Scale(new Vector3(-1, 1, 1), contentContainer.transform.localPosition);
        }
        set {
            contentContainer.transform.localPosition = Vector3.Scale(new Vector3(-1, 1, 1), value);
        }
    }

    private void SetContentPosition()
    {
        Vector3 localPos = ContentContainerOffset;
        float pos = (contentLength - visibleAreaLength) * Value;

        if (pos < 0) { pos = 0; }

        if (scrollAxes == Axes.XAxis)
        {
            localPos.x = pos;
        }
        else if (scrollAxes == Axes.YAxis)
        {
            localPos.y = pos;
        }

        ContentContainerOffset = localPos;
    }

    private void BackgroundButtonDown()
    {
        if (allowSwipeScrolling && contentLength > visibleAreaLength)
        {
            if (!isBackgroundButtonDown && !isSwipeScrollingInProgress)
            {
                tk2dUIManager.Instance.OnInputUpdate += BackgroundOverUpdate;
            }
            swipeScrollingPressDownStartLocalPos = transform.InverseTransformPoint(CalculateClickWorldPos(backgroundUIItem));
            swipePrevScrollingContentPressLocalPos = swipeScrollingPressDownStartLocalPos;
            swipeScrollingContentStartLocalPos = ContentContainerOffset;
            swipeScrollingContentDestLocalPos = swipeScrollingContentStartLocalPos;
            isBackgroundButtonDown = true;
            swipeCurrVelocity = 0;
        }
    }

    private void BackgroundOverUpdate()
    {
        if (isBackgroundButtonDown)
        {
            UpdateSwipeScrollDestintationPosition();
        }
        if (isSwipeScrollingInProgress)
        {
            float destValue = 0;
            if (scrollAxes == Axes.XAxis)
            {
                destValue = swipeScrollingContentDestLocalPos.x;
            }
            else if (scrollAxes == Axes.YAxis)
            {
                destValue = swipeScrollingContentDestLocalPos.y;
            }

            float minDest = 0;
            float maxDest = contentLength - visibleAreaLength;
            if (isBackgroundButtonDown)
            {
                if (destValue < minDest)
                {
                    destValue += (-destValue / visibleAreaLength) / 2;
                    if (destValue > minDest )
                    {
                        destValue =minDest;
                    }
                }
                else if (destValue > maxDest)
                {
                    destValue-= ((destValue-maxDest)/visibleAreaLength)/2;
                    
                    if (destValue< maxDest)
                    {
                        destValue=maxDest;
                    }
                }

                if (scrollAxes == Axes.XAxis)
                {
                    swipeScrollingContentDestLocalPos.x = destValue;
                }
                else if (scrollAxes == Axes.YAxis)
                {
                    swipeScrollingContentDestLocalPos.y = destValue;
                }
                ContentContainerOffset = swipeScrollingContentDestLocalPos;
            }
            else //background button not down
            {
                if (destValue < minDest || destValue > maxDest)
                {
                    float target = ( destValue < minDest ) ? minDest : maxDest;
                    destValue = Mathf.SmoothDamp( destValue, target, ref snapBackVelocity, 0.05f, Mathf.Infinity, tk2dUITime.deltaTime );
                    swipeCurrVelocity = 0;
                }
                else if (swipeCurrVelocity != 0) //velocity scrolling
                {
                    destValue += swipeCurrVelocity * tk2dUITime.deltaTime * 20; //swipe velocity multiplier
                    if (swipeCurrVelocity > 0.001f || swipeCurrVelocity < -0.001f)
                    {
                        swipeCurrVelocity = Mathf.Lerp(swipeCurrVelocity, 0, tk2dUITime.deltaTime * 2.5f); //change multiplier to change slowdown velocity
                    }
                    else
                    {
                        swipeCurrVelocity = 0;
                    }
                }
                else
                {
                    isSwipeScrollingInProgress = false;
                    tk2dUIManager.Instance.OnInputUpdate -= BackgroundOverUpdate;
                }

                if (scrollAxes == Axes.XAxis)
                {
                    swipeScrollingContentDestLocalPos.x = destValue;
                }
                else if (scrollAxes == Axes.YAxis)
                {
                    swipeScrollingContentDestLocalPos.y = destValue;
                }

                ContentContainerOffset = swipeScrollingContentDestLocalPos;
            }

            if (scrollBar != null)
            {
                float scrollBarPercent = percent;
                if (scrollAxes == Axes.XAxis)
                {
                    scrollBarPercent = (ContentContainerOffset.x / (contentLength - visibleAreaLength));
                }
                else if (scrollAxes == Axes.YAxis)
                {
                    scrollBarPercent = (ContentContainerOffset.y / (contentLength - visibleAreaLength));

                }

                scrollBar.SetScrollPercentWithoutEvent(scrollBarPercent);
            }
        }
    }

    private void UpdateSwipeScrollDestintationPosition()
    {
        Vector3 currTouchPosLocal = transform.InverseTransformPoint(CalculateClickWorldPos(backgroundUIItem));
 
        // X axis is inverted
        Vector3 moveDiffVector = currTouchPosLocal - swipeScrollingPressDownStartLocalPos;
        moveDiffVector.x *= -1;

        float moveDiff = 0;
        if (scrollAxes == Axes.XAxis)
        {
            moveDiff = moveDiffVector.x;
            // Invert x axis
            swipeCurrVelocity = -(currTouchPosLocal.x - swipePrevScrollingContentPressLocalPos.x);
        }
        else if (scrollAxes == Axes.YAxis)
        {
            moveDiff = moveDiffVector.y;
            swipeCurrVelocity = currTouchPosLocal.y - swipePrevScrollingContentPressLocalPos.y;
        }
        if (!isSwipeScrollingInProgress)
        {
            if (Mathf.Abs(moveDiff) > SWIPE_SCROLLING_FIRST_SCROLL_THRESHOLD)
            {
                isSwipeScrollingInProgress = true;

                //unpress anything currently pressed in list
                tk2dUIManager.Instance.OverrideClearAllChildrenPresses(backgroundUIItem);
            }
        }
        if (isSwipeScrollingInProgress)
        {
            Vector3 destContentPos = swipeScrollingContentStartLocalPos + moveDiffVector;
            destContentPos.z = ContentContainerOffset.z;

            if (scrollAxes == Axes.XAxis)
            {
                destContentPos.y = ContentContainerOffset.y;        
            }
            else if (scrollAxes == Axes.YAxis)
            {
                destContentPos.x = ContentContainerOffset.x;
            }
            destContentPos.z = ContentContainerOffset.z;

            swipeScrollingContentDestLocalPos = destContentPos;
            swipePrevScrollingContentPressLocalPos = currTouchPosLocal;
        }
    }

    private void BackgroundButtonRelease()
    {
        if (allowSwipeScrolling)
        {
            if (isBackgroundButtonDown)
            {
                if (!isSwipeScrollingInProgress)
                {
                    tk2dUIManager.Instance.OnInputUpdate -= BackgroundOverUpdate;
                }
            }
            isBackgroundButtonDown = false;
        }
    }

    private void BackgroundButtonHoverOver()
    {
        if (allowScrollWheel)
        {
            if (!isBackgroundButtonOver)
            {
                tk2dUIManager.Instance.OnScrollWheelChange += BackgroundHoverOverScrollWheelChange;
            }
            isBackgroundButtonOver = true;
        }
    }

    private void BackgroundButtonHoverOut()
    {
        if (isBackgroundButtonOver)
        {
            tk2dUIManager.Instance.OnScrollWheelChange -= BackgroundHoverOverScrollWheelChange;
        }

        isBackgroundButtonOver = false;
    }

    private Vector3 CalculateClickWorldPos(tk2dUIItem btn)
    {
        Vector2 pos = btn.Touch.position;
        Vector3 worldPos = tk2dUIManager.Instance.UICamera.ScreenToWorldPoint(new Vector3(pos.x, pos.y, btn.transform.position.z - tk2dUIManager.Instance.UICamera.transform.position.z));
        worldPos.z = btn.transform.position.z;
        return worldPos;
    }

    private void UpdateScrollbarActiveState()
    {
        bool scrollBarVisible = (contentLength > visibleAreaLength);
        if (scrollBar != null)
        {
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9
            if (scrollBar.gameObject.active != scrollBarVisible)
#else
            if (scrollBar.gameObject.activeSelf != scrollBarVisible)
#endif
            {
                tk2dUIBaseItemControl.ChangeGameObjectActiveState(scrollBar.gameObject, scrollBarVisible);
            }
        }
    }

    private void ContentLengthVisibleAreaLengthChange(float prevContentLength,float newContentLength,float prevVisibleAreaLength,float newVisibleAreaLength)
    {
        float newValue;
        if (newContentLength-visibleAreaLength!=0)
        {
            newValue = ((prevContentLength - prevVisibleAreaLength) * Value) / (newContentLength - newVisibleAreaLength);
        }
        else
        {
            newValue = 0;
        }

        contentLength = newContentLength;
        visibleAreaLength = newVisibleAreaLength;
        UpdateScrollbarActiveState();
        Value = newValue;
    }

    private void UnpressAllUIItemChildren()
    {

    }
}
