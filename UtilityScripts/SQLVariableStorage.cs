using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using SQLite;

public class SQLVariableStorage : VariableStorageBehaviour {

    private SQLiteConnection db; // the database connector

    void Start() {
            // pick a place on disk for the database to save to
            string path = Application.persistentDataPath + "/db.sqlite";
            // create a new database connection to speak to it
            db = new SQLiteConnection(path);
            // create the tables we need
            db.CreateTable<YarnString>();
            db.CreateTable<YarnFloat>();
            db.CreateTable<YarnBool>();
    }

    public override bool TryGetValue<T>(string variableName, out T result) {
        string query = "";
        List<object> results = null;
        // try to get a value from the given table, as a generic object
        if (typeof(T) == typeof(string)) {
            query = $"SELECT value FROM YarnString WHERE key = {variableName}";
        } else if (typeof(T) == typeof(bool)) {
            query = $"SELECT value FROM YarnSBool WHERE key = {variableName}";
        } else if (typeof(T) == typeof(float)) {
            query = $"SELECT value FROM YarnFloat WHERE key = {variableName}";
        }
        // if a result was found, convert it to type T and assign it
        results = db.Query<object>(query);
        if (results?.Count > 0) {
            result = (T)results[0];
            return true;
        }
        // otherwise TryGetValue has failed
        result = default(T);
        return false;
    }

    public override void SetValue(string variableName, string stringValue) {
		// check it doesn't exist already in other table
		if (Exists(variableName, typeof(bool))) {
				throw new System.ArgumentException($"{variableName} is a bool.");
		// check if doesn't exist already in other other table
		} else if (Exists(variableName, typeof(float))) {
				throw new System.ArgumentException($"{variableName} is a float.");
		}
		// if not, insert or update row in this table to the given value
		string query = "INSERT OR REPLACE INTO YarnString (key, value)";
		query += $"VALUES ({variableName}, {stringValue})";
		db.Execute(query);
    }

    public override void SetValue(string variableName, float floatValue) {
		// check it doesn't exist already in other table
		if (Exists(variableName, typeof(string))) {
				throw new System.ArgumentException($"{variableName} is a string.");
		// check if doesn't exist already in other other table
		} else if (Exists(variableName, typeof(float))) {
				throw new System.ArgumentException($"{variableName} is a float.");
		}
		// if not, insert or update row in this table to the given value
		string query = "INSERT OR REPLACE INTO YarnFloat (key, value)";
		query += $"VALUES ({variableName}, {floatValue})";
		db.Execute(query);
    }

    public override void SetValue(string variableName, bool boolValue) {
		// check it doesn't exist already in other table
		if (Exists(variableName, typeof(string))) {
				throw new System.ArgumentException($"{variableName} is a string.");
		// check if doesn't exist already in other other table
		} else if (Exists(variableName, typeof(float))) {
				throw new System.ArgumentException($"{variableName} is a float.");
		}
		// if not, insert or update row in this table to the given value
		string query = "INSERT OR REPLACE INTO YarnBool (key, value)";
		query += $"VALUES ({variableName}, {boolValue})";
		db.Execute(query);
    }

    public override void Clear() {
        db.Execute("DELETE * FROM YarnString;");
        db.Execute("DELETE * FROM YarnBool;");
        db.Execute("DELETE * FROM YarnFloat;");
    }

    public override bool Contains(string variableName) {
        return Exists(variableName, typeof(string)) || 
            Exists(variableName, typeof(bool)) || 
            Exists(variableName, typeof(float));
    }

    private bool Exists(string variableName, System.Type type) {
        if (type == typeof(string)) {
            string stringResult;
            if (TryGetValue<string>(variableName, out stringResult)) {
                return (stringResult != null);
            }
        } else if (type == typeof(bool)) {
            string boolResult;
            if (TryGetValue<string>(variableName, out boolResult)) {
                return (boolResult != null);
            }
        } else if (type == typeof(float)) {
            string floatResult;
            if (TryGetValue<string>(variableName, out floatResult)) {
                return (floatResult != null);
            }
        }
        return false;
    }
}

public class YarnString {
	[PrimaryKey]
	public string key { get; set; }
	public string value { get; set; }
}
public class YarnFloat {
	[PrimaryKey]
	public string key { get; set; }
	public float value { get; set; }
}
public class YarnBool {
	[PrimaryKey]
	public string key { get; set; }
	public bool value { get; set; }
}