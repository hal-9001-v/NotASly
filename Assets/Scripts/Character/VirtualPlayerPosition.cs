using UnityEngine;

public class VirtualPlayerPosition : MonoBehaviour
{
    void Start()
    {
        var player = FindFirstObjectByType<Player>();
        if(player)
        {
			transform.SetParent(player.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
		}
    }

}
