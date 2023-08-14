using UnityEngine;
using UnityEngine.EventSystems;
public class ShootingBtnCheck : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GunController.instance.shootingBtnIsPress = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        GunController.instance.shootingBtnIsPress = false;
    }
}