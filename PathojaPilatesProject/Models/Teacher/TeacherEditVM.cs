namespace PathojaPilatesProject.Models.Teacher
{
    public class TeacherEditVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public List<int> CategoryIds { get; set; } = new();
        public List<int> BranchIds { get; set; } = new();
    }
}
