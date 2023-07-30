using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private const float SmoothSpeed = 0.2f;

    private float _strength;

    private Vector3 _initialCameraPosition;
    private float _remainingShakeTime;
    private Vector3 _newShakePosition;

    public void Shake(float power, float duration)
    {
        _initialCameraPosition = transform.localPosition;
        _strength = power;
        _remainingShakeTime = duration;
    }


    private void LateUpdate()
    {
        if (_remainingShakeTime < 0)
        {
            return;
        }

        _newShakePosition = _initialCameraPosition + (Vector3)Random.insideUnitCircle * _strength;
        transform.localPosition = Vector3.Lerp(transform.localPosition, _newShakePosition, SmoothSpeed);
        _remainingShakeTime -= Time.deltaTime;
    }
}