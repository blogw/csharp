//-----------------------------------------------------------------------
// <copyright file="BaseModel.cs" company="Redlotus">
//     Copyright (C) 2013
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace CPA.Models
{
    /// <summary>
    /// 共通クラスです.
    /// </summary>
    /// <typeparam name="T">モデル</typeparam>
    public abstract class BaseModel<T> : IEnumerable<T> ,IDisposable
    {
        /// <summary>
        /// 構造メッソド
        /// </summary>
        protected BaseModel()
        {
            this.Source = new List<T>();
            this.MaxRowVersion = 0;
        }

        /// <summary>
        /// コレクション
        /// </summary>
        public ICollection<T> Source { get; set; }

        /// <summary>
        /// MaxRowVersion
        /// </summary>
        public int MaxRowVersion { get; set; }

        /// <summary>
        /// 存在位置を判断します.
        /// </summary>
        /// <param name="t">モデル</param>
        /// <returns>位置</returns>
        public abstract T Exists(T t);

        /// <summary>
        /// テーブル名前を返します.
        /// </summary>
        /// <returns>テーブル名前</returns>
        public string TableName()
        {
            return typeof(T).Name;
        }

        /// <summary>
        /// フィールド名前とフィールド値によってTのリストを取得します.
        /// </summary>
        /// <param name="fieldName">フィールド名前</param>
        /// <param name="fieldValue">フィールド値</param>
        /// <returns>Tのリスト</returns>
        public IEnumerable<T> GetByField(string fieldName, object fieldValue)
        {
            // 基準日を満たす表示順の営業所コード、名称のデータを検索する
            ICollection<T> result = new List<T>();

            foreach (T t in this.Source)
            {
                var value = t.GetType().GetProperty(fieldName).GetValue(t, null);

                if (value == null && fieldValue == null)
                {
                    result.Add(t);
                }
                else if (value.Equals(fieldValue))
                {
                    result.Add(t);
                }
            }

            // データを返す
            return result;
        }

        /// <summary>
        /// BINファイルからデータを読込.
        /// </summary>        
        /// <returns>bool</returns>
        public bool OpenBinarytoMemory()
        {
            // パス
            string path = this.BinPath();

            // ファイルのパースに存在しないの場合
            if (!File.Exists(path))
            {
                return false;
            }

            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                var formatter = new BinaryFormatter();

                // データ反序列化
                this.Source = (ICollection<T>)formatter.Deserialize(fileStream);

                this.SetMaxRowVersion();

                return true;
            }
        }

        /// <summary>
        /// データをマージします.
        /// </summary>
        /// <param name="dataTable">データテーブル</param>
        public void DataMerge(DataTable dataTable)
        {
            foreach (DataRow dataRow in dataTable.Rows)
            {
                T t = this.CreateT(dataTable, dataRow);

                T oldT = this.Exists(t);

                if (oldT != null)
                {
                    this.Source.Remove(oldT);
                }

                this.Source.Add(t);
            }

            this.SetMaxRowVersion();

            // データ序列化
            if (this.Source.Count > 0)
            {
                this.Serialize();
            }

        }

        /// <summary>
        /// メモリを開放する
        /// </summary>
        public void CloseMemory()
        {
            // クリアリスト
            if (this.Source != null)
            {
                this.Source.Clear();
                this.Source = null;
            }
        }

        /// <summary>
        /// 列挙子を返します.
        /// </summary>
        /// <returns>列挙子を</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.Source.GetEnumerator();
        }

        /// <summary>
        /// 列挙子を返します.
        /// </summary>
        /// <returns>列挙子</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Binファイルパース
        /// </summary>
        /// <returns>パース</returns>
        private string BinPath()
        {
            // binファイル
            string path = this.TableName() + ".bin";
            return path;
        }

        /// <summary>
        /// DataRowからモーデルを作成します.
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="dataRow">DataRow</param>
        /// <returns>モーデル</returns>
        private T CreateT(DataTable dataTable, DataRow dataRow)
        {
            var t = (T)Activator.CreateInstance(typeof(T), new object[] { });

            IEnumerator it = dataTable.Columns.GetEnumerator();

            int counter = 0;

            while (it.MoveNext())
            {
                var dataColumn = (DataColumn)it.Current;

                string cname = dataColumn.ColumnName;

                PropertyInfo propertyInfo = t.GetType().GetProperty(cname);

                if (dataRow[counter] != System.DBNull.Value)
                {
                    propertyInfo.SetValue(t, Convert.ChangeType(dataRow[counter], propertyInfo.PropertyType));
                }

                counter++;
            }

            return t;
        }

        /// <summary>
        /// MaxRowVersion
        /// </summary>
        private void SetMaxRowVersion()
        {

        }

        /// <summary>
        /// データ序列化
        /// </summary>        
        /// <returns>bool</returns>
        private bool Serialize()
        {
            // ファイルの作成
            using (var fileStream = new FileStream(this.BinPath(), FileMode.Create))
            {
                // Construct a BinaryFormatter and use it to serialize the data to the stream.
                var binaryFormatter = new BinaryFormatter();

                // データ序列化
                binaryFormatter.Serialize(fileStream, this.Source);
                return true;
            }
        }

        /// <summary>
        /// データ反序列化
        /// </summary>
        /// <param name="path">パース</param>
        /// <returns>DataTable</returns>
        private ICollection<T> Deserialize(string path)
        {
            string fileName = path;

            // ファイルを開く
            if (File.Exists(fileName))
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open))
                {
                    // Construct a BinaryFormatter and use it to serialize the data to the stream.
                    var binaryFormatter = new BinaryFormatter();

                    // データ反序列化
                    return (ICollection<T>)binaryFormatter.Deserialize(fileStream);
                }
            }
            else
            {
                return null;
            }
        }

        public void Dispose()
        {
            CloseMemory();
            GC.WaitForFullGCComplete();
        }

        
    }
}
