using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace DatabaseIO
{
    
    public class RoleDao
    {
        MyDB db = new MyDB();

        /**
         * get list role from database 
         * @return
         */
        public List<role> getAll()
        {
            try {
                return db.roles.ToList();
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
           
        }
    }
}
