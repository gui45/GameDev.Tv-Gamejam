using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Header("Animation")]

    [SerializeField]
    private float transitionTime;
    [SerializeField]
    private AnimationCurve progressCurve;
    [SerializeField]
    private float fillLevel;

    [Header("Image")]

    [SerializeField]
    private Image image;
    [SerializeField]
    private Image.FillMethod fillMethod;
    [SerializeField]
    private bool clockwise = true;
    [SerializeField]

    [Header("Fill origins, conditionnel a la method")]
    [Header("Tente pas de faire l'editor code pour sa lol")]

    private Image.Origin360 origin360;
    [SerializeField]
    private Image.Origin180 origin180;
    [SerializeField]
    private Image.Origin90 origin90;
    [SerializeField]
    private Image.OriginHorizontal originHorizontal;
    [SerializeField]
    private Image.OriginVertical originVertical;

    private float startingFillLevel;
    private float endingFillLevel;
    private float fillCountDown;

    private void Start()
    {
        image.fillAmount = fillLevel;
        image.fillClockwise = clockwise;
        image.fillMethod = fillMethod;

        switch (image.fillMethod)
        {
            case Image.FillMethod.Vertical:
                image.fillOrigin = (int)originVertical;
                break;
            case Image.FillMethod.Horizontal:
                image.fillOrigin = (int)originHorizontal;
                break;
            case Image.FillMethod.Radial180:
                image.fillOrigin = (int)origin180;
                break;
            case Image.FillMethod.Radial90:
                image.fillOrigin= (int)origin90;
                break;
            case Image.FillMethod.Radial360:
                image.fillOrigin= (int)origin360;
                break;
        }

        fillCountDown = transitionTime + 1;
        startingFillLevel = fillLevel;
        endingFillLevel = fillLevel;
    }

    private void Update()
    {
        if (fillCountDown <= transitionTime)
        {
            UpdateFillLevel();
        }
    }

    private void UpdateFillLevel()
    {
        if (transitionTime != 0)
        {
            fillCountDown += Time.deltaTime;

            float timeProgress = fillCountDown / transitionTime;
            float progress = (endingFillLevel - startingFillLevel) * progressCurve.Evaluate(timeProgress);

            image.fillAmount = startingFillLevel + progress;
        }
        else
        {
            image.fillAmount = endingFillLevel;
        }
    }

    public void SetFillLevel(float targetLevel)
    {
        startingFillLevel = image.fillAmount;
        endingFillLevel = targetLevel;
        fillCountDown = 0;
    }
}
