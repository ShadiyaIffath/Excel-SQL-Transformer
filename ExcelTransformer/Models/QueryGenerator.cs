using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelTransformer.Models
{
    class QueryGenerator
    {
        public string tableName { get; set; }

        public List<string> columns { get; set; }

        public QueryGenerator(string _tableName, List<string> _columns)
        {
            this.tableName = _tableName;
            this.columns = _columns;
        }

        public string generateInsertQuery(List<string> data)
        {
            string query = "INSERT INTO " + this.tableName + " (";

            for(int i = 0; i < this.columns.Count -1; i++)
            {
                query += this.columns[i] + ", ";
            }
            query += this.columns[this.columns.Count - 1] + " ) \n VALUES (";

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

                query += i < data.Count - 1 ? ", " : ");";
            }
            return query;
        }

        public string generateUpdateQuery(List<string> data)
        {
            return "";
        }
    }
}
