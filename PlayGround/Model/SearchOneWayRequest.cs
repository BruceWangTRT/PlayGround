using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayGround.Model
{
    public class SearchOneWayRequest
    {
        public string client_ref { get; set; }

        public string Adult { get; set; }

        public string Child { get; set; }

        public string Infant { get; set; }

        public string currency { get; set; }

        public string dep_from { get; set; }

        public string dep_to { get; set; }

        public string departure_date { get; set; }

        public string ret_from { get; set; }

        public string ret_to { get; set; }

        public string return_date { get; set; }

        public string submit { get; set; }

        public string trip_class { get; set; }

        public string trip_type { get; set; }

    }
}
