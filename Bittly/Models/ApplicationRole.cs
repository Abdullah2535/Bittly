using Microsoft.AspNetCore.Identity;

namespace Bittly.Models
{
    public class ApplicationRole : IdentityRole<int>
    {

        public ApplicationRole() : base()
        {
        }
        public ApplicationRole(string Name) : base(Name)
        {
        }
    }
}
