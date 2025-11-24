using UnityEngine;

public class BoomIconUI : MonoBehaviour
{
    public GameObject[] boomIcon;

    public void SetBoomCount(int count)
    {
        if(count < 0) count = 0;
        if(count > boomIcon.Length) count = boomIcon.Length;

        // 보유 하고 있는 폭탄 갯수 만큼 boomicon을 활성화
        for(int i = 0; i < boomIcon.Length; i++)
        {
            bool active = i < count;
            boomIcon[i].SetActive(active);
        }
    }
}
