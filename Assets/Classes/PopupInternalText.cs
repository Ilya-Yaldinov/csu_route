class PopupInternalText
{
    public string Header {get; private set;}

    public string HintFirst {get; private set;}
    public string HintSecond {get; private set;}
    public string HintThird {get; private set;}

    public PopupInternalText(string head, string text1, string text2, string text3)
    {
        this.Header = head;
        this.HintFirst = text1;
        this.HintSecond = text2;
        this.HintThird = text3;
    }
}