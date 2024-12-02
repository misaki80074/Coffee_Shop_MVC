using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffee.Models
{
    public class DBCNcart
    {
            private readonly ProjectContext _context;

            // 建構式，注入 ProjectContext
            public DBCNcart(ProjectContext context)
            {
                _context = context;
            }

            // 使用 EF 執行 SQL 查詢並返回 DataTable
            public DataTable SQL(string sql, Dictionary<string, string>? parameters = null)
            {
                // 這部分保留原始邏輯，透過 context.Database.GetDbConnection() 做資料庫操作
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            var parameter = command.CreateParameter();
                            parameter.ParameterName = param.Key;
                            parameter.Value = param.Value;
                            command.Parameters.Add(parameter);
                        }
                    }

                    _context.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(result);
                        return dataTable;
                    }
                }
            }

            // 插入資料的簡化版
            public void Insert(string sql)
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    _context.Database.OpenConnection();
                    command.ExecuteNonQuery();
                }
            }

            // 取得流水號邏輯
            public string ItemNO(string notype)
            {
                string result = string.Empty;
                string sqltable = string.Empty;
                string id = string.Empty;

                // 依照 notype 判斷資料表和欄位
                switch (notype)
                {
                    case "O":
                        sqltable = "ORDERHEADER";
                        id = "OrderId";
                        break;
                    case "C":
                        sqltable = "CARTHEATER";
                        id = "CartId";
                        break;
                    case "P":
                        sqltable = "PRODUCT";
                        id = "ProductID";
                        break;
                    default:
                        return "Invalid Type";
                }

                // 使用原生 SQL 查詢來取得最大ID
                string sql = $"SELECT SUBSTRING(MAX({id}), 2, 10) FROM {sqltable}";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;

                    _context.Database.OpenConnection();
                    var resultScalar = command.ExecuteScalar()?.ToString();

                    // 處理結果並生成流水號
                    try
                    {
                        if (!string.IsNullOrEmpty(resultScalar) && resultScalar.Substring(0, 2) == DateTime.Today.ToString("yyyy").Substring(2, 2))
                        {
                            result = resultScalar.Substring(6, 4);
                            result = (Convert.ToInt32(result) + 1).ToString().PadLeft(4, '0');
                            result = $"{notype}{DateTime.Today:yy}{DateTime.Today:MM}{DateTime.Today:dd}{result}";
                        }
                        else
                        {
                            result = $"{notype}{DateTime.Today:yy}{DateTime.Today:MM}{DateTime.Today:dd}0001";
                        }
                    }
                    catch
                    {
                        result = $"{notype}{DateTime.Today:yy}{DateTime.Today:MM}{DateTime.Today:dd}0001";
                    }
                }

                return result;
            }
        }
    }
