namespace Pets.Infrastructure.Settings
{
    public class MyOptions
    {
        public MyOptions()
        {
        }

        public string SqlConnectionString { get; set; }
        public bool InMemoryDatabase { get; set; }
    }
}
