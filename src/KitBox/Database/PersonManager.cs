using System;
using System.Linq;
using UnityNpgsql;
using UnityNpgsqlTypes;
using System.Collections.Generic;

public class PersonManager
{

	private NpgsqlConnection connection;
	private NpgsqlCommand command;

	public PersonManager(NpgsqlConnection conn)
	{
		this.connection = conn;
	}

	public Person SelectPerson(Role role, string email, string password)
	{
		Person person = null;
		string select;
		if (role == Role.CUSTOMER)
		{
			select = "SELECT * FROM \"customer\" WHERE email='" + email + "' AND password='" + Person.hash(password) + "';";
		}
		else
		{
			select = "SELECT * FROM \"worker\" WHERE email='" + email + "' AND password='" + Person.hash(password) + "';";
		}

		this.connection.Open();
		this.command = new NpgsqlCommand(select, this.connection);
		NpgsqlDataReader reader = this.command.ExecuteReader();
		while (reader.Read())
		{
            if (role == Role.CUSTOMER)
            {
                person = new Customer((string)reader["name"], (string)reader["address"], (string)reader["phone"], (string)reader["email"]);
            }
            else
            {
                person = new Worker((string)reader["name"], (string)reader["address"], (string)reader["phone"], (string)reader["email"]);
            }
			
			person.Id = (int)reader["id"];
		}
		reader.Close();
		this.connection.Close();

		return person;

	}

    public Person InsertPerson(Person person, Role role, string password)
	{
		this.connection.Open();
        string r = role == Role.CUSTOMER ? "customer" : "worker";
		string insert = "INSERT INTO \"" + r + "\"(name,address,phone,email, password) values(:name,:address,:phone,:email, :password)";
		this.command = new NpgsqlCommand(insert, this.connection);

		this.command.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Varchar)).Value = person.Name;
		this.command.Parameters.Add(new NpgsqlParameter("address", NpgsqlDbType.Varchar)).Value = person.Address;
		this.command.Parameters.Add(new NpgsqlParameter("phone", NpgsqlDbType.Varchar)).Value = person.Phone;
		this.command.Parameters.Add(new NpgsqlParameter("email", NpgsqlDbType.Varchar)).Value = person.Email;
        this.command.Parameters.Add(new NpgsqlParameter("password", NpgsqlDbType.Varchar)).Value = Person.hash(password);

		this.command.ExecuteNonQuery();

		this.connection.Close();

        person.Id = SelectPerson(role, person.Email, password).Id;

		return person;
	}

	public void UpdatePerson(Person person, Role role)
	{
		this.connection.Open();
        string r = role == Role.CUSTOMER ? "customer" : "worker";
		string update = "UPDATE \"" + r + "\" SET name:name, address:address, phone:phone, email:email) WHERE(id =:id);";
		this.command = new NpgsqlCommand(update, this.connection);

		this.command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Varchar)).Value = person.Id;
		this.command.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Varchar)).Value = person.Name;
		this.command.Parameters.Add(new NpgsqlParameter("address", NpgsqlDbType.Varchar)).Value = person.Address;
		this.command.Parameters.Add(new NpgsqlParameter("phone", NpgsqlDbType.Varchar)).Value = person.Phone;
		this.command.Parameters.Add(new NpgsqlParameter("email", NpgsqlDbType.Varchar)).Value = person.Email;

		this.command.ExecuteNonQuery();

		this.connection.Close();

		return;
	}

	public List<Person> SelectAllPerson(Role role)
	{
		this.connection.Open();
		string select = SelectRole(role);
		this.command = new NpgsqlCommand(select, this.connection);
		NpgsqlDataReader reader = this.command.ExecuteReader();
		List<Person> people = new List<Person>();
		while (reader.Read())
		{
            
            if (role == Role.CUSTOMER)
                people.Add( new Customer((string)reader["name"], (string)reader["address"], (string)reader["phone"], (string)reader["email"]) );
            else
                people.Add( new Worker((string)reader["name"], (string)reader["address"], (string)reader["phone"], (string)reader["email"]));

            people.Last().Id = (int)reader["id"];
		}

		reader.Close();
		this.connection.Close();

		return people;
	}

	private string SelectRole(Role role)
	{
		string select;

		switch (role)
		{
			case Role.WORKER:
				select = "SELECT * FROM \"worker\" WHERE(email=:email)";
				break;
			case Role.CUSTOMER:
				select = "SELECT * FROM \"customer\" WHERE(email=:email)";
				break;
            default:
                throw new Exception("Invalid Role");
		}

		return select;
	}
}
