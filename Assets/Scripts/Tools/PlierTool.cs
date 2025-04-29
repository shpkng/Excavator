public class PlierTool : BSBSystemTool
{
    private ICollectible currentElement;

    public void AddElement(ICollectible element)
    {
        currentElement = element;
    }

    public void RemoveElement(ICollectible element)
    {
        if (currentElement != element)
            return;
        currentElement = null;
    }
}