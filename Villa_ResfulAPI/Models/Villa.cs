namespace Villa_ResfulAPI.Models
{
    public class Villa
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
