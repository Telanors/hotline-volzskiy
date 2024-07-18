using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InfoWeaponPanel : MonoBehaviour
{
    public static InfoWeaponPanel Instance;
    [SerializeField] private new TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI description;
    private Transform weapon;
    private void Start()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (weapon == null) return;
        transform.position = Camera.main.WorldToScreenPoint(weapon.position);
    }
    public void OnPanel(Transform weapon, string name, string description)
    {
        this.weapon = weapon;
        this.name.text = ">" + name;
        this.description.text = ">" + description;
        gameObject.SetActive(true);
    }
    public void OffPanel()
    {
        weapon = null;
        gameObject.SetActive(false);
    }
}
