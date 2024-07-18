using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private InputsController inputs;
    [SerializeField] private RectTransform menuPanel;
    [SerializeField] private RectTransform infoPanel;
    [SerializeField] private RectTransform deathPanel;
    public RectTransform DeathPanel => deathPanel;

    private bool menuOn;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if(inputs != null)
        {
            menuPanel.gameObject.SetActive(false);
            inputs.menuAction.started += context => MenuToggle();
        }
    }
    public void ScaleAnimation(RectTransform transform, float from, float to, Action action = null, float time = 1.0f)
    {
        StartCoroutine(ScaleAnimationCoroutine(transform, from, to, action, time));
    }
    private IEnumerator ScaleAnimationCoroutine(RectTransform transform, float from, float to, Action action = null, float time = 1.0f)
    {
        Vector3 fromScale = new Vector3(from, from, from);
        Vector3 toScale = new Vector3(to, to, to);
        float multiply = 1.0f / time;
        float timer = 0.0f;
        while (timer < 1.0f)
        {
            timer += Time.unscaledDeltaTime * multiply;
            transform.localScale = Vector3.Lerp(fromScale, toScale, timer);
            yield return null;
        }
        transform.localScale = toScale;
        action?.Invoke();
    }
    private void MenuToggle()
    {
        menuOn = !menuOn;
        menuPanel.gameObject.SetActive(menuOn);
        Cursor.visible = menuOn;
        if(menuOn)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
    public void StartButton()
    {
        SceneManager.LoadScene(1);
        INoiseSensitive.noiseSensitives.Clear();
    }
    public void BackToMenuButton()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
