using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private List<BackgroundSlider> _sliders;
    private List<Vector3> _startingPositions;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _sliders = new List<BackgroundSlider>();
        _startingPositions = new List<Vector3>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);

            _sliders.Add(childTransform.GetComponent<BackgroundSlider>());
            _startingPositions.Add(childTransform.position);
        }


    }

    public void TurnOn()
    {
        foreach (BackgroundSlider slider in _sliders)
            slider.enabled = true;
    }

    public void TurnOff()
    {
        foreach (BackgroundSlider slider in _sliders)
            slider.enabled = false;
    }

    public void ResetPositions()
    {
        for (int i = 0; i < _sliders.Count; i++)
            _sliders[i].transform.position = _startingPositions[i];
    }

}
