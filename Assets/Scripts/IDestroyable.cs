using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestroyable
{
    public void HandleZeroHealth();

    public void HandleHealthDrop(float factorRemaining);


}
