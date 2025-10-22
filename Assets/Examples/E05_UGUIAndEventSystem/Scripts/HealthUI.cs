using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
  public Slider slider;
  public Text remainUI;
  public Vector3 offset;

  private bool _isInit = false;
  private Transform _target;
  private Transform _camTransform;

  public void Init(Transform camera, Transform target, Transform parent)
  {
    _camTransform = camera;
    _target = target;
    transform.SetParent(parent);
    _isInit = true;
  }

  public void ChangeValue(int remain, float value)
  {
    if (!_isInit) return;

    slider.value = value;
    remainUI.text = remain.ToString();
  }

  public void Activate(bool enabled, int remain = 0,  float initValue = 0f)
  {
    if (!_isInit) return;

    if (enabled)
    {
      remainUI.text = remain.ToString();
      slider.value = initValue;
    }

    gameObject.SetActive(enabled);
  }

  void LateUpdate()
  {
    if (_camTransform == null)
      return;

    transform.SetPositionAndRotation(_target.position + offset, _camTransform.rotation);
  }
}
