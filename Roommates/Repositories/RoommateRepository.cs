using System;
using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;


namespace Roommates.Repositories
{
    class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();    
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT rm.Id 'rmId', rm.FirstName, rm.LastName, rm.RentPortion, rm.MoveInDate, rm.RoomId, r.Id 'rId', r.Name, r.MaxOccupancy FROM Roommate rm JOIN Room r ON rm.RoomId = r.Id";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        int rmIdValue = reader.GetInt32(reader.GetOrdinal("rmId"));
                        string FirstNameValue = reader.GetString(reader.GetOrdinal("FirstName"));
                        string LastNameValue = reader.GetString(reader.GetOrdinal("LastName"));
                        int RentPortionValue = reader.GetInt32(reader.GetOrdinal("RentPortion"));
                        DateTime MoveInValue = reader.GetDateTime(reader.GetOrdinal("MoveInDate"));
                        int roomIdValue = reader.GetInt32(reader.GetOrdinal("RoomId"));
                        int rIdValue = reader.GetInt32(reader.GetOrdinal("rId"));
                        string nameValue = reader.GetString(reader.GetOrdinal("Name"));
                        int maxValue = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"));

                        Roommate rm = new Roommate()
                        {
                            Id = rmIdValue,
                            FirstName = FirstNameValue,
                            LastName = LastNameValue,
                            RentPortion = RentPortionValue,
                            MovedInDate = MoveInValue,
                            Room = new Room()
                            {
                                Id = rIdValue,
                                Name = nameValue,
                                MaxOccupancy = maxValue
                            }
                        };

                        roommates.Add(rm);
                    }

                    reader.Close();

                    return roommates;
                }
            }
        }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT rm.FirstName, rm.LastName, rm.RentPortion, rm.MoveInDate, rm.RoomId, r.Name, r.MaxOccupancy FROM Roommate rm JOIN Room r ON rm.RoomId = r.Id WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = new Room()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                            }

                        };
                    }

                    reader.Close();

                    return roommate;
                }
            }
        }
    }
}
