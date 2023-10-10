namespace MedicineProject.AuthService.Domain.Dtos.Identity
{
    [AttributeUsage(AttributeTargets.All)]
    public class ScopeAttribute : Attribute
    {
        public string Scope { get; set; }

        public ScopeAttribute(string scope)
        {
            Scope = scope;
        }
    }
}
