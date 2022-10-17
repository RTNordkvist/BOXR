using BOXR.Data.Models;
using BOXR.Data.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BOXR.DataAccess.Repositories
{
    public class DogRepository
    {
        private readonly string _connectionString;

        public DogRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Add(Dog dog)
        {
            if (!string.IsNullOrEmpty(dog.PedigreeNumber) && !string.IsNullOrEmpty(dog.Name) && !string.IsNullOrEmpty(dog.Breeder))
            {
                string command = @"INSERT INTO Dog 
                (PedigreeNumber, Name, BirthDate, Gender, ChipNumber, HdGrade, SpondylosisGrade, HeartGrade, Color, IsAlive, MotherPedigreeNumber, FatherPedigreeNumber, Breeder, Owner, Image, RegisteredDate, LastUpdated) 
                VALUES 
                (@PedigreeNumber, @Name, @BirthDate, @Gender, @ChipNumber, @HdGrade, @SpondylosisGrade, @HeartGrade, @Color, @IsAlive, @MotherPedigreeNumber, @FatherPedigreeNumber, @Breeder, @Owner, @Image, @RegisteredDate, @LastUpdated);
                SELECT SCOPE_IDENTITY();";

                SqlConnection conn = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand(command, conn);
                cmd.Parameters.AddWithValue("@PedigreeNumber", dog.PedigreeNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Name", dog.Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@BirthDate", dog.BirthDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Gender", dog.Gender);
                cmd.Parameters.AddWithValue("@ChipNumber", dog.ChipNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@HdGrade", dog.HdGrade);
                cmd.Parameters.AddWithValue("@SpondylosisGrade", dog.SpondylosisGrade);
                cmd.Parameters.AddWithValue("@HeartGrade", dog.HeartGrade);
                cmd.Parameters.AddWithValue("@Color", dog.Color);
                cmd.Parameters.AddWithValue("@IsAlive", dog.IsAlive);
                cmd.Parameters.AddWithValue("@MotherPedigreeNumber", dog.MotherPedigreeNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@FatherPedigreeNumber", dog.FatherPedigreeNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Breeder", dog.Breeder ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Owner", dog.Owner ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Image", dog.Image ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@RegisteredDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@LastUpdated", DateTime.Now);

                conn.Open();
                cmd.CommandType = CommandType.Text;
                var res = cmd.ExecuteScalar();
                conn.Close();
                dog.Id = Convert.ToInt32(res);
            }
            else
            {
                throw new ArgumentException("All required fields must be submitted");
            }

            return dog.Id;
        }

        public int Update(Dog dog)
        {
            if (!string.IsNullOrEmpty(dog.PedigreeNumber) && !string.IsNullOrEmpty(dog.Name) && !string.IsNullOrEmpty(dog.Breeder))
            {
                string command = @"UPDATE Dog 
                SET
                PedigreeNumber=@PedigreeNumber, Name=@Name, BirthDate=@BirthDate, Gender=@Gender, ChipNumber=@ChipNumber, HdGrade=@HdGrade, SpondylosisGrade=@SpondylosisGrade, HeartGrade=@HeartGrade, Color=@Color, IsAlive=@IsAlive, MotherPedigreeNumber=@MotherPedigreeNumber, FatherPedigreeNumber=@FatherPedigreeNumber, Breeder=@Breeder, Owner=@Owner, Image=@Image, LastUpdated=@LastUpdated
                WHERE Id=@Id;";

                SqlConnection conn = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand(command, conn);
                cmd.Parameters.AddWithValue("@Id", dog.Id);
                cmd.Parameters.AddWithValue("@PedigreeNumber", dog.PedigreeNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Name", dog.Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@BirthDate", dog.BirthDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Gender", dog.Gender);
                cmd.Parameters.AddWithValue("@ChipNumber", dog.ChipNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@HdGrade", dog.HdGrade);
                cmd.Parameters.AddWithValue("@SpondylosisGrade", dog.SpondylosisGrade);
                cmd.Parameters.AddWithValue("@HeartGrade", dog.HeartGrade);
                cmd.Parameters.AddWithValue("@Color", dog.Color);
                cmd.Parameters.AddWithValue("@IsAlive", dog.IsAlive);
                cmd.Parameters.AddWithValue("@MotherPedigreeNumber", dog.MotherPedigreeNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@FatherPedigreeNumber", dog.FatherPedigreeNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Breeder", dog.Breeder ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Owner", dog.Owner ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Image", dog.Image ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@LastUpdated", DateTime.Now);

                conn.Open();
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                throw new ArgumentException("All required fields must be submitted");
            }

            return dog.Id;
        }

        public Dog Get(string pedigreeNumber)
        {
            DataTable dataTable = new DataTable();

            string query = "SELECT * FROM Dog WHERE PedigreeNumber = @pedigreeNumber";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@pedigreeNumber", pedigreeNumber);
            conn.Open();
            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();

            var result = dataTable.AsEnumerable().Any() ? ConvertToDog(dataTable.AsEnumerable().First()) : null;

            return result;
        }

        public Dog Get(int id)
        {
            DataTable dataTable = new DataTable();

            string query = "SELECT * FROM Dog WHERE Id = @id";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id); //Add og AddWithValue?
            conn.Open();
            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();

            var result = dataTable.AsEnumerable().Any() ? ConvertToDog(dataTable.AsEnumerable().First()) : null;

            return result;
        }

        public IEnumerable<Dog> Find(string pedigreeNumber, string name, string breeder)
        {
            DataTable dataTable = new DataTable();

            string query = @"SELECT * FROM Dog WHERE
                           (@pedigreeNumber IS NULL OR PedigreeNumber LIKE @pedigreeNumber) AND
                           (@name IS NULL OR Name LIKE @name) AND
                           (@breeder IS NULL OR Breeder LIKE @breeder)";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@pedigreeNumber", pedigreeNumber == null ? (object)DBNull.Value : "%" + pedigreeNumber + "%");
            cmd.Parameters.AddWithValue("@name", name == null ? (object)DBNull.Value : "%" + name + "%");
            cmd.Parameters.AddWithValue("@breeder", breeder == null ? (object)DBNull.Value : "%" + breeder + "%");

            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();

            var result = new List<Dog>();
            foreach (var rw in dataTable.AsEnumerable())
            {
                result.Add(ConvertToDog(rw));
            }
            // result indeholder nu alle hunde i databasen, fordi query ikke har en WHERE clause
            return result;
        }

        public IEnumerable<Dog> FindOffspring(string pedigreeNumber)
        {
            DataTable dataTable = new DataTable();

            string query = @"SELECT * FROM Dog WHERE 
                            (MotherPedigreeNumber = @pedigreeNumber OR FatherPedigreeNumber = @pedigreeNumber)";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@pedigreeNumber", pedigreeNumber);

            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();

            var result = new List<Dog>();
            foreach (var rw in dataTable.AsEnumerable())
            {
                result.Add(ConvertToDog(rw));
            }
            // result indeholder nu alle hunde i databasen, fordi query ikke har en WHERE clause
            return result;
        }

        private Dog ConvertToDog(DataRow rw)
        {
            Dog dog = new();
            dog.Id = Convert.ToInt32(rw["Id"]);
            dog.PedigreeNumber = Convert.ToString(rw["PedigreeNumber"]);
            dog.Name = Convert.ToString(rw["Name"]);
            dog.Breeder = Convert.ToString(rw["Breeder"]);
            dog.BirthDate = DateTime.TryParse(Convert.ToString(rw["BirthDate"]), out DateTime birthdate) ? birthdate : null;
            dog.Gender = Enum.TryParse(Convert.ToString(rw["Gender"]), out Gender gender) ? gender : Gender.Undecided;
            dog.ChipNumber = Convert.ToString(rw["ChipNumber"]);
            dog.HdGrade = Enum.TryParse(Convert.ToString(rw["HdGrade"]), out HdGrade hdGrade) ? hdGrade : HdGrade.Undecided;
            dog.SpondylosisGrade = Enum.TryParse(Convert.ToString(rw["SpondylosisGrade"]), out SpondylosisGrade spondylosisGrade) ? spondylosisGrade : SpondylosisGrade.Undecided;
            dog.HeartGrade = Enum.TryParse(Convert.ToString(rw["HeartGrade"]), out HeartGrade heartGrade) ? heartGrade : HeartGrade.Undecided;
            dog.Color = Enum.TryParse(Convert.ToString(rw["Color"]), out Color color) ? color : Color.Undecided;
            dog.IsAlive = Convert.ToBoolean(rw["IsAlive"]);
            dog.MotherPedigreeNumber = string.IsNullOrEmpty(Convert.ToString(rw["MotherPedigreeNumber"])) ? null : Convert.ToString(rw["MotherPedigreeNumber"]);
            dog.FatherPedigreeNumber = string.IsNullOrEmpty(Convert.ToString(rw["FatherPedigreeNumber"])) ? null : Convert.ToString(rw["FatherPedigreeNumber"]);
            dog.Owner = Convert.ToString(rw["Owner"]);
            dog.Image = string.IsNullOrEmpty(Convert.ToString(rw[nameof(Dog.Image)])) ? null : Convert.ToString(rw[nameof(Dog.Image)]);
            dog.RegisteredDate = DateTime.Parse(Convert.ToString(rw["RegisteredDate"]));
            dog.LastUpdated = DateTime.Parse(Convert.ToString(rw["LastUpdated"]));

            return dog;
        }
    }
}
