namespace ElectionApp.Entity
{
    public class Voter
    {
        public int NID { get; set; }
        public string V_IDENTIFIER { get; set; }
        public string V_NAME { get; set; }
        public string V_EMAIL { get; set; }
        public byte[] PIC { get; set; }
        public bool HAS_VOTE { get; set; }
    }
}
