using BenchmarkLibrary.Models;
using BenchmarkLibrary.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BenchmarkLibrary.Data
{
    public static class Database
    {
        private static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BenchmarkDB;Integrated Security=True;Encrypt=False";

        // 🔐 Vérification login avec hash
        public static Company CheckCompanyLogin(string login, string password)
        {
            string hashed = PasswordHasher.Hash(password);
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Companies WHERE login = @login AND password = @password AND status = 'active'", conn);
            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@password", hashed);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Company
                {
                    Id = (int)reader["id"],
                    Name = reader["name"].ToString(),
                    Login = reader["login"].ToString(),
                    Status = reader["status"].ToString(),
                    Nacecode = reader["nacecode_code"].ToString(),
                    Logo = reader["logo"] == DBNull.Value ? null : (byte[])reader["logo"]
                };
            }
            return null;
        }

        public static List<Company> GetAllCompanies()
        {
            List<Company> companies = new();
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Companies ORDER BY name", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                companies.Add(new Company
                {
                    Id = (int)reader["id"],
                    Name = reader["name"].ToString(),
                    Status = reader["status"].ToString(),
                    Login = reader["login"].ToString(),
                    Nacecode = reader["nacecode_code"].ToString(),
                    Logo = reader["logo"] == DBNull.Value ? null : (byte[])reader["logo"]
                });
            }
            return companies;
        }

        public static void AddCompany(Company bedrijf)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(@"INSERT INTO Companies 
                (name, login, password, status, nacecode_code, logo) 
                VALUES (@name, @login, @password, @status, @nacecode, @logo)", conn);

            cmd.Parameters.AddWithValue("@name", bedrijf.Name);
            cmd.Parameters.AddWithValue("@login", bedrijf.Login ?? "");
            cmd.Parameters.AddWithValue("@password", PasswordHasher.Hash(bedrijf.Password ?? ""));
            cmd.Parameters.AddWithValue("@status", bedrijf.Status ?? "pending");
            cmd.Parameters.AddWithValue("@nacecode", bedrijf.Nacecode ?? "");
            cmd.Parameters.AddWithValue("@logo", bedrijf.Logo ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public static void UpdateCompany(Company bedrijf)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(@"UPDATE Companies SET 
                name = @name,
                login = @login,
                password = @password,
                status = @status,
                nacecode_code = @nacecode,
                logo = @logo
                WHERE id = @id", conn);

            cmd.Parameters.AddWithValue("@id", bedrijf.Id);
            cmd.Parameters.AddWithValue("@name", bedrijf.Name);
            cmd.Parameters.AddWithValue("@login", bedrijf.Login ?? "");
            cmd.Parameters.AddWithValue("@password", PasswordHasher.Hash(bedrijf.Password ?? ""));
            cmd.Parameters.AddWithValue("@status", bedrijf.Status ?? "pending");
            cmd.Parameters.AddWithValue("@nacecode", bedrijf.Nacecode ?? "");
            cmd.Parameters.AddWithValue("@logo", bedrijf.Logo ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public static void DeleteCompany(int id)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Companies WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateCompanyStatus(int id, string status)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Companies SET status = @status WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.ExecuteNonQuery();
        }

        public static void AddYearReport(int companyId, string jaar, int workplaces, int cleaning)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Yearreports (company_id, jaar, workplaces, cleaning_frequency) VALUES (@cid, @jaar, @wp, @cf)", conn);
            cmd.Parameters.AddWithValue("@cid", companyId);
            cmd.Parameters.AddWithValue("@jaar", jaar);
            cmd.Parameters.AddWithValue("@wp", workplaces);
            cmd.Parameters.AddWithValue("@cf", cleaning);
            cmd.ExecuteNonQuery();
        }

        public static List<Yearreport> GetReportsForCompany(int companyId)
        {
            List<Yearreport> list = new();
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Yearreports WHERE company_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", companyId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Yearreport
                {
                    Id = (int)reader["id"],
                    CompanyId = (int)reader["company_id"],
                    Jaar = reader["jaar"].ToString(),
                    Workplaces = (int)reader["workplaces"],
                    CleaningFrequency = (int)reader["cleaning_frequency"]
                });
            }
            return list;
        }

        public static List<Yearreport> GetYearReportsForSectorAndYear(string nacecode, string jaar)
        {
            List<Yearreport> list = new();
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(@"
                SELECT r.* FROM Yearreports r
                JOIN Companies c ON c.id = r.company_id
                WHERE c.nacecode_code = @nace AND r.jaar = @jaar", conn);
            cmd.Parameters.AddWithValue("@nace", nacecode);
            cmd.Parameters.AddWithValue("@jaar", jaar);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Yearreport
                {
                    Id = (int)reader["id"],
                    CompanyId = (int)reader["company_id"],
                    Jaar = reader["jaar"].ToString(),
                    Workplaces = (int)reader["workplaces"],
                    CleaningFrequency = (int)reader["cleaning_frequency"]
                });
            }
            return list;
        }

        public static void UpdateYearReport(int reportId, int workplaces, int cleaning)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Yearreports SET workplaces = @wp, cleaning_frequency = @cf WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", reportId);
            cmd.Parameters.AddWithValue("@wp", workplaces);
            cmd.Parameters.AddWithValue("@cf", cleaning);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteYearReport(int reportId)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Yearreports WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", reportId);
            cmd.ExecuteNonQuery();
        }
    }
}

