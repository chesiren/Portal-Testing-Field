using UnityEngine;
using System.Collections;

public interface IFPSInteractable
{
    void Interact(FPSWeapon script);
    void Action(FPSWeapon script);
}