using UnityEngine;

public enum GameState{BUILDING, EXPLORING, PAUSED}

[CreateAssetMenu(fileName = "GlobalGameState", menuName = "GlobalVariables/GlobalGameState")]
public class GlobalGameState : GlobalVariable<GameState>
{
}
