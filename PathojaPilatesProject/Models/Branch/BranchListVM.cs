namespace PathojaPilatesProject.Models.Branch
{
    public class BranchListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }         
        public string Address { get; set; }      
        public string? PhoneNumber { get; set; }  
        public string? Email { get; set; }
        public string? GoogleMapsUrl { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
