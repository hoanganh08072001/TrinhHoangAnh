using Model;

namespace DatabaseIO
{
    
    public class GenericDao
    {
        MyDB mydb = new MyDB();
        public void AddObject<T>(T obj)
        {
            mydb.Set(obj.GetType()).Add(obj);
        } 
        public void Save()
        {
            mydb.SaveChanges();
        }
    }
}
