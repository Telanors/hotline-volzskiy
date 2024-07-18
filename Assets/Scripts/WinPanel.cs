using UnityEngine;
using TMPro;
public class WinPanel : MonoBehaviour
{
    public AIEnemy aIEnemy;
    public TextMeshProUGUI winText;

    private void Start()
    {
        winText.enabled = false;
    }

    private void FixedUpdate()
    {
        if(aIEnemy.Health <= 0)
        {
            winText.enabled = true;
            Destroy(this);
        }
    }
}
