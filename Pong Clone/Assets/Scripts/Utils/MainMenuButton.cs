namespace Utils
{
    public class MainMenuButton : DynamicButton
	{
        public override void AssignFunction()
        {
            GameManager.instance.LoadMainMenu();
        }
	}   
}