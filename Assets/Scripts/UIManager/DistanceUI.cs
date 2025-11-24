using UnityEngine;
using UnityEngine.UI;

public class DistancdeUI : MonoBehaviour
{
    public Text distanceText;
    public float distance = 0f;
    void Update()
    {
        distance += Time.deltaTime;

        // 화면에 날아간 거리 표시 1초에 1m 씩 증가
        distanceText.text = distance.ToString("F1") + "m";
    }
}
