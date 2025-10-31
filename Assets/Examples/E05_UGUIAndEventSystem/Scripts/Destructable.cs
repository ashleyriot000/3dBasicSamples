using UnityEngine;
using UnityEngine.EventSystems;

public class Destructible : MonoBehaviour, IPointerDownHandler
{
  public int maxHealth = 100;
  public int damage = 10;
  public HealthUI healthUI;
  public string parentName;

  private int _currentHealth;

  private void Awake()
  {
    Transform camTr = Camera.main.transform;
    if (camTr == null)
      return;

    GameObject parent = GameObject.Find(parentName);
    if (parent == null)
      return;

    healthUI.Init(camTr, transform, parent.transform);    
  }

  private void OnEnable()
  {
    _currentHealth = maxHealth;
    healthUI.Activate(true, _currentHealth, (float)_currentHealth / maxHealth);
  }

  public void Enabled(bool enabled)
  {
    if(enabled)
    {
      gameObject.SetActive(enabled);
    }
    else
    {
      gameObject.SetActive(enabled);
      healthUI.Activate(enabled);
    }
  }
  
  public void OnPointerDown(PointerEventData eventData)
  {
    _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);      
    healthUI.ChangeValue(_currentHealth, (float)_currentHealth / maxHealth);

    if(_currentHealth == 0)
    {
      Enabled(false);
    }
  }
}
