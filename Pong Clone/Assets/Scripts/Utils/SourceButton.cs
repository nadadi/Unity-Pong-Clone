namespace Utils
{
    public class SourceButton : DynamicButton
	{
        public override void AssignFunction()
        {
            GameManager.instance.OpenCodeSourceLink();
        }
	}   
}