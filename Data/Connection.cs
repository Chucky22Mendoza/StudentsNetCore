using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data {
    public class Connection : DataConnection {
        public Connection() : base("DB1") { }
        public ITable<Student> _Student { get { return GetTable<Student>(); } }

    }
}
