namespace Canaa.DataContracts.Whatsapp
{
    public class SendTextMessageDataObjec
    {
        public string number { get; set; }
        public OptionsDataObjec options { get; set; }
        public TextMessage textMessage { get; set; }
        public string instancia { get; set; }
    }

    public class OptionsDataObjec
    {
        public int delay { get; set; }
        public string presence { get; set; }
        public bool? linkPreview { get; set; }
    }

    public class TextMessage
    {
        public string text { get; set; }
    }
}
