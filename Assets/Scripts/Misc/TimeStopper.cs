using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopper : MonoBehaviour
{
    public static TimeStopper Instance { get; private set; }

    private bool scaled, stoped;
    private float currentScale = 1.0f;
    private void Awake()
    {
        Instance = this;
        TimeScaleSet(1.0f);
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        if (stoped) TimeScaleSet(currentScale);
    //        else TimeScaleSet(0.0f);
    //        stoped = !stoped;
    //    }
    //    if (Input.GetKeyDown(KeyCode.O))
    //    {
    //        currentScale = !scaled ? 0.1f : 1.0f;
    //        TimeScaleSet(currentScale);
    //        scaled = !scaled;
    //    }
    //}
    public void TimeScaleLerp(float from, float to, float time = 1.0f)
    {
        StartCoroutine(TimeScaleCoroutine(from, to, time));
    }
    private IEnumerator TimeScaleCoroutine(float from, float to, float time = 1.0f)
    {
        float multiply = 1 / time;
        float timer = 0.0f;
        while (timer < 1.0f) 
        {
            timer += Time.deltaTime * multiply;
            TimeScaleSet(Mathf.Lerp(from, to, timer));
            yield return null;
        }
        TimeScaleSet(to);
    }
    private void TimeScaleSet(float scale)
    {
        Time.timeScale = scale;
    }
}
