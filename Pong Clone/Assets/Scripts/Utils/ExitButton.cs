namespace Utils
{
    public class ExitButton : DynamicButton
	{
        public override void AssignFunction()
        {
            GameManager.instance.ExitGame();
        }
	}   
}