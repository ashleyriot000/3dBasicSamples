using UnityEngine;

public class LifeManager : MonoBehaviour
{
  public Destructible[] golds;
  public Destructible[] greens;
  public Destructible[] blues;
  public Destructible[] purples;

  public void EnabledGolds(bool enabled)
  {
    foreach (var d in golds)
    {
      d.Enabled(enabled);
    }   
  }


  public void EnabledGreens(bool enabled)
  {
    foreach (Destructible d in greens)
    {
      d.Enabled(enabled);
    }
  }  
  public void EnabledBlues(bool enabled)
  {
    foreach (Destructible d in blues)
    {
      d.Enabled(enabled);
    }
  }  
  public void EnabledPurples(bool enabled)
  {
    foreach (Destructible d in purples)
    {
      d.Enabled(enabled);
    }
  }
  
  public void EnabledAll(bool enabled)
  {
    EnabledGolds(enabled);
    EnabledGreens(enabled);
    EnabledBlues(enabled);
    EnabledPurples(enabled);
  }


  private void Update()
  {
    if(Input.GetMouseButtonDown(0))
    {
      // 총쏴라
    }
  }
}
