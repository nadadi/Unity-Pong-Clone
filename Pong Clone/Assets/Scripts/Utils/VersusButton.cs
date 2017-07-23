namespace Utils
{
	public class VersusButton : DynamicButton
	{
        public override void AssignFunction()
        {
            GameManager.instance.LoadTwoHumans();
        }
	}
}
