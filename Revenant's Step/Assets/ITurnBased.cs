using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnBased {//interface for game manager and agents to enforce stealth and combat states
    bool DoTurn();

    void doStealth();

    void TransitionToCombat();

    void StartTurn();

    void StartStealth();
}
