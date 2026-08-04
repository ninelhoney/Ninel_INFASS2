using System.ComponentModel.DataAnnotations;

namespace Ninel_INFASS2.Models
{
    public class User
    {
        public string SqlInsert(string[] fields, string[] values, string tb)
        {
            string fieldlist = "";
            string valuelist = "";

            for (int i = 0; i < fields.Length; i++)
            {
                fieldlist += fields[i];

                if (i < fields.Length - 1)
                    fieldlist += ", ";
            }

            for (int i = 0; i < values.Length; i++)
            {
                valuelist += $"'{values[i]}'";

                if (i < values.Length - 1)
                    valuelist += ", ";
            }

            return $"INSERT INTO {tb} ({fieldlist}) VALUES ({valuelist})";
        }

        public string SqlUpdate(
            string[] fields,
            string[] values,
            string tb,
            string idField,
            string idValue)
        {
            string setValues = "";

            for (int i = 0; i < fields.Length; i++)
            {
                setValues += $"{fields[i]} = '{values[i]}'";

                if (i < fields.Length - 1)
                    setValues += ", ";
            }

            return $"UPDATE {tb} SET {setValues} WHERE {idField} = '{idValue}';";
        }

        public string SqlDelete(
            string tb,
            string idField,
            string idValue)
        {
            return $"DELETE FROM {tb} WHERE {idField} = '{idValue}';";
        }

        public string ViewAll(string tb)
        {
            return $"SELECT * FROM {tb};";
        }
    }
    public class UserData
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class LoginViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class RegisterViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        [Range(15, 100, ErrorMessage = "Age must be between 15 and 100.")]
        public int Age { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
