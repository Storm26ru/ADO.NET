﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            //const string CONNECTION_STRING = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Movies_VPD_311;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //Connector connector = new Connector(CONNECTION_STRING);
            Connector connector = new Connector();
            //connector.Select("SELECT * FROM Directors");
           // connector.Select("*", "Movies");
            connector.Select("title,release_date,FORMATMESSAGE(N'%s %s', first_name, last_name) AS N'DIRECTOR'", "Movies,Directors", "director = director_id");
        }
    }
}
