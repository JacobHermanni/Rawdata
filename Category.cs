using System;
using System.Collections.Generic;
using System.Text;

namespace ServerAssignment
{
    class Category
    {
        public Category(int cid, string name)
        {
            this.cid = cid;
            this.name = name;
        }

        public int cid { get; set; }

        public string name { get; set; }
    }
}
