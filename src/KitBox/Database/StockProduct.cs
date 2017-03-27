using System.Drawing;
using System;
using System.Data;
using Npgsql;
using NpgsqlTypes;

public class StockProduct
{
	private NpgsqlConnection connection;
	private NpgsqlCommand command;
	private string reference;
	private string code;
	private Dimensions dimensions;
	private Color color;
	private int stock;
	private int stockMin;
	private double price;
	private int piecePerBloc;
	private int Supplier;
	private int delivery_time;

	public StockProduct(string reference, string code, Dimensions dim, Color color, int stock, int stockMin, double price, int piecepb, int Supplier, int delivery_time)
	{
		this.reference = reference;
		this.code = code;
		this.dimensions = dim;
		this.color = color;
		this.stock = stock;
		this.stockMin = stockMin;
		this.price = price;
		this.piecePerBloc = piecepb;
		this.Supplier = Supplier;
		this.delivery_time = delivery_time;
	}

	public string Reference
	{
		get { return reference; }
	}

	public string Code
	{
		get { return code; }
	}

	public Dimensions Dimensions
	{
		get { return dimensions; }
	}

	public string Color
	{
		get { return color; }
	}

	public int Stock
	{
		get { return stock; }
	}

	public int StockMin
	{
		get { return stockMin; }
	}

	public double Price
	{
		get { return price; }
	}

	public int PiecePerBloc
	{
		get { return piecePerBloc; }
	}




	public StockProduct(NpgsqlConnection conn)
	{
		this.connection = conn;
	}

	/*public  Supplier BestSupplier()
	{
		
		this.connection.Open();
		string select = "SELECT * FROM \"feature_Supplier\" WHERE code = '" + product + "';";
		NpgsqlDataAdapter objDataAdapter = new NpgsqlDataAdapter(select, this.connection);
		DataSet features = new DataSet();
		objDataAdapter.Fill(features, "Features");
		this.connection.Close();






		foreach (DataTable table in features.Tables)
		{
			foreach (DataRow row in table.Rows)
			{

				foreach (DataColumn column in table.Columns )
				{
					if (row[column] = "code"){
						object item = row[column];
						//if (column.Namespace == ref
						Console.WriteLine(item.ToString());
						// read column and item
					}
				}
			}
		}

		return %;

	}*/
}

