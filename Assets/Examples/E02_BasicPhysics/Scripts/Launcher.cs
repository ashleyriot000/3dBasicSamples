using UnityEngine;

public class Launcher : MonoBehaviour
{
  public Projectile[] projectiles;

  private int _index = 0;
  
  public void OnAttack()
  {
    if (projectiles.Length < 1 || projectiles[_index] == null)
    {
      Debug.Log("발사체가 비어있습니다.");
      return;
    }

    Projectile p = Instantiate(projectiles[_index], transform.position, Quaternion.identity);
    p.Launch(transform.forward);
  }

  public void OnNext()
  {
    _index++;
    if (_index >= projectiles.Length)
      _index = 0;
  }

  public void OnPrev()
  {
    _index--;
    if (_index < 0)
      _index = projectiles.Length - 1;
  }
}
