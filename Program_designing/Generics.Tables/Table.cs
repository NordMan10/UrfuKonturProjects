using System;
using System.Collections.Generic;
using System.Linq;

namespace Generics.Tables
{
    public class Table<TRow, TColumn, TValue>
    {
        public Table()
        {
            Columns = new Dictionary<TColumn, TValue>();
            Rows = new Dictionary<TRow, Dictionary<TColumn, TValue>>();

            Data = Rows;
            Open = new OpenTableData<TRow, TColumn, TValue>(this, Data);
            Existed = new ExistedTableData<TRow, TColumn, TValue>(this, Data);
        }

        private Dictionary<TRow, Dictionary<TColumn, TValue>> Data { get; }

        public OpenTableData<TRow, TColumn, TValue> Open { get; set; }

        public ExistedTableData<TRow, TColumn, TValue> Existed { get; set; }

        public Dictionary<TRow, Dictionary<TColumn, TValue>> Rows { get; }

        public Dictionary<TColumn, TValue> Columns { get; }

        public void AddColumn(TColumn columnKey)
        {
            if (!Columns.ContainsKey(columnKey))
            {
                Columns.Add(columnKey, default);

                foreach (var rowColumns in Rows.Values)
                {
                    rowColumns.Add(columnKey, default);
                }
            }
        }

        public void AddRow(TRow rowKey)
        {
            if (!Rows.ContainsKey(rowKey))
            {
                Rows.Add(rowKey, new Dictionary<TColumn, TValue>());

                foreach (var column in Columns)
                {
                    Rows.Values.Last().Add(column.Key, default);
                }
            }
        }
    }

    public class OpenTableData<TRow, TColumn, TValue>
    {
        public OpenTableData(Table<TRow, TColumn, TValue> table, Dictionary<TRow, Dictionary<TColumn, TValue>> data)
        {
            this.table = table;
            Data = data;
        }

        private readonly Table<TRow, TColumn, TValue> table;

        private Dictionary<TRow, Dictionary<TColumn, TValue>> Data { get; set; }

        public TValue this[TRow rowKey, TColumn columnKey]
        {
            get 
            {
                if (!Data.ContainsKey(rowKey) || !Data[rowKey].ContainsKey(columnKey))
                    return default;

                return Data[rowKey][columnKey]; 
            }
            set 
            {
                if (!Data.ContainsKey(rowKey))
                    table.AddRow(rowKey);

                if (!Data[rowKey].ContainsKey(columnKey))
                    table.AddColumn(columnKey);

                Data[rowKey][columnKey] = value; 
            }
        }
    }

    public class ExistedTableData<TRow, TColumn, TValue>
    {
        public ExistedTableData(Table<TRow, TColumn, TValue> table, Dictionary<TRow, Dictionary<TColumn, TValue>> data)
        {
            this.table = table;
            Data = data;
        }

        private readonly Table<TRow, TColumn, TValue> table;

        private Dictionary<TRow, Dictionary<TColumn, TValue>> Data { get; set; }

        public TValue this[TRow rowKey, TColumn columnKey]
        {
            get 
            {
                if (!ContainsCell(rowKey, columnKey))
                    throw new ArgumentException();

                return Data[rowKey][columnKey];
            }
            set 
            {
                if (!ContainsCell(rowKey, columnKey))
                    throw new ArgumentException();

                Data[rowKey][columnKey] = value;
            }
        }

        private bool ContainsCell(TRow rowKey, TColumn columnKey)
        {
            return Data.ContainsKey(rowKey) && Data[rowKey].ContainsKey(columnKey);
        }
    }
}
