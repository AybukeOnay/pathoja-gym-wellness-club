namespace PathojaPilatesProject.Models.Teacher
{
    public class TeacherCardVM
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Title { get; set; } = "";        // Pilates Instructor, Yoga Trainer...
        public string Area { get; set; } = "";         // Mat Pilates, Reformer, Hamile Pilates...
        public string BranchLabel { get; set; } = "";  // Alacaatlı / İncek / Wellness
        public string ImageUrl { get; set; } = "";     // /images/teachers/t1.jpg
        public string Bio { get; set; } = "";          // modal açıklama
        public List<string> Tags { get; set; } = new();
    }
}
