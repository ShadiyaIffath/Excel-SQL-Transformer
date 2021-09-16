using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelTransformer.Models
{
    class QueryGenerator
    {
        public static List<string> columns { get; set; }
        public static string generateInsertQuery(string tableName, List<string> data)
        {
            string query = "\nINSERT INTO " + tableName + " (";

            for(int i = 0; i < columns.Count -1; i++)
            {
                query += columns[i] + ", ";
            }
            query += columns[columns.Count - 1] + " ) \nVALUES (";

            for(int i =0; i < data.Count; i++)
            {
                if(int.TryParse(data[i], out int intTemp)){
                    query += intTemp;
                }
                else if(double.TryParse(data[i], out double doubleTemp))
                {
                    query += doubleTemp;
                }
                else
                {
                    query += "'" +data[i] + "'";
                }

                query += i < data.Count - 1 ? ", " : ");\n";
            }
            return query;
        }

        public static string generateUpdateQuery(string tableName, List<string> columns, List<string> data, string whereClause)
        {
            string query = "\nUPDATE " + tableName + " SET \n" ;
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < columns.Count; j++)
                {
                    query += columns[i] + " = ";

                    if (int.TryParse(data[i], out int intTemp))
                    {
                        query += intTemp;
                    }
                    else if (double.TryParse(data[i], out double doubleTemp))
                    {
                        query += doubleTemp;
                    }
                    else
                    {
                        query += "'" + data[i] + "'";
                    }

                    query += j < columns.Count - 1 ? ", " : "\n"+ whereClause+" ;";
                }
            }
            return query;
        }
    }
}
