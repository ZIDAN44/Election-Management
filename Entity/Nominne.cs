namespace ElectionApp.Entity
{
    public class Nominee
    {
        public int NOM_ID { get; set; }
        public string N_IDENTIFIER { get; set; }
        public string N_NAME { get; set; }
        public string P_NAME { get; set; }
        public string N_EMAIL { get; set; }
        public byte[] LOGO { get; set; }
        public int VCOUNT { get; set; }
    }
}
