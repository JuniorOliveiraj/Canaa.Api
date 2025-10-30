namespace Canaa.DataContracts.Whatsapp
{
    public class SendMidiaDataObject
    {
        public string number { get; set; }
        public OptionsDataObjec options { get; set; }
        public MediaMessage mediaMessage { get; set; }
        public string instancia { get; set; }

    }

    public class MediaMessage
    {
        public Mediatype mediatype { get; set; } 
        public string caption { get; set; }   
        public string media { get; set; }  
    }
}
