using PostManagementApp.Models;

namespace PostManagementApp.Models
{
    public class DatabaseViewModel
    {
        public IEnumerable<Grave> Graves { get; set; } = new List<Grave>();
        public IEnumerable<DeceasedPerson> DeceasedPersons { get; set; } = new List<DeceasedPerson>();
        public IEnumerable<Relative> Relatives { get; set; } = new List<Relative>();
    }
}