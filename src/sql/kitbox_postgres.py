import sys
import csv
import psycopg2
from itertools import groupby

def Postgres(user, db):

     conn = psycopg2.connect("dbname='{}' user='{}' host='localhost'".format(db, user))

     cursor = conn.cursor()

     cursor.execute("""
     CREATE TABLE IF NOT EXISTS supplier(
          id SERIAL PRIMARY KEY NOT NULL,
          name_society VARCHAR(255),
          name_shop VARCHAR(255),
          address VARCHAR(255),
          city VARCHAR(255)
     )
     """)
     conn.commit()

     cursor.execute("""
     CREATE TABLE IF NOT EXISTS product(
          reference VARCHAR(255),
          code VARCHAR(255) PRIMARY KEY,
          height INT,
          depth INT,
          width INT,
          color VARCHAR(255),
          stock INT,
          stock_min INT,
          price FLOAT,
          piece_per_bloc INT
     )
     """)
     conn.commit()

     cursor.execute("""
     CREATE TABLE IF NOT EXISTS customer(
          id SERIAL PRIMARY KEY NOT NULL,
          name VARCHAR(255),
          address VARCHAR(255),
          phone VARCHAR(255),
          email VARCHAR(255),
          password VARCHAR(255)
     )
     """)
     conn.commit()


     cursor.execute("""
     CREATE TYPE purchase_type AS ENUM ('draft', 'deposit', 'paid', 'closed')
     """)
     conn.commit()


     cursor.execute("""
     CREATE TABLE IF NOT EXISTS purchase(
          id SERIAL PRIMARY KEY NOT NULL,
          date_order TIMESTAMP,
          id_customer INT,
          status purchase_type,
          price FLOAT,
          FOREIGN KEY (id_customer)
          REFERENCES customer(id)
     )
     """)
     conn.commit()

     cursor.execute("""
     CREATE TABLE IF NOT EXISTS feature_supplier(
          id SERIAL PRIMARY KEY NOT NULL,
          id_supplier INT,
          code VARCHAR(255),
          time_supplier INT,
          price_supplier FLOAT,
          FOREIGN KEY (code)
          REFERENCES product(code),
          FOREIGN KEY (id_supplier)
          REFERENCES supplier(id)
     )
     """)
     conn.commit()

     cursor.execute("""
     CREATE TYPE orderitem_pos AS ENUM ('left', 'right', 'top', 'bottom', 'back', 'front', 'inner')
     """)
     conn.commit()


     cursor.execute("""
     CREATE TABLE IF NOT EXISTS orderitem(
          id SERIAL PRIMARY KEY NOT NULL,
          id_order INT,
          nbr_bloc INT,
          code_product VARCHAR(255),
          type orderitem_pos,
          quantity INT,
          unit_cost FLOAT,
          FOREIGN KEY (id_order)
          REFERENCES purchase(id),
          FOREIGN KEY (code_product)
          REFERENCES product(code)
     )
     """)
     conn.commit()

     cursor.execute("""
     CREATE TABLE IF NOT EXISTS worker(
          id SERIAL PRIMARY KEY UNIQUE NOT NULL,
          name VARCHAR(255),
          address VARCHAR(255),
          phone VARCHAR(255),
          email VARCHAR(255),
          password VARCHAR(255)
     )
     """)
     conn.commit()

     cursor.execute("""
     CREATE TABLE IF NOT EXISTS oder_product_supplier(
          id SERIAL PRIMARY KEY UNIQUE NOT NULL,
          code_product VARCHAR(255),
          number_product INT,
          price FLOAT,
          FOREIGN KEY (code_product)
          REFERENCES product(code)
     )
     """)
     conn.commit()

     cursor.execute("""
     CREATE TABLE IF NOT EXISTS oder_supplier(
          id SERIAL PRIMARY KEY UNIQUE NOT NULL,
          id_supplier INT,
          date_order TIMESTAMP,
          price_total FLOAT,
          FOREIGN KEY (id_supplier)
          REFERENCES oder_product_supplier(id),
          FOREIGN KEY (id_supplier)
          REFERENCES supplier(id)
     )
     """)
     conn.commit()

     with open('kitbox.csv') as csvfile:
          spamreader = csv.reader(csvfile, delimiter=';')
          csv_doc = list(spamreader)[1:]
          csvfile.close()

     query = "INSERT INTO product (reference, code, height, depth, width, color, stock, stock_min, price, piece_per_bloc) VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s)"

     insert = []
     for row in csv_doc:
          insert.append((row[0], row[1], int(row[2]), int(row[3]), int(row[4]), row[5], int(row[6]), int(row[7]), float(row[8].replace(',', '.')), int(row[9])))

     cursor.executemany(query, tuple(insert))
     conn.commit()

     with open('fournisseurs.txt', 'r') as f:
          doc = [word.strip() for word in f.readlines()]
          doc_filtered = list(filter(None, doc))
          final = [list(g) for k, g in groupby(doc_filtered, lambda x: '------' not in x and 'Fournisseur' not in x) if k]
          f.close()

     query = "INSERT INTO supplier (name_society, name_shop, address, city) VALUES (%s, %s, %s, %s)"
     insert = []

     for supplier in final:
          insert.append((supplier[0], supplier[1], supplier[2], supplier[3]))

     cursor.executemany(query, tuple(insert))
     conn.commit()

     query = "INSERT INTO feature_supplier (id_supplier, code, time_supplier, price_supplier) VALUES (%s, %s, %s, %s)"
     insert = []

     for row in csv_doc:
          insert.append((1, row[1], int(row[11]), float(row[10].replace(',', '.'))))
          insert.append((2, row[1], int(row[13]), float(row[12].replace(',', '.'))))

     cursor.executemany(query, tuple(insert))
     conn.commit()

if __name__ == '__main__':

    if len(sys.argv) == 3:
        Postgres(sys.argv[1], sys.argv[2])
        print("Database ready !")
    else:
        print("Number of arguments invalid")
