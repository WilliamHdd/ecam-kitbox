using System;
using UnityNpgsql;
using UnityNpgsqlTypes;
using System.Collections.Generic;

public class SupplierManager
{
	private NpgsqlConnection connection;
	private NpgsqlCommand command;

	public SupplierManager(NpgsqlConnection conn)
	{
		this.connection = conn;
	}

	public Supplier SelectSupplier(string nameSociety)
	{
        Supplier supplier = null;
		this.connection.Open();
		string select = "SELECT * FROM \"supplier\" WHERE(nameSociety=:name_society)";
		this.command = new NpgsqlCommand(select, this.connection);

		this.command.Parameters.Add(new NpgsqlParameter("name_society", NpgsqlDbType.Varchar)).Value = nameSociety;

		NpgsqlDataReader reader = this.command.ExecuteReader();
		while (reader.Read())
		{
            supplier = new Supplier((string)reader["name_society"], (string)reader["name_shop"], (string)reader["address"], (string)reader["city"]);
            supplier.Id = (int)reader["id"];
		}
		reader.Close();
		this.connection.Close();
		return supplier;
	}

	public Supplier InsertSupplier(Supplier supplier)
	{
		this.connection.Open();
		string insert = "INSERT INTO \"supplier\"(name_society, name_shop, address, city) values(:name_society, :name_shop, :address, :city)";
		this.command = new NpgsqlCommand(insert, this.connection);

		this.command.Parameters.Add(new NpgsqlParameter("name_society", NpgsqlDbType.Varchar)).Value = supplier.Society;
		this.command.Parameters.Add(new NpgsqlParameter("name_shop", NpgsqlDbType.Varchar)).Value = supplier.Shop;
		this.command.Parameters.Add(new NpgsqlParameter("address", NpgsqlDbType.Varchar)).Value = supplier.Address;
		this.command.Parameters.Add(new NpgsqlParameter("city", NpgsqlDbType.Varchar)).Value = supplier.City;

		this.command.ExecuteNonQuery();

		this.connection.Close();

        supplier.Id = SelectSupplier(supplier.Society).Id;

		return supplier;
	}

	public Supplier UpdateSupplier(Supplier supplier)
	{
		this.connection.Open();
		string update = "UPDATE \"customer\" SET name_society:name_society, name_shop:name_shop, address:address, city:city) WHERE(id =:id);";
		this.command = new NpgsqlCommand(update, this.connection);

		this.command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Varchar)).Value = supplier.Id;
		this.command.Parameters.Add(new NpgsqlParameter("name_society", NpgsqlDbType.Varchar)).Value = supplier.Society;
		this.command.Parameters.Add(new NpgsqlParameter("name_shop", NpgsqlDbType.Varchar)).Value = supplier.Shop;
		this.command.Parameters.Add(new NpgsqlParameter("address", NpgsqlDbType.Varchar)).Value = supplier.Address;
		this.command.Parameters.Add(new NpgsqlParameter("city", NpgsqlDbType.Varchar)).Value = supplier.City;
		this.command.ExecuteNonQuery();

		this.connection.Close();

		return SelectSupplier(supplier.Society);
	}

	public List<Supplier> SelectAllSupplier()
	{
		this.connection.Open();
		string select = "SELECT * FROM \"supplier\"";
		this.command = new NpgsqlCommand(select, this.connection);
		NpgsqlDataReader reader = this.command.ExecuteReader();
		List<Supplier> suppliers = new List<Supplier>();
		while (reader.Read())
		{
            Supplier supplier = new Supplier((string)reader["name_society"], (string)reader["name_shop"], (string)reader["address"], (string)reader["city"]);
            supplier.Id = (int)reader["id"];
            suppliers.Add(supplier);
		}

		reader.Close();
		this.connection.Close();

		return suppliers;
	}


}
