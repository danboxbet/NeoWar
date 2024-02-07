using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIVehicleHitPointSlider : MonoBehaviour
{
    [SerializeField] private Vehicle vehicle;

    [SerializeField] private Image fillImage;

    [SerializeField] private Slider slider;

    private void Start()
    {
        vehicle.HitPointChange += OnHitPointChange;

        fillImage.color = vehicle.owner.GetComponent<Player>().PlayerColor;

        slider.maxValue = vehicle.MaxHitPoint;
        slider.value = vehicle.HitPoint;
    }

    private void OnDestroy()
    {
        vehicle.HitPointChange -= OnHitPointChange;
    }
    private void OnHitPointChange(int hitPoint)
    {
        slider.value = hitPoint;
    }
}
