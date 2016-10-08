using System.Data.Entity;

namespace <%=baseName%>.Data
{
    public class DataStore : DbContext
    {
        public DataStore() :
            base("name=defaultConnectionString")
        {

        }

        static DataStore()
        {
            Database.SetInitializer<DataStore>(null);
        }
    }
}
