using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    public Image fillbar;
    public TextMeshPro textMeshPro;

    public void Updatebar(int currentValue,int maxValue)
    {
        fillbar.fillAmount = (float)currentValue /(float)maxValue;
        textMeshPro.text = currentValue.ToString() + " / " + maxValue.ToString();
        
    }


}
