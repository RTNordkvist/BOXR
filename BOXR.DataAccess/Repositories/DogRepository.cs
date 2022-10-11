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
        private List<Dog> dogs = new List<Dog>();

        private List<Color> colors;

        private string path = "Dog.txt";

        private DataTable dataTable = new DataTable();

        private readonly string _connectionString;

        public DogRepository(ColorRepository colorRepository, string connectionString)
        {
            InitializeRepository();
            colors = colorRepository.GetAll();
            _connectionString = connectionString;
            //PullData();
            Find1("BR123456", null, null); //TODO make sure this works
        }

        private void InitializeRepository()
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    var lines = new List<string>();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                    sr.Close();

                    for (int i = 0; i < lines.Count; i++)
                    {
                        string[] data = lines[i].Split(';');
                        dogs.Add(new Dog()
                        {
                            Id = int.Parse(data[0]),
                            PedigreeNumber = data[1],
                            Name = data[2],
                            Breeder = data[3],
                            BirthDate = string.IsNullOrEmpty(data[4]) ? null : DateTime.Parse(data[4]), //Denne property er nullable, så der parses til null eller DateTime
                            Gender = Enum.TryParse(data[5], out Gender gender) ? null : gender, //Denne property er nullable
                            ChipNumber = data[6],
                            InbreedingCoefficient = double.TryParse(data[7], out double ic) == false ? null : ic, //Denne property er nullable
                            HdGrade = Enum.TryParse(data[8], out HdGrade hdg) == false ? null : hdg, //Denne property er nullable
                            HdIndex = int.TryParse(data[9], out int hi) == false ? null : hi, //Denne property er nullable
                            SpondylosisGrade = Enum.TryParse(data[10], out SpondylosisGrade sg) == false ? null : sg, //Denne property er nullable
                            HeartGrade = Enum.TryParse(data[11], out HearthGrade hs) == false ? null : hs, //Denne property er nullable
                            Color = colors.FirstOrDefault(x => x.Name == data[12]),
                            IsAlive = bool.Parse(data[13]),
                            MotherPedigreeNumber = data[14],
                            FatherPedigreeNumber = data[15],
                            Owner = data[16],
                        });
                    }
                }
            }

            catch (Exception)
            {
                dogs = new List<Dog>();
            }

        }

        public int Add(Dog dog)
        {
            if (!string.IsNullOrEmpty(dog.PedigreeNumber) && !string.IsNullOrEmpty(dog.Name) && !string.IsNullOrEmpty(dog.Breeder))
            {
                dog.Id = GetId();
                dogs.Add(dog);
                UpdateDatabase();
            }
            else
            {
                throw new ArgumentException("Not all arguments are valid");
            }
            return dog.Id;
        }

        public Dog Get(int id)
        {
            return dogs.FirstOrDefault(x => x.Id == id);
        }

        public Dog Get(string pedigreeNumber)
        {
            return dogs.FirstOrDefault(x => x.PedigreeNumber == pedigreeNumber);
        }

        public IEnumerable<Dog> Find(string pedigreeNumber, string name, string breeder)
        {
            return dogs.Where(x =>
                    (string.IsNullOrWhiteSpace(pedigreeNumber) || x.PedigreeNumber.Contains(pedigreeNumber, StringComparison.InvariantCultureIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(name) || x.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(breeder) || x.Breeder.Contains(breeder, StringComparison.InvariantCultureIgnoreCase)));
        }

        private int GetId()
        {
            if (dogs.Any())
            {
                return dogs.Max(x => x.Id) + 1;
            }

            return 1;
        }

        public void UpdateDatabase()
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (var dog in dogs)
                {
                    sw.WriteLine($"{dog.Id};{dog.PedigreeNumber};{dog.Name};{dog.Breeder};{dog.BirthDate};" +
                    $"{dog.Gender};{dog.ChipNumber};{dog.InbreedingCoefficient};{dog.HdGrade};" +
                    $"{dog.HdIndex};{dog.SpondylosisGrade};{dog.HeartGrade};{dog.Color.Name};" +
                    $"{dog.IsAlive};{dog.MotherPedigreeNumber};{dog.FatherPedigreeNumber};{dog.Owner}");
                }
            }
        }

        public Dog Get1(string pedigreeNumber)
        {
            string query = "SELECT * FROM Dog WHERE PedigreeNumber = @pedigreeNumber";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@pedigreeNumber", pedigreeNumber); //Add og AddWithValue?
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


        public Dog Get1(int id)
        {
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

        public IEnumerable<Dog> Find1(string pedigreeNumber, string name, string breeder)
        {
            string query = @"SELECT * FROM Dog WHERE 
                            (@pedigreeNumber IS NULL OR PedigreeNumber LIKE @pedigreeNumber) AND
                            (@name IS NULL OR Name LIKE @name) AND
                            (@breeder IS NULL OR Breeder LIKE @breeder)";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@pedigreeNumber", "%"+pedigreeNumber+"%");
            cmd.Parameters.AddWithValue("@name", "%" + name + "%");
            cmd.Parameters.AddWithValue("@breeder", "%" + breeder + "%");

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
            //dog.Gender = Enum.TryParse(Convert.ToString(rw["Gender"]), out Gender gender) ? null : gender;
            dog.ChipNumber = Convert.ToString(rw["ChipNumber"]);
            //dog.HdStatus = ;
            //dog.SpondylosisStatus = ;
            //dog.HeartStatus = ;
            //dog.Color = ;
            dog.IsAlive = Convert.ToBoolean(rw["IsAlive"]);
            dog.MotherPedigreeNumber = string.IsNullOrEmpty(Convert.ToString(rw["MotherPedigreeNumber"])) ? null : Convert.ToString(rw["MotherPedigreeNumber"]);
            dog.FatherPedigreeNumber = string.IsNullOrEmpty(Convert.ToString(rw["FatherPedigreeNumber"])) ? null : Convert.ToString(rw["FatherPedigreeNumber"]);

            return dog;
        }
    }
}
