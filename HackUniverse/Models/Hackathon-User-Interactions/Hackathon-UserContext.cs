﻿using HackUniverse.Models.User;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackUniverse.Models.Hackathon_User_Interactions
{
    public class Hackathon_UserContext
    {
        public String ConnectionString { get; set; }
        public Hackathon_UserContext(string connectionString)
        {
            ConnectionString = connectionString;
        }
        private MySqlConnection GetConnecton()
        {
            return new MySqlConnection(ConnectionString);
        }
        public bool register(dynamic UserHandle,int hid,int pid)
        {   string query;
            if (UserHandle.Profile.Type != 'P')
            {
                return false;
            }
                       
            query=$"insert into hackathon_participant (username,HackathonID,statementID) values ('{UserHandle.User.UserName}','{hid}',''{pid})";
            using( var connection = GetConnecton())
            {
                connection.Open();
                var command = new MySqlCommand(query, connection);
                return command.ExecuteNonQuery()>0?true:false;
            }
        }
        public List<int> GetUserHackathons(dynamic UserHandle)
        {
            string query;
            List<int> list=new List<int>();
            if (UserHandle.Profile.Type == 'C')
            {
                query = $"select * from hackathon_creator where username='{UserHandle.User.UserName}'";
            }
            else
            {
                query= $"select * from hackathon_participant where username='{UserHandle.User.UserName}'";
            }
            using (var connection= GetConnecton())
            {
                connection.Open();
                var command =new MySqlCommand(query, connection);
                //command.Parameters.AddWithValue("@username", UserHandle.User.UserName);

                using (var read = command.ExecuteReader())
                {
                    while (read.Read())
                    {
                        var id = Convert.ToInt32(read["HackathonId"].ToString());
                        list.Add(id);
                    }
                }
            }
            return list;
        }
    }
}