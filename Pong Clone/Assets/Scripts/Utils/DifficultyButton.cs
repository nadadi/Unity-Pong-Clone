using UnityEngine;

namespace Utils
{
    public class DifficultyButton : DynamicButton
	{
        [SerializeField] int difficulty;

        public override void AssignFunction()
        {
            GameManager.instance.SetDifficulty(difficulty);
        }
	}
}