namespace Utils
{
	public class PlayAgainButton : DynamicButton
	{
        public override void AssignFunction()
        {
            GameManager.instance.PlayAgain();
        }
	}   
}